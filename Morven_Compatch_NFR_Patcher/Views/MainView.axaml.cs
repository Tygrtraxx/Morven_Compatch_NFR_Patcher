/*=============================================================================================*
*   Class: MainView
*
*   Description:
*       This is the code-behind for the MainView UserControl. It initializes the UI components
*       defined in the associated XAML file (MainView.axaml) and handles user interactions,
*       such as clicks on the question mark buttons which display an InfoDialog.
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
using Avalonia.Media;
using Avalonia.Controls.Converters;
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
            dialog.ViewModel.Message = "Open Steam’s Settings, navigate to \n Downloads -> Steam Library Folder \n and check the installation path.";
            await dialog.ShowDialog(GetParentWindow());
        }

        // Event handler for the Mod question mark button click.
        // It creates an InfoDialog, sets its message for the Mod folder, and shows the dialog modally.
        private async void ModQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            dialog.ViewModel.Message = "Typically the mod folder is in your user’s Documents or AppData folder.";
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

        // Checks if the button is processing to prevent text spam from rapid button pressing.
        private bool _isProcessing = false;

        // Event handler for the Patch button click.
        private async void PatchButton_Click(object sender, RoutedEventArgs e)
        {
            // Prevent re-entrancy: if already processing, exit.
            if (_isProcessing)
            {
                return;
            }

            _isProcessing = true;
            try
            {

                // Check if the Inlines collection is unexpectedly null; if so, throw an exception to indicate a critical error.
                if (ConsoleOutputTextBlock.Inlines == null || InstructionsTextBlock.Inlines == null || InstructionsLinesTextBlock.Inlines == null)
                {
                    throw new InvalidOperationException("One of the inlines collections are unexpectedly null.");
                }

                // Clear the instructions (To make room for the upcoming messages in the ConsoleOutputTextBlock).
                ConsoleOutputTextBlock.Inlines.Clear();

                if (InstructionsBorder.Parent is Panel parentPanel)
                {
                    parentPanel.Children.Remove(InstructionsBorder);
                }

                // Remove the InstructionsLinesTextBlock if its parent is a Panel
                if (InstructionsLinesTextBlock.Parent is Panel parentPanel2)
                {
                    parentPanel2.Children.Remove(InstructionsLinesTextBlock);
                }

                // Validate that both folder paths are provided.
                if (string.IsNullOrWhiteSpace(SteamFolderTextBox.Text) || string.IsNullOrWhiteSpace(ModFolderTextBox.Text) || (!Directory.Exists(SteamFolderTextBox.Text) && !Directory.Exists(ModFolderTextBox.Text)))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Both folders are invalid or must be specified.", 1);

                    return;
                }

                // Validate that the Steam folder exists.
                if (!Directory.Exists(SteamFolderTextBox.Text))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The specified Steam folder does not exist.", 1);

                    return;
                }

                // Validate that the Mod folder exists.
                if (!Directory.Exists(ModFolderTextBox.Text))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The specified mod folder does not exist.", 1);

                    return;
                }

                // Output to the console that both files are valid.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Both file paths are valid.", 0);

                // Check if the selected folder's name is "Steam" or "steamapps".

                // Get the path entered by the user.
                string steamFolderPath = SteamFolderTextBox.Text;

                // Trim any trailing directory separators to ensure the folder name is extracted correctly.
                string trimmedSteamFolderPath = steamFolderPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                // Extract the folder name.
                string steamFolderName = Path.GetFileName(trimmedSteamFolderPath);

                if (!string.Equals(steamFolderName, "Steam", StringComparison.OrdinalIgnoreCase) && !string.Equals(steamFolderName, "steamapps", StringComparison.OrdinalIgnoreCase))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The selected Steam folder must be named \"Steam\" or \"steamapps\".", 1);

                    return;
                }

                // Output to the console that the file is named correctly.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The folder is named Steam or steamapps.", 0);

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
                        await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The selected Steam folder does not contain the \"steamapps\" folder.", 1);
                        await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "--> Are you sure this is the right Steam folder? <--", 2);

                        return;
                    }
                }

                // Output to the console that Steam has a valid steamapps folder
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The Steam folder has a valid \"steamapps\" folder in it.", 0);

                // The game and mod that needs to be patched (Morven's Mods Compatch) location
                string targetSubfolder = System.IO.Path.Combine(steamFolderPath, "workshop", "content", "1158310", "3001489429");

                // Check if the target subfolder exists.
                if (!System.IO.Directory.Exists(targetSubfolder))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The required folder \"workshop\\content\\1158310\\3001489429\" does not exist within the steamapps folder.", 1);
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "--> Are you sure you have Morven's Mods Compatch installed? <--", 2);

                    return;
                }

                // Output to the console that the mod has been found in steam.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The required mod has been found in the Steam folder.", 0);

                // Get the mod folder path entered by the user and trim any trailing separators to ensure the folder name is extracted correctly.
                string modFolderPath = ModFolderTextBox.Text.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                // Create a DirectoryInfo object for the mod folder.
                DirectoryInfo modDir = new(modFolderPath);

                // Check that the selected folder is actually named "mod".
                if (!string.Equals(modDir.Name, "mod", StringComparison.OrdinalIgnoreCase))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The selected folder must be named \"mod\".", 1);

                    return;
                }

                // Output to the console that the mod folder is named correctly.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The mod folder is named correctly.", 0);

                // Check that the parent folder exists and is named "Crusader Kings III".
                DirectoryInfo? parentDir = modDir.Parent;
                if (parentDir == null || !string.Equals(parentDir.Name, "Crusader Kings III", StringComparison.OrdinalIgnoreCase))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The selected mod folder must be within the \"Crusader Kings III\" folder.", 1);

                    return;
                }

                // Output to the console that the mod folder has a valid Crusader Kings III folder.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The mod folder has a Crusader Kings III folder.", 0);

                // Check that the grandparent folder exists and is named "Paradox Interactive".
                DirectoryInfo? grandParentDir = parentDir.Parent;
                if (grandParentDir == null || !string.Equals(grandParentDir.Name, "Paradox Interactive", StringComparison.OrdinalIgnoreCase))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The selected mod folder must be within the \"Paradox Interactive\\Crusader Kings III\\\" folder.", 1);

                    return;
                }

                // Output to the console that the mod folder has a valid Crusader Kings III folder, within a Paradox Interactive folder.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The Crusader Kings III folder is located within a folder called Paradox Interactive.", 0);

                // Define the source directory where the mod files in the patch program are located.
                string sourceDir = Path.Combine(AppContext.BaseDirectory, "Assets", "ModFiles");

                // Copy the mod file (morven_patch_NFR.mod) to the mod directory.
                FileHelper.CopyDirectory(sourceDir, modFolderPath);

                // Build the relative path from the Steam folder to the source base folder.
                string relativeSteamPath = Path.Combine("workshop", "content", "1158310", "3001489429");
                string sourceBase = Path.Combine(steamFolderPath, relativeSteamPath);

                // Specify the file name "descriptor.mod" in that directory.
                string descriptorFilePath = Path.Combine(sourceBase, "descriptor.mod");

                // Check if the file exists.
                if (!File.Exists(descriptorFilePath))
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, $"The file \"descriptor.mod\" was not found at {descriptorFilePath}.", 1);

                    return;
                }

                // Read all lines from the file (We are looking for the version in the file).
                string[] lines = File.ReadAllLines(descriptorFilePath);

                // Check if there are at least 8 lines in the file.
                if (lines.Length < 8)
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, $"The file \"descriptor.mod\" is missing its version number on line 8.", 1);

                    return;
                }

                // The 8th line is at index 7.
                string eigthLine = lines[7];

                // Use a regular expression to extract the text between the first pair of quotation marks.
                // The regex pattern \"([^\"]+)\" captures the text between quotes.
                Match match = Regex.Match(eigthLine, "\"([^\"]+)\"");
                if (!match.Success)
                {
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The game version was not found on line 8 in the \"descriptor.mod\" file.", 1);

                    return;
                }

                // Output to the console that the proper game version has been found.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The mod's game version has been found.", 0);

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
                        await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The required subfolders needed for the upcoming patch, were not found in Morven's Mods 1.14 Compatch folder.", 1);
                        await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "--> I suggest redownloading the mod, those folders should always exist. <--", 2);

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

                // Output to the console that we have copied all of the files to the new patched mod.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Copying all of the mod's files to the new patch mod.", 0);

                // Output to the console that we have successfully deleted the necessary files patching the mod.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Deleted the \"No Fervour Rebalance\" files.", 3);

                // Determine the absolute mod folder path and normalize the path to use forward slashes.
                string normalizedModFolderPath = modFolderPath.Replace("\\", "/");

                // Append the desired subfolder to the mod folder path.
                string fullModPath = $"{normalizedModFolderPath}/morven_patch_NFR";

                // Build the line that needs to be placed at the 7th line.
                string lineToInsert = $"path=\"{fullModPath}\"";

                // Specify the path to the mod file to update.
                string modFilePath = Path.Combine(modFolderPath, "morven_patch_NFR.mod");

                // Ensure the file exists before attempting to modify it.
                if (File.Exists(modFilePath))
                {
                    // Read all lines from the file
                    var fileLines = new List<string>(File.ReadAllLines(modFilePath));

                    // If the file already has at least 7 lines, replace the 7th line with the local mod's path location.
                    if (fileLines.Count >= 7)
                    {
                        fileLines[6] = lineToInsert;

                        // Output to the console that the program is changing the absolute path to the new mod.
                        await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Modifying the path, for the patched version of the mod.", 3);
                    }
                    else
                    {
                        // If the file has fewer than 7 lines, append the local mod's path location to the file.
                        fileLines.Add(lineToInsert);

                        // Output to the console that the program is changing the absolute path to the new mod.
                        await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Adding a new path, for the patched version of the mod.", 3);
                    }

                    // Write the modified content back to the file
                    File.WriteAllLines(modFilePath, fileLines);
                }

                else
                {
                    // Tell the user something went wrong and the program couldn't find an important file that should be bundled with it.
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The mod file \"morven_patch_NFR.mod\" was not found.", 1);
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "(BUG?) You shouldn't be seeing this error. Contact Tygrtraxx.", 2);
                    await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, modFilePath, 4);

                    return;
                }

                // Update the mod files with the correct game version
                ModFileUpdater.UpdateModFiles(modFolderPath, gameVersion);

                // Output to the console that we have are adding the correct version number to all of the files.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Adding the correct version number to all of the files.\n", 3);

                // Simulate patching logic with an asynchronous delay.
                await Task.Delay(700);

                // Output to the console that we have successfully patched everything and the mod is ready.
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "The mod has been patched!! It will be located in your mod folder. You must disable/uninstall his mod now to\n avoid conflicts. Otherwise, Enjoy!", 5);
            }

            catch (Exception ex)
            {
                // Output to the console that a critical error occurred
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, ex.Message, 6);
                await ConsoleOutputTextHelper.ShowStatusText(ConsoleOutputTextBlock, "Patch operation was unsuccessful.", 1);
            }

            // Finally, turn off the processing if we are done.
            finally
            {
                _isProcessing = false;
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