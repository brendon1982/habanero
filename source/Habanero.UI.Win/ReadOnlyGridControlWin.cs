using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;

namespace Habanero.UI.Win
{
    public class ReadOnlyGridControlWin : ControlWin, IReadOnlyGridControl
    {
        private IBusinessObjectEditor _BusinessObjectEditor;
        private IBusinessObjectCreator _BusinessObjectCreator;
        private IBusinessObjectDeletor _businessObjectDeletor;
        private ClassDef _classDef;
        private IFilterControl _filterControl;
        private string _uiDefName = "";
        private readonly ReadOnlyGridWin _grid;
        private readonly GridInitialiser _gridInitialiser;


        public ReadOnlyGridControlWin()
        {
            _grid = new ReadOnlyGridWin();
            _grid.Name = "GridControl";
            _gridInitialiser = new GridInitialiser(this);
        }

        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        public void Initialise(ClassDef classDef)
        {
            _gridInitialiser.InitialiseGrid(classDef);
        }

        public void Initialise(ClassDef classDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
        }

        public IReadOnlyGrid Grid
        {
            get { return _grid; }
        }

        public BusinessObject SelectedBusinessObject
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Returns the button control held. This property can be used
        /// to access a range of functionality for the button control
        /// (eg. myGridWithButtons.Buttons.AddButton(...)).
        /// </summary>
        public IReadOnlyGridButtonsControl Buttons
        {
            get { throw new System.NotImplementedException(); }
        }

        public IBusinessObjectEditor BusinessObjectEditor
        {
            get { return _BusinessObjectEditor; }
            set { _BusinessObjectEditor = value; }
        }

        public IBusinessObjectCreator BusinessObjectCreator
        {
            get { return _BusinessObjectCreator; }
            set { _BusinessObjectCreator = value; }
        }

        public IBusinessObjectDeletor BusinessObjectDeletor
        {
            get { return _businessObjectDeletor; }
            set { _businessObjectDeletor = value; }
        }

        public string UiDefName
        {
            get { return _uiDefName; }
            set { _uiDefName = value; }
        }

        public ClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value; }
        }

        public IFilterControl FilterControl
        {
            get { return _filterControl; }
        }

        public bool IsInitialised
        {
            get { throw new System.NotImplementedException(); }
        }

        public FilterModes FilterMode
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }


        public void SetBusinessObjectCollection(IBusinessObjectCollection boCollection)
        {
            throw new System.NotImplementedException();
        }

       

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}