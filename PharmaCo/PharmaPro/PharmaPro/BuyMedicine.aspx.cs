using BusinessLogicLayer;
using DataAccessLayer;
using DomainModels;
using System;

namespace PharmaPro
{
    public partial class BuyMedicine : System.Web.UI.Page
    {
        private int currentProductId;

        // Persist productUnitPrice across postbacks
        private decimal productUnitPrice
        {
            get { return (decimal)(ViewState["ProductUnitPrice"] ?? 0m); }
            set { ViewState["ProductUnitPrice"] = value; }
        }

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
                        btnAddToCart.Enabled = false;
                    }
                }
                else
                {
                    lblMessage.Text = "No product ID specified. Please select a medicine to buy.";
                    lblMessage.CssClass = "message-label error-message";
                    btnConfirmPurchase.Enabled = false;
                    btnAddToCart.Enabled = false;
                }
            }
        }

        private void LoadMedicineDetails(int productId)
        {
            var bll = new BLL();
            var medicines = bll.ViewAllProducts();

            var medicine = medicines.Find(m => m.ProductId == productId);
            if (medicine != null)
            {
                ProductName.Text = medicine.Name;
                lblDescription.Text = medicine.Description;
                productUnitPrice = medicine.Price;

                if (productUnitPrice == 0)
                {
                    lblMessage.Text = "⚠ Price is 0. Please check your Products table or BLL column mapping.";
                    lblMessage.CssClass = "message-label error-message";
                }

                // Default numeric format (no Rs prefix)
                lblPrice.Text = $"Unit Price: {productUnitPrice:N2}";
                lblStock.Text = "Available";
            }
            else
            {
                lblMessage.Text = "Medicine not found.";
                lblMessage.CssClass = "message-label error-message";
                btnConfirmPurchase.Enabled = false;
                btnAddToCart.Enabled = false;
            }
        }

        // Confirm Purchase flow
        protected void btnConfirmPurchase_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductID"] != null && int.TryParse(Request.QueryString["ProductID"], out currentProductId))
            {
                int quantity;
                if (int.TryParse(txtQuantity.Text, out quantity) && quantity > 0)
                {
                    var medicine = new Medicine(
                        currentProductId,
                        ProductName.Text,
                        lblDescription.Text,
                        productUnitPrice
                    );

                    var cart = new Cart();
                    var cartItem = new CartItem(medicine.ProductId, medicine.Name, productUnitPrice, quantity);
                    cart.AddItem(cartItem);

                    var bll = new BLL();

                    // ✅ Use logged-in user ID from session
                    if (Session["UserID"] == null)
                    {
                        lblMessage.Text = "You must be logged in as a customer to place an order.";
                        lblMessage.CssClass = "message-label error-message";
                        return;
                    }

                    int userId = Convert.ToInt32(Session["UserID"]);
                    string paymentMethod = ddlPaymentMethod.SelectedValue;
                    bll.PlaceOrder(userId, cart, paymentMethod);

                    decimal totalAmount = productUnitPrice * quantity;

                    // Default numeric format
                    lblMessage.Text = $"Successfully purchased {quantity} unit(s) of {medicine.Name}. " +
                                      $"Unit Price: {productUnitPrice:N2}, Total: {totalAmount:N2}. " +
                                      "Your order has been placed!";
                    lblMessage.CssClass = "message-label success-message";

                    btnConfirmPurchase.Enabled = false;
                    txtQuantity.Enabled = false;
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

        // Add to Cart flow
        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductID"] != null && int.TryParse(Request.QueryString["ProductID"], out currentProductId))
            {
                int quantity;
                if (int.TryParse(txtQuantity.Text, out quantity) && quantity > 0)
                {
                    var medicine = new Medicine(
                        currentProductId,
                        ProductName.Text,
                        lblDescription.Text,
                        productUnitPrice
                    );

                    var cartItem = new CartItem(medicine.ProductId, medicine.Name, productUnitPrice, quantity);

                    Cart cart;
                    if (Session["Cart"] == null)
                    {
                        cart = new Cart();
                    }
                    else
                    {
                        cart = (Cart)Session["Cart"];
                    }

                    cart.AddItem(cartItem);
                    Session["Cart"] = cart;

                    Response.Redirect("Cart.aspx");
                }
                else
                {
                    lblMessage.Text = "Please enter a valid quantity (a positive number).";
                    lblMessage.CssClass = "message-label error-message";
                }
            }
            else
            {
                lblMessage.Text = "Cannot add to cart: Product ID missing or invalid.";
                lblMessage.CssClass = "message-label error-message";
            }
        }
    }
}