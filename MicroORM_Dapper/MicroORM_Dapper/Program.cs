﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Drawing;
using System.IO;
using Dapper;
using MicroORM_Dapper.Data;

namespace MicroORM_Dapper
{
    class Program
    {
        private static IRepository _repository;
        static void Main(string[] args)
        {
            _repository = new DapperRepository();

            ReadData();
            WriteData();
            UpdateData();
            DeleteData();

            ReadFromView();

            OneToNRelation();

            OneToOneRelation();

            NToMRelation();

            FullTextSearch();

            SemanticSearch();

            WorkingWithEnums();
        }

        private static void ReadData()
        {
            var books = _repository.GetAllBooks();
            foreach (var currentBook in books)
            {
                Console.WriteLine(currentBook);
            }
        }

        private static void WriteData()
        {
            Book book = new Book() {Title = "A New Book", Summary = "Not much", Pages = 100, Rating = 3 };

            Book persisted = _repository.Add(book);
            
            Console.WriteLine(persisted);
        }

        private static void UpdateData()
        {
            var book = _repository.GetLatestBook();
            book.Title = "An Updated Title";

            var result = _repository.Update(book);

            Console.WriteLine(book);
        }

        private static void DeleteData()
        {
            var newestBook = _repository.GetLatestBook();
            _repository.RemoveBook(newestBook.Id);

            var result = _repository.FindBook(newestBook.Id);
            Console.WriteLine("Book with id {0} still exists? {1}", newestBook.Id, result != null);
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
                Name = "The Pragmatic Programmers, LLC",
                EMail = "support@pragmaticprogrammer.com",
                Url = "https://pragprog.com/"
            };
            _repository.Add(publisher);
            Console.WriteLine(publisher);

            // Create and save a book with a publisher
            var book = new Book()
            {
                Title = "Pragmatic Thinking & Learning",
                Publisher = publisher
            };

            _repository.Add(book);

            // List all books of the publisher
            var books = _repository.GetBooksByPublisher(publisher);
            foreach (var currentBook in books)
            {
                Console.WriteLine(currentBook);
            }

        }

        private static void OneToOneRelation()
        {
            var cover = Image.FromFile(".\\Images\\cover_prag_think_learn.jpg");
            var book = new Book() {Title = "One with a Cover"};
            book.Cover = cover;
            _repository.Add(book);

            using (var connection = Program.GetOpenConnection())
            {
                var result = connection.Query<int>("SELECT 1 FROM Cover WHERE BookId = @Id", book).SingleOrDefault();
              
                Console.WriteLine("Was the cover found? {0}", result);

                var bookWithCover = _repository.FindBook(book.Id);
                FileStream fs = new FileStream(bookWithCover.Id+".png",FileMode.Create);
                bookWithCover.Cover.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                fs.Close();
            }


        }

        private static void NToMRelation()
        {
            using (var connection = Program.GetOpenConnection())
            {
                var author = new Author() {LastName = "Hunt", FirstName = "Andy"};
                _repository.Add(author);

                var book = new Book() {Title = "One with an Author"};
                book.Authors.Add(author);

                Console.WriteLine("New Author Id: {0}", author.Id);
                _repository.Add(book);


                var authorId =
                    connection.Query<int>("SELECT authorId FROM BookAuthor WHERE BookId = @Id", book).SingleOrDefault();
                var bookAuthor = _repository.FindAuthor(authorId);
                Console.WriteLine("Author of the book 'One with an Author': {0}", bookAuthor);
            }


        }

        private static void FullTextSearch()
        {
            var books = _repository.SearchFullText("\"Ruby on Rails\" or math or Rails");

            Console.WriteLine("Results from the fulltext search:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }

        }

        private static void SemanticSearch()
        {
            //Get Top 25 results where "Software" is in title or summary, order by score
            var books = _repository.SemanticSearch("Software");

            Console.WriteLine("Results from the semantic search:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        private static void WorkingWithEnums()
        {
            var book = new Book() {Title = "Enums are fun", ReadingStatus = ReadingStatus.WantToRead};
            var saved = _repository.Add(book);

            var fromDb = _repository.FindBook(saved.Id);
            Console.WriteLine("Enum of saved book is: {0}", fromDb.ReadingStatus);
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
