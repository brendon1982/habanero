using System;
using System.Xml;
using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Loads property definitions from xml data
    /// </summary>
    public class XmlPropertyLoader : XmlLoader
    {
        //private Type _propertyType;
    	private string _assemblyName;
    	private string _typeName;
        private PropReadWriteRule _readWriteRule;
        private string _propertyName;
        //private object _defaultValue;
    	private string _defaultValueString;
        private string _databaseFieldName;
        private PropDef _propDef;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlPropertyLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlPropertyLoader()
        {
        }

        /// <summary>
        /// Loads the property definition from the xml string provided
        /// </summary>
        /// <param name="xmlPropDef">The xml string</param>
        /// <returns>Returns a property definition</returns>
        public PropDef LoadProperty(string xmlPropDef)
        {
            if (xmlPropDef == null || xmlPropDef.Length == 0)
            {
                throw new InvalidXmlDefinitionException("An error has occurred while attempting to " +
                   "load a property definition, contained in a 'propertyDef' element. " +
                   "Check that you have correctly specified at least one 'propertyDef' " +
                   "element, which defines a property that is to be mapped from a " +
                   "database field to a property in a class.");
            }
            return LoadProperty(CreateXmlElement(xmlPropDef));
        }

        /// <summary>
        /// Loads the property definition from the xml element provided
        /// </summary>
        /// <param name="propertyElement">The xml property element</param>
        /// <returns>Returns a PropDef object</returns>
        public PropDef LoadProperty(XmlElement propertyElement)
        {
            return (PropDef) Load(propertyElement);
        }

        /// <summary>
        /// Creates the property definition from the data already loaded
        /// </summary>
        /// <returns>Returns a PropDef object</returns>
        protected override object Create()
        {
            return _propDef;
        }

        /// <summary>
        /// Loads the property definition from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadPropertyName();
            LoadPropertyType();
            LoadReadWriteRule();
            LoadDefaultValue();
            LoadDatabaseFieldName();

            _reader.Read();

			_propDef = new PropDef(_propertyName, _assemblyName, _typeName, 
				_readWriteRule, _databaseFieldName, _defaultValueString);

			//if (_databaseFieldName != null)
			//{
			//    _propDef =
			//        new PropDef(_propertyName, _propertyType, _readWriteRule, _databaseFieldName,
			//                    _defaultValue);
			//}
			//else
			//{
			//    _propDef = new PropDef(_propertyName, _propertyType, _readWriteRule, _defaultValue);
			//}

            if (_reader.Name.Length >= 12 && _reader.Name.Substring(0, 12) == "propertyRule")
            {
                XmlPropertyRuleLoader.LoadRuleIntoProperty(_reader.ReadOuterXml(), _propDef, _dtdPath);
            }
            int len = "lookupListSource".Length;
            if (_reader.Name.Length >= len &&
                _reader.Name.Substring(_reader.Name.Length - len, len) == "LookupListSource")
            {
                XmlLookupListSourceLoader.LoadLookupListSourceIntoProperty(_reader.ReadOuterXml(), _propDef,
                                                                           _dtdPath);
            }
        }

        /// <summary>
        /// Loads the property name attribute from the reader
        /// </summary>
        private void LoadPropertyName()
        {
            _propertyName = _reader.GetAttribute("name");
            if (_propertyName == null || _propertyName.Length == 0)
            {
                throw new InvalidXmlDefinitionException("A 'propertyDef' element has no 'name' attribute " +
                   "set. Each 'propertyDef' element requires a 'name' attribute that " +
                   "specifies the name of the property in the class to map to.");
            }
        }

        /// <summary>
        /// Loads the property type from the reader (the "assembly" attribute)
        /// </summary>
        private void LoadPropertyType()
        {
            _assemblyName = _reader.GetAttribute("assembly");
            _typeName = _reader.GetAttribute("type");

            if (_assemblyName == "System")
            {
                Hashtable typeConverter = new Hashtable();
                typeConverter.Add("byte", "Byte");
                typeConverter.Add("sbyte", "Sbyte");
                typeConverter.Add("short", "Int16");
                typeConverter.Add("int", "Int32");
                typeConverter.Add("long", "Int64");
                typeConverter.Add("ushort", "UInt16");
                typeConverter.Add("uint", "UInt32");
                typeConverter.Add("ulong", "UInt64");
                typeConverter.Add("float", "Single");
                typeConverter.Add("object", "Object");
                typeConverter.Add("char", "Char");
                typeConverter.Add("string", "String");
                typeConverter.Add("decimal", "Decimal");
                typeConverter.Add("boolean", "Boolean");
                typeConverter.Add("bool", "Boolean");
                typeConverter.Add("guid", "Guid");

                if (typeConverter.ContainsKey(_typeName))
                {
                    _typeName = (string)typeConverter[_typeName];
                }
            }

			//try
			//{
			//    _propertyType = TypeLoader.LoadType(assemblyName, typeName);
			//}
			//catch (Exception ex)
			//{
			//    throw new UnknownTypeNameException("Unable to load the property type while " +
			//           "attempting to load a property definition, given the 'assembly' as: '" +
			//           assemblyName + "', and the 'type' as: '" + typeName +
			//           "'. Check that the type exists in the given assembly name and " +
			//           "that spelling and capitalisation are correct.", ex);
			//}
        }

        /// <summary>
        /// Loads the read-write rule from the reader
        /// </summary>
        private void LoadReadWriteRule()
        {
            _readWriteRule =
                (PropReadWriteRule)
                Enum.Parse(typeof (PropReadWriteRule), _reader.GetAttribute("readWriteRule"));
        }

        /// <summary>
        /// Loads the default value from the reader
        /// </summary>
        private void LoadDefaultValue()
        {
            _defaultValueString = _reader.GetAttribute("defaultValue");
//            try
//            {
//                if (_defaultValueString != null)
//                {
//                    if (_propertyType == typeof(Guid))
//                    {
//                        _defaultValue = new Guid(_defaultValueString);
//                    }
//                    else
//                    {
//                        _defaultValue = Convert.ChangeType(_defaultValueString, _propertyType);
//                    }
//                }
//                else
//                {
//                    _defaultValue = null;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new InvalidXmlDefinitionException(String.Format(
//                    "The supplied default value for the property definition of " +
//                    "'{0}' is not of the same {1} type as that specified in the " +
//                    "'type' attribute.", _propertyName, _propertyType));
//            }
        }

        /// <summary>
        /// Loads the database field name from the reader
        /// </summary>
        private void LoadDatabaseFieldName()
        {
            _databaseFieldName = _reader.GetAttribute("databaseFieldName");
        }
    }
}