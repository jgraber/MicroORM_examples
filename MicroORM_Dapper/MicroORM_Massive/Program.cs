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
        }

        private static void ReadData()
        {
            dynamic books = new Book().All();
            foreach (var book in books)
            {
                Console.WriteLine(String.Format("[{0}] {1} - {2}, {3}",
                    book.Id, new string(((string) book.Title).Take(30).ToArray()), book.ISBN, book.Pages));
            }
        }
    }

    public class Book : DynamicModel
    {
        //you don't have to specify the connection - Massive will use the first one it finds in your config
        public Book() : base("OrmConnection", "Book", "Id") { }
    }
}
