﻿using System.Threading;

namespace HandyValidation.Rules
{
    public class StringContainsOnlyDigitsLettersOrWhiteSpaceValidationRule : IValidationRule<string, object>
    {
        public virtual object Validate(string value, CancellationToken cancellationToken = default)
        {
            if (value == null) return null;

            foreach (var c in value) if (!(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))) return string.Empty;

            return null;
        }
    }
}