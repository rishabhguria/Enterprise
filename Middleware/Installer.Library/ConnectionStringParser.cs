using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Installer.Library
{
    /// <summary>
    /// Connection String Parser
    /// </summary>
    /// <remarks></remarks>
    public class ConnectionStringParser
    {

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        /// <remarks></remarks>
        public string DataSource { get; set; }
       
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        /// <remarks></remarks>
        public string Database { get; set; }
       
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        /// <remarks></remarks>
        public string UserId { get; set; }
       
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringParser"/> class.
        /// </summary>
        /// <param name="cn">The cn.</param>
        /// <remarks></remarks>
        public ConnectionStringParser(string cn)
        {
            string[] tokens = cn.Split(new string[] { "=", ";" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].ToLower() == "data source")
                    DataSource = tokens[i + 1];

                if (tokens[i].ToLower() == "initial catalog")
                    Database = tokens[i + 1];

                if (tokens[i].ToLower() == "user id")
                    UserId = tokens[i + 1];
                if (tokens[i].ToLower() == "password")
                    Password = tokens[i + 1];

            }
        }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return string.Format("[{0}].dbo.[{1}]", DataSource, Database);
        }
    }
}
