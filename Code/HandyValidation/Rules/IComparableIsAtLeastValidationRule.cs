using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class IComparableIsAtLeastValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _leastAllowedValue;

        public IComparableIsAtLeastValidationRule(T leastAllowedValue)
        {
            _leastAllowedValue = leastAllowedValue;
        }

        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.CompareTo(_leastAllowedValue) < 0) return string.Empty;

            return null;   
        }
    }
}