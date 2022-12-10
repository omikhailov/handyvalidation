using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Interface for validator checking some specific value
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public interface IValueValidator<in T> : IValidator
    {
        /// <summary>
        /// Validates value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        Task Validate(T value, CancellationToken cancellationToken);
    }
}
