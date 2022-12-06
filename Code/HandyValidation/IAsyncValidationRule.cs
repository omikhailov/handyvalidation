using System;
using System.Threading.Tasks;

namespace HandyValidation
{
    public interface IAsyncValidationRule<in T, I> : IValidationRule<T, Task<I>>
    {
    }
}
