using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Validatable item such as property or validator
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Item's validator
        /// </summary>
        IValidator Validator { get; }

        /// <summary>
        /// Forces validatable item to use its validator
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel validation</param>
        /// <returns>Task</returns>
        Task Validate(CancellationToken cancellationToken);

        /// <summary>
        /// Forces validatable item to use its validator
        /// </summary>
        /// <returns>Task</returns>
        Task Validate();
    }
}
