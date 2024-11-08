using Authentication.Web.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers;

public class AccountController : Controller
{
    /// <summary>
    /// Returns the page that allows a user to enter their name as part of the redirector flow.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Challenge the user to authenticate with the configured IdP
    /// </summary>
    /// <param name="userViewModel">The user details</param>
    /// <returns>An <see cref="IActionResult"/></returns>
    [HttpGet]
    public IActionResult ChallengeUser(UserViewModel userViewModel)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/"
        };

        return Challenge(properties);
    }

    /// <summary>
    /// This is method called from the home page when Log in is clicked by the user. This will direct the user directly to zitadel to login.
    /// </summary>
    /// <param name="returnUrl">An optional param to redirect the user to upon authentication</param>
    /// <returns>An <see cref="IActionResult"/></returns>
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return Challenge(props);
    }

    /// <summary>
    /// This method is called when the logout button is pressed after the user has logged in from the home screen.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/></returns>
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
