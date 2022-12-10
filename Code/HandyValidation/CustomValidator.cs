using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Validator for checking arbitrary rules and conditions
    /// </summary>
    public class CustomValidator : Validator, IValidator, IValidatable
    {
        /// <summary>
        /// Validation function
        /// </summary>
        protected Func<ObservableCollection<object>, CancellationToken, Task> _function;

        /// <summary>
        /// Most recent validation task
        /// </summary>
        protected Task _lastValidateOperation;

        /// <summary>
        /// Cancellation token to cancel last validation task
        /// </summary>
        protected CancellationTokenSource _cts;

        /// <summary>
        /// Creates a new instance of CustomValidator
        /// </summary>
        /// <param name="function">Validation function</param>
        /// <exception cref="ArgumentNullException">Validation function cannot be null</exception>
        public CustomValidator(Func<ObservableCollection<object>, CancellationToken, Task> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            _function = function;
        }

        /// <summary>
        /// Validator of this validatable which is CustomValidator itself
        /// </summary>
        public IValidator Validator
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Performs validation
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for validation task</param>
        /// <returns>Task</returns>
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

        /// <summary>
        /// Performs validation
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Validate()
        { 
            await Validate(CancellationToken.None);
        }

        /// <summary>
        /// Calls validation function and sets state
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
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
