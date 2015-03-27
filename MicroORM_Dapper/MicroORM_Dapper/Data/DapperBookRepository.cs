﻿using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;


namespace MicroORM_Dapper.Data
{
    public class DapperBookRepository : IBookRepository
    {
        private IDbConnection db = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString);


        public Book Find(int id)
        {
            var book = this.db.Query<Book>("SELECT * FROM Book WHERE Id = @Id;", new {Id = id}).SingleOrDefault();

            if (book != null)
            {
                var cover = this.db.Query<byte[]>("SELECT Cover FROM Cover WHERE BookId = @Id;", new {Id = id}).SingleOrDefault();
                if (cover != null)
                {
                    book.Cover = Image.FromStream(new MemoryStream(cover));
                }
            }
           
            return book;
        }

        public List<Book> GetAll()
        {
            return this.db.Query<Book>("SELECT * FROM Book;").ToList(); 
        }

        public Book Add(Book book)
        {
            string sql = @"INSERT INTO Book (Title, Pages, ISBN, Summary, Rating, PublisherId) 
                                VALUES (@Title, @Pages, @ISBN, @Summary, @Rating, @PublisherId);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            int id =  this.db.Query<int>(sql, book).Single();
            book.Id = id;

            if (book.Cover != null)
            {
                string insertCover = @"INSERT INTO Cover (BookId, Cover) Values (@Id, @Cover);";
                this.db.Execute(insertCover, new {Id = book.Id, Cover = book.CoverAsBytes()});
            }
            
            return book;
        }



        public Book Update(Book book)
        {
            string sql = @"UPDATE Book SET Title = @Title, Pages = @Pages, 
                            ISBN = @ISBN, Summary = @Summary, Rating = @Rating WHERE Id = @Id;";
            this.db.Execute(sql, book);
            return book;
        }

        public void Remove(int id)
        {
            string sql = @"DELETE FROM Book WHERE Id = @Id;";
            this.db.Execute(sql, new {Id = id});
        }

        public Book GetLatest()
        {
            return this.db.Query<Book>("SELECT TOP 1 * FROM Book ORDER By Id desc;").SingleOrDefault();
        }
    }
}
