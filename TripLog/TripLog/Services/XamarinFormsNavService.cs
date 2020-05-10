using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TripLog.Services;
using TripLog.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

// Remove assembly attribute
//[assembly: Dependency(typeof(XamarinFormsNavService))]
namespace TripLog.Services
{
    class XamarinFormsNavService : INavService
    {
        readonly IDictionary<Type, Type> _map =
            new Dictionary<Type, Type>();
        public INavigation XamarinFormsNav { get; set; }
        public bool CanGoBack =>
            XamarinFormsNav.NavigationStack != null
            && XamarinFormsNav.NavigationStack.Count > 0;

        public event PropertyChangedEventHandler CanGoBackChanged;

        public void RegisterViewMapping(Type viewModel, Type view)
        {
            _map.Add(viewModel, view);
        }

        public async Task GoBack()
        {
            if (CanGoBack)
            {
                await XamarinFormsNav.PopAsync(true);
                OnCanGoBackChanged();
            }
        }

        private void OnCanGoBackChanged()
        {
            CanGoBackChanged?.Invoke(this,
                new PropertyChangedEventArgs("CanGoBack"));
        }

        public async Task NavigateTo<TVM>()
            where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext
                is BaseViewModel)
            {
                ((BaseViewModel)XamarinFormsNav.NavigationStack.
                    Last().BindingContext).Init();
            }
        }

        private async Task NavigateToView(Type viewModelType)
        {
            if (!_map.TryGetValue(viewModelType, out Type viewType))
            {
                throw new ArgumentException("No view found in view mapping " +
                    "for " + viewModelType.FullName + ".");
            }
            // Use reflection to get the View's constructor and create an
            // instance of the View
            var constructor = viewType.GetTypeInfo().DeclaredConstructors
                .FirstOrDefault(dc => !dc.GetParameters().Any());
            var view = constructor.Invoke(null) as Page;
            
            var vm = ((App)Application.Current).Kernel
                .GetService(viewModelType);
            view.BindingContext = vm;

            await XamarinFormsNav.PushAsync(view, true);
        }

        public async Task NavigateTo<TVM, TParameter>(TParameter parameter)
            where TVM : BaseViewModel
        {
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext
                is BaseViewModel<TParameter>)
            {
                ((BaseViewModel<TParameter>)XamarinFormsNav.NavigationStack
                    .Last().BindingContext).Init(parameter);
            }
        }

        public void RemoveLastView()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
            {
                return;
            }

            var lastView = XamarinFormsNav.NavigationStack
                [XamarinFormsNav.NavigationStack.Count - 2];

            XamarinFormsNav.RemovePage(lastView);
        }

        public void ClearBackStack()
        {
            if (XamarinFormsNav.NavigationStack.Count < 2)
            {
                return;
            }

            for (var i = 0;
                i < XamarinFormsNav.NavigationStack.Count -1; i++)
            {
                XamarinFormsNav.RemovePage(XamarinFormsNav
                    .NavigationStack[i]);
            }
        }

        public async Task NavigateToUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentException("Invalid URI");
            }
            
            // Device obsolete
            //Device.OpenUri(uri);
            // Using Launcher instead
            await Launcher.OpenAsync(uri);
        }

    }
}
