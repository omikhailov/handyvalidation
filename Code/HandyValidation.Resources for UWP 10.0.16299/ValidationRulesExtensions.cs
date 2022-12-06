using System;
using HandyValidation.Rules;
using Windows.ApplicationModel.Resources;

namespace HandyValidation
{
    public static class ValidationRulesExtensions
    {

        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceKey)
        {
            return new VerboseStaticValidationRule<T>(rule, GetResourceString(resourceKey));
        }

        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceKey)
        {
            return new VerboseStaticAsyncValidationRule<T>(rule, GetResourceString(resourceKey));
        }

        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return new VerboseStaticValidationRule<T>(rule, GetResourceString(resourceFile, resourceKey));
        }

        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return new VerboseStaticAsyncValidationRule<T>(rule, GetResourceString(resourceFile, resourceKey));
        }

        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicValidationRule<T>(rule, (value) => 
            {
                return messageFunction(value, GetResourceString(resourceKey));
            });
        }

        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicAsyncValidationRule<T>(rule, (value) =>
            {
                return messageFunction(value, GetResourceString(resourceKey));
            });
        }

        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceFile, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicValidationRule<T>(rule, (value) =>
            {
                return messageFunction(value, GetResourceString(resourceFile, resourceKey));
            });
        }

        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceFile, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicAsyncValidationRule<T>(rule, (value) =>
            {
                return messageFunction(value, GetResourceString(resourceFile, resourceKey));
            });
        }

        public static IValidationRule<T, string> WithFormattedResourceString<T>(this IValidationRule<T, object> rule, string resourceKey)
        {
            return rule.WithResourceString(resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        public static IAsyncValidationRule<T, string> WithFormattedResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceKey)
        {
            return rule.WithResourceString(resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        public static IValidationRule<T, string> WithFormattedResourceString<T>(this IValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return rule.WithResourceString(resourceFile, resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        public static IAsyncValidationRule<T, string> WithFormattedResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return rule.WithResourceString(resourceFile, resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        private static string GetResourceString(string resourceKey)
        {
            var resourceLoader = ResourceLoader.GetForViewIndependentUse();

            return resourceLoader.GetString(resourceKey);
        }

        private static string GetResourceString(string resourceFile, string resourceKey)
        {
            var resourceLoader = ResourceLoader.GetForViewIndependentUse(resourceFile);

            return resourceLoader.GetString(resourceKey);
        }
    }
}
