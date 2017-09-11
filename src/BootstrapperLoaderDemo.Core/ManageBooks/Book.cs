namespace BootstrapperLoaderDemo.Core.ManageBooks
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Publisher { get; private set; }

        public Book(string title, string author, string publisher)
        {
            Title = title;
            Author = author;
            Publisher = publisher;
        }

        //For EF
        private Book() { }
    }
}
