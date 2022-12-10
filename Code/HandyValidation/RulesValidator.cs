using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// A validator that checks if a value matches certain rules
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class RulesValidator<T> : Validator, IValueValidator<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Asynchronous rules
        /// </summary>
        protected IAsyncValidationRule<T, object>[] _asyncRules;

        /// <summary>
        /// Synchronous rules
        /// </summary>
        protected IValidationRule<T, object>[] _syncRules;

        /// <summary>
        /// Creates new instance of ValueValidator
        /// </summary>
        /// <param name="rules">Validation rules</param>
        public RulesValidator(params IValidationRule<T, object>[] rules)
        {
            Init(rules);
        }

        /// <summary>
        /// Creates new instance of ValueValidator
        /// </summary>
        /// <param name="rules">Validation rules</param>
        public RulesValidator(IEnumerable<IValidationRule<T, object>> rules)
        {
            Init(rules);
        }

        /// <summary>
        /// Stores validation rules
        /// </summary>
        /// <param name="rules">Validation rules</param>
        /// <exception cref="ArgumentNullException">Rules sequence cannot be null</exception>
        protected virtual void Init(IEnumerable<IValidationRule<T, object>> rules)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));

            _asyncRules = rules.OfType<IAsyncValidationRule<T, object>>().ToArray();

            _syncRules = rules.OfType<IValidationRule<T, object>>().ToArray();
        }

        /// <summary>
        /// Validates the value
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <param name="token">Cancellation token for validation task</param>
        /// <returns>Task</returns>
        public virtual async Task Validate(T value, CancellationToken token)
        {
            Reset();

            if (_asyncRules.Length == 0)
            {
                foreach (var rule in _syncRules)
                {
                    if (token.IsCancellationRequested) return;

                    CheckForIssue(rule.Validate(value, token));
                }
            }
            else
            {
                var validationTasks = _asyncRules.Select(rule => rule.Validate(value, token)).ToList();

                foreach (var rule in _syncRules)
                {
                    if (token.IsCancellationRequested) return;

                    CheckForIssue(rule.Validate(value, token));
                }

                while (validationTasks.Count > 0)
                {
                    var completedTask = await Task.WhenAny(validationTasks);

                    if (token.IsCancellationRequested) return;

                    CheckForIssue(completedTask.Result);

                    validationTasks.Remove(completedTask);
                }
            }

            if (Issues.Count == 0) State = ValidatorState.Valid;

            void CheckForIssue(object validationResult)
            {
                if (validationResult != null)
                {
                    Issues.Add(validationResult);

                    HasIssues = true;

                    State = ValidatorState.Invalid;
                }
            }
        }
    }
}
