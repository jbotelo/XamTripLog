using Ninject;
using Ninject.Modules;
using System;
using TripLog.Modules;
using TripLog.Services;
using TripLog.ViewModels;
using TripLog.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TripLog
{
    public partial class App : Application
    {
        public IKernel Kernel { get; set; }
        bool IsSignedIn => !string.IsNullOrWhiteSpace(
            Preferences.Get("apitoken", ""));

        public App(params INinjectModule[] platformModules)
        {
            InitializeComponent();

            // Register core services
            Kernel = new StandardKernel(
                new TripLogCoreModule(),
                new TripLogNavModule());

            // Register platform specific services
            Kernel.Load(platformModules);

            // Setup data service authentication delegates
            var dataService = Kernel.Get<ITripLogDataService>();
            dataService.AuthorizedDelegate = OnSignIn;
            dataService.UnauthorizedDelegate = SignOut;

            SetMainPage();
        }

        private void SetMainPage()
        {
            var mainPage = IsSignedIn
                ? new NavigationPage(new MainPage())
                {
                    BindingContext = Kernel.Get<MainViewModel>()
                }
                : new NavigationPage(new SignInPage())
                {
                    BindingContext = Kernel.Get<SignInViewModel>()
                };

            var navService = Kernel.Get<INavService>()
                as XamarinFormsNavService;

            navService.XamarinFormsNav = mainPage.Navigation;

            MainPage = mainPage;
        }

        void OnSignIn(string accessToken)
        {
            Preferences.Set("apitoken", accessToken);
            SetMainPage();
        }

        void SignOut()
        {
            Preferences.Remove("apitoken");
            SetMainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
