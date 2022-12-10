using System;
using HandyValidation.Rules;
using Windows.ApplicationModel.Resources;

namespace HandyValidation
{
    /// <summary>
    /// Extensions for wrapping silent validation rules into rules with messages
    /// </summary>
    public static class ValidationRulesExtensions
    {
        /// <summary>
        /// When rule returns not null, replace result with specified resource string from default .resx file
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceKey)
        {
            return new VerboseStaticValidationRule<T>(rule, GetResourceString(resourceKey));
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from default .resx file
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceKey)
        {
            return new VerboseStaticAsyncValidationRule<T>(rule, GetResourceString(resourceKey));
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from specified .resx file
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceFile">Resource file name</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return new VerboseStaticValidationRule<T>(rule, GetResourceString(resourceFile, resourceKey));
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from specified .resx file
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceFile">Resource file name</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return new VerboseStaticAsyncValidationRule<T>(rule, GetResourceString(resourceFile, resourceKey));
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from default .resx file, formatted by specified function
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <param name="messageFunction">Formatting function</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicValidationRule<T>(rule, (value) => 
            {
                return messageFunction(value, GetResourceString(resourceKey));
            });
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from default .resx file, formatted by specified function
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <param name="messageFunction">Formatting function</param>
        /// <returns>Validation rule</returns>
        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicAsyncValidationRule<T>(rule, (value) =>
            {
                return messageFunction(value, GetResourceString(resourceKey));
            });
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from specified .resx file, formatted by specified function
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <param name="resourceFile">Resource file name</param>
        /// <param name="messageFunction">Formatting function</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, string> WithResourceString<T>(this IValidationRule<T, object> rule, string resourceFile, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicValidationRule<T>(rule, (value) =>
            {
                return messageFunction(value, GetResourceString(resourceFile, resourceKey));
            });
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from specified .resx file, formatted by specified function
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <param name="resourceFile">Resource file name</param>
        /// <param name="messageFunction">Formatting function</param>
        /// <returns>Validation rule</returns>
        public static IAsyncValidationRule<T, string> WithResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceFile, string resourceKey, Func<T, string, string> messageFunction)
        {
            return new VerboseDynamicAsyncValidationRule<T>(rule, (value) =>
            {
                return messageFunction(value, GetResourceString(resourceFile, resourceKey));
            });
        }

        /// <summary>
        /// When rule returns null, replace result with specified resource string from default .resx file and format it using String.Format()
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, string> WithFormattedResourceString<T>(this IValidationRule<T, object> rule, string resourceKey)
        {
            return rule.WithResourceString(resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        /// <summary>
        /// When rule returns null, replace result with specified resource string from default .resx file and format it using String.Format()
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IAsyncValidationRule<T, string> WithFormattedResourceString<T>(this IAsyncValidationRule<T, object> rule, string resourceKey)
        {
            return rule.WithResourceString(resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from specified .resx file and format it using String.Format()
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceFile">Resource file name</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, string> WithFormattedResourceString<T>(this IValidationRule<T, object> rule, string resourceFile, string resourceKey)
        {
            return rule.WithResourceString(resourceFile, resourceKey, (value, resourceString) => string.Format(resourceString, value));
        }

        /// <summary>
        /// When rule returns not null, replace result with specified resource string from specified .resx file and format it using String.Format()
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="resourceFile">Resource file name</param>
        /// <param name="resourceKey">Name of the resource string</param>
        /// <returns>Validation rule</returns>
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
