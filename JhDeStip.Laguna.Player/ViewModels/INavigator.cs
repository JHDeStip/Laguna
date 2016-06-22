using System.Windows.Controls;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public interface INavigator
    {
        UserControl CurrentView { get; set; }
    }
}
