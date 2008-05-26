using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestEditableGrid : TestUsingDatabase
    {


        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();



        [TestFixture]
        public class TestEditableGridWin : TestEditableGrid
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            //protected override IGridBase CreateGridBaseStub()
            //{
            //    GridBaseWinStub gridBase = new GridBaseWinStub();
            //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            //    frm.Controls.Add(gridBase);
            //    return gridBase;
            //}

            private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                         IGridBase gridBase)
            {
                System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView) gridBase;
                System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
                return row.Cells[propName];
            }

            //protected override void AddControlToForm(IGridBase gridBase)
            //{
            //    throw new NotImplementedException();
            //}
        }

        [TestFixture]
        public class TestEditableGridGiz : TestEditableGrid
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            //protected override IGridBase CreateGridBaseStub()
            //{
            //    GridBaseGizStub gridBase = new GridBaseGizStub();
            //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //    frm.Controls.Add(gridBase);
            //    return gridBase;
            //}

            //protected override void AddControlToForm(IGridBase gridBase)
            //{
            //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //    frm.Controls.Add((Gizmox.WebGUI.Forms.Control) gridBase);
            //}

            private static Gizmox.WebGUI.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                            IGridBase gridBase)
            {
                Gizmox.WebGUI.Forms.DataGridView dgv = (Gizmox.WebGUI.Forms.DataGridView)gridBase;
                Gizmox.WebGUI.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
                return row.Cells[propName];
            }

            private object GetCellValue(int rowIndex, IGridBase gridBase, string propName)
            {
                Gizmox.WebGUI.Forms.DataGridViewCell cell = GetCell(rowIndex, propName, gridBase);
                return cell.Value;
            }

            
        }


        [Test]
        public void TestConstructGrid()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            //---------------Test Result -----------------------
            Assert.IsNotNull(grid);
            Assert.IsFalse(grid.ReadOnly);
            Assert.IsTrue(grid.AllowUserToAddRows);
            Assert.IsTrue(grid.AllowUserToDeleteRows);
           //---------------Tear Down -------------------------
        }

    }

    
}
