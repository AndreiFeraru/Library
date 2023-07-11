namespace Library.Models
{
    public class Book : LibraryItem
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string? Author { get; set; }
        public DateOnly? PublishDate { get; set; }
        public BookType? Type { get; set; }

        public Book(string title, string iSBN, int id, double price)
            : base(id, price)
        {
            Title = title;
            ISBN = iSBN;
        }
    }

    public enum BookType
    {
        PaperBack,
        HardCover,
        Ebook
    }
}
