namespace Library.Models
{
    public class LibraryCatalog
    {
        public List<LibraryItem> Items { get; set; }

        public LibraryCatalog(IEnumerable<LibraryItem> items)
        {
            Items = new List<LibraryItem>(items);
        }

        public int GetMaxID() => Items.Any() ? Items.Max(i => i.ID) : Constants.EMPTY_CATALOG_ID;

        public IEnumerable<Book> GetAllBooks()
        {
            return Items.Where(x => x is Book).Cast<Book>();
        }
    }
}
