using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Class representing property of a view model
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class Property<T> : IProperty<T>, IAsyncValue<T>, IValidatable, INotifyPropertyChanged
    {
        /// <summary>
        /// Default EqualityComparer for T
        /// </summary>
        protected static EqualityComparer<T> _equalityComparer = EqualityComparer<T>.Default;

        /// <summary>
        /// Default state of the Validator that was passed as parameter in constructor
        /// </summary>
        protected readonly ValidatorState _defaultValidatorState;

        /// <summary>
        /// Most recent validation task
        /// </summary>
        protected Task _lastSetOperation;

        /// <summary>
        /// The value assigned during _lastSetOperation 
        /// </summary>
        protected T _lastSetValue;

        /// <summary>
        /// Cancellation token to cancel last validation task
        /// </summary>
        protected CancellationTokenSource _cts;

        /// <summary>
        /// Creates new instance of Property
        /// </summary>
        public Property() { }

        /// <summary>
        /// Creates new instance of Property
        /// </summary>
        /// <param name="defaultValue">Default value</param>
        /// <param name="defaultValidatorState">Validation state for the default value</param>
        public Property(T defaultValue, ValidatorState defaultValidatorState = ValidatorState.Valid)
        {
            _value = defaultValue;

            _defaultValidatorState = defaultValidatorState;
        }

        /// <summary>
        /// Backing field for Value property
        /// </summary>
        protected T _value;

        /// <summary>
        /// Property value
        /// </summary>
        public virtual T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _ = SetAsync(value);
            }
        }

        /// <summary>
        /// Backing field for IsReadonly property
        /// </summary>
        protected bool _isReadonly;

        /// <summary>
        /// Flag indicating that property is read only
        /// </summary>
        public virtual bool IsReadonly
        {
            get
            {
                return _isReadonly;
            }
            set
            {
                if (_isReadonly != value)
                {
                    _isReadonly = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Backing field for IsDirty property
        /// </summary>
        protected bool _isDirty = true;

        /// <summary>
        /// Flag indicating that Value has not yet been assigned
        /// </summary>
        public virtual bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            protected set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Backing field for Validator property
        /// </summary>
        protected IValueValidator<T> _validator;

        /// <summary>
        /// Validator of this validatable Property
        /// </summary>
        public virtual IValueValidator<T> Validator
        {
            get
            {
                return _validator;
            }
            set
            {
                if (_validator != value)
                {
                    _validator = value;

                    if (_validator != null && _isDirty) _validator.State = _defaultValidatorState;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Backing field for Delay property
        /// </summary>
        protected TimeSpan _delay;

        /// <summary>
        /// Assignment delay
        /// </summary>
        public TimeSpan Delay
        {
            get
            {
                return _delay;
            }
            set
            {
                if (_delay != value)
                {
                    _delay = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Backing field for Metadata property
        /// </summary>
        protected object _metaData;

        /// <summary>
        /// Custom data associated with this property
        /// </summary>
        public virtual object MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                if (_metaData != value)
                {
                    _metaData = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Delegate firing before assignmet delay starts
        /// </summary>
        public Action<PropertyChangeInfo<T>> DelayStarting;

        /// <summary>
        /// Delegate firing before assignmet delay starts
        /// </summary>
        public Func<PropertyChangeInfo<T>, Task> DelayStartingAsync;

        /// <summary>
        /// Delegate firing before the value changes
        /// </summary>
        public Action<PropertyChangeInfo<T>> ValueChanging;

        /// <summary>
        /// Asynchronous delegate firing before the value changes
        /// </summary>
        public Func<PropertyChangeInfo<T>, Task> ValueChangingAsync;

        /// <summary>
        /// Delegate firing after the value changed
        /// </summary>
        public Action<PropertyChangeInfo<T>> ValueChanged;

        /// <summary>
        /// Asynchronous delegate firing after the value changed
        /// </summary>
        public Func<PropertyChangeInfo<T>, Task> ValueChangedAsync;

        /// <summary>
        /// Delegate firing on error
        /// </summary>
        public Action<PropertyChangeInfo<T>, Exception> OnError;

        /// <summary>
        /// Asynchronous delegate firing on error
        /// </summary>
        public Func<PropertyChangeInfo<T>, Exception, Task> OnErrorAsync;

        /// <summary>
        /// Standard PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Waits for the last operation on this property, to safely get its most recent value
        /// </summary>
        /// <returns>Value</returns>
        public virtual async Task<T> GetAsync()
        {
            if (_lastSetOperation != null && !_lastSetOperation.IsFaulted && !_lastSetOperation.IsCanceled)
            {
                try
                {
                    await _lastSetOperation;
                }
                catch (OperationCanceledException) { }
            }
            
            return _value;
        }

        /// <summary>
        /// Waits for the last operation on this property, to reliably set its value
        /// </summary>
        /// <returns>Value</returns>
        public virtual async Task SetAsync(T value)
        {
            if (!_isReadonly)
            {
                if (_isDirty && _equalityComparer.Equals(_value, value)) return;
                
                if (!_isDirty && _equalityComparer.Equals(_lastSetValue, value)) return;

                if (_cts != null)
                {
                    _cts.Cancel();

                    if (_lastSetOperation != null) await _lastSetOperation;
                }

                _cts = new CancellationTokenSource();

                _lastSetValue = value;

                _lastSetOperation = InternalSetAsync(value, _cts.Token).ContinueWith(t => 
                {
                    _cts?.Dispose();

                    _cts = null;

                    if (t.Result)
                    {
                        OnPropertyChanged(nameof(Value));

                        IsDirty = false;
                    }
                }, 
                TaskScheduler.FromCurrentSynchronizationContext());
                
                await _lastSetOperation;
            }
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        protected virtual async Task<bool> InternalSetAsync(T value, CancellationToken token)
        {
            var previousValue = _value;

            var changeInfo = new PropertyChangeInfo<T>(this, previousValue, value, token);

            try
            {
                if (_delay > TimeSpan.Zero)
                {
                    if (_validator != null && _validator.State == ValidatorState.Invalid) _validator.Reset();

                    var delay = _delay;

                    if (DelayStarting != null || DelayStartingAsync != null) 
                    {
                        var startedAt = DateTime.UtcNow;

                        if (DelayStarting != null) DelayStarting(changeInfo);

                        if (DelayStartingAsync != null) await DelayStartingAsync(changeInfo);

                        var timePassed = DateTime.UtcNow - startedAt;

                        delay = timePassed < delay ? delay - timePassed : TimeSpan.Zero;
                    }

                    if (delay > TimeSpan.Zero) await Task.Delay(delay, token);

                    if (token.IsCancellationRequested) return false;
                }

                if (ValueChanging != null) ValueChanging(changeInfo);

                if (ValueChangingAsync != null) await ValueChangingAsync(changeInfo);

                if (token.IsCancellationRequested) return false;

                if (_validator != null) await _validator.Validate(value, token);

                if (token.IsCancellationRequested) return false;

                _value = value;

                if (ValueChanged != null) ValueChanged(changeInfo);

                if (ValueChangedAsync != null) await ValueChangedAsync(changeInfo);

                if (token.IsCancellationRequested)
                {
                    _value = previousValue;

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                _value = previousValue;
                
                if (!(e is OperationCanceledException))
                {
                    try
                    {
                        if (OnError != null) OnError(changeInfo, e);

                        if (OnErrorAsync != null) await OnErrorAsync(changeInfo, e);
                    }
                    catch { }
                }

                return false;
            }
        }

        /// <summary>
        /// Validator of this validatable property
        /// </summary>
        IValidator IValidatable.Validator
        {
            get
            {
                return _validator;
            }
        }

        /// <summary>
        /// Validates last set value of this property
        /// </summary>
        /// <param name="token">Cancellation token for validation task</param>
        /// <returns>Task</returns>
        public virtual async Task Validate(CancellationToken token)
        {
            if (_validator != null)
            {
                var value = await GetAsync();

                if (token.IsCancellationRequested) return;

                if (_lastSetOperation == null) await _validator.Validate(value, token);
            }
        }

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
    }
}
