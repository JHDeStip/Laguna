using System;
using JhDeStip.Laguna.Player.ViewModels;

namespace JhDeStip.Laguna.Player.Utility
{
    public interface INavigationService
    {
        string CurrentViewKey { get; }

        void Configure(string viewKey, Type viewType);
        void Initialize(INavigator navigator);
        void NavigateTo(string viewKey);
        void NavigateTo(string viewKey, object parameter);
    }
}