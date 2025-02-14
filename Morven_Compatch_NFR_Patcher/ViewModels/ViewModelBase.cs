/*=============================================================================================*
* Class: ViewModelBase
*
* Description: Serves as a base class for all ViewModel classes in the application.
* It inherits from ObservableObject, which implements INotifyPropertyChanged.
* This provides built-in support for property change notifications,
* enabling the UI to update automatically when properties in the ViewModel change.
*
* Note: Although no additional code is present now, any ViewModel that derives from ViewModelBase
*       will have the necessary functionality for data binding, and common functionality can be added here in the future.
*=============================================================================================*/
using CommunityToolkit.Mvvm.ComponentModel;

namespace Morven_Compatch_NFR_Patcher.ViewModels;

public class ViewModelBase : ObservableObject
{
}
