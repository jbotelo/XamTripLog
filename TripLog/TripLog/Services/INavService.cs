using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using TripLog.ViewModels;

namespace TripLog.Services
{
    public interface INavService
    {
        bool CanGoBack { get; }
        Task GoBack();
        Task NavigateTo<TVM>()
            where TVM : BaseViewModel;
        Task NavigateTo<TVM, TParameter>(TParameter parameter)
            where TVM : BaseViewModel;
        void RemoveLastView();
        void ClearBackStack();
        Task NavigateToUri(Uri uri);

        event PropertyChangedEventHandler CanGoBackChanged;
    }
}
