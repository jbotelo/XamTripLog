using System.ComponentModel;
using System.Runtime.CompilerServices;
using TripLog.Services;

namespace TripLog.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected INavService NavService { get; private set; }

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        protected IAnalyticsService AnalyticsService { get; private set; }
        protected BaseViewModel(INavService navService,
            IAnalyticsService analyticsService)
        {
            NavService = navService;
            AnalyticsService = analyticsService;
        }

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Init()
        {
        }
    }

    public class BaseViewModel<TParameter> : BaseViewModel
    {
        protected BaseViewModel(INavService navService,
            IAnalyticsService analyticsService)
            : base(navService, analyticsService)
        {
        }

        public override void Init()
        {
            Init(default(TParameter));
        }

        public virtual void Init(TParameter parameter)
        {
        }
    }
}