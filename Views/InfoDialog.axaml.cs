using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Morven_Compatch_NFR_Patcher.ViewModels;

namespace Morven_Compatch_NFR_Patcher.Views
{
    /*=============================================================================================*
     * Class: InfoDialog
     *
     * Description:
     *   This dialog window displays informational text that is provided by its view model.
     *   It includes an OK button that closes the window.
     *=============================================================================================*/
    public partial class InfoDialog : Window
    {
        // Expose the view model for the dialog.
        public InfoDialogViewModel ViewModel { get; } = new InfoDialogViewModel();

        // Constructor: Initializes the dialog and sets the DataContext to the view model.
        public InfoDialog()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        // Loads the XAML defined for this dialog.
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /*=============================================================================================*
         * Function: OkButton_Click
         *
         * Description:
         *   Event handler for the OK button click event.
         *   When invoked, it closes the dialog.
         *
         * @var sender: The source of the event.
         * @var e: The event data.
         *=============================================================================================*/
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
