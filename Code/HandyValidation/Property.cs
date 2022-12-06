using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace HandyValidation
{
    public class Property<T> : IProperty<T>, IAsyncValue<T>, IValidatable, INotifyPropertyChanged
    {
        private readonly ValidatorState _defaultValidatorState;

        protected Task _lastSetOperation;

        protected CancellationTokenSource _cts;

        public Property() { }

        public Property(T defaultValue, ValidatorState defaultValidatorState = ValidatorState.Valid)
        {
            _value = defaultValue;

            _defaultValidatorState = defaultValidatorState;
        }

        protected T _value;

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
        
        protected bool _isReadonly;

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

        protected IValueValidator<T> _validator;

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

        protected object _metaData;

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

        public Action<PropertyChangeInfo<T>> ValueChanging;

        public Func<PropertyChangeInfo<T>, Task> ValueChangingAsync;

        public Action<PropertyChangeInfo<T>> ValueChanged;

        public Func<PropertyChangeInfo<T>, Task> ValueChangedAsync;

        public Action<PropertyChangeInfo<T>, Exception> OnError;

        public Func<PropertyChangeInfo<T>, Exception, Task> OnErrorAsync;

        public event PropertyChangedEventHandler PropertyChanged;

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

        IValidator IValidatable.Validator
        {
            get
            {
                return _validator;
            }
        }

        public virtual async Task Validate(CancellationToken token)
        {
            if (_validator != null)
            {
                var value = await GetAsync();

                if (token.IsCancellationRequested) return;

                if (_lastSetOperation == null) await _validator.Validate(value, token);
            }
        }

        public virtual async Task Validate()
        {
            await Validate(CancellationToken.None);
        }

        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
