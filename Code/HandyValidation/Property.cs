using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Base class for Property&lt;T&gt;
    /// </summary>
    public abstract class Property : IValidatable, ISwitchable, INotifyPropertyChanged
    {
        /// <summary>
        /// Validator of this validatable property
        /// </summary>
        IValidator IValidatable.Validator
        {
            get
            {
                return GetIValidator();
            }
        }

        /// <summary>
        /// Backing field for IsEnabled property
        /// </summary>
        protected bool _isEnabled = true;

        /// <summary>
        /// Flag indicating that property is enabled
        /// </summary>
        public virtual bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Standard PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Get Property Validator as IValidator
        /// </summary>
        protected abstract IValidator GetIValidator();

        /// <summary>
        /// Validates last set value of this property
        /// </summary>
        /// <param name="token">Cancellation token for validation task</param>
        /// <returns>Task</returns>
        public abstract Task Validate(CancellationToken token);

        /// <summary>
        /// Validates last set value of this property
        /// </summary>
        public virtual async Task Validate()
        {
            await Validate(CancellationToken.None);
        }

        /// <summary>
        /// Fires PropertyChanged event
        /// </summary>
        /// <param name="property">Property name</param>
        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public static Property[] List(params Property[] properties)
        {
            return properties;
        }
    }
}
