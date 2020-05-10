using TripLog.Models;
using TripLog.Services;

namespace TripLog.ViewModels
{
    internal class DetailViewModel : BaseViewModel<TripLogEntry>
    {
        private TripLogEntry _entry;

        public TripLogEntry Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                OnPropertyChanged();
            }
        }

        public DetailViewModel(INavService navService)
            : base(navService)
        {
        }

        public override void Init(TripLogEntry parameter)
        {
            Entry = parameter;
        }
    }
}