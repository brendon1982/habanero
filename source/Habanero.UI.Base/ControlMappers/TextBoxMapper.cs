//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Maps a TextBox object in a user interface.  Note that there are some
    /// limitations with using a TextBox for numbers.  For greater control 
    /// of user input with numbers, you should consider using a NumericUpDown 
    /// control.
    /// </summary>
    public class TextBoxMapper : ControlMapper
    {
      

        private readonly ITextBox _textBox;
        private string _oldText;
        private readonly ITextBoxMapperStrategy _textBoxMapperStrategy;

        /// <summary>
        /// Constructor to initialise a new instance of the mapper
        /// </summary>
        /// <param name="tb">The TextBox to map</param>
        /// <param name="propName">The property name</param>
        /// <param name="isReadOnly">Whether this control is read only</param>
        /// <param name="factory">the control factory to be used when creating the controlMapperStrategy</param>
        public TextBoxMapper(ITextBox tb, string propName, bool isReadOnly, IControlFactory factory)
            : base(tb, propName, isReadOnly, factory)
        {
            _textBox = tb;
            _textBoxMapperStrategy = factory.CreateTextBoxMapperStrategy();

            //_textBox.Enabled = false;
            //_textBox.TextChanged += ValueChangedHandler;
            _oldText = "";
        }

        public ITextBoxMapperStrategy TextBoxMapperStrategy
        {
            get { return _textBoxMapperStrategy; }
        }

        public override BusinessObject BusinessObject
        {
            get { return base.BusinessObject; }
            set
            {
                base.BusinessObject = value;
                TextBoxMapperStrategy.AddKeyPressEventHandler(this, base.CurrentBOProp());
            }
        }

        ///// <summary>
        ///// A handler to carry out changes to the business object when the
        ///// value has changed in the user interface
        ///// </summary>
        ///// <param name="sender">The object that notified of the event</param>
        ///// <param name="e">Attached arguments regarding the event</param>
        //private void ValueChangedHandler(object sender, EventArgs e)
        //{

        //    string value = _textBox.Text;


        //    //if (IsDecimalType())
        //    //{
        //    //    if (value.EndsWith(".")) return;
        //    //}
        //    //if (IsDecimalType() || IsIntegerType())
        //    //{
        //    //    if (value.EndsWith("-")) return;
        //    //    if (value.Length == 0)
        //    //    {
        //    //        value = null;
        //    //    }
        //    //}

        //    if (!_isEditable) return;

        //    try
        //    {
        //        SetPropertyValue(value);
        //        //_businessObject.SetPropertyValue(_propertyName, value);
        //    }
        //    catch (FormatException)
        //    {
        //        _textBox.Text = _oldText;
        //    }
        //    _oldText = _textBox.Text;
        //}

        /// <summary>
        /// Updates the interface when the value has been changed in the
        /// object being represented
        /// </summary>
        protected override void InternalUpdateControlValueFromBo()
        {
            _textBox.Text = Convert.ToString(GetPropertyValue());
        }

        public override void ApplyChangesToBusinessObject()
        {
            string value = _textBox.Text;

            if (!_isEditable) return;

            try
            {
                SetPropertyValue(value);
            }
            catch (FormatException)
            {
                _textBox.Text = _oldText;
                throw new BusObjectInAnInvalidStateException("The business object '" +
                                                             this._businessObject.ClassDef.ClassName + "' - '" +
                                                             this._businessObject +
                                                             "' could not be updated since the value '" + value +
                                                             "' is not valid for the property '" + _propertyName + "'");
            }
            _oldText = _textBox.Text;
        }
    }
}