using System;
using System.Web.UI;
using BusinessLogicLayer;
using DomainModels;

namespace PharmaPro
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        BLL bll = new BLL();

        protected void Signinbtn_Click(object sender, EventArgs e)
        {
            string username = Username_txtbox.Text.Trim();
            string password = Password_txtbox.Text.Trim();
            string userType = DropDownList1.SelectedValue;

            var user = bll.GetUser(username, password, userType);

            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserType"] = user.UserType;
                Session["FullName"] = user.FullName;

                switch (user.UserType.ToLower())
                {
                    case "admin":
                        Response.Redirect("AdminDashboard.aspx");
                        break;
                    case "customer":
                        Response.Redirect("CustomerDashboard.aspx");
                        break;
                    default:
                        ShowMessage("Unknown user type.");
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
    }
}