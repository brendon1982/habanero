using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestFilterControlCheckBox
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }
        #region Tests
        [Test]
        public void TestAddCheckBoxGizmox()
        {
            TestAddCheckBox(new ControlFactoryGizmox());
        }

        [Test]
        public void TestAddCheckBoxWinForms()
        {
            TestAddCheckBox(new ControlFactoryWin());
        }

        public void TestAddCheckBox(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            //IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = factory.CreateFilterControl();
            //---------------Execute Test ----------------------
            string labelName = "aa";
            IControlChilli cb = filterControl.AddBooleanFilterCheckBox(labelName, "", true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(cb);
            Assert.IsTrue(cb is ICheckBox);
            ICheckBox cBox = (ICheckBox)cb  ;
            Assert.AreEqual(labelName, cBox.Text);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddStringFilterCheckBoxGiz()
        {
            TestAddStringFilterCheckBox(new ControlFactoryGizmox());
        }
        [Test]
        public void TestAddStringFilterCheckBoxWinForms()
        {
            TestAddStringFilterCheckBox(new ControlFactoryWin());
        }
        public void TestAddStringFilterCheckBox(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddBooleanFilterCheckBox("Test?", "TestColumn", true);
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "true");

            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestGetCheckBoxFilterClauseWinForms()
        {
            TestGetCheckBoxFilterClause(new ControlFactoryWin());
        }

        [Test]
        public void TestGetCheckBoxFilterClauseGiz()
        {
            TestGetCheckBoxFilterClause(new ControlFactoryGizmox());
        }

        public void TestGetCheckBoxFilterClause(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            ICheckBox checkBox = filterControl.AddBooleanFilterCheckBox("Test?", "TestColumn", true);

            //---------------Execute Test ----------------------
            checkBox.Checked = false;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "false");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestTwoCheckBoxFilterWinForms()
        {
            TestTwoCheckBoxFilter(new ControlFactoryWin());
        }

        [Test]
        public void TestTwoCheckBoxFilterGiz()
        {
            TestTwoCheckBoxFilter(new ControlFactoryGizmox());
        }

        public void TestTwoCheckBoxFilter(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            filterControl.AddBooleanFilterCheckBox("Test1?", "TestColumn1", true);
            filterControl.AddBooleanFilterCheckBox("Test2?", "TestColumn2", false);

            //---------------Execute Test ----------------------
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause1 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn1", FilterClauseOperator.OpEquals, "true");
            IFilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpEquals, "false");
            IFilterClause fullClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(fullClause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }
        [Test]
        public void TestOnlyCheckBoxAreOnPanelWinForms()
        {
            TestOnlyCheckBoxAreOnPanel(new ControlFactoryWin(), 1);//only check box
        }

        [Test]
        public void TestOnlyCheckBoxAreOnPanelGiz()
        {
            TestOnlyCheckBoxAreOnPanel(new ControlFactoryGizmox(), 2);//buttons panel plus check box
        }

        public void TestOnlyCheckBoxAreOnPanel(IControlFactory factory, int controlCount)
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = factory.CreateFilterControl();

            //---------------Execute Test ----------------------
            ICheckBox checkBox = filterControl.AddBooleanFilterCheckBox("Test2?", "TestColumn2", false);

            //---------------Test Result -----------------------
            Assert.AreEqual(controlCount, filterControl.Controls.Count);
            //TODO_Port: Assert.Contains(checkBox, filterControl.Controls);
            Assert.IsTrue(filterControl.Controls.Contains(checkBox));
            //---------------Tear Down -------------------------          
        }
        #endregion


    }
}