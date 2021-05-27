using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpToSqlLibrary
{
    public class Connection
    {
        //Creating a Sql Connection property and storing it into Sqlconn
        public SqlConnection Sqlconn { get; set; }

        //creating our Connection method, passing the server name and which database
        public Connection(string server, string database)
        {
            //creating our connection string = "server=(servername) 
            var connStr = $"server={server};" +
            //"database=(dbName)
            $"database={database};" +
            //"trustedconnection=true" 
            "trusted_connection=true;";
            //can replace trusted_connection=true; with
            //uid=(username);pwd=(password); for a login

            //initialize an instance of our connection string
            Sqlconn = new SqlConnection(connStr);
            //opens our sql connection 
            Sqlconn.Open();
            //checks if  our connection state is equal to an open state ( did it open succesfully)
            if (Sqlconn.State != System.Data.ConnectionState.Open)
            {
                Sqlconn = null;
                //creates an exception to stop the program and print this if it's not open
                throw new Exception("Connection did not open!");
            }
            //just to tell us it was successful
            Console.WriteLine("Open connection successful!");
        }
        //method to disconnect from SQL
        public void Disconnect()
        {
            //checks if a connection was attempted, if not just return
            if (Sqlconn == null)
            {
                return;
            }
            //Close our connection Sqlconn
            Sqlconn.Close();
            //resets Sqlconn to null
            Sqlconn = null;
        }
    }

}

