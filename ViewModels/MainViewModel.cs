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
                    // Extract the part after '+'
                    var metadata = infoVersion.Substring(plusIndex + 1);

                    // Limit the metadata to 7 characters (adjust as needed).
                    if (metadata.Length > 7)
                    {
                        metadata = metadata.Substring(0, 7);
                    }

                    // Reconstruct: everything up to '+' + shortened metadata
                    infoVersion = infoVersion.Substring(0, plusIndex + 1) + metadata;
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
