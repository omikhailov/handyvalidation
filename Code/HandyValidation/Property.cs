﻿using System;
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
        private readonly ValidatorState _defaultValidatorState;

        /// <summary>
        /// Most recent validation task
        /// </summary>
        protected Task _lastSetOperation;

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
        /// Gets or sets flag indicating that property is read only
        /// </summary>
        public virtual bool IsReadonly
        {
            get
            {
                return _isReadonly;
            }
            set
            {
                _isReadonly = value;

                OnPropertyChanged();
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
                _validator = value;

                if (_validator != null) _validator.State = _defaultValidatorState;

                OnPropertyChanged();
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
                _metaData = value;

                OnPropertyChanged();
            }
        }

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
                if (_lastSetOperation != null)
                {
                    _cts?.Cancel();

                    await _lastSetOperation;
                }

                _cts?.Dispose();

                _cts = new CancellationTokenSource();

                _lastSetOperation = InternalSetAsync(value, _cts.Token);
                
                await _lastSetOperation;
            }
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>Task</returns>
        protected virtual async Task InternalSetAsync(T value, CancellationToken token)
        {
            var previousValue = _value;
            
            try
            {
                if (_validator != null) await _validator.Validate(value, token);

                if (token.IsCancellationRequested) return;

                var changeInfo = new PropertyChangeInfo<T>(this, previousValue, value, token);

                if (ValueChanging != null) ValueChanging(changeInfo);

                if (ValueChangingAsync != null) await ValueChangingAsync(changeInfo);

                if (token.IsCancellationRequested) return;

                _value = value;

                if (ValueChanged != null) ValueChanged(changeInfo);

                if (ValueChangedAsync != null) await ValueChangedAsync(changeInfo);

                if (token.IsCancellationRequested)
                {
                    _value = previousValue;

                    return;
                }

                OnPropertyChanged(nameof(Value));
            }
            catch (Exception e)
            {
                _value = previousValue;

                if (!(e is OperationCanceledException))
                {
                    try
                    {
                        if (OnError != null) OnError(new PropertyChangeInfo<T>(this, previousValue, value, token), e);

                        if (OnErrorAsync != null) await OnErrorAsync(new PropertyChangeInfo<T>(this, previousValue, value, token), e);
                    }
                    catch { }
                }
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
