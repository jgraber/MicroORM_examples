namespace Quotations.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Context { get; set; }
        public Person Author { get; set; }
        public string Year { get; set; }
    }
}