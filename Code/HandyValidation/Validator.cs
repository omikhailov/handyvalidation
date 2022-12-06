using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandyValidation
{
    public abstract class Validator : IValidator
    {
        protected ValidatorState _state;

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

        protected bool _hasIssues;

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

        protected ObservableCollection<object> _issues = new ObservableCollection<object>();

        public virtual ObservableCollection<object> Issues
        {
            get
            {
                return _issues;
            }
        }

        public object FirstIssue
        {
            get
            {
                if (_issues.Count > 0) return _issues[0];

                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Reset()
        {
            Issues.Clear();

            HasIssues = false;

            if (State != ValidatorState.NotSet) State = ValidatorState.Valid;
        }

        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
