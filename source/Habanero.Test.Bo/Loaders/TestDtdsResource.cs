//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.Loaders;
using Habanero.Util;
using Habanero.Util.File;
using NMock;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// The purpose of this test class is simply to improve test coverage
    /// </summary>
    [TestFixture]
    public class TestDtdsResource
    {
        [Test]
        public void TestDtdAccess()
        {
            Assert.IsTrue(Dtds._class.Length > 0);
            Assert.IsTrue(Dtds.businessObjectLookupList.Length > 0);
            Assert.IsTrue(Dtds.classes.Length > 0);
            Assert.IsTrue(Dtds.column.Length > 0);
            Assert.IsTrue(Dtds.columnLayout.Length > 0);
            Assert.IsTrue(Dtds.databaseLookupList.Length > 0);
            Assert.IsTrue(Dtds.field.Length > 0);
            Assert.IsTrue(Dtds.form.Length > 0);
            Assert.IsTrue(Dtds.formGrid.Length > 0);
            Assert.IsTrue(Dtds.grid.Length > 0);
            Assert.IsTrue(Dtds.key.Length > 0);
            Assert.IsTrue(Dtds.parameter.Length > 0);
            Assert.IsTrue(Dtds.primaryKey.Length > 0);
            Assert.IsTrue(Dtds.Prop.Length > 0);
            Assert.IsTrue(Dtds.property.Length > 0);
            Assert.IsTrue(Dtds.relationship.Length > 0);
            Assert.IsTrue(Dtds.Rule.Length > 0);
            Assert.IsTrue(Dtds.simpleLookupList.Length > 0);
            Assert.IsTrue(Dtds.superClass.Length > 0);
            Assert.IsTrue(Dtds.tab.Length > 0);
            Assert.IsTrue(Dtds.ui.Length > 0);

            Assert.IsNull(Dtds.Culture);
            CultureInfo culture = new CultureInfo("");
            Dtds.Culture = culture;
            Assert.AreEqual(culture, Dtds.Culture);
            Dtds.Culture = null;

            Assert.AreEqual(typeof(ResourceManager), Dtds.ResourceManager.GetType());
        }
    }
}