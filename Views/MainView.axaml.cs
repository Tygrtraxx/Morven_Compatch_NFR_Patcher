/*=============================================================================================*
 * Class: MainView
 *
 * Description:
 *   This is the code-behind for the MainView UserControl. It initializes the UI components
 *   defined in the associated XAML file (MainView.axaml) and handles user interactions,
 *   such as clicks on the question mark buttons which display an InfoDialog.
 *=============================================================================================*/

using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Platform.Storage;
using Morven_Compatch_NFR_Patcher.ViewModels;
using Avalonia.Threading;

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

        // Event handler for the Patch button click.
        private async void PatchButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if the Inlines collection is unexpectedly null; if so, throw an exception to indicate a critical error.
            if (ConsoleOutputTextBlock.Inlines == null)
            {
                throw new InvalidOperationException("Inlines collection is unexpectedly null.");
            }

            // Clear the console to make way for the next message to show the user.
            ConsoleOutputTextBlock.Inlines.Clear();

            // TODO: Add the ability to check if the user has already patched their game.
            /*
            if ()
            {
                ConsoleOutputTextBox.Text = "Error: The files have already been patched.";
                return;
            }
            */

            // Validate that both folder paths are provided
            if (string.IsNullOrWhiteSpace(SteamFolderTextBox.Text) || string.IsNullOrWhiteSpace(ModFolderTextBox.Text) || (!Directory.Exists(SteamFolderTextBox.Text) && !Directory.Exists(ModFolderTextBox.Text)))
            {
                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Both folders are invalid or must be specified.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Validate that the Steam folder exists.
            if (!Directory.Exists(SteamFolderTextBox.Text))
            {
                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The specified ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Steam folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "does not exist.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Validate that the Mod folder exists.
            if (!Directory.Exists(ModFolderTextBox.Text))
            {
                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The specified ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Mod folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "does not exist.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            try
            {
                // Simulate patching logic with an asynchronous delay.
                await Task.Delay(1000);

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Success: ",
                    Foreground = Avalonia.Media.Brushes.Green
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The files have been successfully patched .",
                    Foreground = Avalonia.Media.Brushes.White
                });
            }
            catch (Exception ex)
            {
                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Critical Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "" + ex.Message,
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines!.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "\n\nPatch operation was unsuccessful.",
                    Foreground = Avalonia.Media.Brushes.White
                });
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
