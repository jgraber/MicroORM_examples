using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

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
        public Image Cover { get; set; }
        public List<Author> Authors { get; set; }
        public ReadingStatus ReadingStatus { get; set; }

        public Book()
        {
            Authors = new List<Author>();
        }

        public int? PublisherId
        {
            get { return Publisher != null ? (int?) Publisher.Id : null; }
        }

        public byte[] CoverAsBytes()
        {
            MemoryStream ms = new MemoryStream();
            if (Cover != null)
            {
                Cover.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
           
            return ms.ToArray();
        }


        public override string ToString()
        {
            return String.Format("[{0}] {1} - {2}, {3}",
                Id, new string(Title.Take(30).ToArray()), ISBN, Pages);
        }
    }
}
