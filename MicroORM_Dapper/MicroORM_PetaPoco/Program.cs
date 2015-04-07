using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroORM_Dapper;
using PetaPoco;

namespace MicroORM_PetaPoco
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Get all books");
            var database = new Database("OrmConnection");

            var books = database.Query<Book>("SELECT * FROM BOOK;");

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }

        }
    }
}
