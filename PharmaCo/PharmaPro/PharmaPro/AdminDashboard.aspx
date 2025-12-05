<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PharmaPro.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdminDashboard</title>
    <link href="AdminDashboard_stylesheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
       <div class="Header" style="display: flex; justify-content: space-between; align-items: center; padding: 10px; background-color: #4CAF50;">
    <a href="LogIn.aspx" style="text-decoration: none;">
        <h2 class="head1" style="color: white; margin: 0;">Pharmaco Admin Dashboard</h2>
    </a>
    <asp:Button ID="Button5" runat="server" Text="Logout" OnClick="Button5_Click" 
                style="background-color: white; color: #4CAF50; border: none; padding: 10px 20px; cursor: pointer; border-radius: 5px;" />
</div>


        <div class="main">
            <div class="wrapper1">
                <div class="col">
                    <h1>Manage User</h1>
                    <center><asp:Button ID="Button1" runat="server" Text="Manage" OnClick="Button1_Click" /></center>
                </div>

            </div>

            <div class="wrapper2">
                <div class="col">
                    <h1>Handle Inventory</h1>
                    <center><asp:Button ID="Button2" runat="server" Text="Manage" OnClick="Button2_Click" /></center>
                </div>

            </div>

            <div class="wrapper3">
                <div class="col">
                    <h1>Products</h1>
                    <center><asp:Button ID="Button3" runat="server" Text="View" OnClick="Button3_Click" /></center>
                </div>

            </div>

            <div class="wrapper4">
                <div class="col">
                    <h1>Order History</h1>
                    <center><asp:Button ID="Button4" runat="server" Text="View" OnClick="Button4_Click1" /></center>
                </div>

            </div>

        </div>
    </form>
</body>
</html>