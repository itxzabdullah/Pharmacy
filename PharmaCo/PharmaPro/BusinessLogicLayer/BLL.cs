using BusinessLogicLayer.Services;
using DataAccessLayer;
using DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;

namespace BusinessLogicLayer
{
    public class BLL
    {
        // DAL helpers
        DAL dal = new DAL();
        UserManager um = new UserManager();
        InventoryManager im = new InventoryManager();
        ProductManager pm = new ProductManager();
        private string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        // 🔐 User authentication
        public bool VerifyUser(string username, string password, string userType)
        {
            // Reuse GetUser internally
            var user = GetUser(username, password, userType);
            return user != null;
        }

        // ✅ Fetch full user record
        public User GetUser(string username, string password, string userType)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT UserID, Username, PasswordHash, Email, FullName, Address, PhoneNumber, UserType, CreatedAt
                                 FROM UserAccounts
                                 WHERE Username = @username 
                                   AND PasswordHash = @password 
                                   AND UserType = @userType";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@userType", userType);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User(
                        Convert.ToInt32(reader["UserID"]),
                        reader["Username"].ToString(),
                        reader["PasswordHash"].ToString(),
                        reader["Email"].ToString(),
                        reader["FullName"].ToString(),
                        reader["Address"].ToString(),
                        reader["PhoneNumber"].ToString(),
                        reader["UserType"].ToString(),
                        Convert.ToDateTime(reader["CreatedAt"])
                    );
                }
            }
            return null;
        }

        // 🔎 Search user
        public List<User> SearchUser(int userId, string username)
        {
            var dt = um.search(userId, username); // DAL returns DataTable
            var users = new List<User>();

            foreach (DataRow row in dt.Rows)
            {
                users.Add(new User(
                    Convert.ToInt32(row["UserID"]),
                    row["Username"].ToString(),
                    row["PasswordHash"].ToString(),
                    row["Email"].ToString(),
                    row["FullName"].ToString(),
                    row["Address"].ToString(),
                    row["PhoneNumber"].ToString(),
                    row["UserType"].ToString(),
                    Convert.ToDateTime(row["CreatedAt"])
                ));
            }

            return users;
        }

        // 👥 User management
        public List<User> ViewAllUsers()
        {
            var dt = um.viewall(); // DAL returns DataTable
            var users = new List<User>();

            foreach (DataRow row in dt.Rows)
            {
                users.Add(new User(
                    Convert.ToInt32(row["UserID"]),
                    row["Username"].ToString(),
                    row["PasswordHash"].ToString(),
                    row["Email"].ToString(),
                    row["FullName"].ToString(),
                    row["Address"].ToString(),
                    row["PhoneNumber"].ToString(),
                    row["UserType"].ToString(),
                    Convert.ToDateTime(row["CreatedAt"])
                ));
            }

            return users;
        }

        public void AddUser(string username, string password, string email, string name,
                            string address, string phone, string userType)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password cannot be empty."); // Invariant

            um.AddUser(username, password, email, name, address, phone, userType);
        }

        public void UpdateUser(int userId, string username, string password, string email,
                               string name, string address, string phone, string userType)
        {
            um.UpdateUser(userId, username, password, email, name, address, phone, userType);
        }

        public void DeleteUser(int id)
        {
            um.Deleteuser(id);
        }

        // 📦 Inventory management
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
            if (price <= 0)
                throw new ArgumentException("Price must be positive."); // Invariant

            im.AddProduct(productName, description, price, location, available);
        }

        public void DeleteProduct(int id)
        {
            im.DeleteProduct(id);
        }

        // 🛒 Cart + Orders
        public void PlaceOrder(int userId, Cart cart, string paymentMethod)
        {
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cart cannot be empty.");

            decimal totalAmount = cart.Items.Sum(i => i.UnitPrice * i.Quantity);

            // Convert CartItems → OrderDetailDTO list
            var orderDetails = cart.Items.Select(i => new OrderDetailDTO
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            dal.InsertOrder(userId, orderDetails, totalAmount, paymentMethod);
        }

        // 🛒 Product manager
        public DataTable ViewInProducts()
        {
            return pm.viewinproducts();
        }

        public bool AddProductInProducts(int id)
        {
            return pm.AddProductinproducts(id);
        }

        public void DeleteFromProducts(int id)
        {
            pm.DeletefromProducts(id);
        }
    }
}