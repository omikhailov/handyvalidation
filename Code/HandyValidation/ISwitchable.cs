using System;

namespace HandyValidation
{
    /// <summary>
    /// Switchable item that can be turned on or off
    /// </summary>
    public interface ISwitchable
    {
        /// <summary>
        /// Property indicating that item is on or off
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
