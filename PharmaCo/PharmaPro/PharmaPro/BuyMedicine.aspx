<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuyMedicine.aspx.cs" Inherits="PharmaPro.BuyMedicine" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buy Medicine</title>
    <link href="CustomerDashboard_stylesheet.css" rel="stylesheet" />
    <style>
        body, html {
            margin: 0;
            padding: 0;
            background: #e9f0f7;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
        }

        .header {
            background-color: #4caf50;
            padding: 18px 0;
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.35);
            text-align: center;
        }

        .title-heading {
            color: #fff;
            font-size: 2.6rem;
            font-weight: 700;
            letter-spacing: 1.3px;
            margin: 0;
            user-select: none;
        }

        .buy-container {
            max-width: 550px;
            background: #fff;
            margin: 50px auto 70px;
            padding: 40px 35px 50px;
            border-radius: 16px;
            box-shadow: 0 14px 40px rgba(0,0,0,0.08);
            text-align: center;
        }

        .buy-container h2 {
            color: #2e7d32;
            font-size: 2rem;
            margin-bottom: 30px;
            font-weight: 700;
        }

        .detail-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 0;
            border-bottom: 1px solid #e0e0e0;
            font-size: 1.1rem;
        }

        .detail-row:last-child {
            border-bottom: none;
        }

        .detail-label {
            font-weight: 600;
            color: #555;
            flex: 1;
            text-align: left;
        }

        .detail-value {
            flex: 1.5;
            color: #333;
            text-align: right;
        }

        .price-value {
            color: #4caf50;
            font-size: 1.3rem;
            font-weight: 700;
        }

        input[readonly][id$="ProductName"] {
            border: none;
            background-color: #f9f9f9;
            color: #333;
            font-size: 1.1rem;
            font-weight: 600;
            padding: 8px 12px;
            width: 100%;
            max-width: 280px;
            border-radius: 6px;
            text-align: right;
        }

        .quantity-input {
            width: 100px;
            padding: 10px 14px;
            font-size: 1.15rem;
            border: 2px solid #4caf50;
            border-radius: 40px;
            text-align: center;
            color: #2e7d32;
            font-weight: 600;
        }

        .confirm-btn, .cart-btn {
            font-weight: 700;
            font-size: 1.2rem;
            padding: 12px 30px;
            border: none;
            border-radius: 40px;
            margin-top: 25px;
            cursor: pointer;
        }

        .confirm-btn {
            background: linear-gradient(135deg, #66bb6a, #388e3c);
            color: #fff;
        }

        .cart-btn {
            background: linear-gradient(135deg, #42a5f5, #1e88e5);
            color: #fff;
            margin-left: 15px;
        }

        .message-label {
            margin-top: 25px;
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
            <h1 class="title-heading">PharmaCo Buy Medicine</h1>
        </div>

        <div class="buy-container">
            <h2>Confirm Your Purchase</h2>

            <!-- Product details -->
            <div class="detail-row">
                <span class="detail-label">Product Name:</span>
                <asp:TextBox ID="ProductName" runat="server" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="detail-row">
                <span class="detail-label">Description:</span>
                <span class="detail-value"><asp:Label ID="lblDescription" runat="server"></asp:Label></span>
            </div>

            <div class="detail-row">
                <span class="detail-label">Price Per Unit:</span>
                <!-- ✅ Default numeric price, no Rs prefix -->
                <span class="detail-value price-value"><asp:Label ID="lblPrice" runat="server"></asp:Label></span>
            </div>

            <div class="detail-row">
                <span class="detail-label">Available Stock:</span>
                <span class="detail-value"><asp:Label ID="lblStock" runat="server"></asp:Label></span>
            </div>

            <div class="detail-row">
                <span class="detail-label">Quantity:</span>
                <span class="detail-value">
                    <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" Text="1" CssClass="quantity-input"></asp:TextBox>
                </span>
            </div>

            <!-- Payment method -->
            <div class="detail-row">
                <span class="detail-label">Payment Method:</span>
                <span class="detail-value">
                    <asp:DropDownList ID="ddlPaymentMethod" runat="server">
                        <asp:ListItem Text="Cash on Delivery" Value="Cash"></asp:ListItem>
                        <asp:ListItem Text="Online Payment" Value="Online"></asp:ListItem>
                        <asp:ListItem Text="Credit Card" Value="Credit Card"></asp:ListItem>
                        <asp:ListItem Text="Debit Card" Value="Debit Card"></asp:ListItem>
                    </asp:DropDownList>

                </span>
            </div>

            <!-- Action buttons -->
            <asp:Button ID="btnConfirmPurchase" runat="server" Text="Confirm Purchase" CssClass="confirm-btn" OnClick="btnConfirmPurchase_Click" />
            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="cart-btn" OnClick="btnAddToCart_Click" />

            <!-- Message label -->
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
        </div>
    </form>
</body>
</html>