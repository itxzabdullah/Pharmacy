using System;
using System.Data;
using System.Data.SqlClient;
using DomainModels;
using System.Collections.Generic;



namespace DataAccessLayer
{
    // Connection helper
    public class DatabaseConnect
    {
        private readonly string connectionString;

        public DatabaseConnect(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }

    // Authentication DAL (kept simple to match current schema/usage)
    public class DAL : IDal
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";

        private readonly DatabaseConnect dbc;

        public DAL()
        {
            dbc = new DatabaseConnect("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True");
        }

        public void InsertOrder(int userId, List<OrderDetailDTO> orderDetails, decimal totalAmount, string paymentMethod)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Defensive: ensure the user exists before attempting to insert an order.
                    using (SqlCommand checkUser = new SqlCommand("SELECT COUNT(1) FROM UserAccounts WHERE UserID = @UserID", conn, transaction))
                    {
                        checkUser.Parameters.AddWithValue("@UserID", userId);
                        int userCount = Convert.ToInt32(checkUser.ExecuteScalar());
                        if (userCount == 0)
                        {
                            throw new InvalidOperationException($"Cannot place order: user with id {userId} does not exist.");
                        }
                    }

                    // Insert into Orders
                    string insertOrderQuery = @"
                INSERT INTO Orders (UserID, OrderDate, TotalAmount, PaymentStatus)
                VALUES (@UserID, @OrderDate, @TotalAmount, @PaymentStatus);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand orderCmd = new SqlCommand(insertOrderQuery, conn, transaction);
                    orderCmd.Parameters.AddWithValue("@UserID", userId);
                    orderCmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    orderCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    orderCmd.Parameters.AddWithValue("@PaymentStatus", "Pending");

                    int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());

                    // Insert order details
                    foreach (var item in orderDetails)
                    {
                        string insertDetailQuery = @"
                    INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price)
                    VALUES (@OrderID, @ProductID, @Quantity, @Price);";

                        SqlCommand detailCmd = new SqlCommand(insertDetailQuery, conn, transaction);
                        detailCmd.Parameters.AddWithValue("@ORDERID", orderId);
                        detailCmd.Parameters.AddWithValue("@ProductID", item.ProductId);
                        detailCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        detailCmd.Parameters.AddWithValue("@Price", item.UnitPrice);

                        detailCmd.ExecuteNonQuery();

                        // Update inventory
                        string updateInventoryQuery = "UPDATE Inventory SET Available = Available - @Quantity WHERE ProductID = @ProductID";
                        SqlCommand updateCmd = new SqlCommand(updateInventoryQuery, conn, transaction);
                        updateCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        updateCmd.Parameters.AddWithValue("@ProductID", item.ProductId);
                        updateCmd.ExecuteNonQuery();
                    }

                    // Insert billing record
                    string insertBillingQuery = @"
                INSERT INTO Billing (OrderID, PaymentMethod, PaymentDate, AmountPaid)
                VALUES (@OrderID, @PaymentMethod, @PaymentDate, @AmountPaid);";

                    SqlCommand billingCmd = new SqlCommand(insertBillingQuery, conn, transaction);
                    billingCmd.Parameters.AddWithValue("@OrderID", orderId);
                    billingCmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    billingCmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    billingCmd.Parameters.AddWithValue("@AmountPaid", totalAmount);
                    billingCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Compares plain text PasswordHash field (as your current DB stores plain strings)
        public bool VerifyUser(string username, string password, string userType)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                string query = @"SELECT PasswordHash 
                                 FROM UserAccounts 
                                 WHERE Username = @Username AND UserType = @UserType";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string storedPassword = Convert.ToString(result);
                        return storedPassword == password;
                    }
                }
            }
            return false;
        }
    }

    // Users CRUD against UserAccounts
    public class UserManager
    {
        private readonly DatabaseConnect dbc;

        public UserManager()
        {
            dbc = new DatabaseConnect("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True");
        }

        // View all users
        public DataTable viewall()
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM UserAccounts", conn);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        // Search by ID or partial username
        public DataTable search(int id, string user)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                string query = "SELECT * FROM UserAccounts WHERE UserID = @id OR Username LIKE @user";
                SqlDataAdapter adp = new SqlDataAdapter(query, conn);
                adp.SelectCommand.Parameters.AddWithValue("@id", id);
                adp.SelectCommand.Parameters.AddWithValue("@user", "%" + user + "%");

                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        // Delete user by UserID
        public void Deleteuser(int id)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM UserAccounts WHERE UserID = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Add user (stores password in PasswordHash column per your schema)
        public void AddUser(string username, string password, string email, string name,
                            string address, string phone, string userType)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO UserAccounts 
                                 (Username, PasswordHash, Email, FullName, Address, PhoneNumber, UserType)
                                 VALUES (@Username, @PasswordHash, @Email, @FullName, @Address, @PhoneNumber, @UserType)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FullName", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Update user details
        public void UpdateUser(int userId, string username, string password, string email,
                               string fullName, string address, string phone, string userType)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE UserAccounts
                                 SET Username = @Username,
                                     PasswordHash = @PasswordHash,
                                     Email = @Email,
                                     FullName = @FullName,
                                     Address = @Address,
                                     PhoneNumber = @PhoneNumber,
                                     UserType = @UserType
                                 WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    // Inventory CRUD (joined to Products)
    public class InventoryManager
    {
        private readonly DatabaseConnect dbc;

        public InventoryManager()
        {
            dbc = new DatabaseConnect("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True");
        }

        // View inventory with product details
        public DataTable viewallProducts()
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                string query = @"
                    SELECT i.InventoryID, i.ProductID, p.ProductName, p.Description, p.Price, p.Category,
                           i.Location, i.Available, i.LastUpdated
                    FROM Inventory i
                    INNER JOIN Products p ON i.ProductID = p.ProductID";

                SqlDataAdapter adp = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        // Search inventory by ProductID, ProductName, or Location
        public DataTable searchProduct(int id, string productOrLocation)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                string query = @"
                    SELECT i.InventoryID, i.ProductID, p.ProductName, p.Description, p.Price, p.Category,
                           i.Location, i.Available, i.LastUpdated
                    FROM Inventory i
                    INNER JOIN Products p ON i.ProductID = p.ProductID
                    WHERE (p.ProductID = @id)
                       OR (p.ProductName LIKE @term)
                       OR (i.Location LIKE @term)";

                SqlDataAdapter adp = new SqlDataAdapter(query, conn);
                adp.SelectCommand.Parameters.AddWithValue("@id", id);
                adp.SelectCommand.Parameters.AddWithValue("@term", "%" + productOrLocation + "%");

                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        // Delete inventory rows for a product (by ProductID)
        public void DeleteProduct(int productId)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Inventory WHERE ProductID = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Add/Upsert inventory for a product by name, creating product if needed
        // 'availableFlag' is mapped to 1 or 0 for initial stock per your BLL signature
        public void AddProduct(string productName, string description, decimal price, string location, bool availableFlag)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();

                // 1) Ensure product exists, create if not
                int productId = GetOrCreateProduct(conn, productName, description, price);

                // 2) Upsert inventory for (ProductID, Location)
                string upsert = @"
                    IF EXISTS (SELECT 1 FROM Inventory WHERE ProductID = @ProductID AND Location = @Location)
                        UPDATE Inventory
                        SET Available = Available + @AddQty, LastUpdated = GETDATE()
                        WHERE ProductID = @ProductID AND Location = @Location
                    ELSE
                        INSERT INTO Inventory (ProductID, Location, Available)
                        VALUES (@ProductID, @Location, @AddQty)";

                using (SqlCommand cmd = new SqlCommand(upsert, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@Location", location);
                    cmd.Parameters.AddWithValue("@AddQty", availableFlag ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Helper: create product if missing, return ProductID
        private int GetOrCreateProduct(SqlConnection conn, string productName, string description, decimal price)
        {
            // Try get existing ProductID by name
            using (SqlCommand check = new SqlCommand("SELECT ProductID FROM Products WHERE ProductName = @Name", conn))
            {
                check.Parameters.AddWithValue("@Name", productName);
                object idObj = check.ExecuteScalar();
                if (idObj != null && idObj != DBNull.Value)
                {
                    return Convert.ToInt32(idObj);
                }
            }

            // Insert new product
            using (SqlCommand insert = new SqlCommand(@"
                INSERT INTO Products (ProductName, Description, Price, Category)
                VALUES (@Name, @Desc, @Price, @Category);
                SELECT SCOPE_IDENTITY();", conn))
            {
                insert.Parameters.AddWithValue("@Name", productName);
                insert.Parameters.AddWithValue("@Desc", description ?? (object)DBNull.Value);
                insert.Parameters.AddWithValue("@Price", price);
                insert.Parameters.AddWithValue("@Category", DBNull.Value); // optional: set category if available

                object newId = insert.ExecuteScalar();
                return Convert.ToInt32(newId);
            }
        }
    }

    // Products CRUD
    public class ProductManager
    {
        private readonly DatabaseConnect dbc;

        public ProductManager()
        {
            dbc = new DatabaseConnect("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True");
        }

        // View all products
        public DataTable viewinproducts()
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Products", conn);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        // Search product by ProductID
        public DataTable searchinProducts(int id)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                string query = "SELECT * FROM Products WHERE ProductID = @id";
                SqlDataAdapter adp = new SqlDataAdapter(query, conn);
                adp.SelectCommand.Parameters.AddWithValue("@id", id);

                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        // Delete product (and optionally its inventory records)
        public void DeletefromProducts(int id)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();

                // First delete inventory rows for this product to satisfy FK constraints
                using (SqlCommand delInv = new SqlCommand("DELETE FROM Inventory WHERE ProductID = @ProductId", conn))
                {
                    delInv.Parameters.AddWithValue("@ProductId", id);
                    delInv.ExecuteNonQuery();
                }

                // Then delete the product
                using (SqlCommand delProd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ProductId", conn))
                {
                    delProd.Parameters.AddWithValue("@ProductId", id);
                    delProd.ExecuteNonQuery();
                }
            }
        }

        // Placeholder: returns true if product exists (keeps BLL signature working)
        public bool AddProductinproducts(int id)
        {
            using (SqlConnection conn = dbc.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Products WHERE ProductID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}