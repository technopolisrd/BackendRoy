using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Common.Core.Contracts.Common;

namespace Backend.Entities.Tables
{
    [DataContract]
    [Table("CODE_Customer")]
    public class Customer : IAuditableEntity, IDeferrableEntity, IConcurrencyEntity
    {

        #region Properties

        [DataMember]
        [Column("CustomerId")]
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

        #endregion

        #region Interface Implentations

        [DataMember]
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        [Required]
        public string CreatedById { get; set; }

        [DataMember]
        [Column(TypeName = "datetime2")]
        public DateTime UpdatedDate { get; set; }

        [DataMember]
        public string UpdatedById { get; set; }

        [DataMember]
        [Required]
        [Column("DeleteFlag")]
        public bool Deferred { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        #endregion
    }
}
