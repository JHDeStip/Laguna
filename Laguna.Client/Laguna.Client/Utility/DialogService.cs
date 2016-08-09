using System;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace JhDeStip.Laguna.Client.Utility
{
    /// <summary>
    /// INavigationService implementation.
    /// This code is not mine but was originally written by Laurent Bugnion.
    /// </summary>
    public class DialogService : IDialogService
    {
        private NavigationPage _navigation;

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public async Task ShowMessage(string message, string title)
        {
            await _navigation.DisplayAlert(title, message, "Ok");
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            return await _navigation.DisplayAlert(title, message, buttonConfirmText, buttonCancelText);
        }

        public Task ShowMessageBox(string message, string title)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes the dialog service. It sets a new NavigationPage instance.
        /// </summary>
        /// <param name="navigation">NavigationPage instance to show dialogs with.</param>
        public void Initialize(NavigationPage navigation)
        {
            _navigation = navigation;
        }
    }
}