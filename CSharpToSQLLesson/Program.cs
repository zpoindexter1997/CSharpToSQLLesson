using System;
using CSharpToSqlLibrary;

namespace TestCSharpToSqlLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlconn = new Connection("localhost\\sqlexpress", "PRS");

            var prodctrl = new ProductsController(sqlconn);

            var chili = new Product()
            {
                Id = 0,
                PartNbr = "SKYLINE",
                Name = "Skyline Chili",
                Price = 5,
                Unit = "Each",
                PhotoPath = null,
                VendorsId = 0
            };
            //Creating a new product newPrudct, linking to VendorCode "BUN"
            var chiliworked = prodctrl.Create(chili, "BUN");


            sqlconn.Disconnect();
            var i = 0;











            //creating an instance of our connection to ("server name", "database name");
            var sqlConn = new Connection("localhost\\sqlexpress", "PRS");

            ////creating a new product controller, using the sqlconn connection 
            ////this is so we can pass an instance of our 1 connection and pass it through the ProductsController constructor
            ////AKA DEPENDENCY INJECTION
            var vendCtrl = new VendorsController(sqlConn);
            var vendors = vendCtrl.GetAll();
            var vendor = vendCtrl.GetByPK(3);

            //using the connection method from our library (less efficient than dependency injection because we'd need this method in each class = multiple connections)
            //creating a new instance of the SqlLibrary for a connection
            var sqlLib = new SqlLibrary();
            //attempting to connect with instance sqlLib
            sqlLib.Connect();

            //creating userc, setting  = method GetByPK(using PK 8)
            var userc = sqlLib.GetByPK(8);
            //we're updating the user with PK 8 with this value for Phone
            userc.Phone = "513-555-1212";
            //goes through the change method (UPDATE sql statement), if it worked, variable csuccess will = true
            var csuccess = sqlLib.Change(userc);

            //creating a new user instance named newUser
            var newUser = new User()
            {
                //setting properties of newUser as it's created
                Id = 0,
                Username = "XYZ1",
                Password = "XX",
                FirstName = "XYZ",
                LastName = "XYZ",
                Phone = "XYZ",
                Email = "XYZ",
                IsReviewer = true,
                IsAdmin = true
            };
            //creating variable success, = Create method using (newUser) to see if it worked
            //if it worked, success will = true because of the return from Create method
            var success = sqlLib.Create(newUser);

            //creates userd = User with PK 8
            var userd = sqlLib.GetByPK(8);
            //calls the remove method (DELETE) on userd, returns true/false to dsuccess if it works/fails
            var dsuccess = sqlLib.Remove(userd);

            //creating users, setting = method GetAllUsers
            var users = sqlLib.GetAllUsers();

            var prodCtrl = new ProductsController(sqlConn);
            var productsc = prodCtrl.GetAll();
            var products = prodCtrl.GetByPK(3);

            var newProduct = new Product()
            {
                Id = 0,
                PartNbr = "SKYLINE",
                Name = "Skyline Chili",
                Price = 5,
                Unit = "Each",
                PhotoPath = null,
                VendorsId = 0
            };
            //Creating a new product newPrudct, linking to VendorCode "BUN"
            var psuccess = prodCtrl.Create(newProduct, "BUN");

            //Disconnects from sqlLib 
            sqlLib.Disconnect();
        }
    }
}


