using System;
using System.Collections.Generic;
using System.Linq;
using TripLog.Models;
using TripLog.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TripLog.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        void New_Clicked(object sender, EventArgs ea)
        {
            Navigation.PushAsync(new NewEntryPage());
        }

        async void Trips_SelectionChanged(object sender,
            SelectionChangedEventArgs ea)
        {
            var trip = (TripLogEntry)ea.CurrentSelection.FirstOrDefault();

            if (trip != null)
            {
                await Navigation.PushAsync(new DetailPage(trip));
            }

            // Clear selection
            trips.SelectedItem = null;
        }
    }
}