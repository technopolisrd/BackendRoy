using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Backend.Entities.Tables.DTO
{
    public class CustomerDTO
    {
        [DataMember]
        [Display(Name = "Customer ID")]
        public long Id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "The document field is required.")]
        [StringLength(100, ErrorMessage = "You have exceeded the maximum number of characters allowed in this field.")]
        [Display(Name = "Document Id")]
        public string DocumentId { get; set; }

        [DataMember]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "You have exceeded the maximum number of characters allowed in this field.")]
        public string FirstName { get; set; }

        [DataMember]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "You have exceeded the maximum number of characters allowed in this field.")]
        public string LastName { get; set; }
    }
}
