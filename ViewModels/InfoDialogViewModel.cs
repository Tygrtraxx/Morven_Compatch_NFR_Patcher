/*=============================================================================================*
 * Class: InfoDialogViewModel
 *
 * Description:
 *   This ViewModel is responsible for providing the data for the InfoDialog.
 *   It includes a Message property that notifies the UI when changed.
 *=============================================================================================*/

using CommunityToolkit.Mvvm.ComponentModel;

namespace Morven_Compatch_NFR_Patcher.ViewModels
{
    public partial class InfoDialogViewModel : ObservableObject
    {
        // The message to display in the dialog.
        [ObservableProperty]
        private string message = "Default explanation text.";
    }
}
