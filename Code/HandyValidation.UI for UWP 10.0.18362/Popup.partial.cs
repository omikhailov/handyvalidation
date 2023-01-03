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
        #region Shadow

        private static Shadow DefaultShadow = new ThemeShadow();

        /// <summary>
        /// Gets the value of ShadowProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Shadow</returns>
        public static Shadow GetShadow(DependencyObject d)
        {
            return (Shadow)d.GetValue(ShadowProperty);
        }

        /// <summary>
        /// Sets the value of ShadowProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New shadow</param>
        public static void SetShadow(DependencyObject d, Shadow value)
        {
            d.SetValue(ShadowProperty, value);
        }

        /// <summary>
        /// Popup shadow
        /// </summary>
        public static DependencyProperty ShadowProperty { get; } = DependencyProperty.RegisterAttached("Shadow", typeof(Shadow), typeof(Popup), new PropertyMetadata(DefaultShadow, ShadowChanged));

        private static void ShadowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Popup.Shadow = (Shadow)e.NewValue;
            }
        }

        #endregion

        private static void SetPopupShadow(FrameworkElement element, XamlPopup popup)
        {
            popup.Shadow = GetShadow(element);

            popup.Translation = new System.Numerics.Vector3(0, 0, 32);
        }

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
