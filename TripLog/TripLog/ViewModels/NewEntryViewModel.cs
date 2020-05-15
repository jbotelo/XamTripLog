using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TripLog.Models;
using TripLog.Services;
using Xamarin.Forms;

namespace TripLog.ViewModels
{
    internal class NewEntryViewModel : BaseValidationViewModel
    {
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Validate(() => !string.IsNullOrEmpty(_title),
                    "Title must be provided.");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        private double _latitude;

        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        private double _longitude;

        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private int _rating;

        public int Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                Validate(() => _rating >= 1 && _rating <= 5,
                    "Rating must be between 1 and 5.");
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        private string _notes;

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private Command _saveCommand;

        public Command SaveCommand =>
            _saveCommand ?? (_saveCommand = new Command(async () =>
            await Save(), CanSave));

        readonly ILocationService _locService;
        readonly ITripLogDataService _tripLogService;

        public NewEntryViewModel(INavService navService,
            ILocationService locService, ITripLogDataService tripLogService)
            : base(navService)
        {
            _locService = locService;
            _tripLogService = tripLogService;

            Date = DateTime.Today;
            Rating = 1;
        }

        async Task Save()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                var newItem = new TripLogEntry
                {
                    Title = Title,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Date = Date,
                    Rating = Rating,
                    Notes = Notes
                };

                await _tripLogService.AddEntryAsync(newItem);
            
                await NavService.GoBack();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Title)
            && !HasErrors;

        public override async void Init()
        {
            try
            {
                var coords = await _locService.GetGeoCoordinatesAsync();

                Latitude = coords.Latitude;
                Longitude = coords.Longitude;
            }
            catch (Exception)
            {
                // TODO: handle exceptions from location service
            }
        }
    }
}