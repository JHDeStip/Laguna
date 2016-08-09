using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.Player.Utility
{
    public interface IDialogService
    {
        void ShowMessageBox(string title, string message);
        void ShowErrorMessageBox(string title, string message);
    }
}
