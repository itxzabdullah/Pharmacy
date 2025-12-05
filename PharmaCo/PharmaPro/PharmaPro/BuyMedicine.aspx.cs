using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PharmaPro
{
    public partial class BuyMedicine : System.Web.UI.Page
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";
        private int currentProductId;
        private decimal productUnitPrice;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ProductID"] != null)
                {
                    if (int.TryParse(Request.QueryString["ProductID"], out currentProductId))
                    {
                        LoadMedicineDetails(currentProductId);
                    }
                    else
                    {
                        lblMessage.Text = "Invalid product ID provided.";
                        lblMessage.CssClass = "message-label error-message";
                        btnConfirmPurchase.Enabled = false;
                    }
                }
                else
                {
                    lblMessage.Text = "No product ID specified. Please select a medicine to buy.";
                    lblMessage.CssClass = "message-label error-message";
                    btnConfirmPurchase.Enabled = false;
                }
            }
        }

        private void LoadMedicineDetails(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductName, Description, Price FROM Products WHERE ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductName.Text = reader["ProductName"].ToString();
                        lblDescription.Text = reader["Description"].ToString();
                        productUnitPrice = Convert.ToDecimal(reader["Price"]);
                        lblPrice.Text = productUnitPrice.ToString("N2");
                    }
                    else
                    {
                        lblMessage.Text = "Medicine not found.";
                        lblMessage.CssClass = "message-label error-message";
                        btnConfirmPurchase.Enabled = false;
                        return;
                    }
                    reader.Close();

                    
                    string stockQuery = "SELECT Available FROM Inventory WHERE ProductID = @ProductID";
                    SqlCommand stockCmd = new SqlCommand(stockQuery, conn);
                    stockCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = productId;

                    object stockObj = stockCmd.ExecuteScalar();
                    lblStock.Text = (stockObj != null) ? stockObj.ToString() : "N/A";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading medicine details: " + ex.Message);
                    lblMessage.Text = "An error occurred while loading medicine details. Please try again later.";
                    lblMessage.CssClass = "message-label error-message";
                    btnConfirmPurchase.Enabled = false;
                }
            }
        }

        protected void btnConfirmPurchase_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductID"] != null && int.TryParse(Request.QueryString["ProductID"], out currentProductId))
            {
                int quantity;
                if (int.TryParse(txtQuantity.Text, out quantity) && quantity > 0)
                {
                    if (productUnitPrice == 0 && !decimal.TryParse(lblPrice.Text, out productUnitPrice))
                    {
                        lblMessage.Text = "Could not retrieve product price. Please try again.";
                        lblMessage.CssClass = "message-label error-message";
                        return;
                    }

                    decimal totalAmount = productUnitPrice * quantity;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            
                            string checkStockQuery = "SELECT Available FROM Inventory WHERE ProductID = @ProductID";
                            SqlCommand checkStockCmd = new SqlCommand(checkStockQuery, conn, transaction);
                            checkStockCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = currentProductId;
                            object availableStockObj = checkStockCmd.ExecuteScalar();

                            if (availableStockObj == null)
                            {
                                lblMessage.Text = "Inventory record not found for this product.";
                                lblMessage.CssClass = "message-label error-message";
                                transaction.Rollback();
                                return;
                            }

                            int availableStock = Convert.ToInt32(availableStockObj);
                            if (availableStock < quantity)
                            {
                                lblMessage.Text = $"Insufficient stock. Only {availableStock} units available.";
                                lblMessage.CssClass = "message-label error-message";
                                transaction.Rollback();
                                return;
                            }

                            
                            string insertOrderQuery = @"
                                INSERT INTO Orders (UserID, OrderDate, TotalAmount, PaymentStatus)
                                VALUES (@UserID, @OrderDate, @TotalAmount, @PaymentStatus);
                                SELECT SCOPE_IDENTITY();";

                            SqlCommand orderCmd = new SqlCommand(insertOrderQuery, conn, transaction);
                            orderCmd.Parameters.Add("@UserID", SqlDbType.Int).Value = 2; // Use session in real-world app
                            orderCmd.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = DateTime.Now;
                            orderCmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = totalAmount;
                            orderCmd.Parameters.Add("@PaymentStatus", SqlDbType.VarChar).Value = "Pending";

                            int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());

                            
                            string insertOrderDetailQuery = @"
                                INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price)
                                VALUES (@OrderID, @ProductID, @Quantity, @Price);";

                            SqlCommand orderDetailCmd = new SqlCommand(insertOrderDetailQuery, conn, transaction);
                            orderDetailCmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId;
                            orderDetailCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = currentProductId;
                            orderDetailCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                            orderDetailCmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = productUnitPrice;

                            orderDetailCmd.ExecuteNonQuery();

                            
                            string updateInventoryQuery = "UPDATE Inventory SET Available = Available - @Quantity, LastUpdated = GETDATE() WHERE ProductID = @ProductID";
                            SqlCommand updateInventoryCmd = new SqlCommand(updateInventoryQuery, conn, transaction);
                            updateInventoryCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                            updateInventoryCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = currentProductId;
                            updateInventoryCmd.ExecuteNonQuery();

                            transaction.Commit();

                            lblMessage.Text = $"Successfully purchased {quantity} unit(s) of {ProductName.Text} for ${totalAmount:N2}. Your order has been placed!";
                            lblMessage.CssClass = "message-label success-message";
                            btnConfirmPurchase.Enabled = false;
                            txtQuantity.Enabled = false;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            System.Diagnostics.Debug.WriteLine("Error during purchase: " + ex.Message);
                            lblMessage.Text = "An error occurred during your purchase. Please try again later. Error: " + ex.Message;
                            lblMessage.CssClass = "message-label error-message";
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "Please enter a valid quantity (a positive number).";
                    lblMessage.CssClass = "message-label error-message";
                }
            }
            else
            {
                lblMessage.Text = "Cannot confirm purchase: Product ID missing or invalid.";
                lblMessage.CssClass = "message-label error-message";
            }
        }
    }
}