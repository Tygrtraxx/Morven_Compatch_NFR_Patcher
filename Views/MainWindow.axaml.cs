/*=============================================================================================*
 * Class: MainWindow.axaml.cs
 *
 * Description: Adds a partial class that defines the code-behind for MainWindow.axaml and
 * it inherits from Avalonia.Controls.Window, making it the main window of the application.
 * 
 * It also creates a constructor that initializes a new instance of the MainWindow class.
 * The call to InitializeComponent() loads and parses the associated XAML file,
 * wiring up the UI elements defined in the XAML to this code-behind class.
 *
 *=============================================================================================*/
using Avalonia.Controls;

namespace Morven_Compatch_NFR_Patcher.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
