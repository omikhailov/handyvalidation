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

#if WPF
using System.Windows;
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

#if WPF
            resourceDictionary.Source = new Uri("pack://application:,,,/HandyValidation.UI;component/Themes/Generic.xaml", UriKind.Absolute);
#else
            resourceDictionary.Source = new Uri("ms-appx:///HandyValidation.UI/Themes/Generic.xaml", UriKind.Absolute);
#endif
            if (!Application.Current.Resources.MergedDictionaries.Contains(resourceDictionary))
            {
                Application.Current.Resources.MergedDictionaries.Insert(0, resourceDictionary);
            }

            _isRegistered = true;
        }

        public static PropertyMetadata GetPropertyMetadataFor(string resourceName, object defaultValue, PropertyChangedCallback propertyChangedCallback)
        {
#if WPF
            return new PropertyMetadata(Get<object>(resourceName) ?? defaultValue, propertyChangedCallback);
#else
            return PropertyMetadata.Create(ResourceHelper.GetDefaultValueCallbackFor(resourceName, defaultValue), propertyChangedCallback);
#endif
        }

        public static PropertyMetadata GetPropertyMetadataFor(string resourceName, PropertyChangedCallback propertyChangedCallback) 
        {
#if WPF
            return new PropertyMetadata(Get<object>(resourceName), propertyChangedCallback);
#else
            return PropertyMetadata.Create(ResourceHelper.GetDefaultValueCallbackFor(resourceName), propertyChangedCallback);
#endif
        }

        public static T Get<T>(string key)
        {
#if WPF
            return (T)Application.Current.Resources[key];
#else
            if (Application.Current.Resources.TryGetValue(key, out var resource))
            {
                if (resource is T) return (T)resource;
            }

            return default;
#endif
        }
#if !WPF
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
#endif
    }
}
