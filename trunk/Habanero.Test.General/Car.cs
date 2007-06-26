// Static Model
using System;
using System.Collections;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Db;

namespace Habanero.Test.General
{
    public class Car : BusinessObject
    {
        #region Constructors

        public Car() : base()
        {
        }

        internal Car(BOPrimaryKey id) : base(id)
        {
        }

        public Car(ClassDef classDef) : base(classDef)
        {
        }

        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Car)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol[typeof (Car)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["CarID"]);

            RelationshipDefCol relDefCol = CreateRelationshipDefCol(lPropDefCol);


            ClassDef lClassDef = new ClassDef(typeof (Car), primaryKey, lPropDefCol, keysCol, relDefCol);
			ClassDef.GetClassDefCol.Add(lClassDef);
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Owner Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            PropDef propDef = lPropDefCol["OwnerId"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "ContactPersonID");
            relKeyDef.Add(lRelPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("Owner", typeof (ContactPerson), relKeyDef, false);

            relDefCol.Add(relDef);

            //Define Driver Relationships
            relKeyDef = new RelKeyDef();
            propDef = lPropDefCol["DriverFK1"];

            lRelPropDef = new RelPropDef(propDef, "PK1Prop1");
            relKeyDef.Add(lRelPropDef);

            propDef = lPropDefCol["DriverFK2"];

            lRelPropDef = new RelPropDef(propDef, "PK1Prop2");
            relKeyDef.Add(lRelPropDef);

            relDef = new SingleRelationshipDef("Driver", typeof (ContactPersonCompositeKey), relKeyDef, true);


            relDefCol.Add(relDef);

            //Define Engine Relationships
            relKeyDef = new RelKeyDef();
            propDef = lPropDefCol["CarID"];

            lRelPropDef = new RelPropDef(propDef, "CarID");
            relKeyDef.Add(lRelPropDef);

            relDef = new SingleRelationshipDef("Engine", typeof (Engine), relKeyDef, false);

            relDefCol.Add(relDef);
            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("CarRegNo", typeof (String), PropReadWriteRule.ReadWrite, "CAR_REG_NO", null);
            lPropDefCol.Add(propDef);

            lPropDefCol.Add("OwnerId", typeof (Guid), PropReadWriteRule.WriteOnce, "OWNER_ID", null);

            propDef = lPropDefCol.Add("CarID", typeof (Guid), PropReadWriteRule.WriteOnce, "CAR_ID", null);
            propDef =
                lPropDefCol.Add("DriverFK1", typeof (String), PropReadWriteRule.WriteOnce, "Driver_FK1", null);
            propDef =
                lPropDefCol.Add("DriverFK2", typeof (String), PropReadWriteRule.WriteOnce, "Driver_FK2", null);

            return lPropDefCol;
        }

        /// <summary>
        /// Creates a new contact person and adds this new contact person to the object manager collection
        /// </summary>
        /// <returns>newly created contact person Car</returns>
        public static Car GetNewCar()
        {
            Car myCar = new Car();
            AddToLoadedBusinessObjectCol(myCar);
            return myCar;
        }

        /// <summary>
        /// returns the Car identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.Bo.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static Car GetCar(BOPrimaryKey id)
        {
            Car myCar = (Car) Car.GetLoadedBusinessObject(id);
            if (myCar == null)
            {
                myCar = new Car(id);
                AddToLoadedBusinessObjectCol(myCar);
            }
            return myCar;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

        #endregion //Properties

        #region Relationships

        public ContactPerson GetOwner()
        {
            return (ContactPerson) Relationships.GetRelatedBusinessObject("Owner");
        }

        public ContactPersonCompositeKey GetDriver()
        {
            return (ContactPersonCompositeKey) Relationships.GetRelatedBusinessObject("Driver");
        }

        public Engine GetEngine()
        {
            return (Engine) Relationships.GetRelatedBusinessObject("Engine");
        }

        #endregion //Relationships

        #region ForTesting

        internal static void ClearCarCol()
        {
            BusinessObject.ClearLoadedBusinessObjectBaseCol();
        }

        internal static Hashtable GetCarCol()
        {
            return BusinessObject.GetLoadedBusinessObjectBaseCol();
        }

        internal static void DeleteAllCars()
        {
            string sql = "DELETE FROM Car";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections 

        //class
        protected internal string GetObjectNewID()
        {
            return _primaryKey.GetObjectNewID();
        }

        protected internal static BusinessObjectCollection LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        protected internal static BusinessObjectCollection LoadBusinessObjCol(string searchCriteria,
                                                                                  string orderByClause)
        {
            BusinessObjectCollection bOCol = new BusinessObjectCollection(GetClassDef());
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        #endregion
    }
}