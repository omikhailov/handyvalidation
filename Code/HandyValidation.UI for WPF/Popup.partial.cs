using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Effects;
using XamlPopup = System.Windows.Controls.Primitives.Popup;
using XamlBorder = System.Windows.Controls.Border;

namespace HandyValidation.UI
{
    public sealed partial class Popup : DependencyObject
    {
        private const double PopupBottomMargin = 16;

        private static void SetPopupShadow(FrameworkElement element, XamlPopup popup)
        {
            popup.AllowsTransparency = true;

            var border = (XamlBorder)popup.Child;

            border.Margin = new Thickness(0, 0, 16, 16);

            border.Effect = new DropShadowEffect() { BlurRadius = 16, Opacity = 0.14 };
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        private static async Task SetXamlRoot(FrameworkElement element, XamlPopup popup)
        {
            ;
        }

        private static async Task SetPopupIsOpen(XamlPopup popup, bool isOpen)
        {
            if (popup == null) return;

            popup.IsOpen = isOpen;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        private static MethodInfo _repositionMethod;

        private static void SetPlacement(XamlPopup popup, FrameworkElement element)
        {
            popup.Placement = PlacementMode.Relative;

            popup.PlacementTarget = element;

            var window = Window.GetWindow(element);

            if (_repositionMethod == null) _repositionMethod = typeof(XamlPopup).GetMethod("Reposition", BindingFlags.NonPublic | BindingFlags.Instance);

            window.LocationChanged += (s, e) => { UpdatePopupPosition(popup); };

            window.SizeChanged += (s, e) => { UpdatePopupPosition(popup); };

            element.SizeChanged += OnElementSizeChanged;

            var child = (FrameworkElement)popup.Child;

            child.SizeChanged += (s, e) => { SetOffset(popup, element); };
        }

        private static void UpdatePopupPosition(XamlPopup popup)
        {
            if (!popup.IsOpen) return;

            _repositionMethod.Invoke(popup, null);
        }

        private static void SetOffset(XamlPopup popup, FrameworkElement element)
        {
            var content = popup.Child as FrameworkElement;

            if (content == null) return;

            popup.HorizontalOffset = element.ActualWidth - content.ActualWidth;

            popup.VerticalOffset = 0 - (content.ActualHeight + PopupBottomMargin);
        }

        private static void OnElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                var element = (FrameworkElement)sender;

                if (_state.TryGetValue(element, out var state))
                {
                    state.Border.MaxWidth = FirstMeaningfulValue(GetMaxWidth(element), element.ActualWidth, element.Width, element.DesiredSize.Width, MaxWidthFallbackValue);
                }

                element.SizeChanged -= OnElementSizeChanged;
            }
        }
    }
}
