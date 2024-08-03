using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Interfaces.Service;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Repositories;
using MusicApplicationAPI.Services.EmailService;
using System.Diagnostics.CodeAnalysis;

namespace MusicApplicationAPI.Services
{
    [ExcludeFromCodeCoverage]
    public class SubscriptionMailService
    {
        private readonly IPremiumUserRepository _premiumUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSenderService;

        public SubscriptionMailService(IUserRepository userRepository, IEmailSender emailSenderService, IPremiumUserRepository premiumUserRepository)
        {
            _premiumUserRepository = premiumUserRepository;
            _emailSenderService = emailSenderService;
            _userRepository = userRepository;
        }

        public async Task CheckAndNotifyExpiringSubscriptions()
        {
            var today = DateTime.UtcNow;
            var twoDaysBeforeThreshold = today.AddDays(2);
            var oneHourBeforeThreshold = today.AddHours(1);

            

            var usersWithExpiringSubscriptions = await _premiumUserRepository.GetUsersWithExpiringSubscriptions(twoDaysBeforeThreshold, oneHourBeforeThreshold);

                                    
            foreach (var premiumUser in usersWithExpiringSubscriptions)
            {
                var user = await _userRepository.GetById(premiumUser.UserId);
                                // Send email if 2 days before end date
                if (premiumUser.EndDate <= twoDaysBeforeThreshold && premiumUser.EndDate > DateTime.UtcNow && premiumUser.LastNotifiedTwoDaysBefore == null)
                {
                    await _emailSenderService.SendEmailAsync(
                        user.Email,
                        "Your Subscription is About to Expire in 2 Days!",
                        GetExpiryInTwoDaysEmailTemplate(user, premiumUser.EndDate)
                    );
                    premiumUser.LastNotifiedTwoDaysBefore = DateTime.UtcNow;
                }
                                                // Send email if 1 hour before end date
                if (premiumUser.EndDate <= oneHourBeforeThreshold && premiumUser.EndDate > DateTime.UtcNow && premiumUser.LastNotifiedOneHourBefore == null)
                {
                    
                    await _emailSenderService.SendEmailAsync(
                        user.Email,
                        "Your Subscription is About to Expire in 1 Hour!",
                        GetExpiryInOneHourEmailTemplate(user, premiumUser.EndDate)
                    );
                    premiumUser.LastNotifiedOneHourBefore = DateTime.UtcNow;
                }

                await _premiumUserRepository.Update(premiumUser);
            }
        }


        #region Private Methods

        public string GetExpiryInOneHourEmailTemplate(User user, DateTime endDate)
        {
            var remainingTime = endDate - DateTime.UtcNow;
            var hoursRemaining = (int)remainingTime.TotalHours;

            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Your Subscription is About to Expire!</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; margin: 0; padding: 0; }}
                        .container {{ max-width: 600px; margin: 50px auto; background-color: #fff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); overflow: hidden; }}
                        .header {{ background-color: #FF5733; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 30px; }}
                        .footer {{ padding: 20px; font-size: 12px; color: #777; text-align: center; background-color: #f9f9f9; }}
                        .banner {{ font-size: 24px; font-weight: bold; margin: 20px 0; color: #FF5733; }}
                        .info-text {{ color: #555; }}
                        .social-links a {{ text-decoration: none; color: #FF5733; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Your Subscription is About to Expire!</h1>
                        </div>
                        <div class='content'>
                            <p>Hello {user.Username},</p>
                            <p>Your subscription is about to expire in {hoursRemaining} hour on {endDate:MMMM dd, yyyy}. Please renew it immediately to avoid interruption to your premium features.</p>
                            <p>To renew your subscription, visit your <a href='https://example.com/profile'>profile</a>.</p>
                            <p>Thank you for being a valued member of our community!</p>
                        </div>
                        <div class='footer'>
                            <p>Thank you for being a valued member of the VibeVault community!</p>
                            <p class='social-links'>
                                Follow us: 
                                <a href='https://twitter.com/vibevault'>Twitter</a> | 
                                <a href='https://facebook.com/vibevault'>Facebook</a> | 
                                <a href='https://instagram.com/vibevault'>Instagram</a>
                            </p>
                            <p>© 2024 VibeVault, All Rights Reserved</p>
                            <p>123 Music Street, Suite 400, Chennai, India</p>
                        </div>
                    </div>
                </body>
                </html>";
        }


        public string GetExpiryInTwoDaysEmailTemplate(User user, DateTime endDate)
        {
            var daysRemaining = (endDate - DateTime.UtcNow).Days;
                        return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Your Subscription is About to Expire!</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; margin: 0; padding: 0; }}
                        .container {{ max-width: 600px; margin: 50px auto; background-color: #fff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); overflow: hidden; }}
                        .header {{ background-color: #FF5733; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 30px; }}
                        .footer {{ padding: 20px; font-size: 12px; color: #777; text-align: center; background-color: #f9f9f9; }}
                        .banner {{ font-size: 24px; font-weight: bold; margin: 20px 0; color: #FF5733; }}
                        .info-text {{ color: #555; }}
                        .social-links a {{ text-decoration: none; color: #FF5733; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Your Subscription is About to Expire!</h1>
                        </div>
                        <div class='content'>
                            <p>Hello {user.Username},</p>
                            <p>Your subscription is about to expire in {daysRemaining} days on {endDate:MMMM dd, yyyy}. Please renew it within the next {daysRemaining} days to continue enjoying our premium features.</p>
                            <p>To renew your subscription, visit your <a href='https://example.com/profile'>profile</a>.</p>
                            <p>Thank you for being a valued member of our community!</p>
                        </div>
                        <div class='footer'>
                            <p>Thank you for being a valued member of the VibeVault community!</p>
                            <p class='social-links'>
                                Follow us: 
                                <a href='https://twitter.com/vibevault'>Twitter</a> | 
                                <a href='https://facebook.com/vibevault'>Facebook</a> | 
                                <a href='https://instagram.com/vibevault'>Instagram</a>
                            </p>
                            <p>© 2024 VibeVault, All Rights Reserved</p>
                            <p>123 Music Street, Suite 400, Chennai, India</p>
                        </div>
                    </div>
                </body>
                </html>";
        }



        #endregion


    }

}
