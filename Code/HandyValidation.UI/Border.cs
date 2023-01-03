using System;
using System.Collections.Generic;
using System.Text;
using HandyValidation.UI.Helpers;

#if UWP
using Windows.Media.Audio;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

#if WINUI
using Windows.Media.Audio;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
#endif

#if WPF
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endif

namespace HandyValidation.UI
{
    /// <summary>
    /// A service to highlight Control's Border
    /// </summary>
    public sealed class Border : DependencyObject
    {
        static Border()
        {
            ResourceHelper.RegisterLibraryResources();
            
            var highlightingBrushPropertyMetadata = ResourceHelper.GetPropertyMetadataFor("ValidationDefaultBorderHighlightingBrush", HighlightingBrushChanged);

            HighlightingBrushProperty = DependencyProperty.RegisterAttached("HighlightingBrush", typeof(Brush), typeof(Border), highlightingBrushPropertyMetadata);
        }

        private static Dictionary<DependencyObject, Brush> NormalBrush = new Dictionary<DependencyObject, Brush>();

        /// <summary>
        /// Highlighting brush
        /// </summary>
        public static DependencyProperty HighlightingBrushProperty { get; }

        /// <summary>
        /// Gets the value of HighlightingBrushProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Brush</returns>
        public static Brush GetHighlightingBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(HighlightingBrushProperty);
        }

        /// <summary>
        /// Sets the value of HighlightingBrushProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">Property value</param>
        public static void SetHighlightingBrush(DependencyObject d, Brush value)
        {
            d.SetValue(HighlightingBrushProperty, value);
        }

        private static void HighlightingBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue)) return;

            if (NormalBrush.ContainsKey(d)) 
            {
                ((Control)d).BorderBrush = (Brush)e.NewValue;
            }
        }

        /// <summary>
        /// Flag indicating whether highlighting brush is applied to Control's BorderBrush or not
        /// </summary>
        public static DependencyProperty IsHighlightedProperty { get; } = DependencyProperty.RegisterAttached("IsHighlighted", typeof(bool), typeof(Border), new PropertyMetadata(false, IsHighlightedChanged));

        /// <summary>
        /// Gets the value of IsHighlightedProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Boolean</returns>
        public static bool GetIsHighlighted(DependencyObject d)
        {
            return (bool)d.GetValue(IsHighlightedProperty);
        }

        /// <summary>
        /// Sets the value of IsHighlightedProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">Property value</param>
        public static void SetIsHighlighted(DependencyObject d, bool value) 
        { 
            d.SetValue(IsHighlightedProperty, value);
        }

        private static void IsHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Control;

            if (control == null) return;

            if ((bool)e.OldValue == false && (bool)e.NewValue == true) 
            {
                NormalBrush[d] = control.BorderBrush;

                control.BorderBrush = (Brush)control.GetValue(HighlightingBrushProperty);
            }
            else
            if ((bool)e.OldValue == true && (bool)e.NewValue == false)
            {
                control.BorderBrush = NormalBrush[d];

                NormalBrush.Remove(d);
            }
        }
    }
}
