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

using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    public abstract class TestInheritanceHeirarchyBase : TestUsingDatabase
    {
        protected BusinessObject _filledCircle;
        protected SqlStatementCollection _insertSql;
        protected SqlStatementCollection _updateSql;
        protected SqlStatementCollection _deleteSql;
        protected SqlStatement _selectSql;
        protected string _filledCircleId;
        protected SqlStatement _loadSql;

        public void SetupTest()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircle();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql = _filledCircle.GetInsertSql();
            _updateSql = _filledCircle.GetUpdateSql();
            _deleteSql = _filledCircle.GetDeleteSql();
            _selectSql = new SqlStatement(DatabaseConnection.CurrentConnection);
            _selectSql.Statement.Append(_filledCircle.SelectSqlStatement(_selectSql));
        }

        public void SetupTestForFilledCircleNoPK()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircleNoPrimaryKey();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql = _filledCircle.GetInsertSql();
            _updateSql = _filledCircle.GetUpdateSql();
            _deleteSql = _filledCircle.GetDeleteSql();
            _selectSql = new SqlStatement(DatabaseConnection.CurrentConnection);
            _selectSql.Statement.Append(_filledCircle.SelectSqlStatement(_selectSql));
        }

        public void SetupTestForFilledCircleInheritsCircleNoPK()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircleInheritsCircleNoPK();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql = _filledCircle.GetInsertSql();
            _updateSql = _filledCircle.GetUpdateSql();
            _deleteSql = _filledCircle.GetDeleteSql();
            _selectSql = new SqlStatement(DatabaseConnection.CurrentConnection);
            _selectSql.Statement.Append(_filledCircle.SelectSqlStatement(_selectSql));
        }

        public void SetupTestForFilledCircleNoPrimaryKeyInheritsCircle()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircleNoPrimaryKeyInheritsCircle();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql = _filledCircle.GetInsertSql();
            _updateSql = _filledCircle.GetUpdateSql();
            _deleteSql = _filledCircle.GetDeleteSql();
            _selectSql = new SqlStatement(DatabaseConnection.CurrentConnection);
            _selectSql.Statement.Append(_filledCircle.SelectSqlStatement(_selectSql));
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}