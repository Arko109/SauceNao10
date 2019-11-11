using System;

using SauceNao10.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SauceNao10.Views
{
    public sealed partial class ResultsPage : Page
    {
        private ResultsViewModel ViewModel => DataContext as ResultsViewModel;

        public ResultsPage()
        {
            InitializeComponent();
        }
    }
}
