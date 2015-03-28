﻿using System.Collections.Generic;

namespace MicroORM_Dapper.Data
{
    public interface IBookRepository
    {
        Book FindBook(int id);
        List<Book> GetAllBooks();
        Book Add(Book book);
        Book Update(Book book);
        void RemoveBook(int id);

        Book GetLatestBook();
    }
}