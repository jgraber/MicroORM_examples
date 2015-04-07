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
            ReadData();
            WriteData();
            UpdateData();
        }

        private static void ReadData()
        {
            Console.WriteLine("Get all books");
            var database = new Database("OrmConnection");

            var books = database.Query<Book>("SELECT * FROM BOOK;");

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        private static void WriteData()
        {
            Book book = new Book() { Title = "PetaPoco - The Book", Summary = "Another Micro ORM", Pages = 200, Rating = 5 };

            var database = new Database("OrmConnection");
            database.Insert("Book", "Id", book);

            Console.WriteLine("Inserted book with PetaPoco:");
            Console.WriteLine(book);
        }

        private static void UpdateData()
        {
            var database = new Database("OrmConnection");
            var book = database.Query<Book>("SELECT TOP 1 * FROM Book ORDER BY Id desc").Single();

            book.Title = "An Updated Title for PetaPoco";

            database.Update("Book", "Id", book);

            var updatedBook = database.Query<Book>("SELECT * FROM Book WHERE Id = @0", book.Id).Single();

            Console.WriteLine("The updated book with PetaPoco:");
            Console.WriteLine(updatedBook);
        }
    }
}
