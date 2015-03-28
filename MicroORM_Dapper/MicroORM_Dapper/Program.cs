using System;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Drawing;
using System.IO;
using Dapper;
using MicroORM_Dapper.Data;

namespace MicroORM_Dapper
{
    class Program
    {
        private static IBookRepository _repository;
        static void Main(string[] args)
        {
            _repository = new DapperBookRepository();

            ReadData();
            WriteData();
            UpdateData();
            DeleteData();

            AggregateFunctions();

            ReadFromView();

            OneToNRelation();

            OneToOneRelation();

            NToMRelation();
        }

        private static void ReadData()
        {
            var books = _repository.GetAllBooks();
            foreach (var currentBook in books)
            {
                Console.WriteLine(currentBook);
            }
        }

        private static void WriteData()
        {
            Book book = new Book() {Title = "A New Book", Summary = "Not much", Pages = 100, Rating = 3 };

            Book persisted = _repository.Add(book);
            
            Console.WriteLine(persisted);
        }

        private static void UpdateData()
        {
            var book = _repository.GetLatestBook();
            book.Title = "An Updated Title";

            var result = _repository.Update(book);

            Console.WriteLine(book);
        }

        private static void DeleteData()
        {
            var newestBook = _repository.GetLatestBook();
            _repository.RemoveBook(newestBook.Id);

            var result = _repository.FindBook(newestBook.Id);
            Console.WriteLine("Book with id {0} still exists? {1}", newestBook.Id, result != null);
        }

        private static void AggregateFunctions()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var book = connection.Query<BookStats>("SELECT count(*) as 'BookCount', sum(Pages) as 'TotalPages', avg(Rating) as 'AverageRating' FROM Book;").Single();
              
                Console.WriteLine(book);
            }
        }

        private static void ReadFromView()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var bookstats = connection.Query<BookStats>("SELECT * FROM BookStats").Single();

                Console.WriteLine(bookstats);
            }
        }

        private static void OneToNRelation()
        {
            using (var connection = Program.GetOpenConnection())
            {
                // Create and save a publisher
                var publisher = new Publisher()
                {
                    Name = "The Pragmatic Programmers, LLC",
                    EMail = "support@pragmaticprogrammer.com",
                    Url = "https://pragprog.com/"
                };
                StorePublisher(publisher, connection);
                Console.WriteLine(publisher);

                // Create and save a book with a publisher
                var book = new Book()
                {
                    Title = "Pragmatic Thinking & Learning", 
                    Publisher = publisher
                };

                _repository.Add(book);

                // List all books of the publisher
                var books = connection.Query<Book>("SELECT * FROM Book WHERE PublisherId = @Id", publisher).ToList();
                foreach (var currentBook in books)
                {
                    Console.WriteLine(currentBook);
                }
            }
        }

        private static void OneToOneRelation()
        {
            var cover = Image.FromFile(".\\Images\\cover_prag_think_learn.jpg");
            var book = new Book() {Title = "One with a Cover"};
            book.Cover = cover;
            _repository.Add(book);

            using (var connection = Program.GetOpenConnection())
            {
                var result = connection.Query<int>("SELECT 1 FROM Cover WHERE BookId = @Id", book).SingleOrDefault();
              
                Console.WriteLine("Was the cover found? {0}", result);

                var bookWithCover = _repository.FindBook(book.Id);
                FileStream fs = new FileStream(bookWithCover.Id+".png",FileMode.Create);
                bookWithCover.Cover.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                fs.Close();
            }


        }

        private static void NToMRelation()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var author = new Author() {LastName = "Hunt", FirstName = "Andy"};
                StoreAuthor(author, connection);

                var book = new Book() {Title = "One with an Author"};
                book.Authors.Add(author);

                _repository.Add(book);


                var authorId =
                    connection.Query<int>("SELECT authorId FROM BookAuthor WHERE BookId = @Id", book).SingleOrDefault();

                Console.WriteLine("Author of the book 'One with an Author': {0}", authorId);
            }


        }

        private static void StorePublisher(Publisher publisher, SqlConnection connection)
        {
            var insert = @"INSERT INTO Publisher (Name, Url, EMail)
                                          VALUES (@Name, @Url, @EMail)
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";
            int id = connection.Query<int>(insert, publisher).Single();
            publisher.Id = id;
        }

        private static void StoreAuthor(Author author, SqlConnection connection)
        {
            var insertAuthor = @"INSERT INTO Author ([FirstName],[LastName],[EMail],[Web],[Twitter])
                           VALUES (@FirstName, @LastName, @EMail, @Web, @Twitter)
                           SELECT CAST(SCOPE_IDENTITY() AS INT)";
            int id = connection.Query<int>(insertAuthor, author).Single();
            author.Id = id;
        }

        private static SqlConnection GetOpenConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString;
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
