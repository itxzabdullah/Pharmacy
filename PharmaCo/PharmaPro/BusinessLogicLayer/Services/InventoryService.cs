using System;
using System.Data.SqlClient;
using System.Configuration;
using DomainModels;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class InventoryService
    {
        // Use a safe lookup so constructing this class in tests doesn't throw when config is absent.
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"]?.ConnectionString ?? string.Empty;

        public List<Medicine> ViewAllProducts()
        {
            var medicines = new List<Medicine>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductID, ProductName, Description, Price FROM Products";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    medicines.Add(new Medicine(
                        reader.GetInt32(reader.GetOrdinal("ProductID")),
                        reader.GetString(reader.GetOrdinal("ProductName")),
                        reader.GetString(reader.GetOrdinal("Description")),
                        reader.GetDecimal(reader.GetOrdinal("Price"))
                    ));
                }
            }
            return medicines;
        }

        public void AddProduct(string productName, string description, decimal price,
                               string location, bool available)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Products (ProductName, Description, Price) 
                                 VALUES (@ProductName, @Description, @Price);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);

                conn.Open();
                int productId = Convert.ToInt32(cmd.ExecuteScalar());

                string invQuery = @"INSERT INTO Inventory (ProductID, Location, Available) 
                                    VALUES (@ProductID, @Location, @Available)";
                SqlCommand invCmd = new SqlCommand(invQuery, conn);
                invCmd.Parameters.AddWithValue("@ProductID", productId);
                invCmd.Parameters.AddWithValue("@Location", location);
                invCmd.Parameters.AddWithValue("@Available", available ? 1 : 0);
                invCmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string deleteInventory = "DELETE FROM Inventory WHERE ProductID=@ProductID";
                    SqlCommand invCmd = new SqlCommand(deleteInventory, conn, transaction);
                    invCmd.Parameters.AddWithValue("@ProductID", productId);
                    invCmd.ExecuteNonQuery();

                    string deleteProduct = "DELETE FROM Products WHERE ProductID=@ProductID";
                    SqlCommand prodCmd = new SqlCommand(deleteProduct, conn, transaction);
                    prodCmd.Parameters.AddWithValue("@ProductID", productId);
                    prodCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}