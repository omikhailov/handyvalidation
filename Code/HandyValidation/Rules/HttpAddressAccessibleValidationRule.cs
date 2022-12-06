using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation.Rules
{
    public class HttpAddressAccessibleValidationRule<T> : IAsyncValidationRule<T, object>
    {
        public async Task<object> Validate(T value, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            return "Ok";
        }
    }
}
