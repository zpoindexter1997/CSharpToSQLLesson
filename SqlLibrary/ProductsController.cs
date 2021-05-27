using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpToSqlLibrary
{
    public class ProductsController
    {
        private static Connection connection { get; set; }


        //creating a Constructor to call our connection constructor and use our 1 connection
        public ProductsController(Connection connection)
        {
            ProductsController.connection = connection;
        }


        public bool Create(Product product)
        {
            var sql = "Insert into Products " +
                        "VALUES " +
                        "(@partnbr, @name, @price, @unit, @photopath, @vendorsid);";
            var cmd = new SqlCommand(sql, ProductsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@partnbr", product.PartNbr);
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@unit", product.Unit);
            //since photopath can be null, we use this statement saying ?? if product.PhotoPath == null, return the SQL equivalent of C# null (DBNull.Value)
            //(object).... turns whatever the datatype is into an object datatype (the base of all datatypes) so it can be converted to null
            cmd.Parameters.AddWithValue("@photopath", (object)product.PhotoPath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@vendorsid", product.VendorsId);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }

        //Creating a create method that passes through the VendorCode to assign a vendor to the product
        public bool Create(Product product, string VendorCode)
        {
            //variable vendCtrl = a new instance of the VendorsController(and connecting to sql)
            var vendCtrl = new VendorsController(connection);
            //variable vendor = through our VendorsController, calling GetByCode method (passing the VendorCode we're using)
            var vendor = vendCtrl.GetByCode(VendorCode);
            //setting the products VendorsId = the Id for the vendor (Linking product FK to vendor PK)
            product.VendorsId = vendor.Id;
            //Execute the Create(product) method to finish the other details, and return the bool Create() gives us
            return Create(product);
        }

        //creating a method to fill out Products in each instance, passing in the reader
        private Product FillProductFromSqlRow(SqlDataReader reader)
        {
            var product = new Product()
            {
                Id = Convert.ToInt32(reader["Id"]),
                PartNbr = Convert.ToString(reader["PartNbr"]),
                Name = Convert.ToString(reader["Name"]),
                Price = Convert.ToDecimal(reader["Price"]),
                Unit = Convert.ToString(reader["Unit"]),
                PhotoPath = Convert.ToString(reader["PhotoPath"]),
                VendorsId = Convert.ToInt32(reader["VendorsId"])
            };
            return product;
        }

        public List<Product> GetAll()
        {
            var sql = "SELECT * From Products;";
            var cmd = new SqlCommand(sql, ProductsController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            var products = new List<Product>();

            while (reader.Read())
            {
                //calls to method FillProductFromSqlRow, utilizing the reader in this class and putting the return into variable product
                var product = FillProductFromSqlRow(reader);
                //adding the returned product to products
                products.Add(product);
            }
            reader.Close();
            GetVendorForProducts(products);
            return products;
        }

        private void GetVendorForProduct(Product product)
        {
            var vendCtrl = new VendorsController(connection);
            product.Vendor = vendCtrl.GetByPK(product.VendorsId);
        }

        private void GetVendorForProducts(List<Product> products)
        {
            foreach (var product in products)
            {
                GetVendorForProduct(product);
            }
        }

        public Product GetByPK(int id)
        {
            var sql = "SELECT * From Products Where Id = @id";
            var cmd = new SqlCommand(sql, ProductsController.connection.Sqlconn);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            reader.Read();
            var product = FillProductFromSqlRow(reader);
            reader.Close();
            GetVendorForProduct(product);
            return product;
        }

        public bool Remove(Product product)
        {
            var sql = "DELETE From Products Where Id = @id;";
            var cmd = new SqlCommand(sql, ProductsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@id", product.Id);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);

        }

        public bool Change(Product product)
        {
            var sql = "UPDATE Products set " +
                        "PartNbr = @partnbr, " +
                        "Name = @name, " +
                        "Price = @price, " +
                        "Unit = @unit, " +
                        "Photopath = @photopath, " +
                        "VendorsId = @vendorsid;";
            var cmd = new SqlCommand(sql, ProductsController.connection.Sqlconn);
            cmd.Parameters.AddWithValue("@partnbr", product.PartNbr);
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@unit", product.Unit);
            cmd.Parameters.AddWithValue("@photopath", product.PhotoPath);
            cmd.Parameters.AddWithValue("@vendorsid", product.VendorsId);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }

        //Creating a method to Change a product by pointing at the VendorCode
        //This would be used if we were switching from Kroger popsicles to Walmart popsicles
        public bool Change(Product product, string VendorCode)
        {
            //variable vendCtrl = a new instance of the VendorsController(and connecting to sql)
            var vendCtrl = new VendorsController(connection);
            //variable vendor = through our VendorsController, calling GetByCode method (passing the VendorCode we're using)
            var vendor = vendCtrl.GetByCode(VendorCode);
            //setting the products VendorsId = the Id for the vendor (Linking product FK to vendor PK)
            product.VendorsId = vendor.Id;
            //Execute the Change(product) method to finish the other details, and return the bool Change() gives us
            return Change(product);
        }
    }
}
