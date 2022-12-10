using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandyValidation
{
    /// <summary>
    /// Base class for validators
    /// </summary>
    public abstract class Validator : IValidator
    {
        /// <summary>
        /// Backing field for State property
        /// </summary>
        protected ValidatorState _state;

        /// <summary>
        /// Current state
        /// </summary>
        public virtual ValidatorState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Backing field for HasIssues property
        /// </summary>
        protected bool _hasIssues;

        /// <summary>
        /// True when recent validation operation found issues
        /// </summary>
        public virtual bool HasIssues
        {
            get
            {
                return _hasIssues;
            }
            protected set
            {
                if (_hasIssues != value)
                {
                    _hasIssues = value;

                    OnPropertyChanged();

                    OnPropertyChanged(nameof(FirstIssue));
                }
            }
        }

        /// <summary>
        /// Backing field for Issues property
        /// </summary>
        protected ObservableCollection<object> _issues = new ObservableCollection<object>();

        /// <summary>
        /// Issues found during the recent validation operation
        /// </summary>
        public virtual ObservableCollection<object> Issues
        {
            get
            {
                return _issues;
            }
        }

        /// <summary>
        /// First issue out of all Issues
        /// </summary>
        public object FirstIssue
        {
            get
            {
                if (_issues.Count > 0) return _issues[0];

                return null;
            }
        }

        /// <summary>
        /// Standard PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Clears validation results
        /// </summary>
        public virtual void Reset()
        {
            Issues.Clear();

            HasIssues = false;

            if (State != ValidatorState.NotSet) State = ValidatorState.Valid;
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
