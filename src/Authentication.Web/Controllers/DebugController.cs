using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers;
public class DebugController : Controller
{
    [Authorize]
    public IActionResult Claims()
    {
        var claimsInfo = new
        {
            Name = User.Identity?.Name,
            Metadata = User.FindFirst("urn:zitadel:iam:user:metadata")?.Value,
            Claims = User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            })
        };

        return Json(claimsInfo);
    }
}