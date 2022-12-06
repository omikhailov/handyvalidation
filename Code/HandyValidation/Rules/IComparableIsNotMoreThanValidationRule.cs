using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class IComparableIsNotMoreThanValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _greatestAllowedValue;

        public IComparableIsNotMoreThanValidationRule(T greatestAllowedValue)
        {
            _greatestAllowedValue = greatestAllowedValue;
        }

        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && value.CompareTo(_greatestAllowedValue) > 0) return string.Empty;

            return null;   
        }
    }
}