<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="PharmaPro.WebForm4" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmaCo Ltd - User Manager</title>
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

        /* Inputs */
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

        /* Actions */
        .actions {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
            align-items: center;
            justify-content: center;
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

        /* Grid */
        .grid { margin-top: 10px; }
        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            background: rgba(255,255,255,0.65);
            border-radius: 8px;
            overflow: hidden;
        }
        .gridview-style th, .gridview-style td {
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
        .gridview-style tr:nth-child(even) { background: #f0f9ff; }
        .gridview-style tr:hover { background: #d9f0ff; }

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-div">
            <h1 class="title-heading">PharmaCo Admin: Manage Users</h1>

            <!-- Search / Delete / View -->
            <div class="card">
                <h2>Search / Delete / View Users</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="Label1" runat="server" Text="User ID" CssClass="label" />
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox" placeholder="Enter User ID" />
                    </div>
                    <div>
                        <asp:Label ID="Label2" runat="server" Text="Username" CssClass="label" />
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox" placeholder="Enter Username" />
                    </div>
                    <div class="actions full-row">
                        <asp:Button ID="Searchbtn" runat="server" Text="Search" CssClass="action-button" OnClick="Searchbtn_Click" />
                        <asp:Button ID="Deletebtn" runat="server" Text="Delete" CssClass="action-button" OnClick="Deletebtn_Click" />
                        <asp:Button ID="Viewbtn" runat="server" Text="View All" CssClass="action-button" OnClick="Viewbtn_Click" />
                    </div>
                </div>
            </div>

            <!-- Add / Update User -->
            <div class="card">
                <h2>Add or update user</h2>
                <div class="form-grid">
                    <div>
                        <asp:Label ID="Label3" runat="server" Text="Username" CssClass="label" />
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="textbox" />
                    </div>
                    <div>
                        <asp:Label ID="Label4" runat="server" Text="Password" CssClass="label" />
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="textbox" TextMode="Password" />
                    </div>
                    <div>
                        <asp:Label ID="Label5" runat="server" Text="Email" CssClass="label" />
                        <asp:TextBox ID="TextBox5" runat="server" CssClass="textbox" TextMode="Email" />
                    </div>
                    <div>
                        <asp:Label ID="Label6" runat="server" Text="Full name" CssClass="label" />
                        <asp:TextBox ID="TextBox7" runat="server" CssClass="textbox" />
                    </div>
                    <div>
                        <asp:Label ID="Label7" runat="server" Text="Address" CssClass="label" />
                        <asp:TextBox ID="TextBox6" runat="server" CssClass="textbox" />
                    </div>
                    <div>
                        <asp:Label ID="Label8" runat="server" Text="Phone number" CssClass="label" />
                        <asp:TextBox ID="TextBox8" runat="server" CssClass="textbox" />
                    </div>
                    <div class="full-row">
                        <asp:Label ID="Label9" runat="server" Text="User type" CssClass="label" />
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="Select role" Value="" />
                            <asp:ListItem Text="Customer" Value="Customer" />
                            <asp:ListItem Text="Admin" Value="Admin" />
                        </asp:DropDownList>
                    </div>

                    <div class="actions full-row">
                        <asp:Button ID="Addbtn" runat="server" Text="Add user" CssClass="action-button" OnClick="Addbtn_Click" OnClientClick="return validateUserForm();" />
                        <asp:Button ID="Updatebtn" runat="server" Text="Update user" CssClass="action-button" OnClick="Updatebtn_Click" OnClientClick="return validateUpdateForm();" />
                    </div>
                </div>
            </div>

            <!-- Users Grid -->
            <div class="grid">
                <asp:GridView ID="GridView1" runat="server" CssClass="gridview-style" AutoGenerateColumns="False" DataKeyNames="UserID" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="PasswordHash" HeaderText="Password (hash)" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="FullName" HeaderText="Full name" />
                        <asp:BoundField DataField="Address" HeaderText="Address" />
                        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" />
                        <asp:BoundField DataField="UserType" HeaderText="User type" />
                        <asp:CommandField ShowSelectButton="True" SelectText="Select" />
                    </Columns>
                </asp:GridView>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>

            <div class="footer">
                PharmaCo Ltd® 2025 — all rights reserved.
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function validateUserForm() {
            var u = document.getElementById("<%= TextBox3.ClientID %>").value.trim();
            var p = document.getElementById("<%= TextBox4.ClientID %>").value.trim();
            var e = document.getElementById("<%= TextBox5.ClientID %>").value.trim();
            var n = document.getElementById("<%= TextBox7.ClientID %>").value.trim();
            var a = document.getElementById("<%= TextBox6.ClientID %>").value.trim();
            var ph = document.getElementById("<%= TextBox8.ClientID %>").value.trim();
            var t = document.getElementById("<%= DropDownList1.ClientID %>").value.trim();

            if (!u || !p || !e || !n || !a || !ph || !t) {
                alert("Please fill in all user details, including user type.");
                return false;
            }
            return true;
        }

        function validateUpdateForm() {
            var id = document.getElementById("<%= TextBox1.ClientID %>").value.trim();
            if (!id) {
                alert("Select or enter a valid User ID to update.");
                return false;
            }
            return validateUserForm();
        }
    </script>
</body>
</html>