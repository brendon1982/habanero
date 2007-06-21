using System;
using System.Xml;
using Habanero.Bo.ClassDefinition;
using Habanero.Generic;
using Habanero.Util;

namespace Habanero.Bo.Loaders
{
    /// <summary>
    /// Loads a relationship from xml data
    /// </summary>
    public class XmlRelationshipLoader : XmlLoader
    {
        private PropDefCol _propDefCol;
		//private Type _relatedClassType;
    	private string _relatedAssemblyName;
    	private string _relatedClassName;
		private RelKeyDef _relKeyDef;
        private string _name;
        private string _type;
        private bool _keepReferenceToRelatedObject;
        private string _orderBy;
        private int _minNoOfRelatedObjects;
        private int _maxNoOfRelatedObjects;
        private DeleteParentAction _deleteParentAction;

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
		/// <param name="dtdPath">The dtd path</param>
		/// <param name="defClassFactory">The factory for the definition classes</param>
		public XmlRelationshipLoader(string dtdPath, IDefClassFactory defClassFactory)
			: base(dtdPath, defClassFactory)
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlRelationshipLoader()
        {
        }

        /// <summary>
        /// Loads a relationship definition from the xml string provided
        /// </summary>
        /// <param name="xmlRelationshipDef">The xml string</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns a relationship definition</returns>
        public RelationshipDef LoadRelationship(string xmlRelationshipDef, PropDefCol propDefs)
        {
            return LoadRelationship(this.CreateXmlElement(xmlRelationshipDef), propDefs);
        }

        /// <summary>
        /// Loads a relationship definition from the xml element provided
        /// </summary>
        /// <param name="relationshipElement">The xml element</param>
        /// <param name="propDefs">The property definition collection</param>
        /// <returns>Returns a relationship definition</returns>
        public RelationshipDef LoadRelationship(XmlElement relationshipElement, PropDefCol propDefs)
        {
            _propDefCol = propDefs;
            return (RelationshipDef) this.Load(relationshipElement);
        }

        /// <summary>
        /// Creates a single or multiple relationship from the data already
        /// loaded
        /// </summary>
        /// <returns>Returns either a SingleRelationshipDef or
        /// MultipleRelationshipDef object, depending on the type
        /// specification
        /// </returns>
        /// <exception cref="InvalidXmlDefinitionException">Thrown if a
        /// relationship other than 'single' or 'multiple' is specified
        /// </exception>
        protected override object Create()
        {
            if (_type == "single")
            {
				return _defClassFactory.CreateSingleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
					_relKeyDef, _keepReferenceToRelatedObject);
				//return new SingleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
				//    _relKeyDef, _keepReferenceToRelatedObject);				//return
				//    new SingleRelationshipDef(_name, _relatedClassType, _relKeyDef,
				//                  _keepReferenceToRelatedObject);
			}
            else if (_type == "multiple")
            {
				return _defClassFactory.CreateMultipleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
					_relKeyDef, _keepReferenceToRelatedObject, _orderBy, _minNoOfRelatedObjects,
					_maxNoOfRelatedObjects, _deleteParentAction);
				//return new MultipleRelationshipDef(_name, _relatedAssemblyName, _relatedClassName, 
				//    _relKeyDef, _keepReferenceToRelatedObject, _orderBy, _minNoOfRelatedObjects,
				//    _maxNoOfRelatedObjects, _deleteParentAction);
				//return
				//    new MultipleRelationshipDef(_name, _relatedClassType, _relKeyDef,
				//                                _keepReferenceToRelatedObject, _orderBy, _minNoOfRelatedObjects,
				//                                _maxNoOfRelatedObjects, _deleteParentAction);
            }
            else
            {
                throw new InvalidXmlDefinitionException(
                    "There seems to be a problem with the relationshipDef dtd as it should only allow relationships of type 'single' or 'multiple'.");
            }
        }

        /// <summary>
        /// Loads the relationship definition from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _reader.Read();
            LoadRelationshipDef();

            _reader.Read();
            LoadRelKeyDef();
        }

        /// <summary>
        /// Loads the relationship definition from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        protected void LoadRelationshipDef()
        {
            _relatedClassName = _reader.GetAttribute("relatedType");
			_relatedAssemblyName = _reader.GetAttribute("relatedAssembly");
            //_relatedClassType = TypeLoader.LoadType(relatedAssemblyName, relatedClassName);
            _name = _reader.GetAttribute("name");
            _type = _reader.GetAttribute("type");
            
            if (_type == null || (_type != "single" && _type != "multiple"))
            {
                throw new InvalidXmlDefinitionException("In a 'relationshipDef' " +
                    "element, the 'type' attribute was not included or was given " +
                    "an invalid value.  The 'type' refers to the type of " +
                    "relationship and can be either 'single' or 'multiple'.");
            }

            if (_reader.GetAttribute("keepReferenceToRelatedObject") == "true")
            {
                _keepReferenceToRelatedObject = true;
            }
            else
            {
                _keepReferenceToRelatedObject = false;
            }
            _orderBy = _reader.GetAttribute("orderBy");

            try
            {
                _minNoOfRelatedObjects = Convert.ToInt32(_reader.GetAttribute("minNoOfRelatedObjects"));
                _maxNoOfRelatedObjects = Convert.ToInt32(_reader.GetAttribute("maxNoOfRelatedObjects"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'relationshipDef' " +
                    "element, either the 'minNoOfRelatedObjects' or " +
                    "'maxNoOfRelatedObjects' attribute has been given an invalid " +
                    "integer value.", ex);
            }

            try
            {
                _deleteParentAction =
                    (DeleteParentAction)
                    Enum.Parse(typeof (DeleteParentAction), _reader.GetAttribute("deleteParentAction"));
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("In a 'relationshipDef' " +
                    "element, the 'deleteParentAction' attribute has been given " +
                    "an invalid value. The available options are " +
                    "DeleteRelatedObjects, DereferenceRelatedObjects and " +
                    "PreventDeleteParent.", ex);
            }
        }

        /// <summary>
        /// Loads the RelKeyDef information from the reader.  This method
        /// is called by LoadFromReader().
        /// </summary>
        private void LoadRelKeyDef()
        {
            _relKeyDef = new RelKeyDef();
            _reader.Read();
            while (_reader.Name == "relProp")
            {
                string defName = _reader.GetAttribute("name");
                string relPropName = _reader.GetAttribute("relatedPropName");
                if (defName == null || defName.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("A 'relProp' element " +
                        "is missing the 'name' attribute, which specifies the " +
                        "property in this class to which the " +
                        "relationship will link.");
                }
                if (relPropName == null || relPropName.Length == 0)
                {
                    throw new InvalidXmlDefinitionException("A 'relProp' element " +
                        "is missing the 'relatedPropName' attribute, which specifies the " +
                        "property in the related class to which the " + 
                        "relationship will link.");
                }
                
                if (!_propDefCol.Contains(defName))
                {
                    throw new InvalidXmlDefinitionException(String.Format(
                        "In a 'relProp' element, the property '{0}' given in the " +
                        "'name' attribute has not been defined among the class's " +
                        "property definitions. Either add " +
                        "the property definition or check the spelling and " +
                        "capitalisation.", defName));
                }
                _relKeyDef.Add(new RelPropDef(_propDefCol[defName], relPropName));
                ReadAndIgnoreEndTag();
            }
        }
    }
}