using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Morven_Compatch_NFR_Patcher.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private async void SteamQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            await dialog.ShowDialog(GetParentWindow());
        }

        private async void ModQuestionMark_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InfoDialog();
            await dialog.ShowDialog(GetParentWindow());
        }

        private Window GetParentWindow()
        {
            // Retrieves the parent window of this control.
            return this.VisualRoot as Window;
        }
    }
}
