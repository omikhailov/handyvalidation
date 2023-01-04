using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HandyValidation
{
    /// <summary>
    /// Turns on or off multiple ISwitchable items at once
    /// </summary>
    public class InputSwitch : ISwitchable, INotifyPropertyChanged
    {
        /// <summary>
        /// ISwitchable items
        /// </summary>
        protected readonly ISwitchable[] _items;

        /// <summary>
        /// Creates new instance of InputSwitch
        /// </summary>
        /// <param name="items">ISwitchable items</param>
        public InputSwitch(params ISwitchable[] items)
        {
            _items = items;
        }

        /// <summary>
        /// Creates new instance of InputSwitch
        /// </summary>
        /// <param name="items">ISwitchable items</param>
        public InputSwitch(IEnumerable<ISwitchable> items)
        {
            _items = items.ToArray();
        }

        /// <summary>
        /// Creates new instance of InputSwitch
        /// </summary>
        /// <param name="items">A set of ISwitchable items</param>
        /// <param name="additionalItems">Additional items</param>
        public InputSwitch(IEnumerable<ISwitchable> items, params ISwitchable[] additionalItems)
        {
            _items = items.Concat(additionalItems).ToArray();
        }

        /// <summary>
        /// Backing store for IsEnabled
        /// </summary>
        protected bool _isEnabled = true;

        /// <summary>
        /// Property indicating that switch is on or off
        /// </summary>
        public bool IsEnabled
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

                    foreach (var item in _items) item.IsEnabled = value;

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Turns off the switch until the specified task is completed, then turns it on
        /// </summary>
        /// <param name="action">Task to await</param>
        /// <returns>Task</returns>
        public async Task OffWhile(Task action)
        {
            try
            {
                IsEnabled = false;

                await action;
            }
            finally
            {
                IsEnabled = true;
            }
        }

        /// <summary>
        /// Standard PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
