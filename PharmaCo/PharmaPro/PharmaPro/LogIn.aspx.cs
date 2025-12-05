using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace PharmaPro
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        BLL bll = new BLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDownList();
            }
        }

        protected void PopulateDropDownList()
        {
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("Select", ""));
            DropDownList1.Items.Add(new ListItem("Admin", "admin"));
            DropDownList1.Items.Add(new ListItem("Customer", "customer"));
        }

        protected void Signinbtn_Click(object sender, EventArgs e)
        {
            string username = Username_txtbox.Text.Trim();
            string password = Password_txtbox.Text.Trim();
            string userType = DropDownList1.SelectedValue;

            bool isValidUser = bll.VerifyUser(username, password, userType);

            if (isValidUser)
            {
                switch (userType.ToLower())
                {
                    case "admin":
                        Response.Redirect("AdminDashboard.aspx");
                        break;
                    case "customer":
                        Response.Redirect("CustomerDashboard.aspx");
                        break;
                    case "pharmacist":
                        Response.Redirect("PharmacistDashboard.aspx");
                        break;
                }
            }
            else
            {
                ShowMessage("Incorrect username or password.");
            }
        }

        private void ShowMessage(string message)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
        }

        public void Redirect(object sender, EventArgs e)
        {
            Response.Redirect("AdminDashboard.aspx");
        }
    }
}
