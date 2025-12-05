<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuyMedicine.aspx.cs" Inherits="PharmaPro.BuyMedicine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buy Medicine</title>
    <link href="CustomerDashboard_stylesheet.css" rel="stylesheet" />
    <style>
        /* Body and container styling */
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

/* Main container */
.buy-container {
    max-width: 550px;
    background: #fff;
    margin: 50px auto 70px;
    padding: 40px 35px 50px;
    border-radius: 16px;
    box-shadow: 0 14px 40px rgba(0,0,0,0.08);
    text-align: center;
    transition: box-shadow 0.3s ease;
}

.buy-container:hover {
    box-shadow: 0 20px 50px rgba(0,0,0,0.15);
}

.buy-container h2 {
    color: #2e7d32;
    font-size: 2rem;
    margin-bottom: 30px;
    font-weight: 700;
}

/* Detail rows */
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

/* Labels and values */
.detail-label {
    font-weight: 600;
    color: #555;
    flex: 1;
    text-align: left;
    user-select: none;
}

.detail-value {
    flex: 1.5;
    color: #333;
    text-align: right;
}

/* Price styling */
.price-value {
    color: #4caf50;
    font-size: 1.3rem;
    font-weight: 700;
}

/* ProductName TextBox style */
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
    user-select: none;
    box-shadow: inset 0 2px 5px rgba(0,0,0,0.05);
}

/* Quantity input */
.quantity-input {
    width: 100px;
    padding: 10px 14px;
    font-size: 1.15rem;
    border: 2px solid #4caf50;
    border-radius: 40px;
    text-align: center;
    color: #2e7d32;
    font-weight: 600;
    transition: border-color 0.3s ease, box-shadow 0.3s ease;
    outline: none;
}

.quantity-input:focus {
    border-color: #388e3c;
    box-shadow: 0 0 8px #66bb6a;
}

/* Confirm purchase button */
.confirm-btn {
    background: linear-gradient(135deg, #66bb6a, #388e3c);
    color: #fff;
    font-weight: 700;
    font-size: 1.3rem;
    padding: 15px 40px;
    border: none;
    border-radius: 40px;
    margin-top: 40px;
    cursor: pointer;
    box-shadow: 0 8px 20px rgba(56, 142, 60, 0.5);
    transition: background 0.4s ease, box-shadow 0.4s ease, transform 0.2s ease;
    user-select: none;
}

.confirm-btn:hover,
.confirm-btn:focus {
    background: linear-gradient(135deg, #388e3c, #1b5e20);
    box-shadow: 0 12px 28px rgba(27, 94, 32, 0.7);
    transform: translateY(-3px);
    outline: none;
}

/* Messages */
.message-label {
    margin-top: 25px;
    font-weight: 700;
    font-size: 1.1rem;
    user-select: none;
}

.success-message {
    color: #2e7d32;
}

.error-message {
    color: #d32f2f;
}

/* Responsive */
@media (max-width: 480px) {
    .buy-container {
        margin: 30px 20px 60px;
        padding: 30px 20px 40px;
    }

    .detail-row {
        flex-direction: column;
        align-items: flex-start;
        font-size: 1rem;
    }

    .detail-value {
        text-align: left;
        margin-top: 6px;
        flex: unset;
        width: 100%;
    }

    .quantity-input {
        width: 100%;
        max-width: none;
    }

    .confirm-btn {
        width: 100%;
        padding: 15px 0;
        font-size: 1.2rem;
    }
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="container">
                <h1 class="title-heading">PharmaCo           Buy Medicine</h1>
            </div>
        </div>

        <div class="buy-container">
            <h2>Confirm Your Purchase</h2>

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
                <span class="detail-value price-value">$<asp:Label ID="lblPrice" runat="server"></asp:Label></span>
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

            <asp:Button ID="btnConfirmPurchase" runat="server" Text="Confirm Purchase" CssClass="confirm-btn" OnClick="btnConfirmPurchase_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
        </div>
    </form>
</body>
</html>
