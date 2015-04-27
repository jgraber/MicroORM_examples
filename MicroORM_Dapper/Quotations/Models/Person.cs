using System;
using System.Collections.Generic;

namespace Quotations.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? Born { get; set; }
        public DateTime? Died { get; set; }
        public List<Quote> Quotes { get; set; }

        public Person()
        {
            Quotes = new List<Quote>();
        }        
    }
}