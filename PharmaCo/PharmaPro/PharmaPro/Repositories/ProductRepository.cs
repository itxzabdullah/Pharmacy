using PharmaPro.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PharmaPro.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string connectionString;

        public ProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Product> SearchProducts(string searchTerm)
        {
            var products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductID, ProductName, Description, Price FROM Products";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE ProductName LIKE @SearchTerm OR Description LIKE @SearchTerm";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    });
                }
            }

            return products;
        }
    }
}