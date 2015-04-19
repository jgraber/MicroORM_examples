using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.Drawing;

namespace MicroORM_PetaPoco.Data
{
    class PetaPocoRepository : IRepository
    {
        private Database database = new Database("OrmConnection");

        public Book FindBook(int id)
        {
            var book = database.Query<Book, Publisher>("SELECT * FROM Book b LEFT JOIN Publisher p ON p.Id = b.PublisherId WHERE b.Id = @0", id).SingleOrDefault();

            if (book != null)
            {
                var authors = database.Query<Author>(
                    @"SELECT a.* FROM Author a 
                    LEFT JOIN BookAuthor ba ON ba.AuthorID = a.Id
                    LEFT JOIN Book b ON b.Id = ba.BookId
                    WHERE b.Id = @0", book.Id).ToList();

                if (authors.Count > 0)
                {
                    book.Authors.AddRange(authors);
                }

                var cover = database.Query<byte[]>(
                    @"SELECT Cover FROM COVER WHERE BookId = @0", book.Id).SingleOrDefault();

                if (cover != null)
                {
                    book.Cover = Image.FromStream(new MemoryStream(cover));
                }

            }

            return book;
        }

        public List<Book> GetAllBooks()
        {
            return database.Query<Book>("SELECT * FROM BOOK;").ToList();
        }

        public Book Add(Book book)
        {
            database.Insert("Book", "Id", book);

            string insertBookAuthor = @"INSERT INTO [dbo].[BookAuthor] ([BookId],[AuthorId])
                                             VALUES (@BookId, @AuthorId);";

            foreach (var author in book.Authors)
            {
                database.Execute(insertBookAuthor, new {BookId = book.Id, AuthorId = author.Id});
            }

            if (book.Cover != null)
            {
                string insertCover = @"INSERT INTO Cover (BookId, Cover) Values (@Id, @Cover);";
                database.Execute(insertCover, new { book.Id, Cover = book.CoverAsBytes() });
            }

            return book;
        }

        public Book Update(Book book)
        {
            database.Update("Book", "Id", book);
            return book;
        }

        public void RemoveBook(int id)
        {
            database.Delete<Book>("WHERE Id=@0", id);
        }

        public Book GetLatestBook()
        {
            return database.Query<Book>("SELECT TOP 1 * FROM Book ORDER BY Id desc").Single();
        }

        public List<Book> GetBooksByPublisher(Publisher publisher)
        {
            return database.Query<Book, Publisher>("SELECT * FROM Book b LEFT JOIN Publisher p ON p.Id = b.PublisherId WHERE p.Id = @0", publisher.Id).ToList(); 
        }

        public Author Add(Author author)
        {
            database.Insert("Author", "Id", author);
            return author;
        }

        public Author FindAuthor(int id)
        {
            return database.Query<Author>("SELECT * FROM Author WHERE Id = @0;", id).SingleOrDefault();
        }

        public Publisher Add(Publisher publisher)
        {
            database.Insert("Publisher", "Id", publisher);
            return publisher;
        }

        public Publisher FindPublisher(int id)
        {
            throw new NotImplementedException();
        }

        public BookStats GetStatistics()
        {
            return database.Query<BookStats>("SELECT * FROM BookStats").SingleOrDefault();
        }

        public List<Book> SearchFullText(string terms)
        {
            throw new NotImplementedException();
        }

        public List<SemanticBook> SemanticSearch(string terms)
        {
            throw new NotImplementedException();
        }
    }
}
