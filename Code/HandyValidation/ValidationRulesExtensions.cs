using System;
using HandyValidation.Rules;

namespace HandyValidation
{
    public static class ValidationRulesExtensions
    {
        public static IValidationRule<T, string> WithMessage<T>(this IValidationRule<T, object> rule, string message)
        {
            return new VerboseStaticValidationRule<T>(rule, message);
        }

        public static IAsyncValidationRule<T, string> WithMessage<T>(this IAsyncValidationRule<T, object> rule, string message)
        {
            return new VerboseStaticAsyncValidationRule<T>(rule, message);
        }

        public static IValidationRule<T, string> WithMessage<T>(this IValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            return new VerboseDynamicValidationRule<T>(rule, messageFunction);
        }

        public static IAsyncValidationRule<T, string> WithMessage<T>(this IAsyncValidationRule<T, object> rule, Func<T, string> messageFunction)
        {
            return new VerboseDynamicAsyncValidationRule<T>(rule, messageFunction);
        }

        public static IValidationRule<T, string> WithFormattedMessage<T>(this IValidationRule<T, object> rule, string message)
        {
            return rule.WithMessage(value => string.Format(message));
        }

        public static IAsyncValidationRule<T, string> WithFormattedMessage<T>(this IAsyncValidationRule<T, object> rule, string message)
        {
            return rule.WithMessage(value => string.Format(message));
        }
    }
}
