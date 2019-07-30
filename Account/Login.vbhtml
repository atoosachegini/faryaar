@* Remove this section if you are using bundling *@
@Section Scripts
    <script src="~/Scripts/jquery.validate.min.js"></script>
    <script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>
End Section

@Code
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Log in"

    ' Initialize general page variables
    Dim نام As String = ""
    Dim password As String = ""
    Dim rememberMe As Boolean = False

    Dim returnUrl As String = Request.QueryString("ReturnUrl")
    If returnUrl.IsEmpty() Then
        ' Some external login providers always require a return URL value
        returnUrl = Href("~/")
    End If

    ' Setup validation
    Validation.RequireField("نام", "You must specify an نام address.")
    Validation.RequireField("password", "You must specify a password.")
    Validation.Add("password",
        Validator.StringLength(
            maxLength:=Int32.MaxValue,
            minLength:=6,
            errorMessage:="Password must be at least 6 characters"))

    ' If this is a POST request, validate and process data
    If IsPost Then
        AntiForgery.Validate()
        ' is this an external login request?
        Dim provider As String = Request.Form("provider")
        If Not Provider.IsEmpty() Then
            OAuthWebSecurity.RequestAuthentication(Provider, Href("~/Account/RegisterService", New With { .ReturnUrl = returnUrl }))
            Return
        ElseIf Validation.IsValid() Then
            نام = Request.Form("نام")
            password = Request.Form("password")
            rememberMe = Request.Form("rememberMe").AsBool()

            If WebSecurity.UserExists(نام) AndAlso WebSecurity.GetPasswordFailuresSinceLastSuccess(نام) > 4 AndAlso WebSecurity.GetLastPasswordFailureDate(نام).AddSeconds(60) > DateTime.UtcNow Then
                Response.Redirect("~/Account/AccountLockedOut")
                Return
            End If

            ' Attempt to log in using provided credentials
            If WebSecurity.Login(نام, password, rememberMe) Then
                Context.RedirectLocal(returnUrl)
                Return
            Else
                ModelState.AddFormError("The user name or password provided is incorrect.")
            End If
        End If
    End If
End Code

<hgroup class="title">
    <h1>@PageData("Title").</h1>
</hgroup>

<section id="loginForm">
    <h2>Use a local account to log in.</h2>
    <form method="post">
        @AntiForgery.GetHtml()
        @* If one or more validation errors exist, show an error *@
        @Html.ValidationSummary("Log in was unsuccessful. Please correct the errors and try again.", excludeFieldErrors:=True, htmlAttributes:=Nothing)

        <fieldset>
            <legend>Log in to Your Account</legend>
            <ol>
                <li class="نام">
                    <label for="نام" @If Not ModelState.IsValidField("نام") Then@<text>class="error-label"</text> End If>نام address</label>
                    <input type="text" id="نام" name="نام" value="@نام" @Validation.For("نام")/>
                    @* Write any user name validation errors to the page *@
                    @Html.ValidationMessage("نام")
                </li>
                <li class="password">
                    <label for="password" @If Not ModelState.IsValidField("password") Then@<text>class="error-label"</text> End If>Password</label>
                    <input type="password" id="password" name="password" @Validation.For("password")/>
                    @* Write any password validation errors to the page *@
                    @Html.ValidationMessage("password")
                </li>
                <li class="remember-me">
                    <input type="checkbox" id="rememberMe" name="rememberMe" value="true" checked="@rememberMe" />
                    <label class="checkbox" for="rememberMe">Remember me?</label>
                </li>
            </ol>
            <input type="submit" value="Log in" />
        </fieldset>
    </form>
    <p>
        <a href="~/Account/Register">Don't have a Account?</a>
        <a href="~/Account/ForgotPassword">Did you forget your password?</a>
    </p>
</section>

<section class="social" id="socialLoginForm">
    <h2>Use another service to log in.</h2>
     @RenderPage("~/Account/_ExternalLoginsList.vbhtml")
</section>
