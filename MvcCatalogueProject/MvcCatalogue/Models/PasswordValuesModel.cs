namespace MvcCatalogue.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PasswordValuesModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Write old password")]
        [DataType(DataType.Password)]
        //[StringLength(50, MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Write new password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        public string RepeatPassword { get; set; }
    }
}