using System;

namespace MicroORM_Dapper
{
    public class BookStats
    {
        public int BookCount { get; set; }
        public int TotalPages { get; set; }
        public decimal AverageRating { get; set; }

        public override string ToString()
        {
            return String.Format("#Books: {0}, total pages: {1}, average rating: {2}", BookCount, TotalPages, AverageRating);
        }
    }
}
