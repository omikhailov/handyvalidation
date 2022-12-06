using System;
using HandyValidation.Rules;

namespace HandyValidation
{
    public static partial class Rule
    {
        public static IValidationRule<object, object> NotNull()
        {
            return new ObjectIsNotNullValidationRule();
        }

        public static IValidationRule<T, object> Custom<T>(Func<T, object> expression)
        {
            return new CustomValidationRule<T>(expression);
        }

        public static IValidationRule<T, object> Values<T>(params T[] allowedValues) where T : IEquatable<T>
        {
            return new IEquatableAllowedValuesValidationRule<T>(allowedValues);
        }

        public static IValidationRule<T, object> GreaterThan<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsGreaterThanValidationRule<T>(value);
        }

        public static IValidationRule<T, object> LessThan<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsLessThanValidationRule<T>(value);
        }

        public static IValidationRule<T, object> AtLeast<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsAtLeastValidationRule<T>(value);
        }

        public static IValidationRule<T, object> NotMoreThan<T>(T value) where T : IComparable<T>
        {
            return new IComparableIsNotMoreThanValidationRule<T>(value);
        }

        public static IValidationRule<T, object> Range<T>(T leastAllowedValue, T greatestAllowedValue) where T : IComparable<T>
        {
            return new IComparableIsInRangeValidationRule<T>(leastAllowedValue, greatestAllowedValue);
        }

        public static IValidationRule<T, object> Between<T>(T leftOuterBorder, T rightOuterBorder) where T : IComparable<T>
        {
            return new IComparableIsBetweenValidationRule<T>(leftOuterBorder, rightOuterBorder);
        }
    }
}
