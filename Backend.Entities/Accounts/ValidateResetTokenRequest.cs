using System.ComponentModel.DataAnnotations;

namespace Backend.Entities.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
