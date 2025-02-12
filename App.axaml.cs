/*=============================================================================================*
 * Class: App
 *
 * Description:
 *   This class is the main application entry point for the Morven Compatch NFR Patcher.
 *   It extends Avalonia's Application class and is responsible for loading the XAML resources,
 *   initializing the application's framework, and setting up the main window based on the type 
 *   of application lifetime.
 *
 *   When the framework initialization is completed, it removes extra Avalonia data validation 
 *   to prevent duplicate validation errors, then checks whether the app is running in classic 
 *   desktop mode or single-view mode, and finally creates and assigns the appropriate main window 
 *   or view with its DataContext set to a new instance of MainViewModel.
 *=============================================================================================*/

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Morven_Compatch_NFR_Patcher.ViewModels;
using Morven_Compatch_NFR_Patcher.Views;

namespace Morven_Compatch_NFR_Patcher
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnFrameworkInitializationCompleted()
        {
            // Gets rid of Avalonia data validation to avoid duplicate validations.
            BindingPlugins.DataValidators.RemoveAt(0);

            // Check if the application is running in classic desktop mode.
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Create the MainWindow and set its DataContext to a new instance of MainViewModel.
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
            }
            // Otherwise, if running in single-view mode (e.g., for mobile or embedded scenarios).
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                // Create the MainView and set its DataContext to a new instance of MainViewModel.
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel()
                };
            }

            // Call the base class implementation.
            base.OnFrameworkInitializationCompleted();
        }
    }
}

