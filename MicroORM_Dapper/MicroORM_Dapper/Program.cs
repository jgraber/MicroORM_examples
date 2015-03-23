using System;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using Dapper;

namespace MicroORM_Dapper
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadData();
            WriteData();
            UpdateData();
            DeleteData();

            AggregateFunctions();

            ReadFromView();

            OneToNRelation();
        }

        private static void ReadData()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var books = connection.Query<Book>("SELECT * FROM Book").ToList();
                foreach (var currentBook in books)
                {
                    Console.WriteLine(currentBook);
                }
            }
        }

        private static void WriteData()
        {
            Book book = new Book() {Title = "A New Book", Summary = "Not much", Pages = 100, Rating = 3 };

            using (var connection = Program.GetOpenConnection())
            {
                string sql = @"INSERT INTO Book (Title, Pages, ISBN, Summary, Rating) 
                                VALUES (@Title, @Pages, @ISBN, @Summary, @Rating);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

                int id = connection.Query<int>(sql, book).Single();
                book.Id = id;
            }

            Console.WriteLine(book);
        }

        private static void UpdateData()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var book = connection.Query<Book>("SELECT TOP 1 * FROM Book ORDER BY Id desc").Single();
                book.Title = "An Updated Title";

                string sql = @"UPDATE Book SET Title = @Title, Pages = @Pages, 
                                ISBN = @ISBN, Summary = @Summary, Rating = @Rating WHERE Id = @Id;";
                connection.Execute(sql, book);

                Console.WriteLine(book);
            }
        }

        private static void DeleteData()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var book = connection.Query<Book>("SELECT TOP 1 * FROM Book ORDER BY Id desc").Single();

                string sql = @"DELETE FROM Book WHERE Id = @Id;";
                connection.Execute(sql, book);
                Console.WriteLine("Book with Id {0} is removed.", book.Id);
            }
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

                var saveRelation = @"INSERT INTO Book (Title, Pages, ISBN, Summary, Rating, PublisherId) 
                                VALUES (@Title, @Pages, @ISBN, @Summary, @Rating, @PublisherId);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";
                int id = connection.Query<int>(saveRelation, book).Single();
                book.Id = id;

                // List all books of the publisher
                var books = connection.Query<Book>("SELECT * FROM Book WHERE PublisherId = @Id", publisher).ToList();
                foreach (var currentBook in books)
                {
                    Console.WriteLine(currentBook);
                }
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

        private static SqlConnection GetOpenConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString;
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
