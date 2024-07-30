using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DTOs.UserDTO;
using Stripe;
using Stripe.Checkout;
using System.IO;
using System.Threading.Tasks;

namespace MusicApplicationAPI.Controllers.v1
{
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IUserService _userService;

        public StripeWebhookController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/webhook")]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                Console.WriteLine("Hit");
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    "whsec_74ba5ae47b4064d66d3381ac473d51c460cab3519d1152c313aff7b0e373a69c" // Your webhook secret
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    Console.WriteLine("Inside if");
                    var session = stripeEvent.Data.Object as Session;
                    if (session != null)
                    {
                        Console.WriteLine("Session ID: " + session.Id);

                        if (session.Metadata.TryGetValue("user_id", out var userIdStr) &&
                            session.Metadata.TryGetValue("duration_in_days", out var durationInDaysStr))
                        {
                            if (int.TryParse(userIdStr, out int userId) &&
                                int.TryParse(durationInDaysStr, out int durationInDays))
                            {

                                    // Optional: Convert amount received to Money if needed
                                    double? money = (session.AmountTotal / 100.0); // Assuming amount total is in cents
                                                                                   //if(moen)
                                
                                var premiumRequest = new PremiumRequestDTO
                                {
                                    Money = (double)money,
                                    DurationInDays = durationInDays
                                };

                                await _userService.UpgradeUserToPremium(userId, premiumRequest);
                                Console.WriteLine($"User {userId} upgraded to premium for {durationInDays} days.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to parse user ID or duration.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("User ID or duration in days not found in metadata.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Session is null.");
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
