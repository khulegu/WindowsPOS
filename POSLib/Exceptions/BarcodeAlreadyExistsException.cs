namespace POSLib.Exceptions
{
    public class BarcodeAlreadyExistsException : Exception
    {
        public BarcodeAlreadyExistsException() { }

        public BarcodeAlreadyExistsException(string message) : base(message) { }

        public BarcodeAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }

}
