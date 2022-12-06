using System;
using System.Collections.Generic;
using System.Text;

#if UWP
using Windows.UI.Xaml;
using XamlStyle = Windows.UI.Xaml.Style;
#endif

#if WINUI
using Microsoft.UI.Xaml;
using XamlStyle = Microsoft.UI.Xaml.Style;
#endif


namespace HandyValidation.UI
{
    public sealed class Style : DependencyObject
    {
        private static Dictionary<DependencyObject, XamlStyle> NormalStyle = new Dictionary<DependencyObject, XamlStyle>();

        public static DependencyProperty ValueProperty { get; } = DependencyProperty.RegisterAttached("Value", typeof(XamlStyle), typeof(Style), new PropertyMetadata(null, ValueChanged));

        public static XamlStyle GetValue(DependencyObject d)
        {
            return (XamlStyle)d.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject d, XamlStyle value)
        {
            d.SetValue(ValueProperty, value);
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null && e.OldValue.Equals(e.NewValue)) return;

            if (NormalStyle.ContainsKey(d))
            {
                ((FrameworkElement)d).Style = (XamlStyle)(e.NewValue);
            }
        }

        public static DependencyProperty IsAppliedProperty { get; } = DependencyProperty.RegisterAttached("IsApplied", typeof(bool), typeof(Style), new PropertyMetadata(false, IsAppliedChanged));

        public static bool GetIsApplied(DependencyObject d)
        {
            return (bool)d.GetValue(IsAppliedProperty);
        }

        public static void SetIsApplied(DependencyObject d, bool value) 
        { 
            d.SetValue(IsAppliedProperty, value);
        }

        private static void IsAppliedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;

            if (frameworkElement == null) return;

            if ((bool)e.OldValue == false && (bool)e.NewValue == true) 
            {
                var errorStyle = (XamlStyle)frameworkElement.GetValue(ValueProperty);

                if (errorStyle == null) return;

                NormalStyle[d] = frameworkElement.Style;

                frameworkElement.Style = errorStyle;
            }
            else
            if ((bool)e.OldValue == true && (bool)e.NewValue == false)
            {
                frameworkElement.Style = NormalStyle[d];

                NormalStyle.Remove(d);
            }
        }
    }
}
