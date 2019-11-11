using System;

using SauceNao10.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SauceNao10.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
