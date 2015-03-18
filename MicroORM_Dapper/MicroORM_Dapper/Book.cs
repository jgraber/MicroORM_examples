﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override string ToString()
        {
            return String.Format("[{0}] {1} - {2}, {3}",
                Id, new string(Title.Take(30).ToArray()), ISBN, Pages);
        }
    }
}
