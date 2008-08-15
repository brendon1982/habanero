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

using System.Collections;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents the collection of items in a ListBox
    /// </summary>
    public interface IListBoxObjectCollection : IEnumerable
    {
        /// <summary>
        /// Adds an item to the list of items for a ListBox
        /// </summary>
        /// <param name="item">An object representing the item to add to the collection</param>
        void Add(object item);

        /// <summary>
        /// Gets the number of items in the collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Removes the specified object from the collection
        /// </summary>
        /// <param name="item">An object representing the item to remove from the collection</param>
        void Remove(object item);

        /// <summary>
        /// Removes all items from the collection
        /// </summary>
        void Clear();
    }
}