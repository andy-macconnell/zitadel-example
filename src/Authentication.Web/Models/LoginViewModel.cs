using System.ComponentModel.DataAnnotations;

namespace Authentication.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
