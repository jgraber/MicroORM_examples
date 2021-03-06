﻿using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;


namespace MicroORM_Dapper.Data
{
    public class DapperRepository : IRepository
    {
        private IDbConnection db = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString);


        public Book FindBook(int id)
        {
            var sql = "SELECT * FROM Book WHERE Id = @Id;" +
                "SELECT Cover FROM Cover WHERE BookId = @Id;";

            using (var multipleResults = this.db.QueryMultiple(sql, new {Id = id}))
            {
                var book = multipleResults.Read<Book>().SingleOrDefault();
                var cover = multipleResults.Read<byte[]>().SingleOrDefault();
                if (book != null && cover != null)
                {
                    book.Cover = Image.FromStream(new MemoryStream(cover));
                }

                return book;
            }
        }

        public List<Book> GetAllBooks()
        {
            return this.db.Query<Book>("SELECT * FROM Book;").ToList(); 
        }

        public Book Add(Book book)
        {
            using (var transaction = new TransactionScope())
            {


                string sql = @"INSERT INTO Book (Title, Pages, ISBN, Summary, Rating, PublisherId, ReadingStatus) 
                                VALUES (@Title, @Pages, @ISBN, @Summary, @Rating, @PublisherId, @ReadingStatus);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

                int id = this.db.Query<int>(sql, book).Single();
                book.Id = id;

                if (book.Cover != null)
                {
                    string insertCover = @"INSERT INTO Cover (BookId, Cover) Values (@Id, @Cover);";
                    this.db.Execute(insertCover, new {Id = book.Id, Cover = book.CoverAsBytes()});
                }

                if (book.Authors.Count > 0)
                {
                    string insertBookAuthor = @"INSERT INTO [dbo].[BookAuthor] ([BookId],[AuthorId])
                                             VALUES (@BookId, @AuthorId);";

                    foreach (var author in book.Authors)
                    {
                        this.db.Execute(insertBookAuthor, new {BookId = book.Id, AuthorId = author.Id});
                    }
                }
                transaction.Complete();
            }
            return book;
        }



        public Book Update(Book book)
        {
            string sql = @"UPDATE Book SET Title = @Title, Pages = @Pages, 
                            ISBN = @ISBN, Summary = @Summary, Rating = @Rating WHERE Id = @Id;";
            this.db.Execute(sql, book);
            return book;
        }

        public void RemoveBook(int id)
        {
            string sql = @"DELETE FROM Book WHERE Id = @Id;";
            this.db.Execute(sql, new {Id = id});
        }

        public Book GetLatestBook()
        {
            return this.db.Query<Book>("SELECT TOP 1 * FROM Book ORDER By Id desc;").SingleOrDefault();
        }

        public List<Book> GetBooksByPublisher(Publisher publisher)
        {
            return this.db.Query<Book>("SELECT * FROM Book WHERE PublisherId = @Id", publisher).ToList(); 
        }

        public Author Add(Author author)
        {
            var insertAuthor = @"INSERT INTO Author ([FirstName],[LastName],[EMail],[Web],[Twitter])
                           VALUES (@FirstName, @LastName, @EMail, @Web, @Twitter)
                           SELECT CAST(SCOPE_IDENTITY() AS INT)";
            int id = this.db.Query<int>(insertAuthor, author).Single();
            author.Id = id;

            return author;
        }

        public Author FindAuthor(int id)
        {
            var author = this.db.Query<Author>("SELECT * FROM Author WHERE Id = @Id;", new { Id = id }).SingleOrDefault();
            return author;
        }

        public Publisher Add(Publisher publisher)
        {
            var insertPublisher = @"INSERT INTO Publisher (Name, Url, EMail)
                                          VALUES (@Name, @Url, @EMail)
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";
            int id = this.db.Query<int>(insertPublisher, publisher).Single();
            publisher.Id = id;

            return publisher;
        }

        public Publisher FindPublisher(int id)
        {
            var publisher = this.db.Query<Publisher>("SELECT * FROM Publisher WHERE Id = @Id;", new { Id = id }).SingleOrDefault();
            return publisher;
        }

        public BookStats GetStatistics()
        {
            return this.db.Query<BookStats>("SELECT * FROM BookStats").Single();
        }

        public List<Book> SearchFullText(string terms)
        {
            return this.db.Query<Book>(
                        "SELECT * FROM [Book] WHERE CONTAINS((Summary, Title),@Terms)", new {Terms = terms})
                        .ToList();
        }

        public List<SemanticBook> SemanticSearch(string terms)
        {
            return this.db.Query<SemanticBook>(
                        @"SELECT TOP (25) score, b.*
                            FROM Book as b
                                INNER JOIN SEMANTICKEYPHRASETABLE
                                (
                                Book,
                                (title, summary)
                                ) AS KEYP_TBL
                            ON b.Id = KEYP_TBL.document_key
                            WHERE KEYP_TBL.keyphrase = @SearchFor
                            ORDER BY KEYP_TBL.Score DESC;", new { SearchFor = terms })
                        .ToList();
        }
    }
}
