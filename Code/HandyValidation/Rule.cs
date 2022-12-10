using System;
using HandyValidation.Rules;

namespace HandyValidation
{
    /// <summary>
    /// A set of predefined rules
    /// </summary>
    public static partial class Rule
    {
        /// <summary>
        /// Value must not be null
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<object, object> NotNull()
        {
            return new ObjectIsNotNullValidationRule();
        }

        /// <summary>
        /// Custom validation rule
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="expression">Validation expression returning issue or null</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> Custom<T>(Func<T, object> expression)
        {
            return new CustomValidationRule<T>(expression);
        }

        /// <summary>
        /// Must be one of the listed values
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="allowedValues">List of the allowed values</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> Values<T>(params T[] allowedValues) where T : IEquatable<T>
        {
            return new IEquatableAllowedValuesValidationRule<T>(allowedValues);
        }

        /// <summary>
        /// Value must be greater than the specified value
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Border</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> GreaterThan<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsGreaterThanValidationRule<T>(value);
        }

        /// <summary>
        /// Value must be less than the specified value
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Border</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> LessThan<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsLessThanValidationRule<T>(value);
        }

        /// <summary>
        /// Value must be equal or greater than the specified value
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Border</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> AtLeast<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsAtLeastValidationRule<T>(value);
        }

        /// <summary>
        /// Value must be equal or less than the specified value
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Border</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> NotMoreThan<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsNotMoreThanValidationRule<T>(value);
        }

        /// <summary>
        /// Value must be between two specified values or equal to one of them
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="leastAllowedValue">Left border (inclusive)</param>
        /// <param name="greatestAllowedValue">Right border (inclusive)</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> Range<T>(T leastAllowedValue, T greatestAllowedValue) where T : IComparable<T>
        {
            return new IComparableIsInRangeValidationRule<T>(leastAllowedValue, greatestAllowedValue);
        }

        /// <summary>
        /// Value must be between two specified values
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="leftOuterBorder">Left border (exclusive)</param>
        /// <param name="rightOuterBorder">Right border (exclusive)</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<T, object> Between<T>(T leftOuterBorder, T rightOuterBorder) where T : IComparable<T>
        {
            return new IComparableIsBetweenValidationRule<T>(leftOuterBorder, rightOuterBorder);
        }
    }
}
