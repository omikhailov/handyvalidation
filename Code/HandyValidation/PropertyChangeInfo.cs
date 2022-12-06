using System.Threading;

namespace HandyValidation
{
    public struct PropertyChangeInfo<T>
    {
        public PropertyChangeInfo(Property<T> property, T oldValue, T newValue, CancellationToken cancellationToken)
        {
            Property = property;

            OldValue = oldValue;

            NewValue = newValue;

            CancellationToken = cancellationToken;
        }

        public Property<T> Property { get; private set; }

        public T OldValue { get; private set; }

        public T NewValue { get; private set; }

        public CancellationToken CancellationToken { get; private set; }
    }
}
