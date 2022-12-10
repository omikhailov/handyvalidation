using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HandyValidation
{
    /// <summary>
    /// Interface representing validator
    /// </summary>
    public interface IValidator : INotifyPropertyChanged
    {
        /// <summary>
        /// State of the validator
        /// </summary>
        ValidatorState State { get; set; }

        /// <summary>
        /// True when validation fails
        /// </summary>
        bool HasIssues { get; }        

        /// <summary>
        /// Validation issues
        /// </summary>
        ObservableCollection<object> Issues { get; }

        /// <summary>
        /// First validation issue out of all Issues
        /// </summary>
        object FirstIssue { get; }

        /// <summary>
        /// Resets validation results
        /// </summary>
        void Reset();
    }
}
