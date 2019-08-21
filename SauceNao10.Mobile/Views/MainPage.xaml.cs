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
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) => ViewModel.QueryCommand.Execute(args.QueryText);
    }
}
