# Library
A console app that servers as a management system for a library.\
The app keeps track of available books in the library but also borrowed books.

The return window is 2 weeks. After 2 weeks, every day, a penalty of 1% of the borrow price will be charged.

## Features
- Add a new book to the library\
  Multiple copies of the same book may be stored in the library at the same time.\
  When adding a new book, at least the following information will be specified:
  * Name
  * ISBN
  * Borrow price
- List out all the books in the library
- Get the number of copies for a book
- Borrow a book
- Return a book 