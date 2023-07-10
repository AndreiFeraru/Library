namespace Library.Models
{
    internal abstract class LibraryItem : IBorrowable
    {
        public int ID { get; set; }

        public double Price { get; set; }

        public DateOnly? LastBorrowDate { get; set; }

        public LibraryItem(int id, double price)
        {
            ID = id;
            Price = price;
        }

        public bool IsBorrowed() => LastBorrowDate.HasValue;

        public void Borrow()
        {
            if (IsBorrowed())
            {
                Console.WriteLine("An error has occured. Cannot borrow book copy, it is already borrowed.");
                return;
            }

            LastBorrowDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public double ReturnAndGetPrice()
        {
            if (!IsBorrowed())
            {
                Console.WriteLine("An error has occured. Cannot return book copy, it is not borrowed.");
                return 0;
            }

            return CalculateReturnPrice();
        }

        private double CalculateReturnPrice()
        {
            if (!LastBorrowDate.HasValue)
                throw new ApplicationException("Borrow date is null, cannot calculate price");

            var todaysDate = DateOnly.FromDateTime(DateTime.Now);
            var daysSinceBorrow = todaysDate.DayNumber - LastBorrowDate.Value.DayNumber;
            var penaltyDays = daysSinceBorrow - Constants.MAX_BORROW_WINDOW_IN_DAYS;

            return penaltyDays > 0 ?
                Price * (1 + penaltyDays * Constants.DAILY_PENALTY_PERCENT_FROM_INITIAL_PRICE / 100) :
                Price;
        }

    }
}
