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
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Provides a set of buttons for use on an <see cref="IReadOnlyGridControl"/>.
    /// By default, Add and Edit buttons are available, but you can also make the standard
    /// Delete button visible by setting the <see cref="ShowDefaultDeleteButton"/>
    /// property to true.
    /// </summary>
    public class ReadOnlyGridButtonsControlGiz : ButtonGroupControlGiz, IReadOnlyGridButtonsControl
    {
        public event EventHandler DeleteClicked;
        public event EventHandler AddClicked;
        public event EventHandler EditClicked;
        private readonly ReadOnlyGridButtonsControlManager _manager;

        public ReadOnlyGridButtonsControlGiz(IControlFactory controlFactory)
            : base(controlFactory)
        {
            _manager = new ReadOnlyGridButtonsControlManager(this);
            _manager.CreateDeleteButton(delegate { if (DeleteClicked != null) DeleteClicked(this, new EventArgs()); });
            _manager.CreateEditButton(delegate { if (EditClicked != null) EditClicked(this, new EventArgs()); });
            _manager.CreateAddButton(delegate { if (AddClicked != null) AddClicked(this, new EventArgs()); });
        }

        /// <summary>
        /// Indicates whether the default delete button is visible.  This
        /// is false by default.
        /// </summary>
        public bool ShowDefaultDeleteButton
        {
            get { return _manager.DeleteButton.Visible; }
            set { _manager.DeleteButton.Visible = value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}