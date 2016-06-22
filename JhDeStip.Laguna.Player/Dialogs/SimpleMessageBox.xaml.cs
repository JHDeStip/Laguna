using System;
using System.Windows;

namespace JhDeStip.Laguna.Player.Dialogs
{
    /// <summary>
    /// Interaction logic for SimpleMessageBox.xaml
    /// </summary>
    public partial class SimpleMessageBox : Window
    {
        public SimpleMessageBox()
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
