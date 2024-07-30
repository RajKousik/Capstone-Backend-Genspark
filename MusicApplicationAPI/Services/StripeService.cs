using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Services
{
    public class StripeService
    {
        private readonly string _secretKey;

        public StripeService(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("Stripe")["SecretKey"];
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<String> CreateCheckoutSession(decimal amount, string currency, int userId, int durationInDays)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Premium Account",
                        },
                        UnitAmount = (long)(amount), // Amount in cents
                    },
                    Quantity = 1,
                    
                },
            },
                Mode = "payment",
                Metadata = new Dictionary<string, string>()
                {
                    { "user_id", userId.ToString() },
                    { "duration_in_days", durationInDays.ToString() } // Add durationInDays as metadata
                },
                SuccessUrl = "https://localhost:3000/user-dashboard", // Replace with your success URL
                CancelUrl = "https://localhost:3000/user-dashboard", // Replace with your cancel URL
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session.Id;
        }
    }
}
