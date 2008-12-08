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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Habanero.BO.Comparer
{
	/// <summary>
	/// Compares a property of two different business objects using reflection.  Used by Sort().
	/// </summary>
	public class ReflectedPropertyComparer<T> : IComparer<T>
	{
		private readonly string _propertyName;
		private readonly PropertyInfo _propInfo;

		/// <summary>
		/// Constructor to instantiate a new comparer
		/// </summary>
		/// <param name="propertyName">The property name to compare on</param>
		public ReflectedPropertyComparer(string propertyName)
		{
			_propertyName = propertyName;
			_propInfo = typeof(T).GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);
		}

	    ///<summary>
	    ///Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
	    ///</summary>
	    ///
	    ///<returns>
	    ///Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
	    ///</returns>
	    ///
	    ///<param name="y">The second object to compare.</param>
	    ///<param name="x">The first object to compare.</param>
	    public int Compare(T x, T y)
		{
			object x1 = _propInfo.GetValue(x, new object[] { });
			object y1 = _propInfo.GetValue(y, new object[] { });

			if (x1 == null && y1 == null)
			{
				return 0;
			}
	        if (x1 == null)
	        {
	            return -1;
	        }
	        if (y1 == null)
	        {
	            return 1;
	        }

	        if (x1 is string)
            {
                return String.Compare((string)x1, (string)y1);
            }

            if (x1 is int)
            {
                if ((int)x1 < (int)y1) return -1;
                if ((int)x1 > (int)y1) return 1;
                return 0;
            }

            if (x1 is double)
            {
                if (Math.Abs((double)x1 - (double)y1) < 0.00001) return 0;
                if ((double)x1 < (double)y1) return -1;
                if ((double)x1 > (double)y1) return 1;
            }

            if (x1 is DateTime)
            {
                return ((DateTime)x1).CompareTo(y1);
            }

            IComparer comparer = Comparer<T>.Default;
            return comparer.Compare(x1, y1);
		}
	}
}