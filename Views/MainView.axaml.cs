/*=============================================================================================*
 * Class: MainView
 *
 * Description:
 *   This is the code-behind for the MainView UserControl. It initializes the UI components
 *   defined in the associated XAML file (MainView.axaml) and handles user interactions,
 *   such as clicks on the question mark buttons which display an InfoDialog.
 *=============================================================================================*/

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Platform.Storage;
using Morven_Compatch_NFR_Patcher.ViewModels;

namespace Morven_Compatch_NFR_Patcher.Views
{
    public partial class MainView : UserControl
    {
        // Constructor: Initializes a new instance of MainView and loads its UI components.
        public MainView()
        {
            InitializeComponent(); // Load the components defined in MainView.axaml.
        }

        // Event handler for the Steam question mark button click.
        // It creates an InfoDialog, sets its message for the Steam folder, and shows the dialog modally.
        private async void SteamQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            dialog.ViewModel.Message = "This is the Steam folder explanation text.";
            await dialog.ShowDialog(GetParentWindow());
        }

        // Event handler for the Mod question mark button click.
        // It creates an InfoDialog, sets its message for the Mod folder, and shows the dialog modally.
        private async void ModQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            dialog.ViewModel.Message = "This is the Mod folder explanation text.";
            await dialog.ShowDialog(GetParentWindow());
        }

        // Event handler for the Steam Browse button click.
        // Opens a folder picker dialog so the user can select the Steam folder and updates the ViewModel.
        private async void SteamBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var window = GetParentWindow();
            var folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select Steam Folder",
                AllowMultiple = false
            });

            // If at least one folder was selected, update the ViewModel's SteamFolder property.
            if (folders != null && folders.Count > 0)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.SteamFolder = folders[0].Path.LocalPath;
                }
            }
        }

        // Event handler for the Mod Browse button click.
        // Opens a folder picker dialog so the user can select the Mod folder and updates the ViewModel.
        private async void ModBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var window = GetParentWindow();
            var folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select Mod Folder",
                AllowMultiple = false
            });
            // If at least one folder was selected, update the ViewModel's ModFolder property.
            if (folders != null && folders.Count > 0)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.ModFolder = folders[0].Path.LocalPath;
                }
            }
        }

        // Retrieves the parent window of this control.
        // It attempts to cast the VisualRoot to a Window and throws an exception if the control is not attached.
        private Window GetParentWindow()
        {
            if (this.VisualRoot is not Window window)
                throw new InvalidOperationException("The control is not attached to a Window.");
            return window;
        }
    }
}
