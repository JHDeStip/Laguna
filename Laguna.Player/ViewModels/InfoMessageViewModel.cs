using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace JhDeStip.Laguna.Player.ViewModels
{
    public class InfoMessageViewModel : ViewModelBase
    {
        #region Commands

        public RelayCommand<string> NavigatedToCommand { get; private set; }

        #endregion

        #region Properties for view

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    RaisePropertyChanged(nameof(Message));
                }
            }
        }

        #endregion

        public InfoMessageViewModel()
        {

        }

        private void CreateCommands()
        {
            NavigatedToCommand = new RelayCommand<string>(OnNavigatedTo);
        }

        private void OnNavigatedTo(string message)
        {
            Message = message;
        }
    }
}
