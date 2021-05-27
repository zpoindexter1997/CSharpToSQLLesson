using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

//must include using Microsoft.Data.SqlClint;
namespace CSharpToSqlLibrary
{
    public class SqlLibrary
    {
        //creating a property to store our SqlConnection
        public SqlConnection sqlconn { get; set; }
        
        //creating a method to DELETE data (user) in our Db
        public bool Remove(User user)
        {
            //creating a string variable with the UPDATE SQL statement with User parameters
            var sql = $"DELETE From Users Where Id = @id;";

            //creating instance sqlcmd = new Sql Command (using sql string, from sql connection)
            var sqlcmd = new SqlCommand(sql, sqlconn);
            //pulls the value from property Id and puts into sql parameter @id
            sqlcmd.Parameters.AddWithValue("@id", user.Id);

            //creates rowsAffected = the SqlCommand ExectureNonQuery
            //ExecuteNonQuery is used on all data that doesn't use a SELECT statement
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            //returns (true if rowsAffected = 1, false if rowsAffected = 0)
            return (rowsAffected == 1);
        }

        //creating a method to UPDATE data (user) in our Db
        public bool Change(User user)
        {
            //creating a string variable with the UPDATE SQL statement with User parameters
            var sql = $"UPDATE Users Set " +
                        "Username = @username, " +
                        "Password = @password, " +
                        "Firstname = @firstname, " +
                        "Lastname = @lastname, " +
                        "Phone = @phone, " +
                        "Email = @email, " +
                        "IsReviewer = @isreviewer, " +
                        "IsAdmin = @isadmin " +
                        "Where Id = @id;";

            //creating instance sqlcmd = new Sql Command (using sql string, from sql connection)
            var sqlcmd = new SqlCommand(sql, sqlconn);
            //pulls the value from property Id and puts into sql parameter @id
            sqlcmd.Parameters.AddWithValue("@id", user.Id);
            sqlcmd.Parameters.AddWithValue("@username", user.Username);
            sqlcmd.Parameters.AddWithValue("@password", user.Password);
            sqlcmd.Parameters.AddWithValue("@firstname", user.FirstName);
            sqlcmd.Parameters.AddWithValue("@lastname", user.LastName);
            sqlcmd.Parameters.AddWithValue("@phone", user.Phone);
            sqlcmd.Parameters.AddWithValue("@email", user.Email);
            sqlcmd.Parameters.AddWithValue("@isreviewer", user.IsReviewer);
            sqlcmd.Parameters.AddWithValue("@isadmin", user.IsAdmin);
            //creates rowsAffected = the SqlCommand ExectureNonQuery
            //ExecuteNonQuery is used on all data that doesn't use a SELECT statement
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            //returns (true if rowsAffected = 1, false if rowsAffected = 0)
            return (rowsAffected == 1);
        }

        //creating a method to INSERT data (user) to our Db
        public bool Create(User user)
        {
            //creating a string variable with the INSERT SQL statement with User parameters
            var sql = $"INSERT into Users " +
                        "(Username, Password, FirstName, LastName, Phone, Email, IsReviewer, IsAdmin) " +
                        "VALUES " +
                        $"(@username, @password, @firstname, @lastname, @phone, @email, @isreviewer, @isadmin);";
            //creating instance sqlcmd = new Sql Command (using sql string, from sql connection)
            var sqlcmd = new SqlCommand(sql, sqlconn);
            //pulls the value from property username and puts into sql parameter @username
            sqlcmd.Parameters.AddWithValue("@username", user.Username);
            sqlcmd.Parameters.AddWithValue("@password", user.Password);
            sqlcmd.Parameters.AddWithValue("@firstname", user.FirstName);
            sqlcmd.Parameters.AddWithValue("@lastname", user.LastName);
            sqlcmd.Parameters.AddWithValue("@phone", user.Phone);
            sqlcmd.Parameters.AddWithValue("@email", user.Email);
            sqlcmd.Parameters.AddWithValue("@isreviewer", user.IsReviewer);
            sqlcmd.Parameters.AddWithValue("@isadmin", user.IsAdmin);
            //creates rowsAffected = the SqlCommand ExectureNonQuery
            //ExecuteNonQuery is used on all data that doesn't use a SELECT statement
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            //returns (true if rowsAffected = 1, false if rowsAffected = 0)
            return (rowsAffected == 1);
        }

        //creating a method to create multiple users from a list at once
        public bool CreateMultiple(List<User> users)
        {
            //sets success to true
            var success = true;
            foreach (var user in users)
            {
                //success is set = success (true) AND Create(user)(which returns true or false), true = true && true, false = true && false
                success = success && Create(user);
            }
            return success;
        }

        //creating a method to get our PrimaryKey from User
        public User GetByPK(int id)
        {
            //We need these 3 statements when doing a SELECT statement, sql command, sql connection, and sql datareader
            //creates var sql which selects all from Users table where Id (PK) = our entered value (id)
            var sql = $"SELECT * FROM Users where Id = {id};";
            //creating instance sqlcmd = new Sql Command (using sql string, from sql connection)
            var sqlcmd = new SqlCommand(sql, sqlconn);
            //new variable = using SqlCommand (sqlcmd).ExecuteReader(); which pushes our result set into var sqldatareader
            var sqldatareader = sqlcmd.ExecuteReader();
            //checks if no data came back
            if (!sqldatareader.HasRows)
            {
                //close the sqldatareader (you can only have it open once at a time, so you want to close after you run it
                sqldatareader.Close();
                //if no data came back, return null
                return null;
            }
            //if data came back, moves pointer to row and read it, returning true if there's data and false if there's no more
            sqldatareader.Read();

            //creating new instance of our User class, initializing all properties from where sqldatareader is pointing as it's created
            var user = new User()
            {
                //sets Id = converted integer of Id read
                Id = Convert.ToInt32(sqldatareader["Id"]),
                Username = Convert.ToString(sqldatareader["Username"]),
                Password = Convert.ToString(sqldatareader["Password"]),
                FirstName = Convert.ToString(sqldatareader["FirstName"]),
                LastName = Convert.ToString(sqldatareader["LastName"]),
                Phone = Convert.ToString(sqldatareader["Phone"]),
                Email = Convert.ToString(sqldatareader["Email"]),
                IsReviewer = Convert.ToBoolean(sqldatareader["IsReviewer"]),
                IsAdmin = Convert.ToBoolean(sqldatareader["IsAdmin"])
            };
            //close the sqldatareader (you can only have it open once at a time, so you want to close after you run it
            sqldatareader.Close();
            //returns user data to our method GetByPK
            return user;
        }

        //creating method using a List<OfAllUsers> named GetAllUsers
        public List<User> GetAllUsers()
        {
            //creating a variable sql, setting = string "SELECT * From Users;" 
            var sql = "SELECT * From Users;";

            //creating instance sqlcmd = new Sql Command (using sql string, from sql connection)
            var sqlcmd = new SqlCommand(sql, sqlconn);

            //new variable = using SqlCommand (sqlcmd).ExecuteReader(); which pushes our result set into var sqldatareader
            var sqldatareader = sqlcmd.ExecuteReader();

            //new generic collection of our Users
            var users = new List<User>();

            //creating a while loop to go through all of the data
            //Read() returns true as long as there is data in another row for it to go through
            while (sqldatareader.Read())
            {
                //creating variable id, = the row the pointer Reader() is pointing to named [rowName] and return the Converted.Integer value to var id
                var id = Convert.ToInt32(sqldatareader["Id"]);
                //creating variable username, = the row the pointer Reader() is pointing to row [Username] and return the Converted.String value to var id
                var username = Convert.ToString(sqldatareader["Username"]);
                //.ToString(); on the end also converts the data to a string before returning, only works for strings
                var password = sqldatareader["Password"].ToString();
                var firstname = sqldatareader["FirstName"].ToString();
                var lastname = sqldatareader["LastName"].ToString();
                var phone = sqldatareader["Phone"].ToString();
                var email = sqldatareader["Email"].ToString();
                var isReviewer = Convert.ToBoolean(sqldatareader["IsReviewer"]);
                var isAdmin = Convert.ToBoolean(sqldatareader["IsAdmin"]);
                //creating new instance of our User class, initializing all properties as it's created
                var user = new User()
                {
                    //parameter Id = data entered to variable id,
                    Id = id,
                    Username = username,
                    Password = password,
                    FirstName = firstname,
                    LastName = lastname,
                    Phone = phone,
                    Email = email,
                    IsReviewer = isReviewer,
                    IsAdmin = isAdmin
                };
                //add each user to our collection users
                users.Add(user);
            }
            //close the sqldatareader (you can only have it open once at a time, so you want to close after you run it
            sqldatareader.Close();
            //returns the collection users to method GetAllUsers()
            return users;
        }

        //creating a method to get our Vendors by PK
        public Vendor GetVendorByPK(int id)
        {
            var sql = $"SELECT * FROM Vendors where Id = {id};";
            var sqlcmd = new SqlCommand(sql, sqlconn);
            var sqldatareader = sqlcmd.ExecuteReader();
            if (!sqldatareader.HasRows)
            {
                sqldatareader.Close();
                return null;
            }
            var vendor = new Vendor()
            {
                Id = Convert.ToInt32(sqldatareader["Id"]),
                Code = Convert.ToString(sqldatareader["Code"]),
                Name = Convert.ToString(sqldatareader["Name"]),
                Address = Convert.ToString(sqldatareader["Address"]),
                City = Convert.ToString(sqldatareader["City"]),
                State = Convert.ToString(sqldatareader["State"]),
                Zip = Convert.ToString(sqldatareader["Zip"]),
                Phone = Convert.ToString(sqldatareader["Phone"]),
                Email = Convert.ToString(sqldatareader["Email"])
            };

            sqldatareader.Close();
            return vendor;

        }
        //creating a method to make a list using all vendors
        public List<Vendor> GetAllVendors()
        {
            var sql = "SELECT * From Vendors;";
            var sqlcmd = new SqlCommand(sql, sqlconn);
            var sqldatareader = sqlcmd.ExecuteReader();
            var vendors = new List<Vendor>();
            while (sqldatareader.Read())
            {
                var vendor = new Vendor()
                {
                    Id = Convert.ToInt32(sqldatareader["Id"]),
                    Code = Convert.ToString(sqldatareader["Code"]),
                    Name = Convert.ToString(sqldatareader["Name"]),
                    Address = Convert.ToString(sqldatareader["Address"]),
                    City = Convert.ToString(sqldatareader["City"]),
                    State = Convert.ToString(sqldatareader["State"]),
                    Zip = Convert.ToString(sqldatareader["Zip"]),
                    Phone = Convert.ToString(sqldatareader["Phone"]),
                    Email = Convert.ToString(sqldatareader["Email"])
                };
                vendors.Add(vendor);
            }
            sqldatareader.Close();
            return vendors;
        }

        //method to connect to SQL
        public void Connect()
        {
            //creating our connection string = "server=(servername) +(adding multiple strings)
            // \ is a special character in c#, \\ prints as 1 \ in c# strings
            var connStr = "server=localhost\\sqlexpress;" +
            //"database=(dbName)
            "database=PRS;" +
            //"trustedconnection=true" 
            "trusted_connection=true;";
            //can replace trusted_connection=true; with
            //uid=(username);pwd=(password); for a login

            //initialize an instance of our connection string
            sqlconn = new SqlConnection(connStr);
            //attempts to opens our sql connection 
            sqlconn.Open();
            //checks if the our connection state is equal to an open state ( did it open succesfully)
            if (sqlconn.State != System.Data.ConnectionState.Open)
            {
                //creates an exception to stop the program and print this if it's not open
                throw new Exception("Connection string is not correct!");
            }
            //just to tell us it was successful
            Console.WriteLine("Open connection successful!");
        }
        //method to disconnect from SQL
        public void Disconnect()
        {
            //checks if a connection was attempted, if not just return
            if (sqlconn == null)
            {
                return;
            }
            //Close our connection sqlconn
            sqlconn.Close();
            //resets sqlconn to null
            sqlconn = null;
        }
    }
}
