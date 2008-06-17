using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base.Grid
{
    public class EditableGridControlManager
    {
        private readonly IEditableGridControl _gridControl;
        private string _uiDefName;
        private IClassDef _classDef;
        private IGridInitialiser _gridInitialiser;


        public EditableGridControlManager(IEditableGridControl gridControl, IControlFactory controlFactory)
        {
            _gridControl = gridControl;
            _gridInitialiser = new GridInitialiser(gridControl, controlFactory);
        }

        public string UiDefName
        {
            get { return _uiDefName; }
            set { _uiDefName = value; }
        }

        public IClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value; }
        }

        public void Initialise(IClassDef classDef)
        {
            _gridInitialiser.InitialiseGrid(classDef);
        }

        public void Initialise(IClassDef classDef, string uiDefName)
        {
            _gridInitialiser.InitialiseGrid(classDef, uiDefName);
        }
    }
}
