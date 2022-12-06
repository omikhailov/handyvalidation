using System;
using System.Collections.Generic;
using System.Text;

#if UWP
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

#if WINUI
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
#endif


namespace HandyValidation.UI
{
    public sealed class Border : DependencyObject
    {
        private static Dictionary<DependencyObject, Brush> NormalBrush = new Dictionary<DependencyObject, Brush>();

        private static readonly Brush DefaultHighlightingBrush = new SolidColorBrush(Color.FromArgb(255, 196, 43, 28));

        public static DependencyProperty HighlightingBrushProperty { get; } = DependencyProperty.RegisterAttached("HighlightingBrush", typeof(Brush), typeof(Border), new PropertyMetadata(DefaultHighlightingBrush, HighlitingBrushChanged));

        public static Brush GetHighlightingBrush(DependencyObject d)
        {
            return (Brush)d.GetValue(HighlightingBrushProperty);
        }

        public static void SetHighlightingBrush(DependencyObject d, Brush value)
        {
            d.SetValue(HighlightingBrushProperty, value);
        }

        private static void HighlitingBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue)) return;

            if (NormalBrush.ContainsKey(d)) 
            {
                ((Control)d).BorderBrush = (Brush)e.NewValue;
            }
        }

        public static DependencyProperty IsHighlightedProperty { get; } = DependencyProperty.RegisterAttached("IsHighlighted", typeof(bool), typeof(Border), new PropertyMetadata(false, IsHighlightedChanged));

        public static bool GetIsHighlighted(DependencyObject d)
        {
            return (bool)d.GetValue(IsHighlightedProperty);
        }

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
