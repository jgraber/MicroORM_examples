using System;
using System.Linq;
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

            OneToNRelation();

            NToMRelation();

            CompleteBook();
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

        private static void NToMRelation()
        {
            using (var database = new Database("OrmConnection"))
            {
                var author = new Author() {LastName = "Peta", FirstName = "Poco"};
                _repository.Add(author);

                var book = new Book() {Title = "One with an Author in PetaPoco"};

                book.Authors.Add(author);

                Console.WriteLine("New Author Id: {0}", author.Id);
                _repository.Add(book);


                var authorId =
                    database.Query<int>("SELECT authorId FROM BookAuthor WHERE BookId = @Id", book).SingleOrDefault();
                var bookAuthor = _repository.FindAuthor(authorId);
                Console.WriteLine("Author of the book 'One with an Author': {0}", bookAuthor);

                
            }
        }

        private static void CompleteBook()
        {
            var author = new Author() { LastName = "Author", FirstName = "The" };
            _repository.Add(author);

            var publisher = new Publisher() {Name = "Publisher", EMail = "ThePublisher@a.com", Url = "http//www.a.com"};
            _repository.Add(publisher);

            var book = new Book() { Title = "One with an Author in PetaPoco" };
            book.Authors.Add(author);
            book.Publisher = publisher;

            Console.WriteLine("New Author Id: {0}", author.Id);
            _repository.Add(book);

            var completeBook = _repository.FindBook(book.Id);
            Console.WriteLine("All together in {0}", completeBook);
            Console.WriteLine("Book was written by {0}", completeBook.Authors.First());
            Console.WriteLine("Book was published by {0}", completeBook.Publisher);
        }
    }
}
