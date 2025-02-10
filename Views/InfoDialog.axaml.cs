/*=============================================================================================*
 * Class: InfoDialog
 *
 * Description:
 *   This class represents a simple informational dialog window.
 *   It inherits from Avalonia.Controls.Window, which is a top-level window in an Avalonia application.
 *
 *   The dialog is initialized by loading its XAML definition via AvaloniaXamlLoader.
 *   It typically contains an OK button that, when clicked, closes the dialog.
 *=============================================================================================*/

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Morven_Compatch_NFR_Patcher.Views
{
    public partial class InfoDialog : Window
    {
        // Constructor: Initializes a new instance of InfoDialog. It then calls InitializeComponent() to load the XAML-defined UI for the dialog.
        public InfoDialog()
        {
            InitializeComponent();
        }

        // This method is automatically generated to wire up the UI components defined in the XAML.
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // OkButton_Click:
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

