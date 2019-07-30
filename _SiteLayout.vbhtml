<!DOCTYPE html>
<html lang="en">
    <head>
        <title>FaYaar-edu-Entekhab-Reshte</title>
        <meta charset="utf-8" />
        <title>@PageData("Title") - My ASP.NET Web Page</title>
        <link href="~/Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
        <link href="~/Content/Site.css" rel="stylesheet" type="text/css" />
        <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
        <script src="~/Scripts/jquery-1.10.2.min.js"></script>
        <script src="~/Scripts/jquery-ui-1.10.3.js"></script>
        <script src="~/Scripts/modernizr-2.6.2.js"></script>
        <meta name="viewport" content="width=device-width" />
    </head>
    <body>
        <header>
            <div class="content-wrapper">
                <div class="float-left">
                    <center> <p class="site-title"><a href="~/"><img src="~/Content/themes/base/IMG-20190626-WA0000.jpg" width="200px" height="120px" /></a><h3>دپارتمان مشاوره و برنامه ریزی فریار</h3></p></center>
                </div>
                <div class="float-right">
                    <section id="login">
                        @If WebSecurity.IsAuthenticated Then
                            @<text>
                                Hello, <a class="نام" href="~/Account/Manage" title="Manage">@WebSecurity.CurrentUserName</a>!
                                <form id="logoutForm" action="~/Account/Logout" method="post">
                                    @AntiForgery.GetHtml()
                                    <a href="javascript:document.getElementById('logoutForm').submit()">Log out</a>
                                </form>
                            </text>

                        End If
                    </section>
                    <nav>
                        <ul id="menu" style="float : right">
                            <li><a href="~/" >خانه</a></li>
                            <li><a href="~/About.vbhtml">درباره ی ما</a></li>
                            <li><a href="~/Contact.vbhtml">ارتباط با ما </a></li>
                        </ul>
                    </nav>
                </div>
            </div>
        </header>
        <div id="body">
            @RenderSection("featured", required:=False)
            <section class="content-wrapper main-content clear-fix">
                @RenderBody()
            </section>
        </div>
        <footer>
            <div class="content-wrapper">
             <br />
                <br />
                <center>
                    <pre style="font-size : 30px ; color:Highlight ; font-family:'B Baran'">
       بر شانه هایت دنبال چه میگردی ؟&#9925
 &#9925انگیزه ی پرواز باید در دلت باشد 
</pre>
                </center>
                <div class="float-left ">
                    <p>&copy; @DateTime.Now.Year - My ASP.NET Web Page developed by Atoosa Chegini</p>
                </div>
            </div>
        </footer>

        @RenderSection("Scripts", required:=False)
    </body>
</html>