using System.Windows.Controls;

using JhDeStip.Laguna.Player.ViewModels;

namespace JhDeStip.Laguna.Player.Views
{
    /// <summary>
    /// Interaction logic for InfoMessageView.xaml
    /// </summary>
    public partial class InfoMessageView : UserControl
    {
        public InfoMessageView()
        {
            InitializeComponent();
        }

        public InfoMessageView(string message) : this()
        {
            ((InfoMessageViewModel)DataContext).Message = message;
        }
    }
}
