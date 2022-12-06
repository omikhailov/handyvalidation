using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;

#if UWP
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using XamlPopup = Windows.UI.Xaml.Controls.Primitives.Popup;
using XamlBorder = Windows.UI.Xaml.Controls.Border;

#elif WINUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using XamlPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;
using XamlBorder = Microsoft.UI.Xaml.Controls.Border;
#endif

namespace HandyValidation.UI
{
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

        private static Dictionary<DependencyObject, PopupState> _state = new Dictionary<DependencyObject, PopupState>();

        #region IsOpen

        public static bool GetIsOpen(DependencyObject d)
        {
            return (bool)d.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(DependencyObject d, bool value)
        {
            d.SetValue(IsOpenProperty, value);
        }

        public static DependencyProperty IsOpenProperty { get; } = DependencyProperty.RegisterAttached(nameof(IsOpenProperty), typeof(bool), typeof(Popup), new PropertyMetadata(false, IsOpenChanged));

        private static async void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state)) await SetPopupIsOpen(state.Popup, (bool)e.NewValue);
        }

        #endregion

        #region ItemsSource

        public static IEnumerable<object> GetItemsSource(DependencyObject d)
        {
            return (IEnumerable<object>)d.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject d, IEnumerable<object> value)
        {
            d.SetValue(ItemsSourceProperty, value);
        }

        public static DependencyProperty ItemsSourceProperty { get; } = DependencyProperty.RegisterAttached(nameof(ItemsSourceProperty), typeof(IEnumerable<object>), typeof(Popup), new PropertyMetadata(null, ItemsSourceChanged));

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
                state.ItemsControl.ItemsSource = e.NewValue;
            }
        }

        #endregion

        #region Background

        private static Brush DefaultBackgroundBrush = new SolidColorBrush(Color.FromArgb(255, 253, 231, 233));

        public static Brush GetBackground(DependencyObject d)
        {
            return (Brush)d.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject d, Brush value)
        {
            d.SetValue(BackgroundProperty, value);
        }

        public static DependencyProperty BackgroundProperty { get; } = DependencyProperty.RegisterAttached(nameof(BackgroundProperty), typeof(Brush), typeof(Popup), new PropertyMetadata(DefaultBackgroundBrush, BackgroundBrushChanged));

        private static void BackgroundBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.BorderBrush = (Brush)e.NewValue;
            }
        }

        #endregion

        #region Foreground

        private static readonly Brush DefaultForegroundBrush = new SolidColorBrush(Colors.Black);

        public static Brush GetForeground(DependencyObject d)
        {
            return (Brush)d.GetValue(ForegroundProperty);
        }

        public static void SetForeground(DependencyObject d, Brush value)
        {
            d.SetValue(ForegroundProperty, value);
        }

        public static DependencyProperty ForegroundProperty { get; } = DependencyProperty.RegisterAttached(nameof(ForegroundProperty), typeof(Brush), typeof(Popup), new PropertyMetadata(DefaultForegroundBrush, ForegroundBrushChanged));

        private static void ForegroundBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.ItemsControl.Foreground = (Brush)e.NewValue;
            }
        }

        #endregion

        #region MaxWidth

        public static double GetMaxWidth(DependencyObject d)
        {
            return (double)d.GetValue(MaxWidthProperty);
        }

        public static void SetMaxWidth(DependencyObject d, double value)
        {
            d.SetValue(MaxWidthProperty, value);
        }

        public static DependencyProperty MaxWidthProperty { get; } = DependencyProperty.RegisterAttached(nameof(MaxWidthProperty), typeof(double), typeof(Popup), new PropertyMetadata(0.0, MaxWidthChanged));

        private static void MaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.MaxWidth = (double)e.NewValue;
            }
        }

        #endregion

        #region CornerRadius

        public static CornerRadius GetCornerRadius(DependencyObject d)
        {
            return (CornerRadius)d.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject d, CornerRadius value)
        {
            d.SetValue(CornerRadiusProperty, value);
        }

        public static DependencyProperty CornerRadiusProperty { get; } = DependencyProperty.RegisterAttached(nameof(CornerRadiusProperty), typeof(CornerRadius), typeof(Popup), new PropertyMetadata(new CornerRadius(12), CornerRadiusChanged));

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.CornerRadius = (CornerRadius)e.NewValue;
            }
        }

        #endregion

        #region Padding

        public static Thickness GetPadding(DependencyObject d)
        {
            return (Thickness)d.GetValue(PaddingProperty);
        }

        public static void SetPadding(DependencyObject d, Thickness value)
        {
            d.SetValue(PaddingProperty, value);
        }

        public static DependencyProperty PaddingProperty { get; } = DependencyProperty.RegisterAttached(nameof(PaddingProperty), typeof(Thickness), typeof(Popup), new PropertyMetadata(new Thickness(20, 8, 16, 12), PaddingChanged));

        private static void PaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_state.TryGetValue(d, out var state))
            {
                state.Border.Padding = (Thickness)e.NewValue;
            }
        }

        #endregion

        #region ItemTemplate

        public static DataTemplate GetItemTemplate(DependencyObject d)
        {
            return (DataTemplate)d.GetValue(ItemTemplateProperty);
        }

        public static void SetItemTemplate(DependencyObject d, DataTemplate value)
        {
            d.SetValue(ItemTemplateProperty, value);
        }

        public static DependencyProperty ItemTemplateProperty { get; } = DependencyProperty.RegisterAttached(nameof(ItemTemplateProperty), typeof(DataTemplate), typeof(Popup), new PropertyMetadata(null, ItemTemplateChanged));

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

            element.KeyDown += async (s, e) => { if (e.Key == VirtualKey.Escape) await SetPopupIsOpen(popup, false); };

            var border = new XamlBorder()
            {
                Background = GetBackground(element),
                CornerRadius = GetCornerRadius(element),
                MaxWidth = FirstMeaningfulValue(GetMaxWidth(element), element.ActualWidth, element.Width, element.DesiredSize.Width, 320),
                Padding = GetPadding(element)
            };

            popup.Child = border;

            ItemsControl itemsControl;

            using (var reader = new StringReader(ItemsControl))
            {
                itemsControl = (ItemsControl)XamlReader.Load(reader.ReadToEnd());
            }

            var customTemplate = GetItemTemplate(element);

            if (customTemplate != null) itemsControl.ItemTemplate = customTemplate;

            itemsControl.ItemsSource = GetItemsSource(element);

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

                if (!double.IsNaN(value) && value != 0) return value;
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
