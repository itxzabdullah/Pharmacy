using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class BLL
    {
        // DAL helpers
        DAL dal = new DAL();
        UserManager um = new UserManager();
        InventoryManager im = new InventoryManager();
        ProductManager pm = new ProductManager();

        // 🔐 User authentication
        public bool VerifyUser(string username, string password, string userType)
        {
            return dal.VerifyUser(username, password, userType);
        }

        // 👥 User management
        public DataTable viewall()
        {
            return um.viewall();
        }

        public DataTable search(int id, string user)
        {
            return um.search(id, user);
        }

        public void deleteUser(int id)
        {
            um.Deleteuser(id);
        }

        public void AddUser(string username, string password, string email, string name,
                            string address, string phone, string userType)
        {
            um.AddUser(username, password, email, name, address, phone, userType);
        }

        // ✏️ NEW: Update user
        public void UpdateUser(int userId, string username, string password, string email,
                               string name, string address, string phone, string userType)
        {
            um.UpdateUser(userId, username, password, email, name, address, phone, userType);
        }

        // 📦 Inventory management
        public DataTable viewallproducts()
        {
            return im.viewallProducts();
        }

        public DataTable searchproduct(int id, string product)
        {
            return im.searchProduct(id, product);
        }

        public void deleteproduct(int id)
        {
            im.DeleteProduct(id);
        }

        public void addproduct(string productName, string description, decimal price,
                               string location, bool available)
        {
            im.AddProduct(productName, description, price, location, available);
        }

        // 🛒 Product manager
        public DataTable Viewinproducts()
        {
            return pm.viewinproducts();
        }

        public DataTable searchinproducts(int id)
        {
            return pm.searchinProducts(id);
        }

        public bool addproductinproducts(int id)
        {
            return pm.AddProductinproducts(id);
        }

        public void deletefromproducts(int id)
        {
            pm.DeletefromProducts(id);
        }
    }
}