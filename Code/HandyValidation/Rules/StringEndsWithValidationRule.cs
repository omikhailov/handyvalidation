﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace HandyValidation.Rules
{
    public class StringEndsWithValidationRule : IValidationRule<string, object>
    {
        private readonly StringComparison _stringComparisonType;

        private readonly IEnumerable<string> _prefixes;

        public StringEndsWithValidationRule(StringComparison stringComparisonType, IEnumerable<string> prefixes)
        {
            _stringComparisonType = stringComparisonType;

            _prefixes = prefixes;
        }

        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            foreach (var prefix in _prefixes)
            {
                if (value.EndsWith(prefix, _stringComparisonType)) return null;
            }

            return string.Empty;
        }
    }
}