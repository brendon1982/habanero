#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.IO;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Console
{
    /// <summary>
    /// Provides a template for a standard Habanero application, including
    /// standard fields and initialisations.  Specific details covered are:
    /// <ul>
    /// <li>The class definitions that define how the data is represented
    /// and limited</li>
    /// <li>The database configuration, connection and settings</li>
    /// <li>A logger to record debugging and error messages</li>
    /// <li>An exception notifier to communicate exceptions to the user</li>
    /// <li>Automatic version upgrades when an application is out-of-date</li>
    /// <li>A synchronisation controller</li>
    /// </ul>
    /// To set up and launch an application:
    /// <ol>
    /// <li>Instantiate the application with the constructor</li>
    /// <li>Specify any individual settings as required</li>
    /// <li>Call the Startup() method to launch the application</li>
    /// </ol>
    /// </summary>
    public class HabaneroAppConsole : HabaneroApp
    {
        private IDatabaseConfig _databaseConfig;
        private string _privateKey;

        /// <summary>
        /// Constructor to initialise a new application with basic application
        /// information.  Use the Startup() method to launch the application.
        /// </summary>
        /// <param name="appName">The application name</param>
        /// <param name="appVersion">The application version</param>
        public HabaneroAppConsole(string appName, string appVersion) : base(appName, appVersion) { }

        /// <summary>
        /// Sets the database configuration object, which contains basic 
        /// connection information along with the database vendor name 
        /// (eg. MySql, Oracle).
        /// </summary>
        public IDatabaseConfig DatabaseConfig
        {
            set { _databaseConfig = value; }
        }

        /// <summary>
        /// Sets the private key used to decrypt the database password. If your database password as supplied is
        /// in plaintext then this is not necessary. If you supply the DatabaseConfig object you can also set the
        /// private key on that instead.
        /// </summary>
        /// <param name="xmlPrivateKey">The private key (RSA) in xml format</param>
        public void SetPrivateKey(string xmlPrivateKey)
        {
            _privateKey = xmlPrivateKey;
        }

        /// <summary>
        /// Gets the loader for the xml class definitions
        /// </summary>
        /// <returns>Returns the loader</returns>
        private XmlClassDefsLoader GetXmlClassDefsLoader()
        {
            try
            {
                string classDefsXml;
                if (String.IsNullOrEmpty(ClassDefsXml))
                    classDefsXml = new StreamReader(ClassDefsFileName).ReadToEnd();
                else
                    classDefsXml = ClassDefsXml;

                return new XmlClassDefsLoader(classDefsXml, new DtdLoader(), new DefClassFactory());
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("Unable to find Class Definitions file. " +
                                                "This file contains all the class definitions that match " +
                                                "objects to database tables. Ensure that you have a classdefs.xml file " +
                                                "and that the file is being copied to your output directory (eg. bin/debug).", ex);
            }
        }

        /// <summary>
        /// Loads the class definitions
        /// </summary>
        protected override void SetupClassDefs()
        {
            if (LoadClassDefs)
            {
                ClassDef.ClassDefs.Add(GetXmlClassDefsLoader().LoadClassDefs());
                ClassDefValidator validator = new ClassDefValidator(new DefClassFactory());
                validator.ValidateClassDefs(ClassDef.ClassDefs);
            }
        }

        /// <summary>
        /// Initialises the settings.  If not provided, DatabaseSettings
        /// is assumed.
        /// </summary>
        protected override void SetupSettings()
        {
            if (Settings == null) Settings = new DatabaseSettings(DatabaseConnection.CurrentConnection);
            GlobalRegistry.Settings = Settings;
        }

        /// <summary>
        /// Sets up the database connection.  If not provided, then
        /// reads the connection from the config file.
        /// </summary>
        protected override void SetupDatabaseConnection()
        {
            if (DatabaseConnection.CurrentConnection != null) return;
            if (_databaseConfig == null) _databaseConfig = Habanero.DB.DatabaseConfig.ReadFromConfigFile();
            if (_databaseConfig.IsInMemoryDB())
            {
                BORegistry.DataAccessor = new DataAccessorInMemory();
            }
            else
            {
                ISupportsRSADecryption encryptedDatabaseConfig = _databaseConfig as ISupportsRSADecryption;
                if (_privateKey != null && encryptedDatabaseConfig != null) encryptedDatabaseConfig.SetPrivateKeyFromXML(_privateKey);
                DatabaseConnection.CurrentConnection = _databaseConfig.GetDatabaseConnection();
                BORegistry.DataAccessor = new DataAccessorDB();
            }
        }

        /// <summary>
        /// Sets up the exception notifier used to display
        /// exceptions to the final user.  If not specified,
        /// assumes the ConsoleExceptionNotifier.
        /// </summary>
        protected override void SetupExceptionNotifier()
        {
            if (ExceptionNotifier == null) ExceptionNotifier = new ConsoleExceptionNotifier();
            GlobalRegistry.UIExceptionNotifier = ExceptionNotifier;
        }
    }
}
