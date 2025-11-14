using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DormApp.Dialogs
{
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog(string question)
        {
            InitializeComponent();
            TxtQuestion.Text = question;
        }

        private void BtnYes_Click(object? sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void BtnNo_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
    }
}
