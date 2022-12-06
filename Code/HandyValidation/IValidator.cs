using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HandyValidation
{
    public interface IValidator : INotifyPropertyChanged
    {
        ValidatorState State { get; set; }

        bool HasIssues { get; }        

        ObservableCollection<object> Issues { get; }

        object FirstIssue { get; }

        void Reset();
    }
}
