//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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


using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI
{
    /// <summary>
    /// An interface to model a mapper that wraps a control in
    /// order to display information related to a business object 
    /// </summary>
    public interface IControlMapper
    {
        /// <summary>
        /// Returns the control being mapped
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Returns the name of the property being edited in the control
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Controls access to the business object being represented
        /// by the control.  Where the business object has been amended or
        /// altered, the ValueUpdated() method is automatically called here to 
        /// implement the changes in the control itself.
        /// </summary>
        IBusinessObject BusinessObject { get; set; }
    }
}