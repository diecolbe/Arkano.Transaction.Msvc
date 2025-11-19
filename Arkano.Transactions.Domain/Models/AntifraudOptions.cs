namespace Arkano.Transactions.Domain.Models
{
    public class AntifraudOptions
    {
        public const string SectionName = "Antifraud";
        
        public decimal MaxTransactionValue { get; set; } = 2000m;

        public decimal MaxDailyAccumulation { get; set; } = 20000m;
    }
}