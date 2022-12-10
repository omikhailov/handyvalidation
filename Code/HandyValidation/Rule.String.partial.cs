using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using HandyValidation.Rules;

namespace HandyValidation
{
    public static partial class Rule
    {
        /// <summary>
        /// String must match regular expression
        /// </summary>
        /// <param name="expression">Regular expression</param>
        /// <param name="options">RegEx options</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> RegExp(string expression, RegexOptions options = RegexOptions.None)
        {
            return new RegularExpressionValidationRule(expression, options);
        }

        /// <summary>
        /// String must be email address
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Email()
        {
            return new StringIsEmailAddressValidationRule();
        }

        /// <summary>
        /// String must be URI address
        /// </summary>
        /// <param name="uriKind">Uri kind</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Uri(UriKind uriKind = UriKind.Absolute)
        {
            return new StringIsUriValidationRule(uriKind);
        }

        /// <summary>
        /// String must represent Guid
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Guid()
        {
            return new StringIsGuidValidationRule();
        }

        /// <summary>
        /// String must represent Int32
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Int()
        {
            return new StringIsIntegerValidationRule();
        }

        /// <summary>
        /// String must represent Double
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Double()
        {
            return new StringIsDoubleValidationRule();
        }

        /// <summary>
        /// String must represent Decimal
        /// </summary>
        /// <param name="numberStyles">NumberStyles for parsing</param>
        /// <param name="formatProvider">FormatProvider for parsing</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Decimal(NumberStyles numberStyles = NumberStyles.Any, IFormatProvider formatProvider = null)
        {
            return new StringIsDecimalValidationRule(numberStyles, formatProvider);
        }

        /// <summary>
        /// String must represent DateTime
        /// </summary>
        /// <param name="dateTimeStyles">DateTimeStyles for parsing</param>
        /// <param name="formatProvider">IFormatProvider for parsing</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> DateTime(DateTimeStyles dateTimeStyles = DateTimeStyles.None, IFormatProvider formatProvider = null)
        {
            return new StringIsDateTimeValidationRule(dateTimeStyles, formatProvider);
        }

        /// <summary>
        /// String must represent DateTimeOffset
        /// </summary>
        /// <param name="dateTimeStyles">DateTimeStyles for parsing</param>
        /// <param name="formatProvider">IFormatProvider for parsing</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> DateTimeOffset(DateTimeStyles dateTimeStyles = DateTimeStyles.None, IFormatProvider formatProvider = null)
        {
            return new StringIsDateTimeOffsetValidationRule(dateTimeStyles, formatProvider);
        }

        /// <summary>
        /// String must represent TimeSpan
        /// </summary>
        /// <param name="formatProvider">IFormatProvider for parsing</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> TimeSpan(IFormatProvider formatProvider = null)
        {
            return new StringIsTimeSpanValidationRule(formatProvider);
        }

        /// <summary>
        /// String must not be null or empty
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> NotNullOrEmpty()
        {
            return new StringIsNotNullOrEmptyValidationRule();
        }

        /// <summary>
        /// String must not be null or whitespace
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> NotNullOrWhiteSpace()
        {
            return new StringIsNotNullOrWhiteSpaceValidationRule();
        }

        /// <summary>
        /// String must have the specified length or be shorter
        /// </summary>
        /// <param name="symbols">Maximim allowed length</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> MaxLength(int symbols)
        {
            return new StringMaxLengthValidationRule(symbols);
        }

        /// <summary>
        /// String must have the specified length or be longer
        /// </summary>
        /// <param name="symbols">Minimum length allowed</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> MinLength(int symbols)
        {
            return new StringMinLengthValidationRule(symbols);
        }

        /// <summary>
        /// The length of the string must be between the specified values or be equal to one of them
        /// </summary>
        /// <param name="leastAllowedLength">Least allowed length (inclusive)</param>
        /// <param name="greatestAllowedLength">Greatest allowed length (inclusive)</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> LengthIsInRange(int leastAllowedLength, int greatestAllowedLength)
        {
            return new StringLengthIsInRangeValidationRule(leastAllowedLength, greatestAllowedLength);
        }

        /// <summary>
        /// String must contain only specified symbols
        /// </summary>
        /// <param name="symbols">Allowed symbols</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> AllowedSymbols(string symbols)
        {
            return new StringContainsOnlyAllowedSymbolsValidationRule(symbols);
        }

        /// <summary>
        /// String must contain only digits, letters and whitespaces
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> DigitsLettersOrWhiteSpace()
        {
            return new StringContainsOnlyDigitsLettersOrWhiteSpaceValidationRule();
        }

        /// <summary>
        /// String must contain only digits or letters
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> DigitsOrLetters()
        {
            return new StringContainsOnlyDigitsOrLettersValidationRule();
        }

        /// <summary>
        /// String must contain only digits
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Digits()
        {
            return new StringContainsOnlyDigitsValidationRule();
        }

        /// <summary>
        /// String must contain specified count of digits. But it still can contain non-digits.
        /// </summary>
        /// <param name="counts">Allowed counts</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> NumberOfDigits(params int[] counts)
        {
            return NumberOfDigits((IEnumerable<int>)counts);
        }

        /// <summary>
        /// String must contain specified count of digits. But it still can contain non-digits.
        /// </summary>
        /// <param name="counts">Allowed counts</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> NumberOfDigits(IEnumerable<int> counts)
        {
            return new StringContainsOnlyAllowedNumberOfDigitsValidationRule(counts);
        }

        /// <summary>
        /// String must contain only letters
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Letters()
        {
            return new StringContainsOnlyLettersValidationRule();
        }

        /// <summary>
        /// All symbols in the string must be in lower case
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Lowercase()
        {
            return new StringIsLowercaseValidationRule();
        }

        /// <summary>
        /// All symbols in the string must be in upper case
        /// </summary>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> Uppercase()
        {
            return new StringIsUppercaseValidationRule();
        }

        /// <summary>
        /// String must start with one of the specified prefixes
        /// </summary>
        /// <param name="stringComparisonType">String comparison type</param>
        /// <param name="allowedPrefixes">Allowed prefixes</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> StartsWith(StringComparison stringComparisonType, params string[] allowedPrefixes)
        {
            return StartsWith(stringComparisonType, (IEnumerable<string>)allowedPrefixes);
        }

        /// <summary>
        /// String must start with one of the specified prefixes
        /// </summary>
        /// <param name="stringComparisonType">String comparison type</param>
        /// <param name="allowedPrefixes">Allowed prefixes</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> StartsWith(StringComparison stringComparisonType, IEnumerable<string> allowedPrefixes)
        {
            return new StringStartsWithValidationRule(stringComparisonType, allowedPrefixes);
        }

        /// <summary>
        /// String must end with one of the specified postfixes
        /// </summary>
        /// <param name="stringComparisonType">String comparison type</param>
        /// <param name="allowedPrefixes">Allowed postfixes</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> EndsWith(StringComparison stringComparisonType, params string[] allowedPrefixes)
        {
            return EndsWith(stringComparisonType, (IEnumerable<string>)allowedPrefixes);
        }

        /// <summary>
        /// String must end with one of the specified postfixes
        /// </summary>
        /// <param name="stringComparisonType">String comparison type</param>
        /// <param name="allowedPostfixes">Allowed postfixes</param>
        /// <returns>Validation rule</returns>
        public static IValidationRule<string, object> EndsWith(StringComparison stringComparisonType, IEnumerable<string> allowedPostfixes)
        {
            return new StringEndsWithValidationRule(stringComparisonType, allowedPostfixes);
        }
    }
}
