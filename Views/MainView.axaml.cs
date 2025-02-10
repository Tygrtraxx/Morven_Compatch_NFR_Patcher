/*=============================================================================================*
 * Class: MainView
 *
 * Description:
 *   This is the code-behind for the MainView UserControl. It initializes the UI components
 *   defined in the associated XAML file (MainView.axaml) and handles user interactions,
 *   such as clicks on the question mark buttons which display an InfoDialog.
 *=============================================================================================*/

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Morven_Compatch_NFR_Patcher.Views
{
    public partial class MainView : UserControl
    {
        // Constructor: Initializes a new instance of MainView and loads its UI components.
        public MainView()
        {
            InitializeComponent();
        }

        // Event handler for the Steam question mark button click.
        // It creates an InfoDialog and displays it modally relative to the parent window.
        private async void SteamQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            await dialog.ShowDialog(GetParentWindow());
        }

        // Event handler for the Mod question mark button click.
        // It creates an InfoDialog and displays it modally relative to the parent window.
        private async void ModQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            await dialog.ShowDialog(GetParentWindow());
        }

        // Retrieves the parent window of this control.
        // It attempts to cast the VisualRoot to a Window. If unsuccessful, it throws an exception.
        private Window GetParentWindow()
        {
            // Try to cast VisualRoot to Window.
            var window = this.VisualRoot as Window;
            if (window == null)
            {
                // If VisualRoot is null or not a Window, throw an exception.
                throw new InvalidOperationException("The control is not attached to a Window.");
            }
            return window;
        }
    }
}
