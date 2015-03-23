using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace MicroORM_Dapper.Data
{
    public class DapperBookRepository : IBookRepository
    {
        private IDbConnection db = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["OrmConnection"].ConnectionString);


        public Book Find(int id)
        {
            return this.db.Query<Book>("SELECT * FROM Book WHERE Id = @Id;", new {Id = id}).SingleOrDefault();
        }

        public List<Book> GetAll()
        {
            return this.db.Query<Book>("SELECT * FROM Book;").ToList(); 
        }

        public Book Add(Book book)
        {
            string sql = @"INSERT INTO Book (Title, Pages, ISBN, Summary, Rating) 
                                VALUES (@Title, @Pages, @ISBN, @Summary, @Rating);
                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            int id =  this.db.Query<int>(sql, book).Single();
            book.Id = id;
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
    }
}
