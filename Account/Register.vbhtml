@* Remove this section if you are using bundling *@
@Section Scripts
    <script src="~/Scripts/jquery.validate.min.js"></script>
    <script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>
End Section

@Code
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Register"

    ' Initialize general page variables
    Dim نام As String = ""
    Dim password As String = ""
    Dim confirmPassword As String = ""

    ' Setup validation
    Validation.RequireField("نام", "The نام address field is required.")
    Validation.RequireField("password", "Password cannot be blank.")
    Validation.Add("confirmPassword",
        Validator.EqualsTo("password", "Password and confirmation password do not match."))
    Validation.Add("password",
        Validator.StringLength(
            maxLength:=Int32.MaxValue,
            minLength:=6,
            errorMessage:="Password must be at least 6 characters"))

    ' If this is a POST request, validate and process data
    If IsPost Then
        AntiForgery.Validate()
        نام = Request.Form("نام")
        password = Request.Form("password")
        confirmPassword = Request.Form("confirmPassword")

        ' Validate the user's captcha answer
        ' If Not ReCaptcha.Validate("PRIVATE_KEY")) Then
        '     ModelState.AddError("recaptcha", "Captcha response was not correct")
        ' End If

        ' If all information is valid, create a new account
        If Validation.IsValid() Then
            ' Insert a new user into the database
            Dim db As Database = Database.Open("StarterSite")

            ' Check if user already exists
            Dim user As Object = db.QuerySingle("SELECT نام FROM UserProfile WHERE LOWER(نام) = LOWER(@0)", نام)
            If user Is Nothing Then
                ' Insert نام into the profile table
                db.Execute("INSERT INTO UserProfile (نام) VALUES (@0)", نام)

                ' Create and associate a new entry in the membership database.
                ' If successful, continue processing the request
                Try
                    Dim requireنامConfirmation As Boolean = Not WebMail.SmtpServer.IsEmpty()
                    Dim token As String = WebSecurity.CreateAccount(نام, password, requireنامConfirmation)
                    If requireنامConfirmation Then
                        Dim hostUrl As String = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
                        Dim confirmationUrl As String = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/Confirm?confirmationCode=" + HttpUtility.UrlEncode(token))

                        WebMail.Send(
                            to:=نام,
                            subject:="Please confirm your account",
                            body:="Your confirmation code is: " + token + ". Visit <a href=""" + confirmationUrl + """>" + confirmationUrl + "</a> to activate your account."
                        )
                    End If

                    If requireنامConfirmation Then
                        ' Thank the user for registering and let them know an نام is on its way
                        Response.Redirect("~/Account/Thanks")
                    Else
                        ' Navigate back to the homepage and exit
                        WebSecurity.Login(نام, password)

                        Response.Redirect("~/")
                    End If
                Catch e As System.Web.Security.MembershipCreateUserException
                    ModelState.AddFormError(e.Message)
                End Try
            Else
                ' User already exists
                ModelState.AddFormError("نام address is already in use.")
            End If
        End If
    End If
End Code

<hgroup class="title">
    <h1>@PageData("Title").</h1>
    <h2>Create a new account.</h2>
</hgroup>

<form method="post">
    @AntiForgery.GetHtml()
    @* If at least one validation error exists, notify the user *@
    @Html.ValidationSummary("Account creation was unsuccessful. Please correct the errors and try again.", excludeFieldErrors:=True, htmlAttributes:=Nothing)

    <fieldset>
        <legend>Registration Form</legend>
        <ol>
            <li class="نام">
                <label for="نام" @If Not ModelState.IsValidField("نام") Then@<text>class="error-label"</text> End If>نام address</label>
                <input type="text" id="نام" name="نام" value="@نام" @Validation.For("نام") />
                @* Write any نام validation errors to the page *@
                @Html.ValidationMessage("نام")
            </li>
            <li class="password">
                <label for="password" @If Not ModelState.IsValidField("password") Then@<text>class="error-label"</text> End If>Password</label>
                <input type="password" id="password" name="password" @Validation.For("password") />
                @* Write any password validation errors to the page *@
                @Html.ValidationMessage("password")
            </li>
            <li class="confirm-password">
                <label for="confirmPassword" @If Not ModelState.IsValidField("confirmPassword") Then@<text>class="error-label"</text> End If>Confirm password</label>
                <input type="password" id="confirmPassword" name="confirmPassword" @Validation.For("confirmPassword") />
                @* Write any password validation errors to the page *@
                @Html.ValidationMessage("confirmPassword")
            </li>
            <li class="recaptcha">
                <div class="message-info">
                    <p>
                        To enable CAPTCHA verification, <a href="http://go.microsoft.com/fwlink/?LinkId=204140">install the 
                        ASP.NET Web Helpers Library</a> and uncomment ReCaptcha.GetHtml and replace 'PUBLIC_KEY'
                        with your public key. At the top of this page, uncomment ReCaptcha. Validate and
                        replace 'PRIVATE_KEY' with your private key.
                        Register for reCAPTCHA keys at <a href="http://recaptcha.net">reCAPTCHA.net</a>.
                    </p>
                </div>
                @*
                @ReCaptcha.GetHtml("PUBLIC_KEY", theme:="white")
                @Html.ValidationMessage("recaptcha")
                *@
            </li>
        </ol>
        <input type="submit" value="Register" />
    </fieldset>
</form>