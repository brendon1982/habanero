using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Manages a collection of tab pages that hold business object controls
    /// </summary>
    public class BoTabColControl : UserControl
    {
        private TabControl _tabControl;
        private CollectionTabControlMapper _collectionTabControlMapper;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public BoTabColControl()
        {
            Permission.Check(this);
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = new TabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _collectionTabControlMapper = new CollectionTabControlMapper(_tabControl);
        }

        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public void SetBusinessObjectControl(IBusinessObjectControl boControl)
        {
            _collectionTabControlMapper.SetBusinessObjectControl(boControl);
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="businessObjectCollection">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IBusinessObjectCollection businessObjectCollection)
        {
            _collectionTabControlMapper.SetCollection(businessObjectCollection);
        }

        /// <summary>
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        public void TabChanged()
        {
            _collectionTabControlMapper.TabChanged();
        }

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        public TabControl TabControl
        {
            get { return _tabControl; }
        }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        public BusinessObject GetBo(TabPage tabPage)
        {
            return _collectionTabControlMapper.GetBo(tabPage);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public TabPage GetTabPage(BusinessObject bo)
        {
            return _collectionTabControlMapper.GetTabPage(bo);
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public BusinessObject CurrentBusinessObject
        {
            get { return _collectionTabControlMapper.CurrentBusinessObject; }
            set { _collectionTabControlMapper.CurrentBusinessObject = value; }
        }
    }
}