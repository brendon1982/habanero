using System;

namespace Habanero.UI.Base
{
    public interface IButtonGroupControl:IControlChilli
    {
        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        IButton AddButton(string buttonName);

        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="buttonName">The name to appear on the button</param>
        /// <returns>Returns the Button object created</returns>
        /// <param name="clickHandler">The event handler to be triggered on the button click</param>
        IButton AddButton(string buttonName, EventHandler clickHandler);
        /// <summary>
        /// Adds a new button to the control by the name specified
        /// </summary>
        /// <param name="buttonName">The name that the button is created with</param>
        /// <returns>Returns the Button object created</returns>
        /// <param name="buttonText">The text to appear on the button</param>
        /// <param name="clickHandler">The event handler to be triggered on the button click</param>
        IButton AddButton(string buttonName, string buttonText, EventHandler clickHandler);

        /// <summary>
        /// A facility to index the buttons in the control so that they can
        /// be accessed like an array (eg. button["name"])
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        /// <returns>Returns the button found by that name, or null if not
        /// found</returns>
        IButton this[string buttonName] { get; }

        /// <summary>
        /// Sets the default button in this control that would be chosen
        /// if the user pressed Enter without changing the focus
        /// </summary>
        /// <param name="buttonName">The name of the button</param>
        void SetDefaultButton(string buttonName);
    }
}