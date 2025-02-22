/*=============================================================================================*
* Class: MainViewModel
*
*   Description: 
*   This ViewModel serves as the primary data context for the main UI. It holds the 
*   folder paths for the Steam folder and mod folder, and provides computed properties:
*   - 'CanPatch' indicates whether both folders have been specified.
*
*   The class inherits from ViewModelBase, which in turn inherits from ObservableObject.
*   ObservableObject implements INotifyPropertyChanged so that the UI automatically updates 
*   when properties change. The [ObservableProperty] attribute generates properties and 
*   raises change notifications automatically.
*=============================================================================================*/

using System;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Morven_Compatch_NFR_Patcher.Helpers;

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

        public string AppVersion => VersionHelper.AppVersion;

        // CanPatch returns true only if both SteamFolder and ModFolder are not null or whitespace.
        public bool CanPatch => !string.IsNullOrWhiteSpace(SteamFolder) && !string.IsNullOrWhiteSpace(ModFolder);

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
