using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// A value that can be retrieved or set asynchronously
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public interface IAsyncValue<T>
    {
        /// <summary>
        /// Get value
        /// </summary>
        /// <returns>Value</returns>
        Task<T> GetAsync();

        /// <summary>
        /// Set value
        /// </summary>
        /// <param name="value">New value</param>
        /// <returns>Task</returns>
        Task SetAsync(T value);
    }
}
