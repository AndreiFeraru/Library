namespace Library.Models
{
    internal interface IBorrowable
    {
        public bool IsBorrowed();
        public void Borrow();
        public double ReturnAndGetPrice();
    }
}
