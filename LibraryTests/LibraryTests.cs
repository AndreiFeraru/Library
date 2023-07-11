using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library.Models;

namespace Library.Tests
{
    [TestClass()]
    public class LibraryTests
    {
        [TestMethod()]
        public void AddBookTest()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            var title = "Atomic Habits";
            var isbn = "9791162540640";
            var price = 30;

            //Act
            var success = myLibrary.AddBook(title, isbn, price);

            //Assert
            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void GetNonBorrowedBooks1()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Morometii", "3726160340677", 25);
            myLibrary.AddBook("Morometii", "3726160340677", 25);

            //Act
            var books = myLibrary.GetNonBorrowedBooks();

            //Assert
            Assert.AreEqual(5, books.Count());
            Assert.AreEqual(3, books.Where(b => b.Title == "Atomic Habits").Count());
            Assert.AreEqual(2, books.Where(b => b.Title == "Morometii").Count());
        }

        [TestMethod()]
        public void GetNonBorrowedBooks2()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Morometii", "3726160340677", 25);
            myLibrary.AddBook("Morometii", "3726160340677", 25);
            myLibrary.BorrowBookByTitle("Atomic Habits");

            //Act
            var books = myLibrary.GetNonBorrowedBooks();

            //Assert
            Assert.AreEqual(4, books.Count());
            Assert.AreEqual(2, books.Where(b => b.Title == "Atomic Habits").Count());
            Assert.AreEqual(2, books.Where(b => b.Title == "Morometii").Count());
        }

        [TestMethod()]
        public void GetNumberOfCopiesByTitleTest()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);

            //Act
            var noOfCopies = myLibrary.GetNumberOfCopiesByTitle("Atomic Habits");

            //Assert
            Assert.AreEqual(3, noOfCopies);
        }

        [TestMethod()]
        public void BorrowBookByTitleTest1()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Morometii", "3726160340677", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);

            //Act
            var borrowedCopyId1 = myLibrary.BorrowBookByTitle("Atomic Habits");
            var borrowedCopyId2 = myLibrary.BorrowBookByTitle("Atomic Habits");
            var borrowedCopyId3 = myLibrary.BorrowBookByTitle("Atomic Habits");

            //Assert
            Assert.AreEqual(1, borrowedCopyId1);
            Assert.AreEqual(3, borrowedCopyId2);
            Assert.AreEqual(4, borrowedCopyId3);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void BorrowBookByTitleTest2()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 30);

            //Act
            myLibrary.BorrowBookByTitle("1984");
        }

        [TestMethod()]
        public void ReturnBookByIdTest1()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 33.3);
            myLibrary.AddBook("Morometii", "3726160340677", 30);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 44.4);
            myLibrary.AddBook("Atomic Habits", "9791162540640", 55.5);

            //Workaround to test penalty prices, setting LastBorrowDate manually
            var copy1 = myLibrary.GetNonBorrowedBooks().Where(b => b.Title == "Atomic Habits").First();
            var date24DaysAgo = DateTime.Now.AddDays(-24);
            copy1.LastBorrowDate = new DateOnly(date24DaysAgo.Year, date24DaysAgo.Month, date24DaysAgo.Day);
            var borrowedCopyId1 = copy1.ID;
            var borrowedCopyId2 = myLibrary.BorrowBookByTitle("Atomic Habits");
            var borrowedCopyId3 = myLibrary.BorrowBookByTitle("Atomic Habits");

            //Act
            var price1 = myLibrary.ReturnBookById(borrowedCopyId1);
            var price2 = myLibrary.ReturnBookById(borrowedCopyId2);
            var price3 = myLibrary.ReturnBookById(borrowedCopyId3);

            //Assert
            Assert.AreEqual(36.63, price1);
            Assert.AreEqual(44.4, price2);
            Assert.AreEqual(55.5, price3);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ReturnBookByIdTest2()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 33.3);
            myLibrary.AddBook("Morometii", "3726160340677", 30);
            var borrowedCopyId = myLibrary.BorrowBookByTitle("Atomic Habits");

            //Act
            var price = myLibrary.ReturnBookById(2);
        }

        [TestMethod()]
        public void BorrowReturnAndReborrowTest()
        {
            //Arrange
            var myLibrary = new Library(new LibraryCatalog(new List<LibraryItem>()));
            myLibrary.AddBook("Atomic Habits", "9791162540640", 33.3);

            //Act
            var borrowedCopyId = myLibrary.BorrowBookByTitle("Atomic Habits");
            myLibrary.ReturnBookById(borrowedCopyId);
            var borrowedCopyId2 = myLibrary.BorrowBookByTitle("Atomic Habits");

            //Assert
            Assert.AreEqual(1, borrowedCopyId);
            Assert.AreEqual(1, borrowedCopyId2);
        }
    }
}