//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// NB Custom code should be placed in the provided stub class.
// Please do not modify this class directly!
// ------------------------------------------------------------------------------

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    using Habanero.Test.Structure;
    
    
    public partial class LegalEntity : Entity
    {
        
        #region Properties
        public virtual Guid? LegalEntityID
        {
            get
            {
                return ((Guid?)(base.GetPropertyValue("LegalEntityID")));
            }
            set
            {
                base.SetPropertyValue("LegalEntityID", value);
            }
        }
        
        public virtual String LegalEntityType
        {
            get
            {
                return ((String)(base.GetPropertyValue("LegalEntityType")));
            }
            set
            {
                base.SetPropertyValue("LegalEntityType", value);
            }
        }
        #endregion
        
        #region Relationships
        public virtual BusinessObjectCollection<Vehicle> VehiclesOwned
        {
            get
            {
                return Relationships.GetRelatedCollection<Vehicle>("VehiclesOwned");
            }
        }
        #endregion
    }
}
