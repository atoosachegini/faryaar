@Code
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "سلام ! به سایت انتخاب رشته تیم فریار خوش امدید "
End Code

@Section featured
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <center> <h1 style="color : black ">&#9728 @PageData("Title")</h1></center>
            </hgroup>
            <center>
                <p style="color : black ; font-size:20px">
                    &#9940در صورتی که مایل به انتخاب رشته  هستید لطفا به سوالات فرم زیر <ins>با دقت</ins> پاسخ دهید &#9940;
                </p>

            </center>
        </div>
    </section>
End Section

<ol class="round">
    <center><li><a href="~/Account/Register" style="color:Highlight; font-size:30px; ">&#9800برای پر کردن فرم انتخاب رشته اینجا کلیک کنید &#9800 </a></li></center>
</ol>


