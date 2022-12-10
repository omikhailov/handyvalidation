using System;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Asynchronous validation rule
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <typeparam name="I">Type of the issue</typeparam>
    public interface IAsyncValidationRule<in T, I> : IValidationRule<T, Task<I>>
    {
    }
}
