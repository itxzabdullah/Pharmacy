using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PharmaPro
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Elementbtn1_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchMedicine.aspx");
        }

        protected void Elementbtn2_Click(object sender, EventArgs e)
        {
            Response.Redirect("BuyMedicine.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("LogIn.aspx");
        }
    }
}
