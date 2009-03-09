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
using System.Data;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    /// <summary>
    /// Provides a super-class for data-set providers for business objects
    /// </summary>
    public abstract class DataSetProvider : IDataSetProvider
    {
        /// <summary>
        /// The <see cref="IBusinessObjectCollection"/> of <see cref="IBusinessObject"/>s that
        ///   are being mapped in this DataSetProvider (i.e. are being copied to the <see cref="DataTable"/>.
        /// </summary>
        protected readonly IBusinessObjectCollection _collection;
        /// <summary>
        /// The collection of <see cref="UIGridColumn"/>s that are being shown in this <see cref="DataTable"/>.
        /// </summary>
        protected ICollection _uiGridProperties;
        /// <summary>
        /// The <see cref="DataTable"/> that is set up to represent the items in this collection.
        /// </summary>
        protected DataTable _table;
        /// <summary>
        /// The object initialiser being used to create a new object if this grid is allowed to create a new object.
        /// </summary>
        protected IBusinessObjectInitialiser _objectInitialiser;
        private const string _idColumnName = "HABANERO_OBJECTID";
        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has been added to the <see cref="_collection"/>.
        /// </summary>
        protected EventHandler<BOEventArgs> _boAddedHandler;
        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has had one of its properties updated
        /// </summary>
        protected readonly EventHandler<BOPropUpdatedEventArgs> _propUpdatedEventHandler;
        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has been persisted.
        /// </summary>
        protected readonly EventHandler<BOEventArgs> _updatedHandler;
        /// <summary>
        /// A handler for the <see cref="IBusinessObject"/> has been removed from the <see cref="_collection"/>.
        /// </summary>
        protected readonly EventHandler<BOEventArgs> _removedHandler;

        ///<summary>
        /// Gets and sets whether the property update handler shold be set or not.
        /// This is used to 
        ///    change behaviour typically to differentiate behaviour
        ///    between windows and web.<br/>
        ///Typically in windows every time a business object property is changed
        ///   the grid is updated with Web the grid is updated only when the object
        ///    is persisted.
        /// </summary>
        public bool RegisterForBusinessObjectPropertyUpdatedEvents { get; set; }

        /// <summary>
        /// Constructor to initialise a provider with a specified business
        /// object collection
        /// </summary>
        /// <param name="collection">The business object collection</param>
        protected DataSetProvider(IBusinessObjectCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            this._collection = collection;
            RegisterForBusinessObjectPropertyUpdatedEvents = true;
            _boAddedHandler = BOAddedHandler;
            _propUpdatedEventHandler = PropertyUpdatedHandler;
            _updatedHandler = UpdatedHandler;
            _removedHandler = RemovedHandler;
        }

        /// <summary>
        /// Returns a data table with the UIGridDef provided
        /// </summary>
        /// <param name="uiGrid">The UIGridDef</param>
        /// <returns>Returns a DataTable object</returns>
        public DataTable GetDataTable(UIGrid uiGrid)
        {
            if (uiGrid == null) throw new ArgumentNullException("uiGrid");
            _table = new DataTable();
            this.InitialiseLocalData();

            _uiGridProperties = uiGrid;
            DataColumn column = _table.Columns.Add();
            column.Caption = _idColumnName;
            column.ColumnName = _idColumnName;
            IClassDef classDef = _collection.ClassDef;
            foreach (UIGridColumn uiProperty in _uiGridProperties)
            {
                AddColumn(uiProperty, (ClassDef) classDef);
            }
            foreach (BusinessObject businessObjectBase in _collection)
            {
                object[] values = GetValues(businessObjectBase);
                _table.LoadDataRow(values, true);
            }
            this.RegisterForEvents();
            return _table;
        }

        private void AddColumn(UIGridColumn uiProperty, ClassDef classDef)
        {
            DataColumn column = _table.Columns.Add();
            if (_table.Columns.Contains(uiProperty.PropertyName))
            {
                throw new DuplicateNameException
                    (String.Format
                         ("In a grid definition, a duplicate column with "
                          + "the name '{0}' has been detected. Only one column " + "per property can be specified.",
                          uiProperty.PropertyName));
            }
            Type columnPropertyType = classDef.GetPropertyType(uiProperty.PropertyName);
            column.DataType = columnPropertyType;
            column.ColumnName = uiProperty.PropertyName;
            column.Caption = uiProperty.GetHeading(classDef);
//            column.ReadOnly = !uiProperty.Editable;
            column.ExtendedProperties.Add("LookupList", classDef.GetLookupList(uiProperty.PropertyName));
            column.ExtendedProperties.Add("Width", uiProperty.Width);
            column.ExtendedProperties.Add("Alignment", uiProperty.Alignment);
        }

        ///<summary>
        /// Updates the row values for the specified <see cref="IBusinessObject"/>.
        ///</summary>
        ///<param name="businessObject">The <see cref="IBusinessObject"/> for which the row values need to updated.</param>
        public virtual void UpdateBusinessObjectRowValues(IBusinessObject businessObject)
        {
            if (businessObject == null) return;
            int rowNum = this.FindRow(businessObject);
            if (rowNum == -1)
            {
                return;
            }
            try
            {
                DeregisterForTableEvents();
                object[] values = GetValues(businessObject);
                foreach (DataColumn column in _table.Columns)
                {
                    column.ReadOnly = false;
                }
                _table.Rows[rowNum].ItemArray = values;
            }
            finally
            {
                RegisterForTableEvents();
            }
        }
        /// <summary>
        /// Deregisters for all events to the <see cref="_table"/>
        /// </summary>
        protected virtual void DeregisterForTableEvents()
        {
        }
        /// <summary>
        /// Registers for all events to the <see cref="_table"/>
        /// </summary>
        protected virtual void RegisterForTableEvents()
        {
        }

        /// <summary>
        /// Gets a list of the property values to display to the user
        /// </summary>
        /// <param name="businessObject">The business object whose
        /// properties are to be displayed</param>
        /// <returns>Returns an array of values</returns>
        protected object[] GetValues(IBusinessObject businessObject)
        {
            object[] values = new object[_uiGridProperties.Count + 1];
            values[0] = businessObject.ID.ObjectID;
            int i = 1;
            BOMapper mapper = new BOMapper(businessObject);
            foreach (UIGridColumn gridProperty in _uiGridProperties)
            {
                object val = mapper.GetPropertyValueToDisplay(gridProperty.PropertyName);
                ////TODO: Do for derived properties may need some logic for setting val
                /// see previous code for this method
                values[i++] = val;
            }
            return values;
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public virtual void DeregisterForEvents()
        {
            DeregisterForBOEvents();
        }

        protected virtual void DeregisterForBOEvents()
        {
            try
            {
                if (RegisterForBusinessObjectPropertyUpdatedEvents)
                {
                    _collection.BusinessObjectPropertyUpdated -= _propUpdatedEventHandler;
                }
                else
                {
                    _collection.BusinessObjectUpdated -= _updatedHandler;
                }
                _collection.BusinessObjectIDUpdated -= IDUpdatedHandler;
                _collection.BusinessObjectAdded -= _boAddedHandler;
                _collection.BusinessObjectRemoved -= _removedHandler;
            }
            catch (KeyNotFoundException)
            {
                //Hide the exception that the event was not registerd for in the first place
            }
        }

        /// <summary>
        /// Adds handlers to be called when updates occur
        /// </summary>
        public virtual void RegisterForEvents()
        {
            RegisterForBOEvents();
            RegisterForTableEvents();
        }
        /// <summary>
        /// Registers for all events from the <see cref="_collection"/>
        /// </summary>
        protected virtual void RegisterForBOEvents()
        {
            this.DeregisterForBOEvents();
            if (RegisterForBusinessObjectPropertyUpdatedEvents)
            {
                _collection.BusinessObjectPropertyUpdated += _propUpdatedEventHandler;
            }
            else
            {
                _collection.BusinessObjectUpdated += _updatedHandler;
            }
            _collection.BusinessObjectIDUpdated += IDUpdatedHandler;
            _collection.BusinessObjectAdded += _boAddedHandler;
            _collection.BusinessObjectRemoved += _removedHandler;
        }

        private void PropertyUpdatedHandler(object sender, BOPropUpdatedEventArgs propEventArgs)
        {
            BusinessObject businessObject = (BusinessObject) propEventArgs.BusinessObject;
            UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Handles the event of a <see cref="IBusinessObject"/> being updated
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void UpdatedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Handles the event of a business object being removed. Removes the
        /// data row that contains the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected virtual void RemovedHandler(object sender, BOEventArgs e)
        {
            lock (_table.Rows)
            {
                int rowNum = this.FindRow(e.BusinessObject);
                if (rowNum == -1) return;

                try
                {
                    this._table.Rows.RemoveAt(rowNum);
                }
                catch (Exception)
                {
                    //IF you hit delete many times in succession then you get an issue with the events interfering and you get a wierd error
                    Console.Write("There was an error");
                }
            }
        }

        /// <summary>
        /// Handles the event of a business object being added. Adds a new
        /// data row containing the object.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        protected virtual void BOAddedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            object[] values = GetValues(businessObject);
            _table.LoadDataRow(values, true);
        }

        /// <summary>
        /// Updates the grid ID column when the Business's ID is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDUpdatedHandler(object sender, BOEventArgs e)
        {
            BusinessObject businessObject = (BusinessObject) e.BusinessObject;
            UpdateBusinessObjectRowValues(businessObject);
        }

        /// <summary>
        /// Initialises the local data
        /// </summary>
        public abstract void InitialiseLocalData();

        /// <summary>
        /// Returns the business object at the row number specified
        /// </summary>
        /// <param name="rowNum">The row number</param>
        /// <returns>Returns a business object</returns>
        public IBusinessObject Find(int rowNum)
        {
            string objectID = this._table.Rows[rowNum][_idColumnName].ToString();
            return _collection.Find(new Guid(objectID));
        }

        /// <summary>
        /// Returns a business object that matches the ID provided
        /// </summary>
        /// <param name="strId">The ID</param>
        /// <returns>Returns a business object</returns>
        public IBusinessObject Find(Guid strId)
        {
            return _collection.Find(strId);
        }

        /// <summary>
        /// Finds the row number in which a specified business object resides
        /// </summary>
        /// <param name="bo">The business object to search for</param>
        /// <returns>Returns the row number if found, or -1 if not found</returns>
        public int FindRow(IBusinessObject bo)
        {
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                DataRow dataRow = _table.Rows[i];
                if (dataRow.RowState == DataRowState.Deleted) continue;
                //string gridIDValue = dataRow[0].ToString();
                //string valuePersisted = bo.ID.AsString_LastPersistedValue();
                //string valueBeforeLastEdit = bo.ID.AsString_PreviousValue();
                //string currentValue = bo.ID.AsString_CurrentValue();
                //if (gridIDValue == valueBeforeLastEdit ||
                //    gridIDValue == valuePersisted ||
                //    gridIDValue == currentValue)
                //{
                //    return i;
                //}

                string rowID = dataRow[0].ToString();
                Guid objectID = bo.ID.ObjectID;
                //string valueBeforeLastEdit = bo.ID.AsString_PreviousValue();
                //string currentValue = bo.ID.AsString_CurrentValue();
                if (rowID == objectID.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets the object initialiser
        /// </summary>
        public IBusinessObjectInitialiser ObjectInitialiser
        {
            set { _objectInitialiser = value; }
        }

        ///<summary>
        /// The column name used for the <see cref="DataTable"/> column which stores the unique object identifier of the <see cref="IBusinessObject"/>.
        /// This column's values will always be the current <see cref="IBusinessObject"/>'s <see cref="IBusinessObject.ID"/> value.
        ///</summary>
        public string IDColumnName
        {
            get { return _idColumnName; }
        }
    }
}