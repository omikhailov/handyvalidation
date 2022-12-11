using System.Threading;

namespace HandyValidation
{
    /// <summary>
    /// Data structure representing details about property value change
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public struct PropertyChangeInfo<T>
    {
        /// <summary>
        /// Creates new PropertyChangeInfo
        /// </summary>
        /// <param name="property">Property instance</param>
        /// <param name="oldValue">Previous value</param>
        /// <param name="newValue">New value</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public PropertyChangeInfo(Property<T> property, T oldValue, T newValue, CancellationToken cancellationToken)
        {
            Property = property;

            OldValue = oldValue;

            NewValue = newValue;

            CancellationToken = cancellationToken;
        }

        /// <summary>
        /// Property instance
        /// </summary>
        public Property<T> Property { get; private set; }

        /// <summary>
        /// Previous value
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// New value
        /// </summary>
        public T NewValue { get; private set; }

        /// <summary>
        /// Cancellation token
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }
    }
}
