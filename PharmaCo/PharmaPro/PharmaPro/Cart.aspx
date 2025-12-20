<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="PharmaPro.CartPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Your Cart</title>
    <link href="CustomerDashboard_stylesheet.css" rel="stylesheet" />
    <style>
        body, html {
            margin: 0;
            padding: 0;
            background: #f4f7fa;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
        }

        .header {
            background-color: #1e88e5;
            padding: 18px 0;
            text-align: center;
            box-shadow: 0 4px 12px rgba(30, 136, 229, 0.35);
        }

        .title-heading {
            color: #fff;
            font-size: 2.4rem;
            font-weight: 700;
            margin: 0;
        }

        .cart-container {
            max-width: 900px;
            background: #fff;
            margin: 40px auto;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.08);
        }

        .cart-grid {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 25px;
        }

        .cart-grid th, .cart-grid td {
            border: 1px solid #ddd;
            padding: 12px;
            text-align: center;
        }

        .cart-grid th {
            background-color: #1e88e5;
            color: #fff;
            font-weight: 600;
        }

        .checkout-btn {
            background: linear-gradient(135deg, #66bb6a, #388e3c);
            color: #fff;
            font-weight: 700;
            font-size: 1.2rem;
            padding: 12px 30px;
            border: none;
            border-radius: 40px;
            cursor: pointer;
            margin-top: 20px;
        }

        .message-label {
            margin-top: 20px;
            font-weight: 700;
            font-size: 1.1rem;
        }

        .success-message { color: #2e7d32; }
        .error-message { color: #d32f2f; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <h1 class="title-heading">Your Shopping Cart</h1>
        </div>

        <div class="cart-container">
            <asp:GridView ID="GridViewCart" runat="server" AutoGenerateColumns="False" CssClass="cart-grid"
                OnRowCommand="GridViewCart_RowCommand" DataKeyNames="ProductId">
                <Columns>
                    <asp:BoundField DataField="ProductId" HeaderText="Product ID" />
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" />

                    <asp:ButtonField Text="Remove" CommandName="RemoveItem" ButtonType="Button" />
                </Columns>
            </asp:GridView>

            <!-- Checkout button -->
            <asp:Button ID="btnCheckout" runat="server" Text="Checkout" CssClass="checkout-btn" OnClick="btnCheckout_Click" />

            <!-- Message label -->
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
        </div>
    </form>
</body>
</html>