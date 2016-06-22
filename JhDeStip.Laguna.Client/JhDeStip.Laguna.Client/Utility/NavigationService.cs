using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace JhDeStip.Laguna.Client.Utility
{
    /// <summary>
    /// INavigationService implementation.
    /// This code is not mine but was originally written by Laurent Bugnion.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private NavigationPage _navigation;

        /// <summary>
        /// Returns the key of the page that's currently displayed.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation.CurrentPage == null)
                        return null;

                    var pageType = _navigation.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType) ? _pagesByKey.First(p => p.Value == pageType).Key : null;
                }
            }
        }

        /// <summary>
        /// Navigates to the previous page.
        /// </summary>
        public void GoBack()
        {
            _navigation.PopAsync();
        }

        /// <summary>
        /// Navigates to the page who's key is given.
        /// </summary>
        /// <param name="pageKey">Key of the page to navigate to.</param>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// Navigates to the page who's key is given and also passes a parameter to the page navigating to.
        /// </summary>
        /// <param name="pageKey">Key of the page to navigate to.</param>
        /// <param name="parameter">Paramter to pass to the page navigating to.</param>
        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
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
                        throw new InvalidOperationException("No suitable constructor found for page " + pageKey);

                    var page = constructor.Invoke(parameters) as Page;
                    _navigation.PushAsync(page);
                }
                else
                    throw new ArgumentException(string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", pageKey), "pageKey");
            }
        }

        /// <summary>
        /// Configur a page to enable navigation towards it.
        /// </summary>
        /// <param name="pageKey">Key for the new page.</param>
        /// <param name="pageType">Type of the page.</param>
        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                    _pagesByKey[pageKey] = pageType;
                else
                    _pagesByKey.Add(pageKey, pageType);
            }
        }

        /// <summary>
        /// Initializes the navigation service. It sets a new NavigationPage instance.
        /// </summary>
        /// <param name="navigation">NavigationPage instance to do navigation with.</param>
        public void Initialize(NavigationPage navigation)
        {
            _navigation = navigation;
        }
    }
}
