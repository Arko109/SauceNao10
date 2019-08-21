using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SauceNao10.Core.Helpers;
using SauceNao10.Core.Models;

namespace SauceNao10.Mobile.ViewModels
{
    public class ResultsViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private ObservableCollection<Result> _source;
        private ICommand _itemClickCommand;
        private ICommand _goBackCommand;
        private ICommand _openFirstSourceCommand;

        public ObservableCollection<Result> Results
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new DelegateCommand<Result?>(OnItemClick));
        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack));
        public ICommand OpenFirstSourceCommand => _openFirstSourceCommand ?? (_openFirstSourceCommand = new DelegateCommand<string>(OnOpenFirstSource));

        public ResultsViewModel(INavigationService navigationServiceInstance)
        {
            _navigationService = navigationServiceInstance;
        }

        public async void OnOpenFirstSource(string source)
        {
            if (Uri.TryCreate(source, UriKind.Absolute, out Uri uri)) await Windows.System.Launcher.LaunchUriAsync(uri);
        }
        private async void OnItemClick(Result? clickedItem)
        {
            _navigationService.Navigate(PageTokens.ResultsDetailPage, await Json.StringifyAsync(clickedItem));
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
            if (e.Parameter is string results)
            {
                Results = new ObservableCollection<Result>();
                foreach (var result in (await Json.ToObjectAsync<IList<Result>>(results)))
                {
                    Results.Add(result);
                }
            }
        }
    }
}
