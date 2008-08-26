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

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a related set of tab pages
    /// </summary>
    public interface ITabControl : IControlChilli
    {
        /// <summary>
        /// Gets the collection of tab pages in this tab control
        /// </summary>
        ITabPageCollection TabPages { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected tab page
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the currently selected tab page
        /// </summary>
        ITabPage SelectedTab { get; set; }

        /// <summary>
        /// Occurs when the SelectedIndex property is changed
        /// </summary>
        event EventHandler SelectedIndexChanged;
    }
}