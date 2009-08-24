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
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlClassLoader
    {
        private XmlClassLoader _loader;

        [SetUp]
        public void SetupTest()
        {
            Initialise();
            ClassDef.ClassDefs.Clear();
        }

        protected void Initialise() {
            _loader = new XmlClassLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), ExpectedMessage = "An invalid node 'class1' was encountered when loading the class definitions.")]
        public void TestInvalidXmlFormatWrongRootElement()
        {
            _loader.LoadClass("<class1 name=\"TestClass\" assembly=\"Habanero.Test.BO.Loaders\" />");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestClassWithNoAssembly()
        {
            IClassDef def = _loader.LoadClass(@"
                <class name=""TestClass"" >
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestClassWithNoClassname()
        {
            IClassDef def = _loader.LoadClass(@"
                <class assembly=""Habanero.Test.BO.Loaders"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
        }

        [Test]
        public void TestNoTableName()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("TestClass", def.TableName);


        }

        [Test]
        public void TestTableName()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" table=""myTable"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("myTable", def.TableName);
        }

        [Test]
        public void TestTypeParameter()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" typeParameter=""TestTypeParameter"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("TestTypeParameter", def.TypeParameter);
        }

        [Test]
        public void TestPropDefClassDefIsSet()
        {
            //---------------Set up test pack-------------------
            string classDefXml =
                @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" table=""myTable"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			";
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IClassDef def = _loader.LoadClass(classDefXml);
            //---------------Test Result -----------------------
            Assert.AreSame(def, def.PropDefcol["TestProp"].ClassDef);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDisplayName()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" displayName=""Testing Class"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("Testing Class", def.DisplayName);
        }

        [Test]
        public void TestTableNameWithSpaces()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" table=""my Table"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("my Table", def.TableName);
        }

        [Test]
        public void TestTwoPropClass()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual(2, def.PropDefcol.Count);
            Assert.AreEqual("TestClass", def.ClassName);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestClassWithNoProps()
        {
            IClassDef def = _loader.LoadClass(
                @"<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
				</class>
			");
        }

        [Test]
        public void TestClassWithNoProps_WithSuperClass()
        {
            ClassDef.LoadClassDefs(
                new XmlClassDefsLoader(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
					</classes>",
                                 new DtdLoader(), GetDefClassFactory()));
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"">
					<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />					
				</class>
			");
            Assert.AreEqual(0, def.PropDefcol.Count, "Should contain no properties.");
            Assert.IsNotNull(def.SuperClassDef);
            //ClassDef parentDef = ClassDef.ClassDefs[typeof(TestClass)];
            IClassDef parentDef = ClassDef.ClassDefs["Habanero.Test.BO.Loaders", "TestClass"];
            
            IClassDef superClassDef = def.SuperClassDef.SuperClassClassDef;
            Assert.AreSame(parentDef, superClassDef);

        }

        [Test]
        public void TestClassWithPrimaryKeyDef()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
				</class>
			");
            Assert.IsNotNull(def.PrimaryKeyDef);
            Assert.AreEqual(1, def.PrimaryKeyDef.Count);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestClassWithNoPrimaryKeyException()
        {
            IClassDef def = _loader.LoadClass(
                @"<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
				    <property  name=""TestProp"" />					
				</class>");
        }

        [Test]
        public void TestClassWithInheritanceAndNoPrimaryKey()
        {
            IClassDef def = _loader.LoadClass(
                @"<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
                    <superClass class=""SomeTestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					<property  name=""TestProp"" />
				</class>
			");
            Assert.IsNull(def.PrimaryKeyDef);
            Assert.IsNotNull(def.SuperClassDef);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestClassWithMoreThanOnePrimaryKeyDef()
        {
            _loader.LoadClass(
                @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
					<primaryKey>
						<prop name=""TestProp2"" />
					</primaryKey>
				</class>
			");
        }

        // Not reaching the exception I was trying to cover (line 254 of XmlClassLoader)
//        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
//        public void TestClassWithInvalidPrimaryKeyDef()
//        {
//            _loader.LoadClass(@"
//				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
//					<property name=""TestProp"" />
//					<primaryKey/>
//				</class>
//			");
//        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestClassWithPrimaryKeyAndNoProps()
        {
            IClassDef def = _loader.LoadClass(
                @"<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<primaryKey/>
				</class>
			");
        }

        [Test]
        public void TestClassWithKeyDefs()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestProp3"" />
					<key>
						<prop name=""TestProp"" />
					</key>
					<key>
						<prop name=""TestProp2"" />
						<prop name=""TestProp3"" />
					</key>
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual(2, def.KeysCol.Count);
        }

        [Test]
        public void TestClassWithSingleRelationship()
        {
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.BO.Loaders""
					>
						<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>
				</class>
			");
            IRelationshipDefCol relDefCol = def.RelationshipDefCol;
            Assert.AreEqual(1, relDefCol.Count, "There should be one relationship def from the given xml definition");
            Assert.IsNotNull(relDefCol["TestRelationship"],
                             "'TestRelationship' should be the name of the relationship created");
        }

        [Test]
        public void TestClassWithSuperClass()
        {
            ClassDef.LoadClassDefs(
                new XmlClassDefsLoader(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
					</classes>",
                                 new DtdLoader(), GetDefClassFactory()));
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"">
					<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					<property  name=""TestProp"" type=""Guid"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.IsNotNull(def.SuperClassDef);
            //ClassDef parentDef = ClassDef.ClassDefs[typeof(TestClass)];
            IClassDef parentDef = ClassDef.ClassDefs["Habanero.Test.BO.Loaders", "TestClass"];
            IClassDef superClassDef = def.SuperClassDef.SuperClassClassDef;
            Assert.AreSame(parentDef, superClassDef);
        }

        [Test]
        public void TestClassWithSuperClassWithNoPK()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Remove(typeof (TestClass));
            ClassDef.ClassDefs.Remove(typeof (TestRelatedClass));
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
					</classes>",
                                new DtdLoader(), GetDefClassFactory()));
            IClassDef parentDef = ClassDef.ClassDefs["Habanero.Test.BO.Loaders", "TestClass"];
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IClassDef def = _loader.LoadClass(
                                          @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"">
					<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					<property  name=""TestProp"" type=""Guid"" />                    
				</class>
			");
            //---------------Test Result -----------------------
            Assert.IsNotNull(def.SuperClassDef);
            IClassDef superClassDef = def.SuperClassDef.SuperClassClassDef;
            Assert.AreSame(parentDef, superClassDef);
            Assert.IsNotNull(def.PrimaryKeyDef);
            Assert.AreSame(parentDef.PrimaryKeyDef, def.PrimaryKeyDef);
        }

        [Test]
        public void TestClassWithSuperClassWithRelationshipAndNoPK()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Remove(typeof(TestClass));
            ClassDef.ClassDefs.Remove(typeof(TestRelatedClass));
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
					</classes>",
                                new DtdLoader(), GetDefClassFactory()));
            IClassDef parentDef = ClassDef.ClassDefs["Habanero.Test.BO.Loaders", "TestClass"];
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IClassDef def = _loader.LoadClass(
                    @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"">
					<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					<property name=""TestProp"" type=""Guid"" />    
					<property name=""RelatedTestClassID"" type=""Guid"" />    
                    <relationship name=""TestRelationship"" type=""single"" 
						relatedClass=""TestClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						<relatedProperty property=""RelatedTestClassID"" relatedProperty=""TestClassID"" />
					</relationship>                
				</class>
			");
            //---------------Test Result -----------------------
            Assert.IsNotNull(def.SuperClassDef);
            IClassDef superClassDef = def.SuperClassDef.SuperClassClassDef;
            Assert.AreSame(parentDef, superClassDef);
            IRelationshipDefCol relDefCol = def.RelationshipDefCol;
            Assert.AreEqual(1, relDefCol.Count, "There should be one relationship def from the given xml definition");
            Assert.IsNotNull(relDefCol["TestRelationship"], "'TestRelationship' should be the name of the relationship created");
        }

        [Test]
        public void TestClassWithUIDef()
        {
            ClassDef.ClassDefs.Remove(typeof(TestClass));
            ClassDef.ClassDefs.Remove(typeof(TestRelatedClass));
            IClassDef def =
                _loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""testtab"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            IUIDef uiDef = def.UIDefCol["default"];
            Assert.IsNotNull(uiDef);
            Assert.IsNotNull(uiDef.UIForm);
            Assert.IsNull(uiDef.UIGrid);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestEmptyXmlStringException()
        {
            IClassDef def = _loader.LoadClass("");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidClassElementException()
        {
            ClassDef.ClassDefs.Remove(typeof(TestClass));
            ClassDef.ClassDefs.Remove(typeof(TestRelatedClass));
            IClassDef def = _loader.LoadClass(@"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"">
					<property  name=""TestProp"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
                    <invalid/>
				</class>
			");
        }

        [Test]
        public void TestClassDefID()
        {
            //---------------Set up test pack-------------------
            Guid classDefID = Guid.NewGuid();
            string classDefXml =
                @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" classID=""" + classDefID.ToString("B") + @""" >
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			";

            //---------------Execute Test ----------------------
            IClassDef def = _loader.LoadClass(classDefXml);
            //---------------Test Result -----------------------
            Assert.IsNotNull(def.ClassID);
            Assert.AreEqual(classDefID, def.ClassID);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public virtual void TestClassDefID_Null()
        {
            //---------------Set up test pack-------------------
            string classDefXml =
                @"
				<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			";

            //---------------Execute Test ----------------------
            IClassDef def = _loader.LoadClass(classDefXml);
            //---------------Test Result -----------------------
            Assert.IsNull(def.ClassID);
            //---------------Tear Down -------------------------          
        }
    }

    public class TestClass : BusinessObject
    {
        protected override IClassDef ConstructClassDef()
        {
            throw new NotImplementedException();
        }
    }

    public class TestRelatedClass : BusinessObject
    {
        protected override IClassDef ConstructClassDef()
        {
            throw new NotImplementedException();
        }
    }
}