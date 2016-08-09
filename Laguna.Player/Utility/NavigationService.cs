using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Reflection;

using JhDeStip.Laguna.Player.ViewModels;

namespace JhDeStip.Laguna.Player.Utility
{
    /// <summary>
    /// INavigationService implementation.
    /// This code is not mine but was originally written by Laurent Bugnion.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> _viewsByKey = new Dictionary<string, Type>();
        private INavigator _navigation;

        /// <summary>
        /// Returns the key of the page that's currently displayed.
        /// </summary>
        public string CurrentViewKey
        {
            get
            {
                lock (_viewsByKey)
                {
                    if (_navigation.CurrentView == null)
                        return null;

                    var pageType = _navigation.CurrentView.GetType();

                    return _viewsByKey.ContainsValue(pageType) ? _viewsByKey.First(p => p.Value == pageType).Key : null;
                }
            }
        }

        /// <summary>
        /// Navigates to the view who's key is given.
        /// </summary>
        /// <param name="viewKey">Key of the view to navigate to.</param>
        public void NavigateTo(string viewKey)
        {
            NavigateTo(viewKey, null);
        }

        /// <summary>
        /// Navigates to the view who's key is given and also passes a parameter to the view navigating to.
        /// </summary>
        /// <param name="viewKey">Key of the view to navigate to.</param>
        /// <param name="parameter">Paramter to pass to the view navigating to.</param>
        public void NavigateTo(string viewKey, object parameter)
        {
            lock (_viewsByKey)
            {
                //-- Added check for CurrentViewKey equality. If we navigate to the same view we shouldn't do anything.
                if (CurrentViewKey != viewKey)
                {
                    if (_viewsByKey.ContainsKey(viewKey))
                    {
                        var type = _viewsByKey[viewKey];
                        ConstructorInfo constructor;
                        object[] parameters;

                        if (parameter == null)
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(c => !c.GetParameters().Any());

                            parameters = new object[] { };
                        }
                        else
                        {
                            constructor = type.GetTypeInfo()
                                .DeclaredConstructors
                                .FirstOrDefault(
                                    c =>
                                    {
                                        var p = c.GetParameters();
                                        return p.Count() == 1 && p[0].ParameterType == parameter.GetType();
                                    });

                            parameters = new[] { parameter };
                        }

                        if (constructor == null)
                            throw new InvalidOperationException("No suitable constructor found for view " + viewKey);

                        var view = constructor.Invoke(parameters) as UserControl;
                        _navigation.CurrentView = view;
                    }
                    else
                        throw new ArgumentException(string.Format("No such view: {0}. Did you forget to call NavigationService.Configure?", viewKey), "viewKey");
                }
            }
        }

        /// <summary>
        /// Configur a view to enable navigation towards it.
        /// </summary>
        /// <param name="viewKey">Key for the new view.</param>
        /// <param name="viewType">Type of the view.</param>
        public void Configure(string viewKey, Type viewType)
        {
            lock (_viewsByKey)
            {
                if (_viewsByKey.ContainsKey(viewKey))
                    _viewsByKey[viewKey] = viewType;
                else
                    _viewsByKey.Add(viewKey, viewType);
            }
        }

        /// <summary>
        /// Initializes the navigation service. It sets a new INavigator instance.
        /// </summary>
        /// <param name="navigator">INavigator instance to do navigation with.</param>
        public void Initialize(INavigator navigator)
        {
            _navigation = navigator;
        }
    }
}
