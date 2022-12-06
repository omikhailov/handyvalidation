using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using XamlPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;

namespace HandyValidation.UI
{
    public sealed partial class Popup : DependencyObject
    {
        #region Placement

        public static PopupPlacementMode GetPlacement(DependencyObject d)
        {
            return (PopupPlacementMode)d.GetValue(PlacementProperty);
        }

        public static void SetPlacement(DependencyObject d, PopupPlacementMode value)
        {
            d.SetValue(PlacementProperty, value);
        }

        public static DependencyProperty PlacementProperty { get; } = DependencyProperty.RegisterAttached(nameof(PlacementProperty), typeof(PopupPlacementMode), typeof(Popup), new PropertyMetadata(PopupPlacementMode.TopEdgeAlignedRight, PlacementChanged));

        private static void PlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Popup.DesiredPlacement = (PopupPlacementMode)e.NewValue;
            }
        }

        #endregion

        private static async Task SetXamlRoot(FrameworkElement element, XamlPopup popup)
        {
            var stopAt = DateTime.Now.AddMilliseconds(1000);

            while (element.XamlRoot == null && stopAt > DateTime.Now) await Task.Delay(1);

            popup.XamlRoot = element.XamlRoot;
        }

        private static async Task SetPopupIsOpen(XamlPopup popup, bool isOpen)
        {
            var stopAt = DateTime.Now.AddMilliseconds(1000);

            while (popup.XamlRoot == null && stopAt > DateTime.Now) await Task.Delay(1);

            if (popup != null) popup.IsOpen = isOpen;
        }

        private static void SetPlacement(XamlPopup popup, FrameworkElement element)
        {
            popup.DesiredPlacement = GetPlacement(element);

            popup.PlacementTarget = element;
        }
    }
}
