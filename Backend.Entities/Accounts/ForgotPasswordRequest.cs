using System.ComponentModel.DataAnnotations;

namespace Backend.Entities.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
