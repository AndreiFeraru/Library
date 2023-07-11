using Library.Models;

namespace Library
{
    public class Library
    {
        private readonly LibraryCatalog _catalog;

        public Library(LibraryCatalog catalog)
        {
            _catalog = catalog;
        }

        public bool AddBook(string title, string isbn, double price, string author = null, DateOnly? publishDate = null, BookType? bookType = null)
        {
            var oldCount = _catalog.Items.Count;
            _catalog.Items.Add(new Book(title, isbn, _catalog.GetMaxID() + 1, price)
            {
                Author = author,
                PublishDate = publishDate,
                Type = bookType
            });
            return _catalog.Items.Count - oldCount == 1;
        }

        public IEnumerable<Book> GetNonBorrowedBooks()
            => _catalog.GetAllBooks().Where(b => b.IsBorrowed() == false);

        public int GetNumberOfCopiesByTitle(string title)
            => _catalog.GetAllBooks()
                .Where(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                .Count();

        public int BorrowBookByTitle(string title)
        {
            var availableBooksByTitle = _catalog.GetAllBooks()
                .Where(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && b.IsBorrowed() == false);

            if (!availableBooksByTitle.Any())
                throw new Exception($"Error: There are no available copies of the book '{title}'");

            var copyToBorrow = availableBooksByTitle.First();
            copyToBorrow.Borrow();
            return copyToBorrow.ID;
        }

        public double ReturnBookById(int id)
        {
            var booksByID = _catalog.GetAllBooks()
                .Where(b => b.ID == id && b.IsBorrowed() == true);

            if (!booksByID.Any())
                throw new Exception($"Error: Book with ID equal to '{id}' is not borrowed or does not exist.");

            var copyToReturn = booksByID.First();
            return copyToReturn.ReturnAndGetPrice();
        }
    }
}
