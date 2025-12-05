using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PharmaPro
{
    public partial class OrderHistory : System.Web.UI.Page
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            LoadOrders(keyword);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadOrders();
        }

        private void LoadOrders(string keyword = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT o.OrderID, u.FullName AS CustomerName, o.OrderDate, o.TotalAmount
                    FROM Orders o
                    JOIN UserAccounts u ON o.UserID = u.UserID
                    WHERE (@keyword = '' OR 
                           o.OrderID LIKE '%' + @keyword + '%' OR 
                           u.FullName LIKE '%' + @keyword + '%')
                    ORDER BY o.OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@keyword", keyword);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ordersTable = new DataTable();
                da.Fill(ordersTable);

                // Add Items column manually
                ordersTable.Columns.Add("Items", typeof(string));

                foreach (DataRow row in ordersTable.Rows)
                {
                    int orderId = Convert.ToInt32(row["OrderID"]);
                    row["Items"] = GetOrderItems(orderId, conn);
                }

                gvOrderHistory.DataSource = ordersTable;
                gvOrderHistory.DataBind();
            }
        }

        private string GetOrderItems(int orderId, SqlConnection conn)
        {
            string query = @"
                SELECT p.ProductName, od.Quantity
                FROM OrderDetails od
                JOIN Products p ON od.ProductID = p.ProductID
                WHERE od.OrderID = @orderId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable itemsTable = new DataTable();
            da.Fill(itemsTable);

            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in itemsTable.Rows)
            {
                sb.AppendFormat("{0} ({1}), ", row["ProductName"], row["Quantity"]);
            }

            return sb.Length > 2 ? sb.ToString(0, sb.Length - 2) : "—";
        }
    }
}
