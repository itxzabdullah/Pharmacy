using BusinessLogicLayer.Services;
using DataAccessLayer;
using DomainModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLogicLayer
{
    public class BLL 
    {
        //private readonly string connectionString = ConfigurationManager.ConnectionStrings["PharmaDB"].ConnectionString;

        private readonly IDal _dal;
        private readonly UserManager um;
        private readonly InventoryService im;
        private readonly ProductManager pm;

        // ✅ Default constructor for production (uses real DAL)
        public BLL() : this(new DAL()) { }

        // ✅ Constructor for dependency injection (tests)
        public BLL(IDal dal, UserManager userManager, InventoryService inventoryService, ProductManager productManager)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
            um = userManager ?? throw new ArgumentNullException(nameof(userManager));
            im = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            pm = productManager ?? throw new ArgumentNullException(nameof(productManager));
        }

        public BLL(IDal dal)
    : this(dal, new UserManager(), new InventoryService(), new ProductManager())
        {
        }



        // 🔐 User authentication
        // 🔐 User authentication
        public User GetUser(string username, string password, string userType)
        {
            // Verify credentials first
            if (!_dal.VerifyUser(username, password, userType))
                return null;

            // Try to load the full user record
            try
            {
                var dt = um.search(0, username);
                var users = MapUsers(dt);
                var user = users.FirstOrDefault(u =>
                    string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(u.UserType, userType, StringComparison.OrdinalIgnoreCase));

                return user; // may be null if not found
            }
            catch
            {
                // If lookup fails, return null rather than a partial user
                return null;
            }
        }

        public bool VerifyUser(string username, string password, string userType)
        {
            // Simple boolean check against DAL — don't depend on full user load
            return _dal.VerifyUser(username, password, userType);
        }

        // 👥 User management
        public List<User> SearchUser(int userId, string username)
        {
            var dt = um.search(userId, username);
            return MapUsers(dt);
        }

        public List<User> ViewAllUsers()
        {
            var dt = um.viewall();
            return MapUsers(dt);
        }

        public void AddUser(string username, string password, string email, string name,
                            string address, string phone, string userType)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password cannot be empty.");

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
        public void AddProduct(string productName, string description, decimal price,
                               string location, bool available)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be positive.");

            im.AddProduct(productName, description, price, location, available);
        }

        public void DeleteProduct(int id)
        {
            im.DeleteProduct(id);
        }

        public List<Medicine> ViewAllProducts()
        {
            return im.ViewAllProducts();
        }


        // 🛒 Cart + Orders
        public void PlaceOrder(int userId, Cart cart, string paymentMethod)
        {
            if (cart == null || cart.Items.Count == 0)
                throw new InvalidOperationException("Cart cannot be empty.");

            decimal totalAmount = cart.Items.Sum(i => i.UnitPrice * i.Quantity);

            var orderDetails = cart.Items.Select(i => new OrderDetailDTO
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            _dal.InsertOrder(userId, orderDetails, totalAmount, paymentMethod);
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

        // 🔧 Helper: map DataTable → List<User>
        private List<User> MapUsers(DataTable dt)
        {
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
    }
}