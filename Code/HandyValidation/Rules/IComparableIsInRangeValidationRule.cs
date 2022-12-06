using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class IComparableIsInRangeValidationRule<T> : IValidationRule<T, object> where T : IComparable<T>
    {
        private readonly T _left;

        private readonly T _right;

        public IComparableIsInRangeValidationRule(T left, T right)
        {
            _left = left;

            _right = right;
        }

        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            if (value != null && (value.CompareTo(_left) < 0 || value.CompareTo(_right) > 0)) return string.Empty;

            return null;   
        }
    }
}