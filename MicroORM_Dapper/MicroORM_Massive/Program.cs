using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroORM_Massive.Massive;

namespace MicroORM_Massive
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadData();
            WriteData();
            UpdateData();
            DeleteData();

            ReadFromView();
        }

        private static void ReadData()
        {
            dynamic books = new Book().All();
            foreach (var book in books)
            {
                Console.WriteLine(FormatBook(book));
            }
        }

        private static void WriteData()
        {
            dynamic bookTable = new Book();
            var book =
                new
                {
                    Title = "A Book about Massive",
                    ISBN = "1234",
                    Summary = "A basic example on how to make an INSERT statement with Massive micro ORM",
                    Pages = 234,
                    Rating = 3.5
                };
            var newID = bookTable.Insert(book);
            Console.WriteLine("New Id: " + newID.ID);
            var newBook = bookTable.First(Id:newID.ID);

            Console.WriteLine("The new book is: {0}", FormatBook(newBook));
        }

        private static void UpdateData()
        {
            dynamic bookTable = new Book();
            var book = bookTable.First(OrderBy:"Id DESC");
            Console.WriteLine("The old book: {0}", FormatBook(book));
            book.Title = "A new Title for an existing book";

            bookTable.Update(book);

            var newBook = bookTable.First(Id:book.Id);
            Console.WriteLine("The updated book: {0}", FormatBook(newBook));
        }

        private static void DeleteData()
        {
            dynamic bookTable = new Book();
            var book = bookTable.First(OrderBy: "Id DESC");
            Console.WriteLine("The old book: {0}", FormatBook(book));
            

            bookTable.Delete(book);

            var newBook = bookTable.First(Id: book.Id);

            Console.WriteLine("The book could not be found? {0}", newBook == null);
        }

        private static void ReadFromView()
        {
            var tbl = new DynamicModel("OrmConnection", tableName: "BookStats");
            dynamic stats = tbl.Query("SELECT * FROM BookStats").First();
            Console.WriteLine(String.Format("#Books: {0}, total pages: {1}, average rating: {2}", stats.BookCount, stats.TotalPages, stats.AverageRating));
        }
        
       

        private static dynamic FormatBook(dynamic book)
        {
            return String.Format("[{0}] {1} - {2}, {3}",
                book.Id, new string(((string)book.Title).Take(30).ToArray()), book.ISBN, book.Pages);
        }

    }

    public class Book : DynamicModel
    {
        //you don't have to specify the connection - Massive will use the first one it finds in your config
        public Book() : base("OrmConnection", "Book", "Id") { }
    }
}
