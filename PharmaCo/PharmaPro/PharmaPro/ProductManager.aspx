<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductManager.aspx.cs" Inherits="PharmaPro.WebForm6" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmaCo Ltd - Product Manager</title>
    <link href="https://fonts.googleapis.com/css2?family=Segoe+UI:wght@400;600&display=swap" rel="stylesheet" />
    <style>
        /* Base layout (matches Login page palette and glassmorphism) */
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

        /* Footer */
        .footer {
            text-align: center;
            padding: 12px;
            font-size: 0.85em;
            color: #333;
            border-top: 1px solid rgba(255,255,255,0.2);
            background: rgba(255,255,255,0.4);
            margin-top: 18px;
        }

        /* Layout helpers */
        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px 18px;
        }

        .full-row {
            grid-column: 1 / -1;
        }

        @media (max-width: 760px) {
            .form-grid { grid-template-columns: 1fr; }
            .actions { justify-content: center; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-div">
            <h1 class="title-heading">PharmaCo Admin: Product Manager</h1>

            <!-- Search / Delete / View -->
            <div class="card">
                <h2>Search and manage products</h2>
                <div class="form-grid">
                    <div class="full-row">
                        <asp:Label ID="LabelSearch" runat="server" Text="Product ID" CssClass="label" />
                        <asp:TextBox ID="TextBoxSearchID" runat="server" CssClass="textbox" placeholder="Enter Product ID" />
                    </div>
                    <div class="full-row actions">
                        <asp:Button ID="Searchbtn" runat="server" Text="Search" OnClick="Searchbtn_Click" CssClass="action-button" />
                        <asp:Button ID="Deletebtn" runat="server" Text="Delete" OnClick="Deletebtn_Click" CssClass="action-button" />
                        <asp:Button ID="Viewbtn" runat="server" Text="View All" OnClick="Viewbtn_Click" CssClass="action-button" />
                    </div>
                </div>
            </div>

            <!-- Add Product -->
            <div class="card">
                <h2>Add new product</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="LabelName" runat="server" Text="Product name" CssClass="label" />
                        <asp:TextBox ID="TextBoxName" runat="server" CssClass="textbox" placeholder="e.g., Paracetamol 500mg" />
                    </div>
                    <div>
                        <asp:Label ID="LabelCategory" runat="server" Text="Category" CssClass="label" />
                        <asp:TextBox ID="TextBoxCategory" runat="server" CssClass="textbox" placeholder="e.g., Analgesic" />
                    </div>
                    <div class="full-row">
                        <asp:Label ID="LabelDesc" runat="server" Text="Description" CssClass="label" />
                        <asp:TextBox ID="TextBoxDesc" runat="server" CssClass="textbox" placeholder="Short product description" />
                    </div>
                    <div>
                        <asp:Label ID="LabelPrice" runat="server" Text="Price" CssClass="label" />
                        <asp:TextBox ID="TextBoxPrice" runat="server" CssClass="textbox" TextMode="Number" placeholder="e.g., 250" />
                    </div>
                    <div class="actions full-row">
                        <asp:Button ID="AddProductBtn" runat="server" Text="Add product" OnClick="AddProductBtn_Click" CssClass="action-button" />
                    </div>
                </div>
            </div>

            <!-- Update Product -->
            <div class="card">
                <h2>Update existing product</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="LabelUpdateID" runat="server" Text="Product ID" CssClass="label" />
                        <asp:TextBox ID="TextBoxUpdateID" runat="server" CssClass="textbox" placeholder="Enter Product ID to update" />
                    </div>
                    <div>
                        <asp:Label ID="LabelUpdateName" runat="server" Text="Product name" CssClass="label" />
                        <asp:TextBox ID="TextBoxUpdateName" runat="server" CssClass="textbox" placeholder="Updated product name" />
                    </div>
                    <div class="full-row">
                        <asp:Label ID="LabelUpdateDesc" runat="server" Text="Description" CssClass="label" />
                        <asp:TextBox ID="TextBoxUpdateDesc" runat="server" CssClass="textbox" placeholder="Updated description" />
                    </div>
                    <div>
                        <asp:Label ID="LabelUpdatePrice" runat="server" Text="Price" CssClass="label" />
                        <asp:TextBox ID="TextBoxUpdatePrice" runat="server" CssClass="textbox" TextMode="Number" placeholder="e.g., 300" />
                    </div>
                    <div>
                        <asp:Label ID="LabelUpdateCategory" runat="server" Text="Category" CssClass="label" />
                        <asp:TextBox ID="TextBoxUpdateCategory" runat="server" CssClass="textbox" placeholder="e.g., Analgesic" />
                    </div>
                    <div class="actions full-row">
                        <asp:Button ID="UpdateProductBtn" runat="server" Text="Update product" OnClick="UpdateProductBtn_Click" CssClass="action-button" />
                    </div>
                </div>
            </div>

            <!-- Products Grid -->
            <div class="grid">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview-style">
                    <Columns>
                        <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                        <asp:BoundField DataField="ProductName" HeaderText="Product name" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="footer">
                © PharmaCo Ltd Secure Portal - 2025
            </div>
        </div>
    </form>
</body>
</html>