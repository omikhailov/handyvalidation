using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    public class RulesValidator<T> : Validator, IValueValidator<T>, INotifyPropertyChanged
    {
        protected IAsyncValidationRule<T, object>[] _asyncRules;

        protected IValidationRule<T, object>[] _syncRules;

        public RulesValidator(params IValidationRule<T, object>[] rules)
        {
            Init(rules);
        }

        public RulesValidator(IEnumerable<IValidationRule<T, object>> rules)
        {
            Init(rules);
        }

        protected virtual void Init(IEnumerable<IValidationRule<T, object>> rules)
        {
            _asyncRules = rules.OfType<IAsyncValidationRule<T, object>>().ToArray();

            _syncRules = rules.OfType<IValidationRule<T, object>>().ToArray();
        }

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
