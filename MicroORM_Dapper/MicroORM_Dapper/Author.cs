using System;

namespace MicroORM_Dapper
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Twitter { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] {1} {2}", Id, FirstName, LastName);
        }
    }
}
