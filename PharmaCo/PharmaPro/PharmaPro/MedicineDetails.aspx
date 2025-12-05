<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MedicineDetails.aspx.cs" Inherits="PharmaPro.MedicineDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medicine Details</title>
    <link href="CustomerDashboard_stylesheet.css" rel="stylesheet" />
    <style>
        body, html {
            margin: 0;
            padding: 0;
            background: linear-gradient(135deg, #e0f7fa, #d0f0ec, #f0fcff); /* unified background */
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
            min-height: 100vh;
        }

        /* Header with frosted effect */
        .header {
            background: rgba(76, 175, 80, 0.25);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            padding: 18px 0;
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.35);
            text-align: center;
            border-bottom: 1px solid rgba(255,255,255,0.3);
        }

        .title-heading {
            color: #2e7d32;
            font-size: 2.4rem;
            font-weight: 700;
            letter-spacing: 1.2px;
            margin: 0;
        }

        /* Container with frosted glass */
        .details-container {
            max-width: 700px;
            background: rgba(255,255,255,0.25);
            margin: 50px auto 70px;
            padding: 40px 35px 50px;
            border-radius: 16px;
            box-shadow: 0 15px 40px rgba(0,0,0,0.08);
            text-align: center;
            transition: box-shadow 0.3s ease;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border: 1px solid rgba(255,255,255,0.3);
        }

        .details-container:hover {
            box-shadow: 0 22px 60px rgba(0,0,0,0.14);
        }

        /* Product Name */
        .medicine-name {
            font-size: 2.2rem !important;
            font-weight: 700;
            color: #2e7d32;
            margin-bottom: 25px;
        }

        /* Description */
        .details-container p {
            color: #555;
            font-size: 1.1rem;
            line-height: 1.6;
            margin-bottom: 20px;
        }

        /* Price styling */
        .details-container strong {
            color: #4caf50;
            font-size: 1.3rem;
            font-weight: 700;
            display: block;
            margin-bottom: 30px;
        }

        /* Buy Now button with gradient */
        .buy-now-btn {
            background: linear-gradient(135deg, #4caf50, #66bb6a);
            color: white;
            font-weight: 700;
            font-size: 1.2rem;
            padding: 14px 40px;
            border: none;
            border-radius: 40px;
            cursor: pointer;
            box-shadow: 0 10px 25px rgba(76, 175, 80, 0.5);
            transition: background 0.4s ease, box-shadow 0.4s ease, transform 0.2s ease;
        }

        .buy-now-btn:hover,
        .buy-now-btn:focus {
            background: linear-gradient(135deg, #388e3c, #1b5e20);
            box-shadow: 0 14px 38px rgba(27, 94, 32, 0.7);
            transform: translateY(-3px);
            outline: none;
        }

        /* Error message */
        .error-message {
            color: #d32f2f;
            font-weight: 700;
            margin-top: 15px;
            font-size: 1rem;
        }

        /* Responsive adjustments */
        @media (max-width: 480px) {
            .details-container {
                margin: 30px 20px 60px;
                padding: 30px 20px 40px;
            }

            .medicine-name {
                font-size: 1.6rem !important;
                margin-bottom: 20px;
            }

            .details-container p {
                font-size: 0.95rem;
                margin-bottom: 15px;
            }

            .details-container strong {
                font-size: 1.1rem;
                margin-bottom: 25px;
            }

            .buy-now-btn {
                width: 100%;
                padding: 14px 0;
                font-size: 1.1rem;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="container">
                <h1 class="title-heading">PharmaCo Medicine Details</h1>
            </div>
        </div>

        <div class="details-container">
            <asp:Label ID="lblProductName" runat="server" Text="Loading Product..." CssClass="medicine-name"></asp:Label>
            
            <p><asp:Label ID="lblDescription" runat="server" Text="Loading Description..."></asp:Label></p>
            
            <p><strong>Price: $<asp:Label ID="lblPrice" runat="server" Text="0.00"></asp:Label></strong></p>
            
            <asp:Button ID="btnBuyNowDetails" runat="server" Text="Buy Now" CssClass="buy-now-btn" OnClick="btnBuyNowDetails_Click" />

            <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" EnableViewState="false"></asp:Label>
        </div>
    </form>
</body>
</html>