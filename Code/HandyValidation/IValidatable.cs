using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    public interface IValidatable
    {
        IValidator Validator { get; }

        Task Validate(CancellationToken cancellationToken);

        Task Validate();
    }
}
