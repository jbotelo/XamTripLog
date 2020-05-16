using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Ninject;
using Ninject.Modules;
using System;
using TripLog.Modules;
using TripLog.Services;
using TripLog.ViewModels;
using TripLog.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TripLog
{
    public partial class App : Application
    {
        public IKernel Kernel { get; set; }
        
        public App(params INinjectModule[] platformModules)
        {
            InitializeComponent();

            // Register core services
            Kernel = new StandardKernel(
                new TripLogCoreModule(),
                new TripLogNavModule());

            // Register platform specific services
            Kernel.Load(platformModules);

            SetMainPage();
        }

        private void SetMainPage()
        {
            var mainPage = new NavigationPage(new MainPage())
            {
                BindingContext = Kernel.Get<MainViewModel>()
            };

            var navService = Kernel.Get<INavService>()
                as XamarinFormsNavService;

            navService.XamarinFormsNav = mainPage.Navigation;

            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            AppCenter.Start("ios={Your iOS app secret here};"
                + "android={Your Android app secret here}",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
