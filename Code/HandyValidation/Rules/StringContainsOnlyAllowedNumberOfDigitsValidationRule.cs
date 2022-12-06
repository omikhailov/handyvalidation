using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringContainsOnlyAllowedNumberOfDigitsValidationRule : IValidationRule<string, object>
    {
        private readonly IEnumerable<int> _counts;

        public StringContainsOnlyAllowedNumberOfDigitsValidationRule(IEnumerable<int> counts)
        {
            _counts = counts;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            var count = value.Count(c => char.IsDigit(c));

            if (!_counts.Contains(count)) return string.Empty;

            return null;
        }
    }
}