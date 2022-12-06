using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    public class CustomValidator : Validator, IValidator, IValidatable
    {
        protected Func<ObservableCollection<object>, CancellationToken, Task> _function;

        protected Task _lastValidateOperation;

        protected CancellationTokenSource _cts;

        public CustomValidator(Func<ObservableCollection<object>, CancellationToken, Task> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            _function = function;
        }

        public IValidator Validator
        {
            get
            {
                return this;
            }
        }

        public virtual async Task Validate(CancellationToken cancellationToken)
        {
            var token = cancellationToken;

            if (cancellationToken == CancellationToken.None)
            {
                _cts?.Cancel();

                _cts?.Dispose();

                _cts = new CancellationTokenSource();

                token = _cts.Token;
            }

            if (_lastValidateOperation != null && !_lastValidateOperation.IsFaulted && !_lastValidateOperation.IsCanceled)
            {
                try
                {
                    await _lastValidateOperation;
                }
                catch (OperationCanceledException) { }
            }

            _lastValidateOperation = InternalValidate(token);

            await _lastValidateOperation;
        }

        public virtual async Task Validate()
        { 
            await Validate(CancellationToken.None);
        }

        protected virtual async Task InternalValidate(CancellationToken token)
        {
            Reset();

            await _function(_issues, token);

            if (token.IsCancellationRequested)
            {
                _issues.Clear();

                return;
            }

            HasIssues = _issues.Count > 0;

            State = HasIssues ? ValidatorState.Invalid : ValidatorState.Valid;
        }
    }
}
