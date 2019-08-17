using SauceNao10.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SauceNao10.Views
{
    public sealed partial class ResultsDetailPage : Page
    {
        public ResultsDetailPage()
        {
            InitializeComponent();
        }

        private ResultsDetailViewModel ViewModel => DataContext as ResultsDetailViewModel;

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
