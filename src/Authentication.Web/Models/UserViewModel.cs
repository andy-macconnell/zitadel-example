using System.ComponentModel.DataAnnotations;

namespace Authentication.Web.Models;

public class UserViewModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }
}