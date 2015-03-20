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

        private static SqlConnection GetOpenConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString;
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
