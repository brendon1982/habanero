// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System.Collections;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a collection of cells that are selected in a DataGridView
    /// </summary>
    public interface IDataGridViewSelectedCellCollection:IEnumerable
    {
        /// <summary>
        /// Gets the total number of cells in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the cell at the specified index.
        /// </summary>
        IDataGridViewCell this[int index] { get; }
    }
}