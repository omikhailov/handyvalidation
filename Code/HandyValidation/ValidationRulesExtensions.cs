using System;
using HandyValidation.Rules;

namespace HandyValidation
{
    /// <summary>
    /// Extensions for wrapping silent validation rules into rules with messages
    /// </summary>
    public static class ValidationRulesExtensions
    {
        /// <summary>
        /// When rule returns not null, replace result with specified message
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="message">Validation message</param>
        /// <returns>Validatoin rule</returns>
        public static IValidationRule<T, string> WithMessage<T>(this IValidationRule<T, object> rule, string message)
        {
            return new VerboseStaticValidationRule<T>(rule, message);
        }

        /// <summary>
        /// When rule returns not null, replace result with specified message
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="message">Validation message</param>
        /// <returns>Validatoin rule</returns>
        public static IAsyncValidationRule<T, string> WithMessage<T>(this IAsyncValidationRule<T, object> rule, string message)
        {
            return new VerboseStaticAsyncValidationRule<T>(rule, message);
        }

        /// <summary>
        /// When rule returns not null, dynamically replace result with specified message
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="messageFunction">Fuction returning validation message</param>
        /// <returns>Validatoin rule</returns>
        public static IValidationRule<T, string> WithMessage<T>(this IValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            return new VerboseDynamicValidationRule<T>(rule, messageFunction);
        }

        /// <summary>
        /// When rule returns not null, dynamically replace result with specified message
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="messageFunction">Fuction returning validation message</param>
        /// <returns>Validatoin rule</returns>
        public static IAsyncValidationRule<T, string> WithMessage<T>(this IAsyncValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            return new VerboseDynamicAsyncValidationRule<T>(rule, messageFunction);
        }

        /// <summary>
        /// When rule returns not null, dynamically replace result with specified formatted message
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="message">Validation message</param>
        /// <returns>Validatoin rule</returns>
        public static IValidationRule<T, string> WithFormattedMessage<T>(this IValidationRule<T, object> rule, string message)
        {
            return rule.WithMessage(value => string.Format(message, value));
        }

        /// <summary>
        /// When rule returns not null, dynamically replace result with specified formatted message
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="rule">Validation rule</param>
        /// <param name="message">Validation message</param>
        /// <returns>Validatoin rule</returns>
        public static IAsyncValidationRule<T, string> WithFormattedMessage<T>(this IAsyncValidationRule<T, object> rule, string message)
        {
            return rule.WithMessage(value => string.Format(message, value));
        }
    }
}
