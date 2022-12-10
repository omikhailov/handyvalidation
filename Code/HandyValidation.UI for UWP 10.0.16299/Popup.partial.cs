using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using XamlPopup = Windows.UI.Xaml.Controls.Primitives.Popup;

namespace HandyValidation.UI
{
    public sealed partial class Popup : DependencyObject
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        private static async Task SetXamlRoot(FrameworkElement element, XamlPopup popup)
        {
            
        }

        private static async Task SetPopupIsOpen(XamlPopup popup, bool isOpen)
        {
            if (popup != null) popup.IsOpen = isOpen;
        }

        private static void SetPopupShadow(FrameworkElement element, XamlPopup popup)
        {
            ;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        private static void SetPlacement(XamlPopup popup, FrameworkElement element)
        {
            var child = (FrameworkElement)popup.Child;

            child.SizeChanged += (s, e) => { SetOffset(popup, element); };
        }

        private static void SetOffset(XamlPopup popup, FrameworkElement element)
        {
            var content = popup.Child as FrameworkElement;

            if (content == null) return;

            UIElement parent = element;

            UIElement topParent = parent;

            while (!(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent) as UIElement;

                if (parent == null) return;

                topParent = parent;
            }

            var transform = element.TransformToVisual(topParent);

            var point = transform.TransformPoint(new Point(element.ActualWidth - content.ActualWidth, -content.ActualHeight));

            popup.HorizontalOffset = point.X;

            popup.VerticalOffset = point.Y;
        }
    }
}
