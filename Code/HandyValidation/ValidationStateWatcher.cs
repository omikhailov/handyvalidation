using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HandyValidation
{
    /// <summary>
    /// Watches specified validatable items such as properties and validators to change its state
    /// when all of them are valid or at least one didn't pass validation
    /// </summary>
    public class ValidationStateWatcher : INotifyPropertyChanged
    {
        /// <summary>
        /// List of validators
        /// </summary>
        protected IValidator[] _validators;

        /// <summary>
        /// Creates new instance of ValidationStateWatcher
        /// </summary>
        /// <param name="items">Validatable items such as properties and validators</param>
        public ValidationStateWatcher(IEnumerable<IValidatable> items)
        {            
            Init(items);
        }

        /// <summary>
        /// Creates new instance of ValidationStateWatcher
        /// </summary>
        /// <param name="items">Validatable items such as properties and validators</param>
        public ValidationStateWatcher(params IValidatable[] items)
        {
            Init(items);
        }

        /// <summary>
        /// Prepares the list of validators
        /// </summary>
        /// <param name="items"></param>
        protected virtual void Init(IEnumerable<IValidatable> items)
        {
            _validators = items.Select(i => i.Validator).Where(v => v != null).ToArray();

            foreach (var validator in _validators)
            {
                validator.PropertyChanged += Validator_PropertyChanged;
            }

            if (_validators.Any(v => v.State == ValidatorState.Invalid)) HasIssues = true;
        }

        /// <summary>
        /// Backing field for HasIssues
        /// </summary>
        protected bool _hasIssues;

        /// <summary>
        /// True when at least one of validatable items failed validation
        /// </summary>
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

        /// <summary>
        /// True when all validatable items passed validation
        /// </summary>
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

        /// <summary>
        /// Standard PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles PropertyChanged event of validators to update HasIssues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Fires PropertyChanged event
        /// </summary>
        /// <param name="property">Property name</param>
        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
