using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Quotations.Models;
using System.Transactions;
using Dapper;
using Quotations.ViewModel;

namespace Quotations.DataAccess
{
    public class DapperRepository
    {
        private readonly IDbConnection _db = new SqlConnection(
                 ConfigurationManager.ConnectionStrings["QuoteDB"].ConnectionString);

        public List<Person> GetAllPersons()
        {
            return _db.Query<Person>("SELECT * FROM person;").ToList();
        }

        public Person Add(Person person)
        {
            using (var transaction = new TransactionScope())
            {
                var insertPerson = @"INSERT INTO [Person]
                                        ([LastName], [FirstName], [Born], [Died])
                                        VALUES
                                        (@LastName, @FirstName, @Born, @Died);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                int id = _db.Query<int>(insertPerson, person).Single();
                person.Id = id;

                if (person.Quotes.Count > 0)
                {
                    foreach (var quote in person.Quotes)
                    {
                        quote.Author = person;
                        Add(quote);
                    }
                }

                transaction.Complete();
            }

            return person;
        }

        public Quote Add(Quote quote)
        {
            var insertQuote = @"INSERT INTO [dbo].[Quote]
                                    ([Text], [Year], [Context], [PersonId])
                                VALUES
                                    (@Text, @Year, @Context, @PersonId);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            int id = _db.Query<int>(insertQuote, 
                new {quote.Text, quote.Year, quote.Context, PersonId = quote.AuthorId}).Single();
            quote.Id = id;

            return quote;
        }

        public Person FindPerson(int id)
        {
            var person = _db.Query<Person>("SELECT * FROM Person WHERE Id = @Id;", new { Id = id }).SingleOrDefault();
            return person;
        }

        public Person Update(Person person)
        {
            string sql = @"UPDATE [dbo].[Person]
                        SET [LastName] = @LastName, [FirstName] = @FirstName, 
                            [Born] = @Born, [Died] = @Died
                        WHERE Id = @Id;";
            _db.Execute(sql, person);
         
            return person;
        }

        public void DeletePerson(int id)
        {
            string sql = @"DELETE FROM Person WHERE Id = @Id;";
            _db.Execute(sql, new { Id = id });
        }

        public List<Quote> GetAllQuotes()
        {
            return _db.Query<Quote>("SELECT * FROM Quote;").ToList();
        }

        public Quote FindQuote(int id)
        {
            return _db.Query<Quote>("SELECT * FROM Quote WHERE Id = @Id;", new { Id = id }).SingleOrDefault();
        }

        public void DeleteQuote(int id)
        {
            string sql = @"DELETE FROM Quote WHERE Id = @Id;";
            _db.Execute(sql, new { Id = id });
        }

        public Quote Update(Quote quote)
        {
            string sql = @"UPDATE [dbo].[Quote]
                           SET [Text] = @Text, [Year] = @Year, [Context] = @Context, 
                               [PersonId] = @PersonId
                           WHERE Id = @Id;";
            _db.Execute(sql, new {quote.Id, quote.Text, quote.Year, quote.Context, PersonId = quote.AuthorId });

            return quote;
        }

        public QuoteVM GetRandomQuote()
        {
            string sql = @"SELECT TOP 1 q.*, p.LastName, p.FirstName
                          FROM [Quote] q
                          INNER JOIN Person p ON q.PersonId = p.Id
                          ORDER BY NEWID();";
            return _db.Query<QuoteVM>(sql).FirstOrDefault();
        }
    }
}