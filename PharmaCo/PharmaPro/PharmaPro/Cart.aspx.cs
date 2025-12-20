using BusinessLogicLayer;
using DomainModels;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PharmaPro
{
    public partial class CartPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCart();
            }
        }

        private void BindCart()
        {
            if (Session["Cart"] != null)
            {
                var cart = (Cart)Session["Cart"];
                GridViewCart.DataSource = cart.Items;
                GridViewCart.DataBind();

                if (cart.Items.Count == 0)
                {
                    lblMessage.Text = "Your cart is empty.";
                    lblMessage.CssClass = "message-label error-message";
                    btnCheckout.Enabled = false;
                }
                else
                {
                    lblMessage.Text = $"Cart total: {cart.GetCartTotal():N2}";
                    lblMessage.CssClass = "message-label success-message";
                    btnCheckout.Enabled = true;
                }
            }
            else
            {
                lblMessage.Text = "Your cart is empty.";
                lblMessage.CssClass = "message-label error-message";
                btnCheckout.Enabled = false;
            }
        }

        // ✅ Handle Remove button clicks
        protected void GridViewCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveItem")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                if (Session["Cart"] != null)
                {
                    var cart = (Cart)Session["Cart"];
                    int productId = cart.Items[rowIndex].ProductId;

                    cart.RemoveItem(productId);
                    Session["Cart"] = cart;

                    lblMessage.Text = "Item removed from cart.";
                    lblMessage.CssClass = "message-label success-message";

                    BindCart();
                }
            }
        }

        // ✅ Handle Checkout
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["Cart"] != null)
            {
                var cart = (Cart)Session["Cart"];
                if (cart.Items.Count == 0)
                {
                    lblMessage.Text = "Your cart is empty.";
                    lblMessage.CssClass = "message-label error-message";
                    return;
                }

                if (Session["UserID"] == null)
                {
                    lblMessage.Text = "You must be logged in as a customer to checkout.";
                    lblMessage.CssClass = "message-label error-message";
                    return;
                }

                var bll = new BLL();
                int userId = Convert.ToInt32(Session["UserID"]);

                // ✅ Reuse payment method from BuyMedicine page
                string paymentMethod = Session["PaymentMethod"] != null
                    ? Session["PaymentMethod"].ToString()
                    : "Cash"; // fallback if not set

                try
                {
                    // Place order
                    bll.PlaceOrder(userId, cart, paymentMethod);

                    // Clear cart after checkout
                    Session["Cart"] = null;
                    GridViewCart.DataSource = null;
                    GridViewCart.DataBind();
                    btnCheckout.Enabled = false;

                    // ✅ Show popup, then redirect
                    string script = "alert('Order placed successfully! Thank you for your purchase.');" +
                                    "window.location='SearchMedicine.aspx';";
                    ScriptManager.RegisterStartupScript(this, GetType(), "OrderSuccess", script, true);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error placing order: {ex.Message}";
                    lblMessage.CssClass = "message-label error-message";
                }
            }
            else
            {
                lblMessage.Text = "Your cart is empty.";
                lblMessage.CssClass = "message-label error-message";
            }
        }
    }
}