namespace HandyValidation
{
    /// <summary>
    /// Validator state
    /// </summary>
    public enum ValidatorState
    {
        /// <summary>
        /// Validation has not yet been performed
        /// </summary>
        NotSet,

        /// <summary>
        /// Validation passed
        /// </summary>
        Valid,

        /// <summary>
        /// Validation failed
        /// </summary>
        Invalid
    }
}