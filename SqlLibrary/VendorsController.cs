using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpToSqlLibrary
{
    public class VendorsController
    {
        //creating a property hold the connection
        private static Connection connection { get; set; }

        //creating a method to pass in our connection
        public VendorsController(Connection connection)
        {
            //taking the connection passed in and storing it into our property
            VendorsController.connection = connection;
        }

        private Vendor FillVendorFromReader(SqlDataReader reader)
        {
            var vendor = new Vendor()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Code = Convert.ToString(reader["Code"]),
                Name = Convert.ToString(reader["Name"]),
                Address = Convert.ToString(reader["Address"]),
                City = Convert.ToString(reader["City"]),
                State = Convert.ToString(reader["State"]),
                Zip = Convert.ToString(reader["Zip"]),
                Phone = Convert.ToString(reader["Phone"]),
                Email = Convert.ToString(reader["Email"])
            };
            return vendor;
        }
        //creating a method to get all vendors from Vendor list
        public List<Vendor> GetAll()
        {
            var sql = "SELECT * From Vendors;";
            var cmd = new SqlCommand(sql, VendorsController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            var vendors = new List<Vendor>();

            while (reader.Read())
            {
                var vendor = FillVendorFromReader(reader);
                vendors.Add(vendor);
            }
            reader.Close();
            return vendors;
        }
        //creating a method to get our Vendors by PK
        public Vendor GetByPK(int id)
        {
            var sql = $"SELECT * FROM Vendors where Id = {id};";
            var cmd = new SqlCommand(sql, VendorsController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            //calling the reader, start reading data
            reader.Read();
            var vendor = FillVendorFromReader(reader);
            //calling the reader, stop reading the data
            reader.Close();
            return vendor;
        }
        //creating a method to get our Vendors by Code since our user would likely know the Code of the vendor, rather than the PK
        public Vendor GetByCode(string code)
        {
            var sql = $"SELECT * FROM Vendors where Code = @code;";
            var cmd = new SqlCommand(sql, VendorsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@code", code);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            };
            reader.Read();
            var vendor = FillVendorFromReader(reader);
            reader.Close();
            return vendor;
        }

        public bool Remove(Vendor vendor)
        {
            var sql = "DELETE From Vendors Where Id = @id;";
            var cmd = new SqlCommand(sql, VendorsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@id", vendor.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }

        public bool Change(Vendor vendor)
        {
            var sql = "UPDATE Vendors set " +
                        "Code = @code, " +
                        "Name = @name, " +
                        "Address = @address, " +
                        "City = @city, " +
                        "State = @state, " +
                        "Zip = @zip, " +
                        "Phone = @phone, " +
                        "Email = @email " +
                        "Where Id = @id;";
            var cmd = new SqlCommand(sql, VendorsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@id", vendor.Id);
            cmd.Parameters.AddWithValue("@code", vendor.Code);
            cmd.Parameters.AddWithValue("@name", vendor.Name);
            cmd.Parameters.AddWithValue("@address", vendor.Address);
            cmd.Parameters.AddWithValue("@city", vendor.City);
            cmd.Parameters.AddWithValue("@state", vendor.State);
            cmd.Parameters.AddWithValue("@zip", vendor.Zip);
            cmd.Parameters.AddWithValue("@phone", vendor.Phone);
            cmd.Parameters.AddWithValue("@email", vendor.Email);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }

        public bool Create(Vendor vendor)
        {
            var sql = "INSERT into Vendors" +
                        "(Code, Name, Address, City, State, Zip, Phone, Email) " +
                        "VALUES " +
                        "(@code, @name, @address, @city, @state, @zip, @phone, @email);";
            var cmd = new SqlCommand(sql, VendorsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@code", vendor.Code);
            cmd.Parameters.AddWithValue("@name", vendor.Name);
            cmd.Parameters.AddWithValue("@address", vendor.Address);
            cmd.Parameters.AddWithValue("@city", vendor.City);
            cmd.Parameters.AddWithValue(@"state", vendor.State);
            cmd.Parameters.AddWithValue("@zip", vendor.Zip);
            cmd.Parameters.AddWithValue("@phone", vendor.Phone);
            cmd.Parameters.AddWithValue("@email", vendor.Email);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
    }
}
