using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Validator for the set of other validators and properties
    /// </summary>
    public class CompositeValidator : Validator, IValidator, IValidatable
    {
        /// <summary>
        /// Validatable items
        /// </summary>
        protected IValidatable[] _items;

        /// <summary>
        /// Most recent validation task
        /// </summary>
        protected Task _lastValidateOperation;

        /// <summary>
        /// Cancellation token to cancel last validation task
        /// </summary>
        protected CancellationTokenSource _cts;

        /// <summary>
        /// Creates a new instance of CompositeValidator
        /// </summary>
        /// <param name="items">Validatable items such as properties and validators</param>
        /// <exception cref="ArgumentNullException">Validatable items array cannot be null</exception>
        public CompositeValidator(params IValidatable[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            _items = items;
        }

        /// <summary>
        /// Creates a new instance of CompositeValidator
        /// </summary>
        /// <param name="items">Validatable items such as properties and validators</param>
        /// <exception cref="ArgumentNullException">Validatable items sequence cannot be null</exception>
        public CompositeValidator(IEnumerable<IValidatable> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            _items = items.ToArray();
        }

        /// <summary>
        /// Validator of this validatable which is CompositeValidator itself
        /// </summary>
        public IValidator Validator
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Validates all validatable items
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
        /// Validates all validatable items
        /// </summary>
        /// <returns>Task</returns>
        public virtual async Task Validate()
        {
            await Validate(CancellationToken.None);
        }

        /// <summary>
        /// Goes through all validatable items and calls their Validate()
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual async Task InternalValidate(CancellationToken token)
        {
            Reset();

            await Task.WhenAll(_items.Select(i => i.Validate(token)));

            if (token.IsCancellationRequested) return;

            foreach (var item in _items)
            {
                if (item.Validator.HasIssues)
                {
                    foreach (var issue in item.Validator.Issues) Issues.Add(issue);

                    HasIssues = true;

                    State = ValidatorState.Invalid;
                }
            }

            if (Issues.Count == 0) State = ValidatorState.Valid;
        }
    }
}
