using System;

namespace MicroORM_Dapper
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string EMail { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] {1} ({2})", Id, Name, EMail);
        }
    }
}
