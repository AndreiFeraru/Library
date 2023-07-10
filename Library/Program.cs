using Library.Models;

namespace Library
{
    internal class Program
    {
        public static LibraryCatalog MyCatalog;

        static void Main(string[] args)
        {
            MyCatalog = new LibraryCatalog { Items = new List<LibraryItem>() };

            Console.WriteLine("Welcome to the library management system!");

            var inputCommand = string.Empty;
            while (inputCommand != "0")
            {
                Console.WriteLine(
                    "\n-----" +
                    "\nCommands:" +
                    "\n  1 - Add book" +
                    "\n  2 - List books in library" +
                    "\n  3 - Get book number of copies" +
                    "\n  4 - Borrow book" +
                    "\n  5 - Return book" +
                    "\n  0 - Exit" +
                    "\n-----\n");

                inputCommand = Console.ReadLine()?.Trim();
                switch (inputCommand)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        ListBooks();
                        break;
                    case "3":
                        GetNumberOfCopies();
                        break;
                    case "4":
                        BorrowBook();
                        break;
                    case "5":
                        ReturnBook();
                        break;
                }
            }
        }

        private static void AddBook()
        {
            var inputTitle = string.Empty;
            while (string.IsNullOrEmpty(inputTitle))
            {
                Console.WriteLine("Please provide the book title:");
                inputTitle = Console.ReadLine()?.Trim();
            }

            var inputISBN = string.Empty;
            while (string.IsNullOrEmpty(inputISBN))
            {
                Console.WriteLine("Please provide the book ISBN:");
                inputISBN = Console.ReadLine()?.Trim();
            }

            var inputPrice = 0.0;
            while (inputPrice == 0.0)
            {
                Console.WriteLine("Please provide the book borrow price:");
                double.TryParse(Console.ReadLine()?.Trim(), out inputPrice);
            }

            MyCatalog.Items.Add(new Book(inputTitle, inputISBN, MyCatalog.GetMaxID() + 1, inputPrice));
            Console.WriteLine("Book successfully added!\n");
        }

        private static void ListBooks()
        {
            Console.WriteLine("Books in library:");
            foreach (var book in MyCatalog.GetAllBooks().Where(b => b.IsBorrowed() == false))
            {
                Console.WriteLine(
                    $"{book.ID} | " +
                    $"{book.Title} | " +
                    $"{book.ISBN} | " +
                    $"{book.Author ?? string.Empty} | " +
                    $"{book.PublishDate.ToString() ?? string.Empty} | " +
                    $"{book.Type.ToString() ?? string.Empty}");
            }
        }

        private static void GetNumberOfCopies()
        {
            var inputTitle = string.Empty;
            while (string.IsNullOrEmpty(inputTitle))
            {
                Console.WriteLine("Please provide the book title:");
                inputTitle = Console.ReadLine()?.Trim();
            }
            var noOfCopies = MyCatalog.GetAllBooks()
                .Where(x => x.Title.Equals(inputTitle, StringComparison.OrdinalIgnoreCase))
                .Count();
            Console.WriteLine($"Number of copies, including borrowed for '{inputTitle}' is {noOfCopies}");
        }

        private static void BorrowBook()
        {
            var inputTitle = string.Empty;
            while (string.IsNullOrEmpty(inputTitle))
            {
                Console.WriteLine("Please provide the book title:");
                inputTitle = Console.ReadLine()?.Trim();
            }

            var booksByName = MyCatalog.GetAllBooks()
                .Where(b => b.Title.Equals(inputTitle, StringComparison.OrdinalIgnoreCase) && b.IsBorrowed() == false);

            if (!booksByName.Any())
            {
                Console.WriteLine($"There are no available copies of the book '{inputTitle}'");
                return;
            }

            var copyToBorrow = booksByName.First();
            copyToBorrow.Borrow();
            Console.WriteLine($"Book '{inputTitle}' with id {copyToBorrow.ID} has been borrowed.");

        }

        private static void ReturnBook()
        {
            var inputID = -1;
            while (inputID == -1)
            {
                Console.WriteLine("Please provide the book ID:");
                int.TryParse(Console.ReadLine()?.Trim(), out inputID);
            }

            var booksByID = MyCatalog.GetAllBooks()
                .Where(b => b.ID == inputID && b.IsBorrowed() == true);

            if (!booksByID.Any())
            {
                Console.WriteLine($"Book with ID equal to '{inputID}' is not borrowed or does not exist.");
                return;
            }

            var copyToReturn = booksByID.First();
            copyToReturn.ReturnAndGetPrice();
        }
    }
}