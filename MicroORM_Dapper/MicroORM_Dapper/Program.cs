using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static SqlConnection GetOpenConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString;
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
