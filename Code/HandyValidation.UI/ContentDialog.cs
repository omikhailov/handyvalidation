using System;
using System.Collections.Generic;

#if UWP
using Windows.UI.Xaml;
using XamlContentDialog = Windows.UI.Xaml.Controls.ContentDialog;
#endif

#if WINUI
using Microsoft.UI.Xaml;
using XamlContentDialog = Microsoft.UI.Xaml.Controls.ContentDialog;
#endif

namespace HandyValidation.UI
{
    public sealed class ContentDialog : DependencyObject
    {
        private static Dictionary<DependencyObject, bool> _state = new Dictionary<DependencyObject, bool>();

        public static bool GetIsOpen(DependencyObject d)
        {
            return (bool)d.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(DependencyObject d, bool value)
        {
            d.SetValue(IsOpenProperty, value);
        }

        public static DependencyProperty IsOpenProperty { get; } = DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(ContentDialog), new PropertyMetadata(false, IsOpenChanged));

        private static async void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = (XamlContentDialog)d;

            if (dialog == null) return;

            var newIsOpenPropertyValue = (bool)e.NewValue;

            var isRegistered = _state.TryGetValue(dialog, out var currentState);

            if (!isRegistered) RegisterDialog(dialog, newIsOpenPropertyValue);

            var mustSyncState = !isRegistered || isRegistered && currentState != newIsOpenPropertyValue;

            if (mustSyncState)
            {
                if (newIsOpenPropertyValue)
                {
                    await dialog.ShowAsync();
                }
                else
                {
                    dialog.Hide();
                }
            }
        }

        private static void RegisterDialog(XamlContentDialog dialog, bool isOpened)
        {
            dialog.Opened += (s, e) => 
            {
                _state[dialog] = true;

                SetIsOpen(dialog, true);
            };

            dialog.Closed += (s, e) => 
            {
                _state[dialog] = false;

                SetIsOpen(dialog, false); 
            };

            _state.Add(dialog, isOpened);
        }
    }
}
