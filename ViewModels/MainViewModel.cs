using CommunityToolkit.Mvvm.ComponentModel;

namespace Morven_Compatch_NFR_Patcher.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string steamFolder;

    [ObservableProperty]
    private string modFolder;

    // CanPatch is true only if both folder properties are not null or whitespace.
    public bool CanPatch => !string.IsNullOrWhiteSpace(SteamFolder) && !string.IsNullOrWhiteSpace(ModFolder);

    partial void OnSteamFolderChanged(string value)
    {
        OnPropertyChanged(nameof(CanPatch));
    }

    partial void OnModFolderChanged(string value)
    {
        OnPropertyChanged(nameof(CanPatch));
    }
}
