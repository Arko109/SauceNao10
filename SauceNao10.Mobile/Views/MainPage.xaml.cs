using System;

using SauceNao10.Mobile.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SauceNao10.Mobile.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();

            //TransitionCollection collection = new TransitionCollection();
            //NavigationThemeTransition theme = new NavigationThemeTransition();

            //var info = new ContinuumNavigationTransitionInfo();

            //theme.DefaultNavigationTransitionInfo = info;
            //collection.Add(theme);
            //this.Transitions = collection;

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsBusy"))
                LoadingControl.Visibility = ViewModel.IsBusy ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) => ViewModel.QueryCommand.Execute(args.QueryText);
    }
}
