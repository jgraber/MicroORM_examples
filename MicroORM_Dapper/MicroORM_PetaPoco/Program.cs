using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroORM_PetaPoco.Data;
using PetaPoco;

namespace MicroORM_PetaPoco
{
    class Program
    {
        private static IRepository _repository;

        static void Main(string[] args)
        {
            _repository = new PetaPocoRepository();

            ReadData();
            WriteData();
            UpdateData();
            DeleteData();

            ReadFromView();
        }

        private static void ReadData()
        {
            Console.WriteLine("Get all books");
            var books = _repository.GetAllBooks();

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        private static void WriteData()
        {
            Book book = new Book() { Title = "PetaPoco - The Book", Summary = "Another Micro ORM", Pages = 200, Rating = 5 };

            _repository.Add(book);

            Console.WriteLine("Inserted book with PetaPoco:");
            Console.WriteLine(book);
        }

        private static void UpdateData()
        {
            var book = _repository.GetLatestBook();
            book.Title = "An Updated Title for PetaPoco";

            _repository.Update(book);
            var updatedBook = _repository.FindBook(book.Id);

            Console.WriteLine("The updated book with PetaPoco:");
            Console.WriteLine(updatedBook);
        }

        private static void DeleteData()
        {
            var book = _repository.GetLatestBook();

            _repository.RemoveBook(book.Id);

            var result = _repository.FindBook(book.Id);

            Console.WriteLine("Book with id {0} still exists? {1}", book.Id, result != null);
        }

        private static void ReadFromView()
        {
            var bookstats = _repository.GetStatistics();
            Console.WriteLine(bookstats);
        }
    }
}
