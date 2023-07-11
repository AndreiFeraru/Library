using Library.Models;

namespace Library
{
    internal class Program
    {
        private static Library _myLibrary = new(new LibraryCatalog(new List<LibraryItem>()));

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the library management system!");

            var inputCommand = string.Empty;
            while (inputCommand != "0")
            {
                ShowCommands();
                inputCommand = Console.ReadLine()?.Trim();
                switch (inputCommand)
                {
                    case "1":
                        AddBookCommand();
                        break;
                    case "2":
                        ListBooksCommand();
                        break;
                    case "3":
                        GetNumberOfCopiesCommand();
                        break;
                    case "4":
                        BorrowBookCommand();
                        break;
                    case "5":
                        ReturnBookCommand();
                        break;
                }
            }
        }

        private static void ShowCommands()
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
        }

        private static void AddBookCommand()
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

            Console.WriteLine("Please provide the book author: (Press enter to skip)");
            string inputAuthor = Console.ReadLine()?.Trim();

            Console.WriteLine("Please provide the book publish date: (YYYY/MM/DD format, Press enter to skip)");
            string inputPublishDate = Console.ReadLine()?.Trim();
            DateOnly? publishDate;
            try
            {
                publishDate = DateOnly.Parse(inputPublishDate);
            }
            catch
            {
                publishDate = null;
            }

            Console.WriteLine("Please provide the book type: (Press enter to skip)\n" +
                "  0 - PaperBack\n" +
                "  1 - HardCover\n" +
                "  2 - Ebook");
            string inputBookType = Console.ReadLine()?.Trim();
            BookType? bookType;
            try
            {
                bookType = (BookType)int.Parse(inputBookType);
            }
            catch
            {
                bookType = null;
            }

            var success = _myLibrary.AddBook(inputTitle, inputISBN, inputPrice, inputAuthor, publishDate, bookType);
            if (success)
                Console.WriteLine("Book successfully added!\n");
            else
                Console.WriteLine("Book successfully added!\n");
        }

        private static void ListBooksCommand()
        {
            Console.WriteLine("Books in library:");
            Console.WriteLine("ID | Title | ISBN | Price | Author | Publish date | Book type");
            foreach (var book in _myLibrary.GetNonBorrowedBooks())
            {
                Console.WriteLine(
                    $"{book.ID} | " +
                    $"{book.Title} | " +
                    $"{book.ISBN} | " +
                    $"{book.Price} | " +
                    $"{book.Author ?? string.Empty} | " +
                    $"{book.PublishDate.ToString() ?? string.Empty} | " +
                    $"{book.Type.ToString() ?? string.Empty}");
            }
        }

        private static void GetNumberOfCopiesCommand()
        {
            var inputTitle = string.Empty;
            while (string.IsNullOrEmpty(inputTitle))
            {
                Console.WriteLine("Please provide the book title:");
                inputTitle = Console.ReadLine()?.Trim();
            }
            var noOfCopies = _myLibrary.GetNumberOfCopiesByTitle(inputTitle);
            Console.WriteLine($"Number of copies, including borrowed for '{inputTitle}' is {noOfCopies}");
        }

        private static void BorrowBookCommand()
        {
            var inputTitle = string.Empty;
            while (string.IsNullOrEmpty(inputTitle))
            {
                Console.WriteLine("Please provide the book title:");
                inputTitle = Console.ReadLine()?.Trim();
            }

            try
            {
                var idOfBorrowedCopy = _myLibrary.BorrowBookByTitle(inputTitle);
                Console.WriteLine($"Book '{inputTitle}' with id {idOfBorrowedCopy} has been borrowed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ReturnBookCommand()
        {
            var inputId = -1;
            while (inputId == -1)
            {
                Console.WriteLine("Please provide the book ID:");
                int.TryParse(Console.ReadLine()?.Trim(), out inputId);
            }

            try
            {
                var price = _myLibrary.ReturnBookById(inputId);
                Console.WriteLine($"Book successfully returned. Price to pay is '{price}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}