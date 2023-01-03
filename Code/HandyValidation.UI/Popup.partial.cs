using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HandyValidation.UI.Helpers;
using System.Xml;
using System.Reflection.PortableExecutable;

#if UWP
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using XamlPopup = Windows.UI.Xaml.Controls.Primitives.Popup;
using XamlBorder = Windows.UI.Xaml.Controls.Border;

#elif WINUI
using Windows.System;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using XamlPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;
using XamlBorder = Microsoft.UI.Xaml.Controls.Border;

#elif WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using XamlPopup = System.Windows.Controls.Primitives.Popup;
using XamlBorder = System.Windows.Controls.Border;
#endif

namespace HandyValidation.UI
{
    /// <summary>
    /// Popup service
    /// </summary>
    public sealed partial class Popup : DependencyObject
    {
        private const string ItemsControl =
@"
                <ItemsControl xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <ItemsControl.ItemsPanel>
                        <ItemsPanelTemplate>
                            <StackPanel />
                        </ItemsPanelTemplate>
                    </ItemsControl.ItemsPanel>
                    <ItemsControl.ItemTemplate>
                        <DataTemplate>
                            <TextBlock Text=""{Binding}"" TextWrapping=""Wrap"" />
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
";
        private const int MaxWidthFallbackValue = 320;

        private static Dictionary<DependencyObject, PopupState> _state = new Dictionary<DependencyObject, PopupState>();

        static Popup()
        {
            ResourceHelper.RegisterLibraryResources();

            var borderBrushPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupBorderBrush", BorderBrushChanged);

            BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(Popup), borderBrushPropertyMetadata);

            var borderThicknessPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupBorderThickness", BorderThicknessChanged);

            BorderThicknessProperty = DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(Popup), borderThicknessPropertyMetadata);

            var cornerRadiusPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupCornerRadius", CornerRadiusChanged);

            CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(Popup), cornerRadiusPropertyMetadata);

            var maxWidthPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupMaxWidth", double.PositiveInfinity, MaxWidthChanged);
            
            MaxWidthProperty = DependencyProperty.RegisterAttached("MaxWidth", typeof(double), typeof(Popup), maxWidthPropertyMetadata);

            var minWidthPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupMinWidth", 0.0, MinWidthChanged);

            MinWidthProperty = DependencyProperty.RegisterAttached("MinWidth", typeof(double), typeof(Popup), minWidthPropertyMetadata);

            var widthPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupWidth", double.NaN, WidthChanged);

            WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(double), typeof(Popup), widthPropertyMetadata);

            var paddingPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupPadding", PaddingChanged);

            PaddingProperty = DependencyProperty.RegisterAttached("Padding", typeof(Thickness), typeof(Popup), paddingPropertyMetadata);

            var backgroundPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupBackgroundBrush", BackgroundChanged);

            BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(Popup), backgroundPropertyMetadata);

            var foregroundPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultPopupForegroundBrush", ForegroundChanged);

            ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(Popup), foregroundPropertyMetadata);

            var defaultItemTemplate = ResourceHelper.Get<DataTemplate>("ValidationDefaultPopupItemTemplate");

            ItemTemplateProperty = DependencyProperty.RegisterAttached("ItemTemplate", typeof(DataTemplate), typeof(Popup), new PropertyMetadata(defaultItemTemplate, ItemTemplateChanged));
    }

        #region IsOpen
        /// <summary>
        /// Gets the value of IsOpenProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Boolean</returns>
        public static bool GetIsOpen(DependencyObject d)
        {
            return (bool)d.GetValue(IsOpenProperty);
        }

        /// <summary>
        /// Sets the value of IsOpenProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">Property value</param>
        public static void SetIsOpen(DependencyObject d, bool value)
        {
            d.SetValue(IsOpenProperty, value);
        }

        /// <summary>
        /// A flag indicating whether Popup is open or not
        /// </summary>
        public static DependencyProperty IsOpenProperty { get; } = DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(Popup), new PropertyMetadata(false, IsOpenChanged));

        private static async void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state)) await SetPopupIsOpen(state.Popup, (bool)e.NewValue);
        }

        #endregion

        #region ItemsSource

        /// <summary>
        /// Gets the value of ItemsSourceProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Sequence of objects</returns>
        public static IEnumerable<object> GetItemsSource(DependencyObject d)
        {
            return (IEnumerable<object>)d.GetValue(ItemsSourceProperty);
        }

        /// <summary>
        /// Sets the value of ItemsSourceProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">Sequence of objects</param>
        public static void SetItemsSource(DependencyObject d, IEnumerable<object> value)
        {
            d.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Source items for popup
        /// </summary>
        public static DependencyProperty ItemsSourceProperty { get; } = DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable<object>), typeof(Popup), new PropertyMetadata(null, ItemsSourceChanged));

        private static async void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;

            if (element == null) return;

            if (!_state.TryGetValue(d, out var state))
            {
                if (e.NewValue != null) await RegisterPopupFor(element, e.NewValue);
            }
            else
            {
                state.ItemsControl.ItemsSource = (IEnumerable<object>)e.NewValue;
            }
        }

        #endregion

        #region Background

        /// <summary>
        /// Gets the value of BackgroundProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Brush</returns>
        public static Brush GetBackground(DependencyObject d)
        {
            return (Brush)d.GetValue(BackgroundProperty);
        }

        /// <summary>
        /// Sets the value of BackgroundProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">Brush</param>
        public static void SetBackground(DependencyObject d, Brush value)
        {
            d.SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// Popup background
        /// </summary>
        public static DependencyProperty BackgroundProperty { get; }

        private static void BackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.BorderBrush = (Brush)e.NewValue;
            }
        }

        #endregion

        #region Foreground

        /// <summary>
        /// Gets the value of ForegroundProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Brush</returns>
        public static Brush GetForeground(DependencyObject d)
        {
            return (Brush)d.GetValue(ForegroundProperty);
        }

        /// <summary>
        /// Sets ForegroudPProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">Brush</param>
        public static void SetForeground(DependencyObject d, Brush value)
        {
            d.SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// Popup foreground
        /// </summary>
        public static DependencyProperty ForegroundProperty { get; }

        private static void ForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.ItemsControl.Foreground = (Brush)e.NewValue;
            }
        }

        #endregion

        #region Width

        /// <summary>
        /// Gets the value of WidthProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Width</returns>
        public static double GetWidth(DependencyObject d)
        {
            return (double)d.GetValue(WidthProperty);
        }

        /// <summary>
        /// Sets the value of WidthProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New width</param>
        public static void SetWidth(DependencyObject d, double value)
        {
            d.SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Popup width
        /// </summary>
        public static DependencyProperty WidthProperty { get; }

        private static void WidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.Width = (double)e.NewValue;
            }
        }

        #endregion

        #region MaxWidth

        /// <summary>
        /// Gets the value of MaxWidthProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Maximum allowed width</returns>
        public static double GetMaxWidth(DependencyObject d)
        {
            return (double)d.GetValue(MaxWidthProperty);
        }

        /// <summary>
        /// Sets the value of MaxWidthProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New maximum width</param>
        public static void SetMaxWidth(DependencyObject d, double value)
        {
            d.SetValue(MaxWidthProperty, value);
        }

        /// <summary>
        /// Maximum allowed width of popup
        /// </summary>
        public static DependencyProperty MaxWidthProperty { get; }

        private static void MaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.MaxWidth = (double)e.NewValue;
            }
        }

        #endregion

        #region MinWidth

        /// <summary>
        /// Gets the value of MinWidthProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Minimum allowed width</returns>
        public static double GetMinWidth(DependencyObject d)
        {
            return (double)d.GetValue(MinWidthProperty);
        }

        /// <summary>
        /// Sets the value of MinWidthProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New minimum allowed width</param>
        public static void SetMinWidth(DependencyObject d, double value)
        {
            d.SetValue(MinWidthProperty, value);
        }

        /// <summary>
        /// Minimum allowed width of popup
        /// </summary>
        public static DependencyProperty MinWidthProperty { get; }

        private static void MinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.MinWidth = (double)e.NewValue;
            }
        }

        #endregion

        #region CornerRadius

        /// <summary>
        /// Gets the value of CornerRadiusProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Corner radius</returns>
        public static CornerRadius GetCornerRadius(DependencyObject d)
        {
            return (CornerRadius)d.GetValue(CornerRadiusProperty);
        }

        /// <summary>
        /// Sets the value of CornerRadiusProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New corner radius</param>
        public static void SetCornerRadius(DependencyObject d, CornerRadius value)
        {
            d.SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Corner radius of popup's border
        /// </summary>
        public static DependencyProperty CornerRadiusProperty { get; }

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.CornerRadius = (CornerRadius)e.NewValue;
            }
        }

        #endregion

        #region BorderThickness

        /// <summary>
        /// Gets the value of BorderThicknessProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Thickness</returns>
        public static Thickness GetBorderThickness(DependencyObject d)
        {
            return (Thickness)d.GetValue(BorderThicknessProperty);
        }

        /// <summary>
        /// Sets the value of BorderThicknessProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New thickness</param>
        public static void SetBorderThickness(DependencyObject d, Thickness value)
        {
            d.SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// Thickness of popup's border
        /// </summary>
        public static DependencyProperty BorderThicknessProperty { get; }

        private static void BorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.BorderThickness = (Thickness)e.NewValue;
            }
        }

        #endregion

        #region BorderBrush

        /// <summary>
        /// Gets the value of BorderBrushProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Brush</returns>
        public static Brush GetBorderBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(BorderBrushProperty);
        }

        /// <summary>
        /// Sets the value of BorderBrushProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New brush</param>
        public static void SetBorderBrush(DependencyObject d, Brush value)
        {
            d.SetValue(BorderBrushProperty, value);
        }

        /// <summary>
        /// Popup border brush
        /// </summary>
        public static DependencyProperty BorderBrushProperty { get; }

        private static void BorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.BorderBrush = (Brush)e.NewValue;
            }
        }

        #endregion

        #region Padding

        /// <summary>
        /// Gets the value of PaddingProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Padding</returns>
        public static Thickness GetPadding(DependencyObject d)
        {
            return (Thickness)d.GetValue(PaddingProperty);
        }

        /// <summary>
        /// Sets the value of PaddingProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New padding</param>
        public static void SetPadding(DependencyObject d, Thickness value)
        {
            d.SetValue(PaddingProperty, value);
        }

        /// <summary>
        /// Popup padding
        /// </summary>
        public static DependencyProperty PaddingProperty { get; }

        private static void PaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.Padding = (Thickness)e.NewValue;
            }
        }

        #endregion

        #region ItemTemplate

        /// <summary>
        /// Gets the value of ItemTemplateProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Template</returns>
        public static DataTemplate GetItemTemplate(DependencyObject d)
        {
            return (DataTemplate)d.GetValue(ItemTemplateProperty);
        }

        /// <summary>
        /// Sets the value of ItemTemplateProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New template</param>
        public static void SetItemTemplate(DependencyObject d, DataTemplate value)
        {
            d.SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Template for popup items
        /// </summary>
        public static DependencyProperty ItemTemplateProperty { get; }

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.ItemsControl.ItemTemplate = (DataTemplate)e.NewValue;
            }
        }

        #endregion

        private static async Task RegisterPopupFor(FrameworkElement element, object itemsSource)
        {
            var popup = new XamlPopup();

            await SetXamlRoot(element, popup);

            element.LostFocus += async (s, e) => { await SetPopupIsOpen(popup, false); };

            element.GotFocus += async (s, e) => { if (GetIsOpen(element)) await SetPopupIsOpen(popup, true); };
#if WPF
            element.KeyDown += async (s, e) => { if (e.Key == Key.Escape) await SetPopupIsOpen(popup, false); };
#else
            element.KeyDown += async (s, e) => { if (e.Key == VirtualKey.Escape) await SetPopupIsOpen(popup, false); };
#endif
            var border = new XamlBorder()
            {
                Background = GetBackground(element),
                BorderBrush = GetBorderBrush(element),
                BorderThickness = GetBorderThickness(element),
                CornerRadius = GetCornerRadius(element),
                MaxWidth = FirstMeaningfulValue(GetMaxWidth(element), element.ActualWidth, element.Width, element.DesiredSize.Width, MaxWidthFallbackValue),
                MinWidth = GetMinWidth(element),
                Padding = GetPadding(element),
            };

            var width = GetWidth(element);

            if (width != double.NaN) { border.Width = width; }

            popup.Child = border;

            SetPopupShadow(element, popup);

            ItemsControl itemsControl;

#if WPF
            using (var sr = new StringReader(ItemsControl)) 
            {
                using (var xr = XmlReader.Create(sr)) 
                {
                    itemsControl = (ItemsControl)XamlReader.Load(xr);
                }
            }
#else
            using (var reader = new StringReader(ItemsControl))
            {
                itemsControl = (ItemsControl)XamlReader.Load(reader.ReadToEnd());
            }
#endif

            var customTemplate = GetItemTemplate(element);

            if (customTemplate != null) itemsControl.ItemTemplate = customTemplate;

            itemsControl.ItemsSource = GetItemsSource(element);

            itemsControl.Foreground = GetForeground(element);

            border.Child = itemsControl;

            SetPlacement(popup, element);

            _state.Add(element, new PopupState() { Popup = popup, Border = border, ItemsControl = itemsControl });

            if ((bool)element.GetValue(IsOpenProperty)) await SetPopupIsOpen(popup, true);
        }

        private static double FirstMeaningfulValue(params double[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                if (!double.IsNaN(value) && !double.IsInfinity(value) && value != 0) return value;
            }

            return 0;
        }
    }

    class PopupState
    {
        public XamlPopup Popup;

        public XamlBorder Border;

        public ItemsControl ItemsControl;
    }
}
