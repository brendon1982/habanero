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
using System.Collections.Generic;
using System.Text;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Represents a window or dialog box that makes up an application's user interface
    /// </summary>
    public class FormGiz : Form, IFormChilli
    {
        private IControlCollection _controls;

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlChilli.Controls
        {
            get { return _controls; }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }

        /// <summary>
        /// Forces the form to invalidate its client area and
        /// immediately redraw itself and any child controls.
        /// Does nothing in the VWG environment.
        /// </summary>
        public void Refresh()
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the current multiple document interface (MDI) parent form of this form
        /// </summary>
        IFormChilli IFormChilli.MdiParent
        {
            get { throw new NotImplementedException(); }
            set { this.MdiParent = (Form)value; }
        }

        /// <summary>
        /// Shows the form as a modal dialog box with the currently active window set as its owner
        /// </summary>
        /// <returns>One of the DialogResult values</returns>
        Base.DialogResult IFormChilli.ShowDialog()
        {
            return (Base.DialogResult)base.ShowDialog();
        }
    }
}
