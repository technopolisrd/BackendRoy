using System.ComponentModel.DataAnnotations;

namespace Backend.Entities.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
