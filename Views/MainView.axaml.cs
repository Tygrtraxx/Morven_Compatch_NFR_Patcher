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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Morven_Compatch_NFR_Patcher.ViewModels;
using Morven_Compatch_NFR_Patcher.Helpers;


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
            if (ConsoleOutputTextBlock.Inlines == null || InstructionsTextBlock.Inlines == null ||InstructionsLinesTextBlock.Inlines == null)
            {
                throw new InvalidOperationException("One of the inlines collections are unexpectedly null.");
            }

            // Clear the instructions (To make room for the upcoming messages in the ConsoleOutputTextBlock).
            ConsoleOutputTextBlock.Inlines.Clear();

            // Clear the instructions lines (To make room for the upcoming messages in the ConsoleOutputTextBlock).
            InstructionsTextBlock.Inlines.Clear();

            // Clear the console to make way for the next message to show the user.
            InstructionsLinesTextBlock.Inlines.Clear();

            // Validate that both folder paths are provided
            if (string.IsNullOrWhiteSpace(SteamFolderTextBox.Text) || string.IsNullOrWhiteSpace(ModFolderTextBox.Text) || (!Directory.Exists(SteamFolderTextBox.Text) && !Directory.Exists(ModFolderTextBox.Text)))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Both folders are invalid or must be specified.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Validate that the Steam folder exists.
            if (!Directory.Exists(SteamFolderTextBox.Text))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The specified ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Steam folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "does not exist.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Validate that the Mod folder exists.
            if (!Directory.Exists(ModFolderTextBox.Text))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The specified ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Mod folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "does not exist.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Check if the selected folder's name is "Steam" or "steamapps".

            // Get the path entered by the user.
            string steamFolderPath = SteamFolderTextBox.Text;

            // Trim any trailing directory separators to ensure the folder name is extracted correctly.
            string trimmedSteamFolderPath = steamFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Extract the folder name.
            string steamFolderName = Path.GetFileName(trimmedSteamFolderPath);

            if (!string.Equals(steamFolderName, "Steam", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(steamFolderName, "steamapps", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The selected ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Steam folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "must be named ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'Steam' ",
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "or ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'steamapps'",
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // If the entered folder is "Steam", check if a "steamapps" subfolder exists.
            if (string.Equals(steamFolderName, "Steam", StringComparison.OrdinalIgnoreCase))
            {
                string steamappsPath = Path.Combine(trimmedSteamFolderPath, "steamapps");
                if (Directory.Exists(steamappsPath))
                {
                    // Use the "steamapps" folder instead.
                    steamFolderPath = steamappsPath;
                }

                // If the folder doesn't exist, show the user an error.
                else
                {
                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "Error: ",
                        Foreground = Avalonia.Media.Brushes.Red
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "The specified ",
                        Foreground = Avalonia.Media.Brushes.White
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "Steam folder ",
                        Foreground = Avalonia.Media.Brushes.Cyan
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "does not contain the ",
                        Foreground = Avalonia.Media.Brushes.White
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "steamapps folder.",
                        Foreground = Avalonia.Media.Brushes.Cyan
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "\n\n --> Are you sure this is the right Steam folder? <--",
                        Foreground = Avalonia.Media.Brushes.Yellow
                    });

                    return;
                }
            }

            // The game and mod that needs to be patched (Morven's Mods 1.14 Compatch) location
            string targetSubfolder = System.IO.Path.Combine(steamFolderPath, "workshop", "content", "1158310", "3001489429");

            // Check if the target subfolder exists.
            if (!System.IO.Directory.Exists(targetSubfolder))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The required folder ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'workshop\\content\\1158310\\3001489429' ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "does not exist within the ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "steamapps folder.",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "\n\n --> Are you sure you have Morven's Mods 1.14 Compatch installed? <--",
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                return;
            }

            // Get the mod folder path entered by the user and trim any trailing separators to ensure the folder name is extracted correctly.
            string modFolderPath = ModFolderTextBox.Text.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Create a DirectoryInfo object for the mod folder.
            DirectoryInfo modDir = new(modFolderPath);

            // Check that the selected folder is actually named "mod".
            if (!string.Equals(modDir.Name, "mod", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The selected folder must be named ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'mod'",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Check that the parent folder exists and is named "Crusader Kings III".
            DirectoryInfo? parentDir = modDir.Parent;
            if (parentDir == null || !string.Equals(parentDir.Name, "Crusader Kings III", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "mod folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "must be within the ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'Crusader Kings III' folder",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });


                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Check that the grandparent folder exists and is named "Paradox Interactive".
            DirectoryInfo? grandParentDir = parentDir.Parent;
            if (grandParentDir == null || !string.Equals(grandParentDir.Name, "Paradox Interactive", StringComparison.OrdinalIgnoreCase))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "mod folder ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "must be within the ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'Paradox Interactive\\Crusader Kings III\\' folder",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Determine the absolute mod folder path and normalize the path to use forward slashes.
            string normalizedModFolderPath = modFolderPath.Replace("\\", "/");

            // Append the desired subfolder to the mod folder path.
            string fullModPath = $"{normalizedModFolderPath}/morven_patch_NFR";

            // Build the line that needs to be placed at the 7th line
            string lineToInsert = $"path=\"{fullModPath}\"";

            // Specify the path to the mod file to update.
            string modFilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "ModFiles", "morven_patch_NFR.mod");

            // Ensure the file exists before attempting to modify it
            if (File.Exists(modFilePath))
            {
                // Read all lines from the file
                var fileLines = new List<string>(File.ReadAllLines(modFilePath));

                // If the file already has at least 7 lines, replace the 7th line with the local mod's path location
                if (fileLines.Count >= 7)
                {
                    fileLines[6] = lineToInsert;
                }
                else
                {
                    // If the file has fewer than 7 lines, append the local mod's path location to the file
                    fileLines.Add(lineToInsert);
                }

                // Write the modified content back to the file
                File.WriteAllLines(modFilePath, fileLines);
            }

            else
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "mod file 'morven_patch_NFR.mod' ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "was not found.",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "\n\n --> (BUG?) You shouldn't be seeing this error. Contact Tygrtraxx. <--",
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "\n\n DEBUG FILE PATH: \n" + modFilePath,
                    Foreground = Avalonia.Media.Brushes.Orange
                });

                return;
            }

            // Define the source directory where the mod files are located.
            string sourceDir = Path.Combine(AppContext.BaseDirectory, "Assets", "ModFiles");

            // Copy the newly edited mod file to the mod directory.
            FileHelper.CopyDirectory(sourceDir, modFolderPath);

            // Build the relative path from the Steam folder to the source base folder.
            string relativeSteamPath = Path.Combine("workshop", "content", "1158310", "3001489429");
            string sourceBase = Path.Combine(steamFolderPath, relativeSteamPath);

            // Check if the resulting folder doesn't exist.
            if (!Directory.Exists(sourceBase))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The required folder structure ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'steamapps\\workshop\\content\\1158310\\3001489429' ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "does not exist within your ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Steam folder",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "\n\n DEBUG FILE PATH: \n" + sourceBase,
                    Foreground = Avalonia.Media.Brushes.Orange
                });

                return;
            }

            // Specify the file name "descriptor.mod" in that directory.
            string descriptorFilePath = Path.Combine(sourceBase, "descriptor.mod");

            // Check if the file exists.
            if (!File.Exists(descriptorFilePath))
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'descriptor.mod' ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "not found at ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = $"{descriptorFilePath}",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Read all lines from the file (We are looking for the version in the file)
            string[] lines = File.ReadAllLines(descriptorFilePath);

            // Check if there are at least 6 lines in the file.
            if (lines.Length < 6)
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'descriptor.mod' ",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "is missing its version number on ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "line 6",
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // The 6th line is at index 5.
            string sixthLine = lines[5];

            // Use a regular expression to extract the text between the first pair of quotation marks.
            // The regex pattern \"([^\"]+)\" captures the text between quotes.
            Match match = Regex.Match(sixthLine, "\"([^\"]+)\"");
            if (!match.Success)
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Game version not found in the ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "6th line ",
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "of the ",
                    Foreground = Avalonia.Media.Brushes.White
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "'descriptor.mod'",
                    Foreground = Avalonia.Media.Brushes.Cyan
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = ".",
                    Foreground = Avalonia.Media.Brushes.White
                });

                return;
            }

            // Save the extracted value as the game_version variable.
            string gameVersion = "supported_version=\"" + match.Groups[1].Value + "\"";

            // Build the destination folder path as "morven_patch_NFR" inside the mod folder.
            string destinationBase = Path.Combine(modFolderPath, "morven_patch_NFR");

            // Ensure the destination folder exists.
            Directory.CreateDirectory(destinationBase);

            // List of subfolders to copy.
            string[] subfolders = ["common", "events", "localization"];

            foreach (string subfolder in subfolders)
            {
                // Build the full source subfolder path.
                string sourceSubfolder = Path.Combine(sourceBase, subfolder);

                // Check if this source subfolder exists.
                if (!Directory.Exists(sourceSubfolder))
                {
                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "Error: ",
                        Foreground = Avalonia.Media.Brushes.Red
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "The required subfolder ",
                        Foreground = Avalonia.Media.Brushes.White
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = $"'{subfolder}' ",
                        Foreground = Avalonia.Media.Brushes.Cyan
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "does not exist in the ",
                        Foreground = Avalonia.Media.Brushes.White
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = "Morven's Mods 1.14 Compatch folder",
                        Foreground = Avalonia.Media.Brushes.Cyan
                    });

                    ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                    {
                        Text = ".",
                        Foreground = Avalonia.Media.Brushes.White
                    });

                    return;
                }

                // Build the destination subfolder path inside "morven_patch_NFR".
                string destinationSubfolder = Path.Combine(destinationBase, subfolder);

                // Copy the entire subfolder recursively.
                FileHelper.CopyDirectory(sourceSubfolder, destinationSubfolder);

                // Define the base mod folder path (i.e. the "morven_patch_NFR" folder inside the mod folder).
                string modBasePath = Path.Combine(modFolderPath, "morven_patch_NFR");

                // Build the full path for each file to delete (Patch).
                string fileToDelete1 = Path.Combine(modBasePath, "events", "religion_events", "heresy_events.txt");
                string fileToDelete2 = Path.Combine(modBasePath, "common", "on_action", "religion_on_actions.txt");

                // Check if the file exists and delete it if it does.
                if (File.Exists(fileToDelete1))
                {
                    File.Delete(fileToDelete1);
                }

                if (File.Exists(fileToDelete2))
                {
                    File.Delete(fileToDelete2);
                }
            }

            try
            {
                // Update the mod files with the correct game version
                ModFileUpdater.UpdateModFiles(gameVersion);

                // Simulate patching logic with an asynchronous delay.
                await Task.Delay(1000);

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Success: ",
                    Foreground = Avalonia.Media.Brushes.Green
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "The files have been successfully patched.",
                    Foreground = Avalonia.Media.Brushes.White
                });
            }
            catch (Exception ex)
            {
                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "Critical Error: ",
                    Foreground = Avalonia.Media.Brushes.Red
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
                {
                    Text = "" + ex.Message,
                    Foreground = Avalonia.Media.Brushes.Yellow
                });

                ConsoleOutputTextBlock.Inlines.Add(new Avalonia.Controls.Documents.Run
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
