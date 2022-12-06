using System;
using System.Threading;

namespace HandyValidation
{
    public interface IValidationRule<in T, out I>
    {
        I Validate(T value, CancellationToken cancellationToken);
    }
}
