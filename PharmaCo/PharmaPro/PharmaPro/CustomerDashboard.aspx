<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDashboard.aspx.cs" Inherits="PharmaPro.WebForm3" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CustomerDashboard</title>
    <link href="CustomerDashboard_stylesheet.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main">
            <div class="header">
                <div class="container">
                    <h1 class="title-heading">PharmaCo</h1>
                </div>
            </div>
            <div class="filter">
                <h2 class="portal-heading">Customer Portal, Welcome to our Pharmacy!&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button1" runat="server" Text="Logout" OnClick="Button1_Click" />
                </h2>
            </div>
            <div class="slider">
                <div class="slides">
                    <div class="slide"><img src="https://www.dvago.pk/_next/image?url=https%3A%2F%2Fdvago-assets.s3.ap-southeast-1.amazonaws.com%2FWeb%2520Banner%2Fhaleon-offer-web.jpg&w=1600&q=50" alt="Image 1"></div>
                    <div class="slide"><img src="https://www.dvago.pk/_next/image?url=https%3A%2F%2Fdvago-assets.s3.ap-southeast-1.amazonaws.com%2FWeb%2520Banner%2Fwholly-web-banner.jpg&w=1600&q=50" alt="Image 2"></div>
                    <div class="slide"><img src="https://www.dvago.pk/_next/image?url=https%3A%2F%2Fdvago-assets.s3.ap-southeast-1.amazonaws.com%2FWeb%2520Banner%2FNido-web.jpg&w=1600&q=50" alt="Image 3"></div>
                    <div class="slide"><img src="https://www.dvago.pk/_next/image?url=https%3A%2F%2Fdvago-assets.s3.ap-southeast-1.amazonaws.com%2FWeb%2520Banner%2Froute-web.jpg&w=1600&q=50" alt="Image 4"></div>
                </div>
                <button type="button" class="prev" onclick="prevSlide()">❮</button>
                <button type="button" class="next" onclick="nextSlide()">❯</button>
            </div>

            <div class="long-wrapper">
                <h4 class="browse-txt">Browse Now!</h4>
            </div>

            <div class="browse-wrapper">
                <div class="element">
                    <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZRGVpoEQTHNIkJxvqlJmvO6Ffs6fVQi_EaFjkuYpApA&s" class="element-img" />
                    <center><h4 class="element-txt">Search Medicine</h4></center>
                    <center><asp:Button ID="Elementbtn1" runat="server" Text="View" OnClick="Elementbtn1_Click" /></center>
                </div>

            </div>
        </div>

<%--Footer Open--%>
<div class="footer">
    <div class="footercont1">
        <div>
            <h1 class="footer-heading">PharmaCo ltd®</h1>
        </div>
        <div class="sociallinks">
            <div class="followtext"><p class="followp">Follow us on:</p></div>
            <div class="followlist">
                <img class="socialimg" src="Fb logo.png" alt="">
                <img class="socialimg" src="Twitter-Logo.png" alt="">
                <img class="socialimg" src="Insta Logo.png" alt="">
                <img class="socialimg" src="in logo.png" alt="">
                <img class="socialimg" src="Yt logo.png" alt="">
            </div>
        </div>
    </div>
    <div class="footercont2">
        <div class="footli1">
            <ul class="footerli1">
                <li class="fli">WHY PharmaCo</li>
                <li class="fli">About Us</li>
                <li class="fli">Founder's Message</li>
            </ul>
        </div>
        <div class="footli2">
            <ul class="footerli1">
                <li class="fli">HOSPITALITY</li>
                <li class="fli">Our Stores</li>
                <li class="fli">Terms and Conditions</li>
            </ul>
        </div>
        <div class="footli3">
            <ul class="footerli1">
                <li class="fli">PharmaCo Assist</li>
                <li class="fli">For Assistance</li>
                <li class="fli">FAQ</li>
            </ul>
        </div>
        <div class="footli4">
            <ul class="footerli1">
                <li class="fli">Media Center</li>
                <li class="fli">Video Gallery</li>
                <li class="fli">News</li>
            </ul>
        </div>
    </div>
</div>

        <%--Footer close--%>

    </form>

    <script>
        let slideIndex = 0;

        function showSlides() {
            const slides = document.querySelectorAll('.slide');
            slides.forEach((slide, index) => {
                slide.style.display = index === slideIndex ? 'block' : 'none';
            });
        }

        function nextSlide() {
            const slides = document.querySelectorAll('.slide');
            slideIndex = (slideIndex + 1) % slides.length;
            showSlides();
        }

        function prevSlide() {
            const slides = document.querySelectorAll('.slide');
            slideIndex = (slideIndex - 1 + slides.length) % slides.length;
            showSlides();
        }

        document.addEventListener('DOMContentLoaded', () => {
            showSlides();
            setInterval(nextSlide, 2000);
        });
    </script>
</body>
</html>