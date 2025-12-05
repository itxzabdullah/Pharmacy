using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PharmaPro
{
    public partial class MedicineDetails : System.Web.UI.Page
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["ProductID"] != null)
                {
                    int productId;
                    if (int.TryParse(Request.QueryString["ProductID"], out productId))
                    {
                        LoadMedicineDetails(productId);
                    }
                    else
                    {
                        lblErrorMessage.Text = "Invalid product ID provided.";
                    }
                }
                else
                {
                    lblErrorMessage.Text = "No product ID specified. Please search for a medicine.";
                }
            }
        }

        private void LoadMedicineDetails(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductName, Description, Price FROM Products WHERE ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblProductName.Text = reader["ProductName"].ToString();
                        lblDescription.Text = reader["Description"].ToString();
                        lblPrice.Text = Convert.ToDecimal(reader["Price"]).ToString("N2");
                    }
                    else
                    {
                        lblErrorMessage.Text = "Medicine not found.";
                        btnBuyNowDetails.Enabled = false;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading medicine details: " + ex.Message);
                    lblErrorMessage.Text = "An error occurred while loading medicine details. Please try again later.";
                    btnBuyNowDetails.Enabled = false;
                }
            }
        }

        protected void btnBuyNowDetails_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductID"] != null)
            {
                int productId;
                if (int.TryParse(Request.QueryString["ProductID"], out productId))
                {
                    Response.Redirect($"BuyMedicine.aspx?ProductID={productId}");
                }
                else
                {
                    lblErrorMessage.Text = "Could not process purchase: Invalid product ID.";
                }
            }
            else
            {
                lblErrorMessage.Text = "Could not process purchase: Product ID missing.";
            }
        }
    }
}