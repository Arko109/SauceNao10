using System;

using SauceNao10.Mobile.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SauceNao10.Mobile.Views
{
    public sealed partial class ResultsDetailPage : Page
    {
        private ResultsDetailViewModel ViewModel => DataContext as ResultsDetailViewModel;

        public ResultsDetailPage()
        {
            InitializeComponent();

            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.OpenSourceCommand.Execute(e.ClickedItem);
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.CopyRawCommand.Execute((sender as AppBarButton).CommandParameter);
        }
    }
}
