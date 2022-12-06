using System.Threading.Tasks;

namespace HandyValidation
{
    public interface IAsyncValue<T>
    {
        Task<T> GetAsync();

        Task SetAsync(T value);
    }
}
