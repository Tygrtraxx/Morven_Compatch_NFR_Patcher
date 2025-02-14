/*=============================================================================================*
* Class: MainViewModel
*
* Description: 
*   This ViewModel serves as the primary data context for the main UI. It holds the 
*   folder paths for the Steam folder and mod folder, and provides computed properties:
*   - 'CanPatch' indicates whether both folders have been specified.
*   - 'AppVersion' retrieves the application's version from the executing assembly.
*
*   The class inherits from ViewModelBase, which in turn inherits from ObservableObject.
*   ObservableObject implements INotifyPropertyChanged so that the UI automatically updates 
*   when properties change. The [ObservableProperty] attribute generates properties and 
*   raises change notifications automatically.
*=============================================================================================*/

using System;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Morven_Compatch_NFR_Patcher.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        // The path to the Steam folder, initialized to an empty string to avoid nullability warnings.
        [ObservableProperty]
        private string steamFolder = string.Empty;

        // The path to the mod folder, initialized to an empty string to avoid nullability warnings.
        [ObservableProperty]
        private string modFolder = string.Empty;

        // CanPatch returns true only if both SteamFolder and ModFolder are not null or whitespace.
        public bool CanPatch => !string.IsNullOrWhiteSpace(SteamFolder) && !string.IsNullOrWhiteSpace(ModFolder);

        // AppVersion retrieves the application's version from the executing assembly.
        public static string AppVersion
        {
            get
            {
                // Retrieve the informational version (set by GitVersion)
                var infoVersion = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion;

                if (string.IsNullOrEmpty(infoVersion))
                    return "N/A";

                // Look for a '+' sign (which often indicates build metadata in semver).
                int plusIndex = infoVersion.IndexOf('+');
                if (plusIndex != -1 && plusIndex < infoVersion.Length - 1)
                {
                    // Convert the string to ReadOnlySpan<char> for efficient slicing
                    ReadOnlySpan<char> infoSpan = infoVersion.AsSpan();

                    // The part before/including '+'
                    ReadOnlySpan<char> prefix = infoSpan[..(plusIndex + 1)];

                    // The build metadata after '+'
                    ReadOnlySpan<char> metadataSpan = infoSpan[(plusIndex + 1)..];

                    // Limit the metadata to 7 characters (adjust as needed).
                    if (metadataSpan.Length > 7)
                    {
                        metadataSpan = metadataSpan[..7];
                    }

                    // Reconstruct using string.Concat for efficiency
                    infoVersion = string.Concat(prefix, metadataSpan);
                }

                return infoVersion;
            }
        }

        // This method is automatically called when the SteamFolder property changes. It notifies the UI that the CanPatch property may have changed.
        partial void OnSteamFolderChanged(string value)
        {
            OnPropertyChanged(nameof(CanPatch));
        }

        // This method is automatically called when the ModFolder property changes. It notifies the UI that the CanPatch property may have changed.
        partial void OnModFolderChanged(string value)
        {
            OnPropertyChanged(nameof(CanPatch));
        }
    }
}
