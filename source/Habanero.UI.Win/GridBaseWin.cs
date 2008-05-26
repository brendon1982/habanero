using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public abstract class GridBaseWin : DataGridView, IGridBase
    {
        public event EventHandler<BOEventArgs> BusinessObjectSelected;
        public event EventHandler CollectionChanged;
        public event EventHandler FilterUpdated;

        public void Clear()
        {
            _mngr.Clear();
        }

        private readonly GridBaseManager _mngr;

        public GridBaseWin()
        {
            _mngr = new GridBaseManager(this);
            this.SelectionChanged += delegate { FireBusinessObjectSelected(); };
            _mngr.CollectionChanged += delegate{ FireCollectionChanged(); };
        }

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        public void SetBusinessObjectCollection(IBusinessObjectCollection col)
        {
            _mngr.SetBusinessObjectCollection(col);
        }

        /// <summary>
        /// Returns the business object collection being displayed in the grid
        /// </summary>
        /// <returns>Returns a business collection</returns>
        public IBusinessObjectCollection GetBusinessObjectCollection()
        {
            return _mngr.GetBusinessObjectCollection();
        }

        /// <summary>
        /// Returns the business object at the row specified
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public BusinessObject GetBusinessObjectAtRow(int row)
        {
            return _mngr.GetBusinessObjectAtRow(row);
        }

        private void FireCollectionChanged()
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, EventArgs.Empty);
            }
        }

        public new IDataGridViewRowCollection Rows
        {
            get
            {
                
                return new DataGridViewRowCollectionWin(base.Rows);
            }
        }

        public new IDataGridViewSelectedRowCollection SelectedRows
        {
            get { return new DataGridViewSelectedRowCollectionWin(base.SelectedRows); }
        }

        public new IDataGridViewRow CurrentRow
        {
            get
            {
                if (base.CurrentRow == null) return null;
                return new DataGridViewRowWin(base.CurrentRow);
            }
        }
        public new IDataGridViewColumnCollection Columns
        {
            get
            {
                return new DataGridViewColumnCollectionWin(base.Columns);
            }
        }

        public BusinessObject SelectedBusinessObject
        {
            get { return _mngr.SelectedBusinessObject; }
            set { _mngr.SelectedBusinessObject = value; }
        }

        public IList<BusinessObject> SelectedBusinessObjects
        {
            get
            {
                //DataGridViewRow row = new DataGridViewRow();
                //row.DataBoundItem
                 return _mngr.SelectedBusinessObjects;
            }
        }

        //IList IGridBase.SelectedRows
        //{
        //    get { return base.SelectedRows; }
        //}

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        #region IGridBase Members

        int IGridBase.ItemsPerPage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        int IGridBase.CurrentPage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void SelectedBusinessObjectEdited(BusinessObject bo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the event of the currently selected business object being edited.
        /// This is used only for internal testing
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectEdited;

        public void RefreshGrid()
        {
            _mngr.RefreshGrid();
        }

        /// <summary>
        /// Not supported.  Does nothing.
        /// </summary>
        /// <param name="rowNum"></param>
        public void ChangeToPageOfRow(int rowNum)
        {
            
        }

        /// <summary>
        /// initiliase the grid to the with the 'default' UIdef.
        /// </summary>
        public void Initialise()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Sets the sort column and indicates whether
        /// it should be sorted in ascending or descending order
        /// </summary>
        /// <param name="columnName">The column number to set</param>
        /// object property</param>
        /// <param name="ascending">Whether sorting should be done in ascending
        /// order ("false" sets it to descending order)</param>
        public void Sort(string columnName, bool ascending)
        {
            _mngr.SetSortColumn(columnName,  ascending);
        }

        /// <summary>
        /// Applies a filter clause to the data table and updates the filter.
        /// The filter allows you to determine which objects to display using
        /// some criteria.
        /// </summary>
        /// <param name="filterClause">The filter clause</param>
        public void ApplyFilter(IFilterClause filterClause)
        {
            _mngr.ApplyFilter(filterClause);
            FireFilterUpdated();
        }

        /// <summary>
        /// Calls the FilterUpdated() method, passing this instance as the
        /// sender
        /// </summary>
        private void FireFilterUpdated()
        {
            if (this.FilterUpdated != null)
            {
                this.FilterUpdated(this, new EventArgs());
            }
        }

        //public void AddColumn(IDataGridViewColumn column)
        //{
        //    _mngr.AddColumn(column);
        //}

        private class DataGridViewRowCollectionWin : IDataGridViewRowCollection
        {
            private readonly DataGridViewRowCollection _rows;

            public DataGridViewRowCollectionWin(DataGridViewRowCollection rows)
            {
                if (rows == null) throw new ArgumentNullException("rows");
                _rows = rows;
            }

            public int Count
            {
                get { return _rows.Count; }
            }

            public IDataGridViewRow this[int index]
            {

                get { return new DataGridViewRowWin(_rows[index]); }
            }
        }

 
        internal class DataGridViewColumnWin :  IDataGridViewColumn
        {
            private readonly DataGridViewColumn _dataGridViewColumn;

            public DataGridViewColumnWin(DataGridViewColumn dataGridViewColumn)
            {
                _dataGridViewColumn = dataGridViewColumn;
            }

            public DataGridViewColumn DataGridViewColumn
            {
                get { return _dataGridViewColumn; }
            }

            /// <summary>Gets or sets the name of the data source property or database column to which the <see cref="IDataGridViewColumn"></see> is bound.</summary>
            /// <returns>The name of the property or database column associated with the <see cref="IDataGridViewColumn"></see>.</returns>
            /// <filterpriority>1</filterpriority>
            //Editor(
            //    "Gizmox.WebGUI.Forms.Design.DataGridViewColumnDataPropertyNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
            //    , typeof (UITypeEditor)), Gizmox.WebGUI.Forms.SRDescription("DataGridView_ColumnDataPropertyNameDescr"),
            //DefaultValue(""),
            //TypeConverter(
            //    "IForms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
            //    ), Browsable(true)]
            public string DataPropertyName
            {
                get { return _dataGridViewColumn.DataPropertyName; }
                set { _dataGridViewColumn.DataPropertyName = value; }
            }

            /// <summary>Gets or sets the caption text on the column's header cell.</summary>
            /// <returns>A <see cref="T:System.String"></see> with the desired text. The default is an empty string ("").</returns>
            /// <filterpriority>1</filterpriority>
            public string HeaderText
            {
                get { return _dataGridViewColumn.HeaderText; }
                set { _dataGridViewColumn.HeaderText = value; }
            }

            /// <summary>Gets or sets the name of the column.</summary>
            /// <returns>A <see cref="T:System.String"></see> that contains the name of the column. The default is an empty string ("").</returns>
            /// <filterpriority>1</filterpriority>
            public string Name
            {
                get { return _dataGridViewColumn.Name; }
                set { _dataGridViewColumn.Name = value; }
            }

            /// <summary>Gets or sets a value indicating whether the user can edit the column's cells.</summary>
            /// <returns>true if the user cannot edit the column's cells; otherwise, false.</returns>
            /// <exception cref="T:System.InvalidOperationException">This property is set to false for a column that is bound to a read-only data source. </exception>
            /// <filterpriority>1</filterpriority>
            public bool ReadOnly
            {
                get { return _dataGridViewColumn.ReadOnly; }
                set { _dataGridViewColumn.ReadOnly = value; }
            }

            /// <summary>Gets or sets the text used for ToolTips.</summary>
            /// <returns>The text to display as a ToolTip for the column.</returns>
            /// <filterpriority>1</filterpriority>
            public string ToolTipText
            {
                get { return _dataGridViewColumn.ToolTipText; }
                set { _dataGridViewColumn.ToolTipText = value; }
            }

            /// <summary>Gets or sets the data type of the values in the column's cells.</summary>
            /// <returns>A <see cref="T:System.Type"></see> that describes the run-time class of the values stored in the column's cells.</returns>
            /// <filterpriority>1</filterpriority>
            public Type ValueType
            {
                get { return _dataGridViewColumn.ValueType; }
                set { _dataGridViewColumn.ValueType = value; }
            }

            /// <summary>Gets or sets the current width of the column.</summary>
            /// <returns>The width, in pixels, of the column. The default is 100.</returns>
            /// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is greater than 65536.</exception>
            /// <filterpriority>1</filterpriority>
            public int Width
            {
                get { return _dataGridViewColumn.Width; }
                set { _dataGridViewColumn.Width = value; }
            }

            public bool Visible
            {
                get { return _dataGridViewColumn.Visible; }
                set { _dataGridViewColumn.Visible = value; }
            }
        }

        private class DataGridViewColumnCollectionWin : IDataGridViewColumnCollection
        {
            private readonly DataGridViewColumnCollection _columns;

            public DataGridViewColumnCollectionWin(DataGridViewColumnCollection columns)
            {
                if (columns == null) throw new ArgumentNullException("columns");
                _columns = columns;
            }

            #region IDataGridViewColumnCollection Members

            public int Count
            {
                get { return _columns.Count; }
            }

            public void Clear()
            {
                _columns.Clear();
            }

            public void Add(IDataGridViewColumn dataGridViewColumn)
            {
                throw new NotImplementedException();
            }

            public int Add(string columnName, string headerText)
            {
                int addedColumn = _columns.Add(columnName, headerText);
                _columns[addedColumn].DataPropertyName = columnName;
                return addedColumn;
            }

            public IDataGridViewColumn this[int index]
            {
                get { return new DataGridViewColumnWin(_columns[index]); }
            }

            public IDataGridViewColumn this[string name]
            {
                get { return new DataGridViewColumnWin(_columns[name]); }
            }

            #endregion

            #region IEnumerable<IDataGridViewColumn> Members

            ///<summary>
            ///Returns an enumerator that iterates through the collection.
            ///</summary>
            ///
            ///<returns>
            ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>1</filterpriority>
            IEnumerator<IDataGridViewColumn> IEnumerable<IDataGridViewColumn>.GetEnumerator()
            {
                foreach (DataGridViewColumn column in _columns)
                {
                    yield return new DataGridViewColumnWin(column);
                }
            }

            #endregion

            #region IEnumerable Members

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (DataGridViewColumn column in _columns)
                {
                    yield return new DataGridViewColumnWin(column);
                }
            }

            #endregion
        }



        private class DataGridViewRowWin : IDataGridViewRow
        {
            private readonly DataGridViewRow _dataGridViewRow;

            public DataGridViewRowWin(DataGridViewRow dataGridViewRow)
            {
                _dataGridViewRow = dataGridViewRow;
            }

            public bool Selected
            {
                get { return _dataGridViewRow.Selected; }
                set { _dataGridViewRow.Selected = value; }
            }

            public int Index
            {
                get { return _dataGridViewRow.Index; }
            }

            public bool SetValues(params object[] values)
            {
                return this._dataGridViewRow.SetValues(values);
            }

            /// <summary>Gets the collection of cells that populate the row.</summary>
            /// <returns>A <see cref="IDataGridViewCellCollection"></see> that contains all of the cells in the row.</returns>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCellCollection Cells
            {
                get { return new DataGridViewCellCollectionWin(_dataGridViewRow.Cells); }
            }

            public object DataBoundItem
            {
                get { return _dataGridViewRow.DataBoundItem; }
            }
        }


        private class DataGridViewCellCollectionWin : IDataGridViewCellCollection
        {
            private readonly DataGridViewCellCollection _cells;

            public DataGridViewCellCollectionWin(DataGridViewCellCollection cells)
            {
                _cells = cells;
            }

            public int Count
            {
                get { return _cells.Count; }
            }

            /// <summary>Adds a cell to the collection.</summary>
            /// <returns>The position in which to insert the new element.</returns>
            /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to add to the collection.</param>
            /// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.-or-dataGridViewCell already belongs to a <see cref="System.Windows.DataGridViewRow"></see>.</exception>
            /// <filterpriority>1</filterpriority>
            public int Add(IDataGridViewCell dataGridViewCell)
            {
                return _cells.Add((DataGridViewCell) dataGridViewCell);
            }

            /// <summary>Clears all cells from the collection.</summary>
            /// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.</exception>
            /// <filterpriority>1</filterpriority>
            public void Clear()
            {
                _cells.Clear();
            }

            /// <summary>Determines whether the specified cell is contained in the collection.</summary>
            /// <returns>true if dataGridViewCell is in the collection; otherwise, false.</returns>
            /// <param name="dataGridViewCell">A <see cref="IDataGridViewCell"></see> to locate in the collection.</param>
            /// <filterpriority>1</filterpriority>
            public bool Contains(IDataGridViewCell dataGridViewCell)
            {
                return _cells.Contains((DataGridViewCell) dataGridViewCell);
            }

            ///// <summary>Copies the entire collection of cells into an array at a specified location within the array.</summary>
            ///// <param name="array">The destination array to which the contents will be copied.</param>
            ///// <param name="index">The index of the element in array at which to start copying.</param>
            ///// <filterpriority>1</filterpriority>
            //public void CopyTo(IDataGridViewCell[] array, int index)
            //{
            //    throw new NotImplementedException();
            //}

            /// <summary>Returns the index of the specified cell.</summary>
            /// <returns>The zero-based index of the value of dataGridViewCell parameter, if it is found in the collection; otherwise, -1.</returns>
            /// <param name="dataGridViewCell">The cell to locate in the collection.</param>
            /// <filterpriority>1</filterpriority>
            public int IndexOf(IDataGridViewCell dataGridViewCell)
            {
                return _cells.IndexOf((DataGridViewCell) dataGridViewCell);
            }

            ///// <summary>Inserts a cell into the collection at the specified index. </summary>
            ///// <param name="dataGridViewCell">The <see cref="IDataGridViewCell"></see> to insert.</param>
            ///// <param name="index">The zero-based index at which to place dataGridViewCell.</param>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.-or-dataGridViewCell already belongs to a <see cref="System.Windows.DataGridViewRow"></see>.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void Insert(int index, IDataGridViewCell dataGridViewCell)
            //{
            //    throw new NotImplementedException();
            //}

            ///// <summary>Removes the specified cell from the collection.</summary>
            ///// <param name="cell">The <see cref="IDataGridViewCell"></see> to remove from the collection.</param>
            ///// <exception cref="T:System.ArgumentException">cell could not be found in the collection.</exception>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void Remove(IDataGridViewCell cell)
            //{
            //    throw new NotImplementedException();
            //}

            ///// <summary>Removes the cell at the specified index.</summary>
            ///// <param name="index">The zero-based index of the <see cref="IDataGridViewCell"></see> to be removed.</param>
            ///// <exception cref="T:System.InvalidOperationException">The row that owns this <see cref="IDataGridViewCellCollection"></see> already belongs to a <see cref="System.Windows.DataGridView"></see> control.</exception>
            ///// <filterpriority>1</filterpriority>
            //public void RemoveAt(int index)
            //{
            //    throw new NotImplementedException();
            //}

            public IDataGridViewCell this[int index]
            {
                get { return new DataGridViewCellWin(_cells[index]); }
            }

            /// <summary>Gets or sets the cell in the column with the provided name. In C#, this property is the indexer for the <see cref="IDataGridViewCellCollection"></see> class.</summary>
            /// <returns>The <see cref="IDataGridViewCell"></see> stored in the column with the given name.</returns>
            /// <param name="columnName">The name of the column in which to get or set the cell.</param>
            /// <exception cref="T:System.InvalidOperationException">The specified cell when setting this property already belongs to a <see cref="System.Windows.DataGridView"></see> control.-or-The specified cell when setting this property already belongs to a <see cref="System.Windows.DataGridViewRow"></see>.</exception>
            /// <exception cref="T:System.ArgumentException">columnName does not match the name of any columns in the control.</exception>
            /// <exception cref="T:System.ArgumentNullException">The specified value when setting this property is null.</exception>
            /// <filterpriority>1</filterpriority>
            public IDataGridViewCell this[string columnName]
            {
                get { return new DataGridViewCellWin(_cells[columnName]); }
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                foreach (DataGridViewRow row in _cells)
                {
                    yield return new DataGridViewRowWin(row);
                }
            }
        }

        private class DataGridViewSelectedRowCollectionWin : IDataGridViewSelectedRowCollection
        {
            private readonly DataGridViewSelectedRowCollection _selectedRows;

            public DataGridViewSelectedRowCollectionWin(DataGridViewSelectedRowCollection selectedRows)
            {
                _selectedRows = selectedRows;
            }

            public int Count
            {
                get {return _selectedRows.Count; }
            }

            public IDataGridViewRow this[int index]
            {
                get { return new DataGridViewRowWin(_selectedRows[index]); }
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                foreach (DataGridViewRow row in _selectedRows)
                {
                    yield return new DataGridViewRowWin(row);
                }
            }
        }
    }

    internal class DataGridViewCellWin : IDataGridViewCell
    {
        private readonly DataGridViewCell _cell;

        public DataGridViewCellWin(DataGridViewCell cell)
        {
            _cell = cell;
        }

        /// <summary>Gets the column index for this cell. </summary>
        /// <returns>The index of the column that contains the cell; -1 if the cell is not contained within a column.</returns>
        /// <filterpriority>1</filterpriority>
        public int ColumnIndex
        {
            get { return _cell.ColumnIndex; }
        }

        /// <summary>Gets a value that indicates whether the cell is currently displayed on-screen. </summary>
        /// <returns>true if the cell is on-screen or partially on-screen; otherwise, false.</returns>
        public bool Displayed
        {
            get { return _cell.Displayed; }
        }

        /// <summary>Gets a value indicating whether the cell is frozen. </summary>
        /// <returns>true if the cell is frozen; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        public bool Frozen
        {
            get { return _cell.Frozen; }
        }

        /// <summary>Gets a value indicating whether this cell is currently being edited.</summary>
        /// <returns>true if the cell is in edit mode; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">The row containing the cell is a shared row.</exception>
        public bool IsInEditMode
        {
            get { return _cell.IsInEditMode; }
        }

        /// <summary>Gets or sets a value indicating whether the cell's data can be edited. </summary>
        /// <returns>true if the cell's data can be edited; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">There is no owning row when setting this property. -or-The owning row is shared when setting this property.</exception>
        /// <filterpriority>1</filterpriority>
        public bool ReadOnly
        {
            get { return _cell.ReadOnly; }
            set { _cell.ReadOnly = value; }
        }

        /// <summary>Gets the index of the cell's parent row. </summary>
        /// <returns>The index of the row that contains the cell; -1 if there is no owning row.</returns>
        /// <filterpriority>1</filterpriority>
        public int RowIndex
        {
            get { return _cell.RowIndex; }
        }

        /// <summary>Gets or sets a value indicating whether the cell has been selected. </summary>
        /// <returns>true if the cell has been selected; otherwise, false.</returns>
        /// <exception cref="T:System.InvalidOperationException">There is no associated <see cref="IDataGridView"></see> when setting this property. -or-The owning row is shared when setting this property.</exception>
        /// <filterpriority>1</filterpriority>
        public bool Selected
        {
            get { return _cell.Selected; }
            set { _cell.Selected = value; }
        }

        /// <summary>Gets or sets the value associated with this cell. </summary>
        /// <returns>Gets or sets the data to be displayed by the cell. The default is null.</returns>
        /// <exception cref="T:System.InvalidOperationException"><see cref="System.Windows.Forms.DataGridViewCell.ColumnIndex"></see> is less than 0, indicating that the cell is a row header cell.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><see cref="System.Windows.Forms.DataGridViewCell.RowIndex"></see> is outside the valid range of 0 to the number of rows in the control minus 1.</exception>
        /// <filterpriority>1</filterpriority>
        public object Value
        {
            get { return _cell.Value; }
            set { _cell.Value = value; }
        }

        /// <summary>Gets or sets the data type of the values in the cell. </summary>
        /// <returns>A <see cref="T:System.Type"></see> representing the data type of the value in the cell.</returns>
        /// <filterpriority>1</filterpriority>
        public Type ValueType
        {
            get { return _cell.ValueType; }
            set { _cell.ValueType = value; }
        }

        /// <summary>Gets a value indicating whether the cell is in a row or column that has been hidden. </summary>
        /// <returns>true if the cell is visible; otherwise, false.</returns>
        /// <filterpriority>1</filterpriority>
        public bool Visible
        {
            get { return _cell.Visible; }
        }
    }
    
}