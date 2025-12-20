<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="PharmaPro.OrderHistory" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmaCo Ltd - Order History</title>
    <link href="https://fonts.googleapis.com/css2?family=Segoe+UI:wght@400;600&display=swap" rel="stylesheet" />
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', sans-serif;
            background: linear-gradient(135deg, #e0f7fa, #d0f0ec, #f0fcff); /* same as login */
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: flex-start;
        }

        .main-div {
            max-width: 1100px;
            margin: 40px auto;
            background: rgba(255, 255, 255, 0.25);
            border-radius: 12px;
            box-shadow: 0 8px 32px rgba(0,0,0,0.2);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255,255,255,0.3);
            padding: 28px;
        }

        .title-heading {
            text-align: center;
            color: #003c58; /* same navy tone as login header */
            font-size: 1.8em;
            font-weight: 600;
            margin: 0 0 22px 0;
        }

        .card {
            background: rgba(255,255,255,0.4);
            border-radius: 10px;
            box-shadow: inset 0 1px 4px rgba(0,0,0,0.08);
            padding: 20px;
            margin-bottom: 18px;
        }

        .card h2 {
            color: #005f73; /* teal accent like login subheading */
            font-size: 1.3em;
            font-weight: 600;
            margin: 0 0 14px 0;
            text-align: center;
        }

        .toolbar {
            display: flex;
            gap: 12px;
            justify-content: center;
            align-items: center;
            flex-wrap: wrap;
            margin-bottom: 14px;
        }

        .textbox {
            padding: 12px;
            width: 260px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 0.95em;
            background: rgba(255,255,255,0.6);
            color: #1a1a1a;
        }

        .textbox:focus {
            background: rgba(255,255,255,0.8);
            border-color: #0077b6; /* same blue glow as login */
            box-shadow: 0 0 6px rgba(0,119,182,0.3);
            outline: none;
        }

        .action-button {
            padding: 12px 18px;
            background: linear-gradient(90deg, #0077b6, #00b4d8); /* same gradient as login button */
            border: none;
            border-radius: 8px;
            color: #fff;
            font-size: 0.95em;
            font-weight: 600;
            cursor: pointer;
            transition: 0.25s;
        }

        .action-button:hover {
            background: linear-gradient(90deg, #005f8a, #0096c7);
        }

        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            background: rgba(255,255,255,0.65);
            border-radius: 8px;
            overflow: hidden;
        }

        .gridview-style th,
        .gridview-style td {
            padding: 12px;
            border: 1px solid #ddd;
            text-align: left;
            font-size: 0.95em;
            color: #1a1a1a;
        }

        .gridview-style th {
            background: #0077b6; /* unify with login button color */
            color: #fff;
            font-weight: 600;
        }

        .gridview-style tr:nth-child(even) { background: #f0fcff; }
        .gridview-style tr:hover { background: #d0f0ec; }

        .footer {
            text-align: center;
            padding: 12px;
            font-size: 0.85em;
            color: #333;
            border-top: 1px solid rgba(255,255,255,0.2);
            background: rgba(255,255,255,0.4);
            margin-top: 18px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-div">
            <h1 class="title-heading">PharmaCo Ltd - Order History</h1>

            <div class="card">
                <h2>Search / Filter Orders</h2>
                <div class="toolbar">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="textbox" placeholder="Search by customer, product, or order ID"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="action-button" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="action-button" OnClick="btnClear_Click" />
                </div>
            </div>

            <div class="card">
                <h2>All Orders (Most Recent First)</h2>
                <asp:GridView ID="gvOrderHistory" runat="server" AutoGenerateColumns="False" CssClass="gridview-style">
                    <Columns>
                        <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:dd-MMM-yyyy HH:mm}" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="Items" HeaderText="Products (qty)" />
                    </Columns>
                </asp:GridView>


            </div>

            <div class="footer">
                © PharmaCo Ltd Admin Console — 2025
            </div>
        </div>
    </form>
</body>
</html>