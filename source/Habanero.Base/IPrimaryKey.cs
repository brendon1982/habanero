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

namespace Habanero.Base
{
    ///<summary>
    /// This is an interface for the primary key for a business object.
    /// The Primary Key inherits from IBOKey and as such will contain a collection
    /// of IBOProps.
    /// The Primary Key Implements the Identity Field Pattern (Fowler (216) -
    ///  'Patterns of Enterprise Application Architecture' - 
    ///  'Saves the database ID Field in an object to maintain indentity between an in memory object and a database row')
    ///  By allowing one or more field to be stored as the primary key this extends the Traditional Object relational mapping
    ///   by allowing composite Primary keys (a common occurence when replacing existing systems).
    ///   By mapping to Many IBoProps we can also track the dirty status and previous values of any
    ///   the primary key properties and as such can handle cases of mutable primary keys. As a rule however it is always preferable
    ///   for primary keys to be Immutable.
    ///</summary>
    public interface IPrimaryKey : IBOKey
    {
        /// <summary>
        /// Returns the object's ID as a string.
        /// </summary>
        /// <returns>Returns a string</returns>
        string GetObjectId();

        /// <summary>
        /// Returns the ID as a Guid in cases where the <see cref="IBusinessObject"/> is using a Guid object ID./>
        /// </summary>
        /// <returns>Returns a Guid</returns>
        Guid GetAsGuid();

        /// <summary>
        /// Sets the object's ID this is used when a new object is constructed. The object is given a unique identifier.
        /// If the object is later loaded from the database then this ID is replaced by the Database ID.
        /// If the <see cref="IBusinessObject"/> has an object ID (i.e. Its primary key in the database is a Guid) then 
        /// a new object will be inserted into the database using this Guid Value for the ID Field.
        /// </summary>
        /// <param name="id">The ID to set to</param>
        void SetObjectGuidID(Guid id);
    }
}