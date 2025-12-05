<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchMedicine.aspx.cs" Inherits="PharmaPro.SearchMedicine" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medicine Search</title>
    <link href="CustomerDashboard_stylesheet.css" rel="stylesheet" />
    <style>
        body, html {
            margin: 0;
            padding: 0;
            background: linear-gradient(135deg, #e0f7fa, #d0f0ec, #f0fcff);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
            min-height: 100vh;
        }

        /* Header with frosted effect */
        .header {
            background: rgba(76, 175, 80, 0.25);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            padding: 15px 0;
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.3);
            text-align: center;
            border-bottom: 1px solid rgba(255,255,255,0.3);
        }

        .title-heading {
            color: #2e7d32;
            font-size: 2.2rem;
            font-weight: 700;
            letter-spacing: 1.2px;
            margin: 0;
            user-select: none;
        }

        /* Search container */
        .search-container {
            margin: 30px auto 40px;
            text-align: center;
            max-width: 600px;
            padding: 20px;
            background: rgba(255,255,255,0.25);
            border-radius: 12px;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border: 1px solid rgba(255,255,255,0.3);
            box-shadow: 0 8px 24px rgba(0,0,0,0.1);
        }

        .search-bar {
            width: 70%;
            max-width: 500px;
            padding: 14px 18px;
            font-size: 1.1rem;
            border: 2px solid #4caf50;
            border-radius: 40px 0 0 40px;
            box-sizing: border-box;
            transition: border-color 0.3s ease;
            outline: none;
            background: rgba(255,255,255,0.6);
        }

        .search-bar:focus {
            border-color: #388e3c;
            box-shadow: 0 0 8px #66bb6a;
            background: rgba(255,255,255,0.8);
        }

        .search-btn {
            background: linear-gradient(90deg, #4caf50, #66bb6a);
            color: white;
            padding: 14px 28px;
            font-size: 1.05rem;
            border: none;
            border-radius: 0 40px 40px 0;
            cursor: pointer;
            font-weight: 600;
            box-shadow: 0 5px 15px rgba(76, 175, 80, 0.4);
            transition: background 0.3s ease, box-shadow 0.3s ease, transform 0.2s ease;
        }

        .search-btn:hover, .search-btn:focus {
            background: linear-gradient(90deg, #388e3c, #2e7d32);
            box-shadow: 0 8px 20px rgba(56, 142, 60, 0.6);
            transform: translateY(-2px);
            outline: none;
        }

        /* Medicine grid container */
        .medicine-grid {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 30px;
            max-width: 1200px;
            margin: 0 auto 60px;
            padding: 0 15px;
        }

        /* Each medicine card with frosted glass */
        .medicine-item {
            flex: 1 1 280px;
            max-width: 320px;
            background: rgba(255,255,255,0.25);
            border-radius: 16px;
            box-shadow: 0 10px 20px rgba(0,0,0,0.1);
            padding: 25px 20px 30px;
            display: flex;
            flex-direction: column;
            align-items: center;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border: 1px solid rgba(255,255,255,0.3);
        }

        .medicine-item:hover {
            transform: translateY(-8px) scale(1.05);
            box-shadow: 0 20px 30px rgba(0,0,0,0.15);
        }

        .medicine-name {
            font-size: 1.5rem;
            font-weight: 700;
            color: #2e7d32;
            margin-bottom: 12px;
            text-align: center;
        }

        .medicine-item p {
            color: #555;
            font-size: 1rem;
            line-height: 1.4;
            margin-bottom: 16px;
            text-align: center;
        }

        .medicine-item strong {
            color: #4caf50;
            font-size: 1.25rem;
            margin-bottom: 20px;
        }

        .medicine-item .search-btn {
            width: 140px;
            padding: 10px 0;
            font-size: 1rem;
            border-radius: 30px;
            background: linear-gradient(135deg, #66bb6a, #388e3c);
            box-shadow: 0 6px 15px rgba(56, 142, 60, 0.5);
        }

        .medicine-item .search-btn:hover, 
        .medicine-item .search-btn:focus {
            background: linear-gradient(135deg, #388e3c, #1b5e20);
            box-shadow: 0 10px 20px rgba(27, 94, 32, 0.7);
            transform: translateY(-3px);
        }

        @media (max-width: 768px) {
            .search-bar {
                width: 100%;
                border-radius: 40px;
                margin-bottom: 12px;
            }
            .search-btn {
                width: 100%;
                border-radius: 40px;
                margin-left: 0;
            }
            .search-container {
                display: flex;
                flex-direction: column;
                align-items: center;
            }
            .medicine-item {
                max-width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="container">
                <h1 class="title-heading">PharmaCo Search Medicine</h1>
            </div>
        </div>

        <div class="search-container">
            <asp:TextBox ID="searchBox" runat="server" CssClass="search-bar" placeholder="Search for medicine..."></asp:TextBox>
            <asp:Button ID="searchButton" runat="server" Text="Search" CssClass="search-btn" OnClick="searchButton_Click" />
        </div>

        <div class="medicine-grid">
            <asp:Repeater ID="medicineRepeater" runat="server" OnItemCommand="medicineRepeater_ItemCommand">
                <ItemTemplate>
                    <div class="medicine-item">
                        <h4 class="medicine-name"><%# Eval("ProductName") %></h4>
                        <p><%# Eval("Description") %></p>
                        <p><strong>Price: $<%# Eval("Price") %></strong></p>
                        <center>
                            <asp:Button runat="server" Text="View Details" CssClass="search-btn"
                                CommandName="ViewDetails" CommandArgument='<%# Eval("ProductID") %>' />
                        </center>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>