@model MyPersonalSite.UI.MVC.Models.ContactViewModel

@{
    ViewBag.Title = "EmailConfirmation";
}

<h2>EmailConfirmation</h2>

<h4>We recieved the following message:</h4>
<p>@Model.Message</p>
<h4>We will respond back as soon as possible to @Model.Email.Thank you for contacting us!</h4>