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
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;


    public partial class Vehicle
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            ClassDef itsClassDef = itsLoader.LoadClass( @"
			  <class name=""Vehicle"" assembly=""Habanero.Test.Structure"" table=""table_Vehicle"">
			    <property name=""VehicleID"" type=""Guid"" databaseField=""field_Vehicle_ID"" />
			    <property name=""VehicleType"" databaseField=""field_Vehicle_Type"" />
			    <property name=""DateAssembled"" type=""DateTime"" databaseField=""field_Date_Assembled"" />
			    <property name=""OwnerID"" type=""Guid"" databaseField=""field_Owner_ID"" />
			    <primaryKey>
			      <prop name=""VehicleID"" />
			    </primaryKey>
			    <relationship name=""Owner"" type=""single"" relatedClass=""LegalEntity"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""OwnerID"" relatedProperty=""LegalEntityID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Vehicle"" assembly=""Habanero.Test.Structure"" table=""table_class_Vehicle"">
			    <superClass class=""Entity"" assembly=""Habanero.Test.Structure"" />
			    <property name=""VehicleID"" type=""Guid"" databaseField=""field_Vehicle_ID"" />
			    <property name=""VehicleType"" databaseField=""field_Vehicle_Type"" />
			    <property name=""DateAssembled"" type=""DateTime"" databaseField=""field_Date_Assembled"" />
			    <property name=""OwnerID"" type=""Guid"" databaseField=""field_Owner_ID"" />
			    <primaryKey>
			      <prop name=""VehicleID"" />
			    </primaryKey>
			    <relationship name=""Owner"" type=""single"" relatedClass=""LegalEntity"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""OwnerID"" relatedProperty=""LegalEntityID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Vehicle CreateSavedVehicle()
        {
            Vehicle vehicle = CreateUnsavedVehicle();
            vehicle.Save();
            return vehicle;
        }

        public static Vehicle CreateSavedVehicle(DateTime dateAssembled)
        {
            Vehicle vehicle = CreateUnsavedVehicle(DateTime.Now);
            vehicle.Save();
            return vehicle;
        }

        private static Vehicle CreateUnsavedVehicle()
        {
            return CreateUnsavedVehicle(DateTime.Now);
        }

        private static Vehicle CreateUnsavedVehicle(DateTime dateAssembled)
        {
            Vehicle vehicle = new Vehicle();
            vehicle.DateAssembled = dateAssembled;
            return vehicle;
        }
    }
}