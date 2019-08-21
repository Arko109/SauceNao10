using System;

using SauceNao10.Mobile.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SauceNao10.Mobile.Views
{
    public sealed partial class ResultsPage : Page
    {
        private ResultsViewModel ViewModel => DataContext as ResultsViewModel;

        public ResultsPage()
        {
            InitializeComponent();

            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => ViewModel.OpenFirstSourceCommand.Execute((sender as AppBarButton).CommandParameter);
    }
}
