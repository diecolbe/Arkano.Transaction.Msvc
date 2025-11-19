namespace Arkano.Transactions.Domain.Exceptions
{
    public class TransactionException : ArgumentException
    {
        public TransactionException() { }

        public TransactionException(string? message) : base(message) { }

        public TransactionException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
