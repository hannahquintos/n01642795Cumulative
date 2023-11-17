using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace n01642795Cumulative1.Models
{
    // A class which connects to your MySQL database
    public class SchoolDbContext
    {
        // readonly properties - only SchoolDbContext can use them
        // my local school database
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }
        
        // database credentials to connect to database
        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }

        /// <summary>
        ///     Returns a connection to the school database.
        /// </summary>
        /// <returns>
        ///     A MySqlConnection Object
        /// </returns>
        /// <example>
        ///     private SchoolDbContext School = new SchoolDbContext();
        ///     MySqlConnection Conn = School.AccessDatabase();
        /// </example>
        public static MySqlConnection AccessDatabase()
        {
            // returns object/instance of class - connection to a specific database
            return new MySqlConnection(ConnectionString);
        }
    }
}