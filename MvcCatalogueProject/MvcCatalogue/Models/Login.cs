namespace MvcCatalogue.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Login
    {
        [Required(ErrorMessage="Username required!", AllowEmptyStrings=false)]
        //[StringLength(200, MinimumLength=6)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}