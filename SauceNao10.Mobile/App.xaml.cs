using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using SauceNao10.Mobile.Helpers;
using SauceNao10.Mobile.Services;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;

namespace SauceNao10.Mobile
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            await LaunchApplicationAsync(PageTokens.MainPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol) await LaunchApplicationAsync(PageTokens.MainPage, (args as ProtocolActivatedEventArgs)?.Uri.AbsolutePath);
            await Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "SauceNao10.Mobile.ViewModels.{0}ViewModel, SauceNao10.Mobile", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
        }

        protected override IDeviceGestureService OnCreateDeviceGestureService()
        {
            var service = base.OnCreateDeviceGestureService();
            service.UseTitleBarBackButton = false;
            return service;
        }

        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            var shareOperation = args.ShareOperation;
            if ((await shareOperation.GetStorageItemsAsync())[0] is StorageFile file)
            {
                await file.CopyAndReplaceAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync(file.Name, CreationCollisionOption.GenerateUniqueName));
                await Windows.System.Launcher.LaunchUriAsync(new Uri("saucenao10:" + file.Name));
                shareOperation.ReportCompleted();
            }
            else shareOperation.ReportError("Failed to read image");
        }
    }
}
