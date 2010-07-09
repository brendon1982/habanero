// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
//using System.Linq;
using Habanero.Base;

namespace Habanero.BO.ClassDefinition
{
    /// <summary>
    /// Defines a relationship where the owner relates to only one other object.
    /// </summary>
    public class SingleRelationshipDef : RelationshipDef
    {
        private bool _setAsOneToOne;
        private bool _setAsCompulsory;

        #region Constructors

        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        /// <summary>
        /// Constructor to create a new single relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectClassType">The class type of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
        /// <param name="deleteParentAction"></param>
        public SingleRelationshipDef
            (string relationshipName, Type relatedObjectClassType, RelKeyDef relKeyDef,
             bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction)
            : base(relationshipName, relatedObjectClassType, relKeyDef, keepReferenceToRelatedObject, deleteParentAction
                )
        {
            OwningBOHasForeignKey = true;
        }

        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        /// <summary>
        /// Constructor to create a new single relationship definition
        /// </summary>
        /// <param name="relationshipName">A name for the relationship</param>
        /// <param name="relatedObjectAssemblyName">The assembly name of the related object</param>
        /// <param name="relatedObjectClassName">The class name of the related object</param>
        /// <param name="relKeyDef">The related key definition</param>
        /// <param name="keepReferenceToRelatedObject">Whether to keep a
        /// reference to the related object.  Could be false for memory-
        /// intensive applications.</param>
        /// <param name="deleteParentAction"></param>
        public SingleRelationshipDef
            (string relationshipName, string relatedObjectAssemblyName, string relatedObjectClassName,
             IRelKeyDef relKeyDef, bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction)
            : this(
                relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef,
                keepReferenceToRelatedObject, deleteParentAction, InsertParentAction.InsertRelationship,
                RelationshipType.Association)
        {
        }

        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        ///<summary>
        /// Constructs a single Relationship
        ///</summary>
        ///<param name="relationshipName"></param>
        ///<param name="relatedObjectAssemblyName"></param>
        ///<param name="relatedObjectClassName"></param>
        ///<param name="relKeyDef"></param>
        ///<param name="keepReferenceToRelatedObject"></param>
        ///<param name="deleteParentAction"></param>
        ///<param name="insertParentAction"><see cref="InsertParentAction"/></param>
        ///<param name="relationshipType"></param>
        public SingleRelationshipDef(string relationshipName, string relatedObjectAssemblyName,
                                     string relatedObjectClassName, IRelKeyDef relKeyDef,
                                     bool keepReferenceToRelatedObject, DeleteParentAction deleteParentAction,
                                     InsertParentAction insertParentAction, RelationshipType relationshipType)
            : base(
                relationshipName, relatedObjectAssemblyName, relatedObjectClassName, relKeyDef,
                keepReferenceToRelatedObject, deleteParentAction, insertParentAction, relationshipType)
        {
            OwningBOHasForeignKey = true;
        }

        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        #endregion Constructors

        ///<summary>
        /// Returns true where the owning business object has the foreign key for this relationship false otherwise.
        /// This is used to differentiate between the two sides of the relationship.
        ///</summary>
        public override bool OwningBOHasForeignKey { get; set; }


        /// <summary>
        /// Overrides abstract method of RelationshipDef to create a new
        /// relationship
        /// </summary>
        /// <param name="owningBo">The business object that will manage
        /// this relationship</param>
        /// <param name="lBOPropCol">The collection of properties</param>
        /// <returns></returns>
        public override IRelationship CreateRelationship(IBusinessObject owningBo, IBOPropCol lBOPropCol)
        {
            Type relationshipBOType = typeof (SingleRelationship<>).MakeGenericType(this.RelatedObjectClassType);
            return (ISingleRelationship) Activator.CreateInstance(relationshipBOType, owningBo, this, lBOPropCol);
        }

        public override bool IsOneToMany
        {
            get { return false; }
        }

        public override bool IsManyToOne
        {
            get { return !_setAsOneToOne; }
        }

        public override bool IsOneToOne
        {
            get { return _setAsOneToOne; }
        }

        public override bool IsCompulsory
        {
            get
            {
                return _setAsCompulsory || AreAllPropsCompulsory();
            }
        }

        /// <summary>
        /// Sets this SingleRelationshipDef as a One To One.
        /// This overrides the default of it being set to ManyToOne
        /// </summary>
        public void SetAsOneToOne()
        {
            _setAsOneToOne = true;
        }

        /// <summary>
        /// Sets the single relationship as compulsory.
        /// If this is set to true then the relationship is treated as compulsory 
        /// else it uses the <see cref="RelationshipDef.IsCompulsory"/>
        /// </summary>
        public void SetAsCompulsory()
        {
            _setAsCompulsory = true;
        }
        
        /// <summary>
        /// Returns true if this RelationshipDef is compulsory.
        /// This relationship def will be considered to be compulsory if this
        /// <see cref="OwningBOHasForeignKey"/> and all the <see cref="IPropDef"/>'s that make up the 
        /// <see cref="IRelKeyDef"/> are compulsory
        /// </summary>
        private bool AreAllPropsCompulsory()
        {
            //TODO_ brett 08 Jun 2010: For DotNet 2_0  
            if(!(this.OwningBOHasForeignKey
                   && this.RelKeyDef != null
                   && this.RelKeyDef.Count > 0)) return false;

            foreach (var relPropDef in this.RelKeyDef)
            {
                if (PropDefIsCompulsory(relPropDef)) continue;
                return false;
            }
            return true;
            // ...end of .NET 2 Code.
            /*//TODO_ brett 08 Jun 2010: For DotNet 2_0
                        return this.OwningBOHasForeignKey
                               && this.RelKeyDef != null
                               && this.RelKeyDef.Count > 0
                               && this.RelKeyDef.All(PropDefIsCompulsory);*/
        }
        

        private bool PropDefIsCompulsory(IRelPropDef def)
        {
            return (def.OwnerPropDef != null && def.OwnerPropDef.Compulsory)
                   || (this.OwningClassDef != null
                       && this.OwningClassDef.GetPropDef(def.OwnerPropertyName).Compulsory);
        }
    }
}