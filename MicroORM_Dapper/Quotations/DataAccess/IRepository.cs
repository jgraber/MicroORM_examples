using System.Collections.Generic;
using Quotations.Models;
using Quotations.ViewModel;

namespace Quotations.DataAccess
{
    public interface IRepository
    {
        List<Person> GetAllPersons();
        Person Add(Person person);
        Quote Add(Quote quote);
        Person FindPerson(int id);
        Person Update(Person person);
        void DeletePerson(int id);
        List<Quote> GetAllQuotes();
        Quote FindQuote(int id);
        void DeleteQuote(int id);
        Quote Update(Quote quote);
        QuoteVM GetRandomQuote();
    }
}