using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MegaDeskMVC.Models
{
    public class Quote
    {
        
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "First Name")]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Display(Name = "Desk Width")]
        public double DeskWidth { get; set;  }

        [Display(Name = "Desk Length")]
        public double DeskLength { get; set; }
        public int Drawers { get; set;  }
        public string Material { get; set;  }

        [Display(Name = "Material Price")]
        public double MaterialPrice { get; set;  }

        [Display(Name = "Shipping Days")]
        public int ShippingDays { get; set; }

        [Display(Name = "Shipping Price")]
        public double ShippingPrice { get; set;  }

        [Display(Name = "Quote Amount")]
        [Range(0, 50000)]
        public double Amount { get; set;  }



    }
}
