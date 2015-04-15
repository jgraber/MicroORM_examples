using System;
using MicroORM_PetaPoco.Data;

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

            OneToNRelation();
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

        private static void OneToNRelation()
        {

            // Create and save a publisher
            var publisher = new Publisher()
            {
                Name = "The Pragmatic Programmers, LLC PetaPoco edition",
                EMail = "support@pragmaticprogrammer.com",
                Url = "https://pragprog.com/"
            };
            _repository.Add(publisher);
            Console.WriteLine(publisher);

            // Create and save a book with a publisher
            var book = new Book()
            {
                Title = "Pragmatic Thinking & Learning in PetaPoco",
                Publisher = publisher
            };

            _repository.Add(book);

            // List all books of the publisher
            var books = _repository.GetBooksByPublisher(publisher);
            foreach (var currentBook in books)
            {
                Console.WriteLine("{0} is published by {1}", currentBook, currentBook.Publisher);
            }

        }
    }
}
