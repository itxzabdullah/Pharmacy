<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="PharmaPro.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PharmaCo Ltd - Login</title>
    <link href="https://fonts.googleapis.com/css2?family=Segoe+UI:wght@400;600&display=swap" rel="stylesheet" />
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', sans-serif;
            background: linear-gradient(135deg, #e0f7fa, #d0f0ec, #f0fcff);
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .MainDiv {
            width: 420px;
            background: rgba(255, 255, 255, 0.25);
            border-radius: 12px;
            box-shadow: 0 8px 32px rgba(0,0,0,0.2);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255,255,255,0.3);
        }

        .Header {
            padding: 20px;
            text-align: center;
            border-bottom: 1px solid rgba(255,255,255,0.2);
        }

        .head1 {
            color: #003c58;
            margin: 0;
            font-size: 1.6em;
            font-weight: 600;
        }

        .HeaderSecond {
            padding: 12px;
            text-align: center;
        }

        .head2 {
            color: #005f73;
            margin: 0;
            font-size: 1.2em;
            font-weight: 500;
        }

        .form {
            padding: 24px;
        }

        .row1, .row2, .row3 {
            margin-bottom: 18px;
            display: flex;
            flex-direction: column;
        }

        .label {
            font-weight: 500;
            margin-bottom: 6px;
            color: #1a1a1a;
        }

        .textbox, #DropDownList1 {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 0.95em;
            background: rgba(255,255,255,0.6);
            color: #1a1a1a;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.05);
        }

        .textbox::placeholder {
            color: #666;
        }

        .textbox:focus, #DropDownList1:focus {
            background: rgba(255,255,255,0.8);
            border-color: #0077b6;
            box-shadow: 0 0 6px rgba(0,119,182,0.3);
            outline: none;
        }

        #Signinbtn {
            width: 100%;
            padding: 12px;
            background: linear-gradient(90deg, #0077b6, #00b4d8);
            border: none;
            border-radius: 8px;
            color: #fff;
            font-size: 1em;
            font-weight: 600;
            cursor: pointer;
            transition: 0.3s;
        }

        #Signinbtn:hover {
            background: linear-gradient(90deg, #005f8a, #0096c7);
        }

        .footer {
            text-align: center;
            padding: 12px;
            font-size: 0.85em;
            color: #333;
            border-top: 1px solid rgba(255,255,255,0.2);
            background: rgba(255,255,255,0.4);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="MainDiv">
            <div class="Header">
                <h2 class="head1">PharmaCo Ltd</h2>
            </div>
            <div class="HeaderSecond">
                <h2 class="head2">User Sign In</h2>
            </div>

            <div class="form">
                <div class="row1">
                    <asp:Label ID="Label1" runat="server" Text="Username" CssClass="label"></asp:Label>
                    <asp:TextBox ID="Username_txtbox" runat="server" placeholder="Enter your username" CssClass="textbox" required="true"></asp:TextBox>
                </div>
                <div class="row2">
                    <asp:Label ID="Label2" runat="server" Text="Password" CssClass="label"></asp:Label>
                    <asp:TextBox ID="Password_txtbox" runat="server" placeholder="Enter your password" TextMode="Password" CssClass="textbox" required="true"></asp:TextBox>
                </div>
                <div class="row3">
                    <asp:Label ID="Label3" runat="server" Text="Role" CssClass="label"></asp:Label>
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="textbox" required="true">
                        <asp:ListItem Text="Select Role" Value="" Disabled="true" Selected="true"></asp:ListItem>
                        <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                        <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Button ID="Signinbtn" runat="server" Text="Sign In" OnClick="Signinbtn_Click" />
            </div>

            <div class="footer">
                © PharmaCo Ltd Secure Portal - 2025
            </div>
        </div>
    </form>
</body>
<script>
    window.onload = function () {
        var selectOption = document.querySelector('#DropDownList1 option[value=""]');
        if (selectOption) {
            selectOption.disabled = true;
        }
    };
</script>
</html>
