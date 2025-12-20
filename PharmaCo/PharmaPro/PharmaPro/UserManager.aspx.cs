using System;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using DomainModels;
using System.Collections.Generic;

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
            List<User> users = bll.ViewAllUsers();
            GridView1.DataSource = users;
            GridView1.DataBind();
        }

        // 🔎 Search user (you’ll need to add a SearchUser method in BLL)
        protected void Searchbtn_Click(object sender, EventArgs e)
        {
            string id = TextBox1.Text.Trim();
            string username = TextBox2.Text.Trim();

            int userId;
            if (!int.TryParse(id, out userId))
            {
                return;
            }

            // You need to implement SearchUser in BLL (returns List<User>)
            List<User> users = bll.SearchUser(userId, username);
            GridView1.DataSource = users;
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
                TextBox5.Text = selectedRow.Cells[2].Text; // Email
                TextBox7.Text = selectedRow.Cells[3].Text; // FullName
                TextBox6.Text = selectedRow.Cells[4].Text; // Address
                TextBox8.Text = selectedRow.Cells[5].Text; // Phone
                DropDownList1.SelectedValue = selectedRow.Cells[6].Text; // UserType
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

            bll.DeleteUser(userId);
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