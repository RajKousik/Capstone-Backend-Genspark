using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using MusicApplicationAPI.Models.DbModels;

namespace MusicApplicationAPI.Services
{
    public class StripeService
    {
        private readonly string _secretKey;
        private readonly string _successUrl;
        private readonly string _cancelUrl;

        public StripeService(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("Stripe")["SecretKey"];
            _successUrl =  configuration["FrontEnd:SuccesUrl"];
            _cancelUrl = configuration["FrontEnd:CancelUrl"];
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<String> CreateCheckoutSession(decimal amount, string currency, int userId, int durationInDays, string email)
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
                SuccessUrl = _successUrl, // Replace with your success URL
                CancelUrl = _cancelUrl, // Replace with your cancel URL
                CustomerEmail = email,
            };
            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session.Id;
        }
    }
}
