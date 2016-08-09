using System;
using System.Windows;

namespace JhDeStip.Laguna.Player.Dialogs
{
    /// <summary>
    /// Interaction logic for SimpleErrorMessageBox.xaml
    /// </summary>
    public partial class SimpleErrorMessageBox : Window
    {
        public SimpleErrorMessageBox()
        {
            InitializeComponent();
        }

        public Nullable<bool> ShowDialog(string title, string message)
        {
            Title = title;
            Message.Content = message;
            return this.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
