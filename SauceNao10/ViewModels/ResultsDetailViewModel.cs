using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SauceNao10.Core.Helpers;
using SauceNao10.Core.Models;
using SauceNao10.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;

namespace SauceNao10.ViewModels
{
    public class ResultsDetailViewModel : ViewModelBase
    {
        private readonly IConnectedAnimationService _connectedAnimationService;
        private readonly INavigationService _navigationService;
        private ICommand _goBackCommand;
        private ICommand _openSourceCommand;
        private ICommand _copyRawCommand;

        private Result _item;

        public Result Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack));
        public ICommand OpenSourceCommand => _openSourceCommand ?? (_openSourceCommand = new DelegateCommand<string>(OnOpenSource));
        public ICommand CopyRawCommand => _copyRawCommand ?? (_copyRawCommand = new DelegateCommand<string>(OnCopyRaw));

        public ResultsDetailViewModel(IConnectedAnimationService connectedAnimationService, INavigationService navigationService)
        {
            _connectedAnimationService = connectedAnimationService;
            _navigationService = navigationService;
        }

        private async void OnCopyRaw(string raw)
        {
            DataPackage dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
            dataPackage.SetText(raw);
            Clipboard.SetContent(dataPackage);
        }
        private async void OnOpenSource(string source)
        {
            if (Uri.TryCreate(source, UriKind.Absolute, out Uri uri)) await Windows.System.Launcher.LaunchUriAsync(uri);
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
