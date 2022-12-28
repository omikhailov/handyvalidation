using System;
using System.Collections.Generic;
using System.Text;

#if UWP
using Windows.UI;
using Windows.UI.Xaml;
#endif

#if WINUI
using Windows.UI;
using Microsoft.UI.Xaml;
#endif

namespace HandyValidation.UI.Helpers
{
    internal static class ResourceHelper
    {
        private static bool _isRegistered;

        public static void RegisterLibraryResources()
        {
            if (_isRegistered) return;

            var resourceDictionary = new ResourceDictionary();

            resourceDictionary.Source = new Uri("ms-appx:///HandyValidation.UI/Themes/Generic.xaml", UriKind.Absolute);

            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Insert(0, resourceDictionary);
            }

            _isRegistered = true;
        }

        public static T Get<T>(string key)
        {
            if (Application.Current.Resources.TryGetValue(key, out var resource))
            {
                if (resource is T) return (T)resource;
            }

            return default;
        }

        public static CreateDefaultValueCallback GetDefaultValueCallbackFor(string key, object defaultValue = null)
        {
            return () => 
            {
                if (Application.Current.Resources.TryGetValue(key, out var resource))
                {
                    return resource;
                }

                return defaultValue;
            };
        }
    }
}
