using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HandyValidation
{
    public class ValidationStateWatcher : INotifyPropertyChanged
    {
        protected IValidator[] _validators;

        public ValidationStateWatcher(IEnumerable<IValidatable> items)
        {            
            Init(items);
        }

        public ValidationStateWatcher(params IValidatable[] items)
        {
            Init(items);
        }

        protected virtual void Init(IEnumerable<IValidatable> items)
        {
            _validators = items.Select(i => i.Validator).Where(v => v != null).ToArray();

            foreach (var validator in _validators)
            {
                validator.PropertyChanged += Validator_PropertyChanged;
            }

            if (_validators.Any(v => v.State == ValidatorState.Invalid)) HasIssues = true;
        }

        protected bool _hasIssues;

        public virtual bool HasIssues
        {
            get
            {
                return _hasIssues;
            }
            set
            {
                if (_hasIssues != value)
                {
                    _hasIssues = value;

                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public virtual bool IsValid
        {
            get
            {
                return !HasIssues;
            }
            set
            {
                HasIssues = !value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void Validator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IValidator.State))
            {
                if (((IValidator)sender).State == ValidatorState.Invalid)
                {
                    HasIssues = true;
                }
                else
                {
                    if (_validators.All(v => v.State == ValidatorState.Valid)) HasIssues = false;
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
