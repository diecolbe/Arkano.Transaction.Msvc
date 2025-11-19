namespace Arkano.Transactions.Domain.Tests.Contants
{
    public static class Amounts
    {
        public const decimal Small = 50m;
        public const decimal Medium = 500m;
        public const decimal Large = 1500m;
        public const decimal Zero = 0m;
        public const decimal Negative = -100m;
        public const decimal AtTransactionLimit = 1000m;
        public const decimal AtDailyLimit = 5000m;
        public const decimal OverTransactionLimit = 1500m;
        public const decimal OverDailyLimit = 6000m;
        public const decimal VerySmall = 0.01m;
        public const decimal ExtremelySmall = 0.001m;
    }
}
