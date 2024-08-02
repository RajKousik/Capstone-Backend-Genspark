namespace MusicApplicationAPI.Models.DbModels
{
    public class CheckoutRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public int userId { get; set; }

        public int durationInDays { get; set; }

        public string email { get; set; }
    }
}
