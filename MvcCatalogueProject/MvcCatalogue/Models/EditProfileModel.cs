namespace MvcCatalogue.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EditProfileModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Write full name")]
        [StringLength(50, MinimumLength = 6)]
        public string FullName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage="Write E-mail")]
        [Display(Name="E-mail")]
        public string Email { get; set; }
    }
}