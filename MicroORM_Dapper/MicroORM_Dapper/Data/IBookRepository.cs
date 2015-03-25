using System.Collections.Generic;

namespace MicroORM_Dapper.Data
{
    public interface IBookRepository
    {
        Book Find(int id);
        List<Book> GetAll();
        Book Add(Book book);
        Book Update(Book book);
        void Remove(int id);

        Book GetLatest();
    }
}
