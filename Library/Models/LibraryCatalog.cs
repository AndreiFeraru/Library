namespace Library.Models
{
    internal class LibraryCatalog
    {
        public required List<LibraryItem> Items { get; set; }

        public int GetMaxID() => Items.Any() ? Items.Max(i => i.ID) : 0;

        public IEnumerable<Book> GetAllBooks()
        {
            return Items.Where(x => x is Book).Cast<Book>();
        }
    }
}
