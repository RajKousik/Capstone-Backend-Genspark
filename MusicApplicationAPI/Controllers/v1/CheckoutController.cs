using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApplicationAPI.Services;

namespace MusicApplicationAPI.Controllers.v1
{
    [Route("api/v1/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly StripeService _stripeService;

        public CheckoutController(StripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest request)
        {
            var session = await _stripeService.CreateCheckoutSession(request.Amount, request.Currency, request.userId, request.durationInDays);
            return Ok(new { sessionId = session});
        }
    }

    public class CheckoutRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public int userId { get; set; }

        public int durationInDays { get; set; }
    }
}
