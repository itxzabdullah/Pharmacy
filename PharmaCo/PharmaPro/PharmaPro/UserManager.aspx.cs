using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace PharmaPro
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        BLL bll = new BLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Viewbtn_Click(sender, e);
            }
        }

        // 📋 View all users
        protected void Viewbtn_Click(object sender, EventArgs e)
        {
            DataTable dt = bll.viewall();
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // 🔎 Search user
        protected void Searchbtn_Click(object sender, EventArgs e)
        {
            string id = TextBox1.Text.Trim();
            string user = TextBox2.Text.Trim();

            int uid;
            if (!int.TryParse(id, out uid))
            {
                return;
            }

            DataTable dt = bll.search(uid, user);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        // 🖱️ Select row from GridView
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow selectedRow = GridView1.SelectedRow;

            if (selectedRow != null)
            {
                TextBox1.Text = selectedRow.Cells[0].Text; // UserID
                TextBox2.Text = selectedRow.Cells[1].Text; // Username
                TextBox3.Text = selectedRow.Cells[1].Text; // Username for update
                TextBox5.Text = selectedRow.Cells[3].Text; // Email
                TextBox7.Text = selectedRow.Cells[4].Text; // FullName
                TextBox6.Text = selectedRow.Cells[5].Text; // Address
                TextBox8.Text = selectedRow.Cells[6].Text; // Phone
                DropDownList1.SelectedValue = selectedRow.Cells[7].Text; // UserType
            }
        }

        // ❌ Delete user
        protected void Deletebtn_Click(object sender, EventArgs e)
        {
            int userId;
            if (!int.TryParse(TextBox1.Text, out userId))
            {
                return;
            }

            bll.deleteUser(userId);
            Viewbtn_Click(sender, e);
        }

        // ➕ Add user
        protected void Addbtn_Click(object sender, EventArgs e)
        {
            string username = TextBox3.Text.Trim();
            string password = TextBox4.Text.Trim();
            string email = TextBox5.Text.Trim();
            string name = TextBox7.Text.Trim();
            string address = TextBox6.Text.Trim();
            string phone = TextBox8.Text.Trim();
            string userType = DropDownList1.SelectedValue;

            bll.AddUser(username, password, email, name, address, phone, userType);
            Viewbtn_Click(sender, e);
        }

        // ✏️ Update user
        protected void Updatebtn_Click(object sender, EventArgs e)
        {
            int userId;
            if (!int.TryParse(TextBox1.Text, out userId))
            {
                return;
            }

            string username = TextBox3.Text.Trim();
            string password = TextBox4.Text.Trim();
            string email = TextBox5.Text.Trim();
            string name = TextBox7.Text.Trim();
            string address = TextBox6.Text.Trim();
            string phone = TextBox8.Text.Trim();
            string userType = DropDownList1.SelectedValue;

            bll.UpdateUser(userId, username, password, email, name, address, phone, userType);
            Viewbtn_Click(sender, e);
        }
    }
}