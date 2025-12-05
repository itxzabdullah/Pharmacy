using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace PharmaPro
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Viewbtn_Click(sender, e);
            }
        }

        // Add Product
        protected void AddProductBtn_Click(object sender, EventArgs e)
        {
            string name = TextBoxName.Text.Trim();
            string desc = TextBoxDesc.Text.Trim();
            string category = TextBoxCategory.Text.Trim();
            decimal price;

            if (!decimal.TryParse(TextBoxPrice.Text.Trim(), out price) || price <= 0)
            {
                ShowMessage("Invalid price entered!", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Products (ProductName, Description, Price, Category) VALUES (@Name, @Desc, @Price, @Category)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Desc", desc);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Category", category);

                    cmd.ExecuteNonQuery();
                    ShowMessage("Product added successfully!", true);
                    Viewbtn_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error adding product: " + ex.Message, false);
                }
            }
        }

        // Update Product
        protected void UpdateProductBtn_Click(object sender, EventArgs e)
        {
            int productId;
            if (!int.TryParse(TextBoxUpdateID.Text.Trim(), out productId))
            {
                ShowMessage("Invalid Product ID!", false);
                return;
            }

            string name = TextBoxUpdateName.Text.Trim();
            string desc = TextBoxUpdateDesc.Text.Trim();
            string category = TextBoxUpdateCategory.Text.Trim();
            decimal price;

            if (!decimal.TryParse(TextBoxUpdatePrice.Text.Trim(), out price) || price <= 0)
            {
                ShowMessage("Invalid price entered!", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Products 
                                     SET ProductName=@Name, Description=@Desc, Price=@Price, Category=@Category 
                                     WHERE ProductID=@ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Desc", desc);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        ShowMessage("Product updated successfully!", true);
                    else
                        ShowMessage("No product found with that ID.", false);

                    Viewbtn_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error updating product: " + ex.Message, false);
                }
            }
        }

        // Search Product
        protected void Searchbtn_Click(object sender, EventArgs e)
        {
            int productId;
            if (!int.TryParse(TextBoxSearchID.Text.Trim(), out productId))
            {
                ShowMessage("Please enter a valid Product ID!", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ProductID, ProductName, Description, Price, Category FROM Products WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                    else
                    {
                        ShowMessage("No product found for the given ID.", false);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error searching for product: " + ex.Message, false);
                }
            }
        }

        // Delete Product
        protected void Deletebtn_Click(object sender, EventArgs e)
        {
            int productId;
            if (!int.TryParse(TextBoxSearchID.Text.Trim(), out productId))
            {
                ShowMessage("Please enter a valid Product ID!", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        ShowMessage("Product deleted successfully!", true);
                    else
                        ShowMessage("No product found to delete.", false);

                    Viewbtn_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error deleting product: " + ex.Message, false);
                }
            }
        }

        // View All Products
        protected void Viewbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT ProductID, ProductName, Description, Price, Category FROM Products";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error retrieving product list: " + ex.Message, false);
                }
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), isSuccess ? "successMessage" : "errorMessage", script, true);
        }
    }
}