using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using HandyValidation.Rules;

namespace HandyValidation
{
    public static partial class Rule
    {
        public static IValidationRule<string, object> RegExp(string expression, RegexOptions options = RegexOptions.None)
        {
            return new RegularExpressionValidationRule(expression, options);
        }

        public static IValidationRule<string, object> Email()
        {
            return new StringIsEmailAddressValidationRule();
        }

        public static IValidationRule<string, object> Uri(UriKind uriKind = UriKind.Absolute)
        {
            return new StringIsUriValidationRule(uriKind);
        }

        public static IValidationRule<string, object> Guid()
        {
            return new StringIsGuidValidationRule();
        }

        public static IValidationRule<string, object> Int()
        {
            return new StringIsIntegerValidationRule();
        }

        public static IValidationRule<string, object> Double()
        {
            return new StringIsDoubleValidationRule();
        }

        public static IValidationRule<string, object> Decimal(NumberStyles numberStyles = NumberStyles.Any, IFormatProvider formatProvider = null)
        {
            return new StringIsDecimalValidationRule(numberStyles, formatProvider);
        }

        public static IValidationRule<string, object> DateTime(DateTimeStyles dateTimeStyles = DateTimeStyles.None, IFormatProvider formatProvider = null)
        {
            return new StringIsDateTimeValidationRule(dateTimeStyles, formatProvider);
        }

        public static IValidationRule<string, object> TimeSpan(IFormatProvider formatProvider = null)
        {
            return new StringIsTimeSpanValidationRule(formatProvider);
        }

        public static IValidationRule<string, object> NotNullOrEmpty()
        {
            return new StringIsNotNullOrEmptyValidationRule();
        }

        public static IValidationRule<string, object> NotNullOrWhiteSpace()
        {
            return new StringIsNotNullOrWhiteSpaceValidationRule();
        }

        public static IValidationRule<string, object> MaxLength(int symbols)
        {
            return new StringMaxLengthValidationRule(symbols);
        }

        public static IValidationRule<string, object> MinLength(int symbols)
        {
            return new StringMinLengthValidationRule(symbols);
        }

        public static IValidationRule<string, object> LengthIsInRange(int leastAllowedLength, int greatestAllowedLength)
        {
            return new StringLengthIsInRangeValidationRule(leastAllowedLength, greatestAllowedLength);
        }

        public static IValidationRule<string, object> AllowedSymbols(string symbols)
        {
            return new StringContainsOnlyAllowedSymbolsValidationRule(symbols);
        }

        public static IValidationRule<string, object> DigitsLettersOrWhiteSpace()
        {
            return new StringContainsOnlyDigitsLettersOrWhiteSpaceValidationRule();
        }

        public static IValidationRule<string, object> DigitsOrLetters()
        {
            return new StringContainsOnlyDigitsOrLettersValidationRule();
        }

        public static IValidationRule<string, object> Digits()
        {
            return new StringContainsOnlyDigitsValidationRule();
        }

        public static IValidationRule<string, object> NumberOfDigits(params int[] counts)
        {
            return NumberOfDigits((IEnumerable<int>)counts);
        }

        public static IValidationRule<string, object> NumberOfDigits(IEnumerable<int> counts)
        {
            return new StringContainsOnlyAllowedNumberOfDigitsValidationRule(counts);
        }

        public static IValidationRule<string, object> Letters()
        {
            return new StringContainsOnlyLettersValidationRule();
        }

        public static IValidationRule<string, object> Lowercase()
        {
            return new StringIsLowercaseValidationRule();
        }

        public static IValidationRule<string, object> Uppercase()
        {
            return new StringIsUppercaseValidationRule();
        }

        public static IValidationRule<string, object> StartsWith(StringComparison stringComparisonType, params string[] allowedPrefixes)
        {
            return StartsWith(stringComparisonType, (IEnumerable<string>)allowedPrefixes);
        }

        public static IValidationRule<string, object> StartsWith(StringComparison stringComparisonType, IEnumerable<string> allowedPrefixes)
        {
            return new StringStartsWithValidationRule(stringComparisonType, allowedPrefixes);
        }

        public static IValidationRule<string, object> EndsWith(StringComparison stringComparisonType, params string[] allowedPrefixes)
        {
            return EndsWith(stringComparisonType, (IEnumerable<string>)allowedPrefixes);
        }

        public static IValidationRule<string, object> EndsWith(StringComparison stringComparisonType, IEnumerable<string> allowedPrefixes)
        {
            return new StringEndsWithValidationRule(stringComparisonType, allowedPrefixes);
        }
    }
}
