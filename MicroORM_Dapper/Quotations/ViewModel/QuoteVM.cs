using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quotations.ViewModel
{
    public class QuoteVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Context { get; set; }
        public string Year { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}