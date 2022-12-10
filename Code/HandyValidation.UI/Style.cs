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
    /// <summary>
    /// A service to swap the Style of FrameworkElement
    /// </summary>
    public sealed class Style : DependencyObject
    {
        private static Dictionary<DependencyObject, XamlStyle> NormalStyle = new Dictionary<DependencyObject, XamlStyle>();

        /// <summary>
        /// Additional style
        /// </summary>
        public static DependencyProperty ValueProperty { get; } = DependencyProperty.RegisterAttached("Value", typeof(XamlStyle), typeof(Style), new PropertyMetadata(null, ValueChanged));

        /// <summary>
        /// Gets the value of ValueProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Style</returns>
        public static XamlStyle GetValue(DependencyObject d)
        {
            return (XamlStyle)d.GetValue(ValueProperty);
        }

        /// <summary>
        /// Sets the value of ValueProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New style</param>
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

        /// <summary>
        /// A flag indicating that additional style must be applied
        /// </summary>
        public static DependencyProperty IsAppliedProperty { get; } = DependencyProperty.RegisterAttached("IsApplied", typeof(bool), typeof(Style), new PropertyMetadata(false, IsAppliedChanged));

        /// <summary>
        /// Gets the value of IsAppliedProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <returns>Boolean</returns>
        public static bool GetIsApplied(DependencyObject d)
        {
            return (bool)d.GetValue(IsAppliedProperty);
        }

        /// <summary>
        /// Sets the value of IsAppliedProperty
        /// </summary>
        /// <param name="d">Target object</param>
        /// <param name="value">New value</param>
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
