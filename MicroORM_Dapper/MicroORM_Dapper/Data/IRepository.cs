using System.Collections.Generic;

namespace MicroORM_Dapper.Data
{
    public interface IRepository
    {
        Book FindBook(int id);
        List<Book> GetAllBooks();
        Book Add(Book book);
        Book Update(Book book);
        void RemoveBook(int id);
        Book GetLatestBook();

        Author Add(Author author);
        Author FindAuthor(int id);

        Publisher Add(Publisher publisher);
        Publisher FindPublisher(int id);
    }
}
