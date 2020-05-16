using System;
using System.Collections.Generic;
using System.Text;
using TripLog.Services;
using Xamarin.Forms;

namespace TripLog.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ITripLogDataService _tripLogService;

        Command _signInCommand;
        public Command SignInCommand => _signInCommand ??
            (_signInCommand = new Command(SignIn));


        public SignInViewModel(INavService navService, IAuthService authService,
            ITripLogDataService tripLogService)
            : base(navService)
        {
            _authService = authService;
            _tripLogService = tripLogService;
        }
        private void SignIn()
        {
            // TODO: Update with your OAuth2 credentiasl and API.
            _authService.SignInAsync("YOUR_FACEBOOK_APPID",
                new Uri("https://m.facebook.com/dialog/oauth"),
                new Uri("https://<your-api-server>/.auth/login/facebook/callback"),
                tokenCallback: async token =>
                {
                    // Use Facebook token to get Azure auth token
                    await _tripLogService.AuthenticateAsync("facebook", token);
                },
                errorCallback: e =>
                {
                    // TODO: Handle invalid authentication here
                });
        }
    }
}
