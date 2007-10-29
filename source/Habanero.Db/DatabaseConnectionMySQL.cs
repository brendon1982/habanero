using System.Data;
using System.Reflection;

namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the MySql database
    /// </summary>
    public class DatabaseConnectionMySql : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionMySql(string assemblyName, string className) : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringMySqlFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionMySql(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public override string LeftFieldDelimiter
        {
            get { return ""; }
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public override string RightFieldDelimiter
        {
            get { return ""; }
        }

        /// <summary>
        /// Returns an empty string in this implementation
        /// </summary>
        /// <param name="limit">The limit - not relevant in this
        /// implementation</param>
        /// <returns>Returns an empty string in this implementation</returns>
        public override string GetLimitClauseForBeginning(int limit)
        {
            return "";
        }

        /// <summary>
        /// Creates a limit clause from the limit provided, in the format of:
        /// "limit [limit]" (eg. "limit 3")
        /// </summary>
        /// <param name="limit">The limit - the maximum number of rows that
        /// can be affected by the action</param>
        /// <returns>Returns a string</returns>
        public override string GetLimitClauseForEnd(int limit)
        {
            return "limit " + limit;
        }

        public override long GetLastAutoIncrementingID(string tableName, IDbTransaction tran, IDbCommand command)
        {
            PropertyInfo propInfo = command.GetType().GetProperty("LastInsertedId", BindingFlags.Public | BindingFlags.Instance);
            return (long)propInfo.GetValue(command, null);
        }
    }
}