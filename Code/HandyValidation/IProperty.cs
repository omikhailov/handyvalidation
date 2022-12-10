namespace HandyValidation
{
    /// <summary>
    /// Interface of view model property
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IProperty<T>
    {
        /// <summary>
        /// Property value
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// A flag indicating that property is read only
        /// </summary>
        bool IsReadonly { get; set; }
    }
}
