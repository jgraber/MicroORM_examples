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

        private static SqlConnection GetOpenConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString;
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
    }
}
