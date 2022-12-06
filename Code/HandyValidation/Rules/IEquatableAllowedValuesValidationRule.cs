using System;
using System.Threading;

namespace HandyValidation.Rules
{
    public class IEquatableAllowedValuesValidationRule<T> : IValidationRule<T, object> where T : IEquatable<T>
    {
        private readonly T[] _values;

        public IEquatableAllowedValuesValidationRule(T[] values)
        {
            _values = values;
        }

        public virtual object Validate(T value, CancellationToken cancellationToken = default)
        {
            foreach (var allowed in _values) if (value.Equals(allowed)) return null;

            return string.Empty;
        }
    }
}