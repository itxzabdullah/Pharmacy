<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryManager.aspx.cs" Inherits="PharmaPro.InventoryManager" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmaCo Ltd - Inventory Manager</title>
    <link href="https://fonts.googleapis.com/css2?family=Segoe+UI:wght@400;600&display=swap" rel="stylesheet" />
    <style>
        /* Base layout */
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', sans-serif;
            background: linear-gradient(135deg, #e0f7fa, #d0f0ec, #f0fcff);
            min-height: 100vh;
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
            color: #003c58;
            font-size: 1.8em;
            font-weight: 600;
            margin: 0 0 22px 0;
        }

        /* Cards */
        .card {
            background: rgba(255,255,255,0.4);
            border-radius: 10px;
            box-shadow: inset 0 1px 4px rgba(0,0,0,0.08);
            padding: 20px;
            margin-bottom: 18px;
        }

        .card h2 {
            color: #005f73;
            font-size: 1.3em;
            font-weight: 600;
            margin: 0 0 14px 0;
            text-align: center;
        }

        /* Inputs and labels */
        .label {
            font-weight: 500;
            color: #1a1a1a;
            display: inline-block;
            margin-bottom: 6px;
        }

        .textbox, .dropdown {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 0.95em;
            background: rgba(255,255,255,0.6);
            color: #1a1a1a;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.05);
        }

        .textbox:focus, .dropdown:focus {
            background: rgba(255,255,255,0.8);
            border-color: #0077b6;
            box-shadow: 0 0 6px rgba(0,119,182,0.3);
            outline: none;
        }

        /* Buttons */
        .actions {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
            align-items: center;
        }

        .action-button {
            padding: 12px 18px;
            background: linear-gradient(90deg, #0077b6, #00b4d8);
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

        /* Grid styling */
        .grid {
            margin-top: 10px;
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
            background: #0077b6;
            color: #fff;
            font-weight: 600;
        }

        .gridview-style tr:nth-child(even) {
            background: #f0f9ff;
        }

        .gridview-style tr:hover {
            background: #d9f0ff;
        }

        /* Footer and messages */
        .footer {
            text-align: center;
            padding: 12px;
            font-size: 0.85em;
            color: #333;
            border-top: 1px solid rgba(255,255,255,0.2);
            background: rgba(255,255,255,0.4);
            margin-top: 18px;
        }

        .message-label {
            margin-top: 10px;
            font-weight: 600;
            color: #005f73;
            text-align: center;
        }
        .success-message { color: #1b7a4a; }
        .error-message { color: #c0392b; }

        /* Layout helpers */
        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px 18px;
        }
        .full-row { grid-column: 1 / -1; }

        @media (max-width: 760px) {
            .form-grid { grid-template-columns: 1fr; }
            .actions { justify-content: center; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-div">
            <h1 class="title-heading">PharmaPro Admin Console: Manage Inventory</h1>

            <!-- Search / Delete / View -->
            <div class="card">
                <h2>Search / Delete / View Inventory</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="Label1" runat="server" Text="Product ID" CssClass="label" />
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox" placeholder="Enter Product ID" />
                    </div>
                    <div>
                        <asp:Label ID="Label2" runat="server" Text="Product Name" CssClass="label" />
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox" placeholder="Enter Product Name" />
                    </div>
                    <div class="actions full-row">
                        <asp:Button ID="Searchbtn" runat="server" Text="Search" OnClick="Searchbtn_Click" CssClass="action-button" OnClientClick="return validateSearch();" />
                        <asp:Button ID="Deletebtn" runat="server" Text="Delete" OnClick="Deletebtn_Click" CssClass="action-button" OnClientClick="return validateDelete();" />
                        <asp:Button ID="Viewbtn" runat="server" Text="View All" OnClick="Viewbtn_Click" CssClass="action-button" />
                    </div>
                </div>
            </div>

            <!-- Add New Product -->
            <div class="card">
                <h2>Add New Product to Inventory</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="Label3" runat="server" Text="Product Name" CssClass="label" />
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="textbox" />
                    </div>
                    <div>
                        <asp:Label ID="Label4" runat="server" Text="Description" CssClass="label" />
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="textbox" />
                    </div>
                    <div>
                        <asp:Label ID="Label5" runat="server" Text="Price" CssClass="label" />
                        <asp:TextBox ID="TextBox5" runat="server" TextMode="Number" CssClass="textbox" />
                    </div>
                    <div>
                        <asp:Label ID="Label7" runat="server" Text="Location" CssClass="label" />
                        <asp:TextBox ID="TextBox6" runat="server" CssClass="textbox" placeholder="Karachi / Lahore / Islamabad" />
                    </div>
                    <div class="full-row">
                        <asp:Label ID="Label8" runat="server" Text="Available quantity" CssClass="label" />
                        <asp:TextBox ID="TextBox8" runat="server" TextMode="Number" CssClass="textbox" />
                    </div>
                    <div class="actions full-row">
                        <asp:Button ID="Addbtn" runat="server" Text="Add Product" OnClick="Addbtn_Click" CssClass="action-button" OnClientClick="return validateAddForm();" />
                    </div>
                </div>
            </div>

            <!-- Update Stock (with Location) -->
            <div class="card">
                <h2>Update Stock (by location)</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="LabelUpdateID" runat="server" Text="Product ID" CssClass="label" />
                        <asp:TextBox ID="TextBoxProductID" runat="server" CssClass="textbox" placeholder="Enter Product ID" />
                    </div>
                    <div>
                        <asp:Label ID="LabelUpdateQty" runat="server" Text="Quantity to add" CssClass="label" />
                        <asp:TextBox ID="TextBoxQuantity" runat="server" CssClass="textbox" TextMode="Number" placeholder="e.g., 50" />
                    </div>
                    <div class="full-row">
                        <asp:Label ID="LabelUpdateLocation" runat="server" Text="Location (city)" CssClass="label" />
                        <asp:TextBox ID="TextBoxLocation" runat="server" CssClass="textbox" placeholder="Karachi / Lahore / Islamabad" />
                    </div>
                    <div class="actions full-row">
                        <asp:Button ID="UpdateStockbtn" runat="server" Text="Update Stock" CssClass="action-button" OnClick="UpdateStockbtn_Click" OnClientClick="return validateUpdateStock();" />
                    </div>
                </div>
            </div>

            <!-- Inventory Grid -->
            <div class="grid">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview-style">
                    <Columns>
                        <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                        <asp:BoundField DataField="ProductName" HeaderText="Product name" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        <asp:BoundField DataField="Available" HeaderText="Available" />
                    </Columns>
                </asp:GridView>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>

            <div class="footer">
                PharmaPro Nexus® 2025 — all rights reserved.
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function validateSearch() {
            var pid = document.getElementById("<%= TextBox1.ClientID %>").value.trim();
            var pname = document.getElementById("<%= TextBox2.ClientID %>").value.trim();
            if (pid === "" && pname === "") {
                alert("Please enter either Product ID or Product Name to search.");
                return false;
            }
            return true;
        }

        function validateDelete() {
            var pid = document.getElementById("<%= TextBox1.ClientID %>").value.trim();
            if (pid === "") {
                alert("Please enter Product ID to delete.");
                return false;
            }
            return true;
        }

        function validateAddForm() {
            var n = document.getElementById("<%= TextBox3.ClientID %>").value.trim();
            var d = document.getElementById("<%= TextBox4.ClientID %>").value.trim();
            var p = document.getElementById("<%= TextBox5.ClientID %>").value.trim();
            var l = document.getElementById("<%= TextBox6.ClientID %>").value.trim();
            var a = document.getElementById("<%= TextBox8.ClientID %>").value.trim();
            if (n === "" || d === "" || p === "" || l === "" || a === "") {
                alert("Please fill in all fields to add a new product.");
                return false;
            }
            return true;
        }

        function validateUpdateStock() {
            var id = document.getElementById("<%= TextBoxProductID.ClientID %>").value.trim();
            var qty = document.getElementById("<%= TextBoxQuantity.ClientID %>").value.trim();
            var loc = document.getElementById("<%= TextBoxLocation.ClientID %>").value.trim();
            if (id === "" || qty === "" || parseInt(qty, 10) <= 0 || loc === "") {
                alert("Please enter Product ID, a positive Quantity, and Location.");
                return false;
            }
            return true;
        }
    </script>
</body>
</html>