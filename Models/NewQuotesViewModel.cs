using System;
using System.ComponentModel.DataAnnotations;

namespace MegaDeskMVC.Models
{
    public class NewQuotesViewModel
    {

        [Required]
        [Display(Name ="First Name")]
        [MinLength(2, ErrorMessage = "First Name field must contain at leaset 2 charcters")]
        [MaxLength(50, ErrorMessage = "First Name field cannot contain more than 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MinLength(2, ErrorMessage = "Last Name field must contain at leaset 2 charcters")]
        [MaxLength(50, ErrorMessage = "Last Name field cannot contain more than 50 characters")]
        public string LastName { get; set; }

        [Required]
        [Range(36,86)]
        [Display(Name = "Desk Width")]
        public double? deskWidth { get; set; }

        [Required]
        [Range(24, 120)]
        [Display(Name = "Desk Length")]
        public double? deskLength { get; set; }

        [Required]
        public string material { get; set; }

        [Required]
        public int shippingDays { get; set; }
        public int drawers { get; set; }
    }
}
