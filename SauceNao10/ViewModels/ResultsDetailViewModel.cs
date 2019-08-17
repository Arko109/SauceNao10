using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SauceNao10.Core.Helpers;
using SauceNao10.Core.Models;
using SauceNao10.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace SauceNao10.ViewModels
{
    public class ResultsDetailViewModel : ViewModelBase
    {
        private readonly IConnectedAnimationService _connectedAnimationService;
        private readonly INavigationService _navigationService;
        private ICommand _goBackCommand;

        private Result _item;

        public Result Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack));

        public ResultsDetailViewModel(IConnectedAnimationService connectedAnimationService, INavigationService navigationService)
        {
            _connectedAnimationService = connectedAnimationService;
            _navigationService = navigationService;
        }

        private void OnGoBack()
        {
            if (_navigationService.CanGoBack())
            {
                _navigationService.GoBack();
            }
        }

        public async override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (e.Parameter is string result)
            {
                Item = await Json.ToObjectAsync<Result>(result);
            }

        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            if (e.NavigationMode == NavigationMode.Back)
            {
                _connectedAnimationService.SetListDataItemForNextConnectedAnimation(Item);
            }
        }
    }
}
