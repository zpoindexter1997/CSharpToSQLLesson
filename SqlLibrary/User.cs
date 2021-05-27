using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpToSqlLibrary
{

    //creating a User class to match with our User Table in SQL Database
    public class User
    {
        //creating properties to capture our Database columns
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }
    }
}
