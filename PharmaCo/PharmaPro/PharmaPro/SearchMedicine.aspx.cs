using PharmaPro.Models;
using PharmaPro.Repositories;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PharmaPro
{
    public partial class SearchMedicine : System.Web.UI.Page
    {
        private readonly IProductRepository _productRepo;

        public SearchMedicine()
        {
            string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PharmaPro;Integrated Security=True";
            _productRepo = new ProductRepository(connString);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMedicines("");
            }
        }

        private void LoadMedicines(string searchTerm)
        {
            var products = _productRepo.SearchProducts(searchTerm);
            medicineRepeater.DataSource = products;
            medicineRepeater.DataBind();
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            LoadMedicines(searchBox.Text.Trim());
        }

        protected void Btn1_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuyMedicine.aspx");
        }

        protected void medicineRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails" && e.CommandArgument != null)
            {
                int productId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"MedicineDetails.aspx?ProductID={productId}");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
