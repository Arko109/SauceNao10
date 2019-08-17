using System;

using SauceNao10.Core.Models;
using SauceNao10.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SauceNao10.Views
{
    public sealed partial class ResultsDetailPage : Page
    {
        public ResultsDetailPage()
        {
            InitializeComponent();
        }

        private ResultsDetailViewModel ViewModel => DataContext as ResultsDetailViewModel;
    }
}
