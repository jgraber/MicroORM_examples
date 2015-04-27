using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Quotations.Models;
using System.Transactions;
using Dapper;

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
                                    (@Text, @Year, @Context, @PersonId)";
            int id = _db.Query<int>(insertQuote, 
                new {quote.Text, quote.Year, quote.Context, PersonId = quote.Author.Id}).Single();
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
    }
}