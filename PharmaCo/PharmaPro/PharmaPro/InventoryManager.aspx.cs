using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace PharmaPro
{
    public partial class InventoryManager : System.Web.UI.Page
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewAllProducts();
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), isSuccess ? "successMessage" : "errorMessage", script, true);
        }

        // 🔎 Search
        protected void Searchbtn_Click(object sender, EventArgs e)
        {
            string productIdText = TextBox1.Text.Trim();
            string productName = TextBox2.Text.Trim();
            int productId;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT p.ProductID, p.ProductName, p.Description, p.Price, p.Category,
                                        i.Location, COALESCE(i.Available,0) AS Available
                                 FROM Products p
                                 LEFT JOIN Inventory i ON p.ProductID = i.ProductID
                                 WHERE 1=1";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (int.TryParse(productIdText, out productId))
                {
                    query += " AND p.ProductID = @ProductID";
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    query += " AND p.ProductName LIKE @ProductName";
                    cmd.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                }

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                if (dt.Rows.Count == 0)
                    ShowMessage("No matches found.", false);
            }
        }

        // ❌ Delete
        protected void Deletebtn_Click(object sender, EventArgs e)
        {
            int productId;
            if (!int.TryParse(TextBox1.Text.Trim(), out productId))
            {
                ShowMessage("Enter a valid Product ID.", false);
                return;
            }

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
                    int rows = prodCmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        transaction.Commit();
                        ShowMessage("Product deleted successfully.", true);
                    }
                    else
                    {
                        transaction.Rollback();
                        ShowMessage("Product not found.", false);
                    }

                    ViewAllProducts();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ShowMessage("Error deleting product: " + ex.Message, false);
                }
            }
        }

        // 📋 View All
        protected void Viewbtn_Click(object sender, EventArgs e)
        {
            ViewAllProducts();
        }

        private void ViewAllProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT p.ProductID, p.ProductName, p.Description, p.Price, p.Category,
                                        i.Location, COALESCE(i.Available,0) AS Available
                                 FROM Products p
                                 LEFT JOIN Inventory i ON p.ProductID = i.ProductID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        // ➕ Add Product
        protected void Addbtn_Click(object sender, EventArgs e)
        {
            string name = TextBox3.Text.Trim();
            string desc = TextBox4.Text.Trim();
            string location = TextBox6.Text.Trim();
            string category = "General"; // you can add a dropdown later
            decimal price;
            int available;

            if (!decimal.TryParse(TextBox5.Text.Trim(), out price) || price <= 0)
            {
                ShowMessage("Invalid price.", false);
                return;
            }
            if (!int.TryParse(TextBox8.Text.Trim(), out available) || available < 0)
            {
                ShowMessage("Invalid available quantity.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertProduct = @"INSERT INTO Products (ProductName, Description, Price, Category)
                                             VALUES (@Name, @Desc, @Price, @Category);
                                             SELECT SCOPE_IDENTITY();";
                    SqlCommand productCmd = new SqlCommand(insertProduct, conn, transaction);
                    productCmd.Parameters.AddWithValue("@Name", name);
                    productCmd.Parameters.AddWithValue("@Desc", desc);
                    productCmd.Parameters.AddWithValue("@Price", price);
                    productCmd.Parameters.AddWithValue("@Category", category);

                    int productId = Convert.ToInt32(productCmd.ExecuteScalar());

                    string insertInventory = @"INSERT INTO Inventory (ProductID, Location, Available)
                                               VALUES (@ProductID, @Location, @Available)";
                    SqlCommand invCmd = new SqlCommand(insertInventory, conn, transaction);
                    invCmd.Parameters.AddWithValue("@ProductID", productId);
                    invCmd.Parameters.AddWithValue("@Location", location);
                    invCmd.Parameters.AddWithValue("@Available", available);

                    invCmd.ExecuteNonQuery();

                    transaction.Commit();
                    ShowMessage("Product and inventory added successfully!", true);
                    ViewAllProducts();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ShowMessage("Error adding product: " + ex.Message, false);
                }
            }
        }

        // 🔄 Update Stock (with Location)
        protected void UpdateStockbtn_Click(object sender, EventArgs e)
        {
            int productId, quantity;
            string location = TextBoxLocation.Text.Trim();

            if (!int.TryParse(TextBoxProductID.Text.Trim(), out productId) ||
                !int.TryParse(TextBoxQuantity.Text.Trim(), out quantity) || quantity <= 0 ||
                string.IsNullOrEmpty(location))
            {
                ShowMessage("Enter valid Product ID, Quantity, and Location.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        IF EXISTS (SELECT 1 FROM Inventory WHERE ProductID=@ProductID AND Location=@Location)
                            UPDATE Inventory 
                            SET Available = Available + @Quantity, LastUpdated=GETDATE()
                            WHERE ProductID=@ProductID AND Location=@Location
                        ELSE
                            INSERT INTO Inventory (ProductID, Location, Available)
                            VALUES (@ProductID, @Location, @Quantity)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Location", location);

                    cmd.ExecuteNonQuery();
                    ShowMessage($"Stock updated! Added {quantity} units in {location}.", true);
                    ViewAllProducts();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error updating stock: " + ex.Message, false);
                }
            }
        }
    }
}