using System;
using System.Linq;

namespace MicroORM_Dapper
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Summary { get; set; }
        public decimal Rating { get; set; }
        public Publisher Publisher { get; set; }

        public int? PublisherId
        {
            get { return Publisher != null ? (int?) Publisher.Id : null; }
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1} - {2}, {3}",
                Id, new string(Title.Take(30).ToArray()), ISBN, Pages);
        }
    }
}
