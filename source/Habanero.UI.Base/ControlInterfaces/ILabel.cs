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

using System.Drawing;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a label
    /// </summary>
    public interface ILabel : IControlChilli
    {
        /// <summary>
        /// Gets the preferred width of the control
        /// </summary>
        int PreferredWidth { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control 
        /// is automatically resized to display its entire contents
        /// </summary>
        bool AutoSize { get; set; }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control
        /// </summary>
        /// TODO: is this needed - it's in the IControl?
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the alignment of text in the label
        /// </summary>
        ContentAlignment TextAlign { get; set; }
    }
}