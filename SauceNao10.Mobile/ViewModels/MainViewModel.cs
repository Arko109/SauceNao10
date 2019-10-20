using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SauceNao10.Core.Helpers;
using SauceNao10.Core.Services;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SauceNao10.Mobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        SauceNaoService _sauceNaoService;
        INavigationService _navigationService;

        public MainViewModel(SauceNaoService sauceNaoServiceInstance, INavigationService navigationServiceInstance)
        {
            _sauceNaoService = sauceNaoServiceInstance;
            _navigationService = navigationServiceInstance;
        }

        private DelegateCommand<string> _queryCommand;
        public DelegateCommand<string> QueryCommand => _queryCommand ?? (_queryCommand = new DelegateCommand<string>(ExecuteQueryCommand));
        async void ExecuteQueryCommand(string query)
        {
            if (Uri.TryCreate(query, UriKind.Absolute, out Uri uri))
            {
                IsBusy = true;
                _navigationService.Navigate(PageTokens.ResultsPage, await Json.StringifyAsync(await _sauceNaoService.GetSauceAsync(uri)));
            }
        }

        private DelegateCommand _browseCommand;
        public DelegateCommand BrowseCommand => _browseCommand ?? (_browseCommand = new DelegateCommand(ExecuteBrowseCommand));
        async void ExecuteBrowseCommand()
        {
            var file = await GetImageStreamAsync();
            if (file.HasValue)
            {
                IsBusy = true;
                _navigationService.Navigate(PageTokens.ResultsPage, await Json.StringifyAsync(await _sauceNaoService.GetSauceAsync(file.Value.Item1, file.Value.Item2)));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        async Task<(Stream, string)?> GetImageStreamAsync()
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            new List<string>() { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".webp" }.ForEach(s => openPicker.FileTypeFilter.Add(s));
            var file = await openPicker.PickSingleFileAsync();
            if (file != null) return (await file.OpenStreamForReadAsync(), file.Name);
            return null;
        }

        async Task HandleShareAsync(string name)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(name);
                IsBusy = true;
                _navigationService.Navigate(PageTokens.ResultsPage, await Json.StringifyAsync(await _sauceNaoService.GetSauceAsync(await file.OpenStreamForReadAsync(), file.Name)));
                await file.DeleteAsync();
            }
            catch (Exception) { }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (e.Parameter is string name) await HandleShareAsync(name);
        }
    }
}
