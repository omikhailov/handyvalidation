using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    public class CompositeValidator : Validator, IValidator, IValidatable
    {
        protected IValidatable[] _items;

        protected Task _lastValidateOperation;

        protected CancellationTokenSource _cts;

        public CompositeValidator(params IValidatable[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            _items = items;
        }

        public CompositeValidator(IEnumerable<IValidatable> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            _items = items.ToArray();
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
