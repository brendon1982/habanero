﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Habanero.BO.Loaders {
    using System;

    //TODO andrew 22 Dec 2010: CF : Removed CompilerGeneratedAttribute
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    //[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Xsds {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Xsds() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Habanero.BO.Loaders.Xsds", typeof(Xsds).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;xs:schema xmlns:xs=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///  &lt;xs:element name=&quot;prop&quot;&gt;
        ///    &lt;xs:complexType&gt;
        ///      &lt;xs:attribute name=&quot;name&quot; type=&quot;xs:NMTOKEN&quot; use=&quot;required&quot; /&gt;
        ///    &lt;/xs:complexType&gt;
        ///  &lt;/xs:element&gt;
        ///  &lt;xs:element name=&quot;key&quot;&gt;
        ///    &lt;xs:complexType&gt;
        ///      &lt;xs:sequence&gt;
        ///        &lt;xs:element minOccurs=&quot;1&quot; maxOccurs=&quot;unbounded&quot; ref=&quot;prop&quot; /&gt;
        ///      &lt;/xs:sequence&gt;
        ///      &lt;xs:attribute name=&quot;name&quot; type=&quot;xs:string&quot; /&gt;
        ///      &lt;xs:attribute default=&quot;false&quot; name [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string classes {
            get {
                return ResourceManager.GetString("classes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error in DataSetProvider: If you hit delete many times in succession then you get an issue with the events interfering and you get a wierd error .
        /// </summary>
        internal static string There_was_an_error_in_DataSetProvider_MultipleDelesHit {
            get {
                return ResourceManager.GetString("There_was_an_error_in_DataSetProvider_MultipleDelesHit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;xs:schema xmlns=&quot;http://tempuri.org/uis&quot; elementFormDefault=&quot;qualified&quot; targetNamespace=&quot;http://tempuri.org/uis&quot; xmlns:xs=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///  &lt;xs:element name=&quot;parameter&quot;&gt;
        ///    &lt;xs:complexType&gt;
        ///      &lt;xs:attribute name=&quot;name&quot; type=&quot;xs:NMTOKEN&quot; use=&quot;required&quot; /&gt;
        ///      &lt;xs:attribute name=&quot;value&quot; type=&quot;xs:string&quot; use=&quot;required&quot; /&gt;
        ///    &lt;/xs:complexType&gt;
        ///  &lt;/xs:element&gt;
        ///  &lt;xs:element name=&quot;column&quot;&gt;
        ///    &lt;xs:complexType&gt;
        ///      &lt;xs:sequence&gt;
        ///       [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ui {
            get {
                return ResourceManager.GetString("ui", resourceCulture);
            }
        }
    }
}
