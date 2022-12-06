using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    public interface IValueValidator<in T> : IValidator
    {
        Task Validate(T value, CancellationToken cancellationToken);
    }
}
