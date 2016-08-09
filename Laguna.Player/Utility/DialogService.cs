using JhDeStip.Laguna.Player.Dialogs;

namespace JhDeStip.Laguna.Player.Utility
{
    public class DialogService : IDialogService
    {
        public void ShowMessageBox(string title, string message)
        {
            new SimpleMessageBox().ShowDialog(title, message);
        }

        public void ShowErrorMessageBox(string title, string message)
        {
            new SimpleErrorMessageBox().ShowDialog(title, message);
        }
    }
}
