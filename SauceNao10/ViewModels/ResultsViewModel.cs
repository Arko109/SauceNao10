﻿using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SauceNao10.Core.Helpers;
using SauceNao10.Core.Models;
using SauceNao10.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;

namespace SauceNao10.ViewModels
{
    public class ResultsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IConnectedAnimationService _connectedAnimationService;

        private ObservableCollection<Result> _source;
        private ICommand _itemClickCommand;
        private ICommand _goBackCommand;

        public ObservableCollection<Result> Results
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new DelegateCommand<Result?>(OnItemClick));
        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack));

        public ResultsViewModel(INavigationService navigationServiceInstance, IConnectedAnimationService connectedAnimationService)
        {
            _navigationService = navigationServiceInstance;
            _connectedAnimationService = connectedAnimationService;
        }

        private async void OnItemClick(Result? clickedItem)
        {
            _connectedAnimationService.SetListDataItemForNextConnectedAnimation(clickedItem);
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
