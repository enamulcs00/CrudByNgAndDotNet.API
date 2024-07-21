using System.ComponentModel.DataAnnotations;

namespace CrudByNgAndDotNet.API.Models.DTO
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientURI { get; set; }
    }
}
