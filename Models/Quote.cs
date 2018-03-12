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


        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        public double DeskWidth { get; set;  }
        public double DeskLength { get; set; }
        public int Drawers { get; set;  }
        public string Material { get; set;  }


        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double MaterialPrice { get; set;  }

        public int ShippingDays { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public double ShippingPrice { get; set;  }

        [DisplayFormat(DataFormatString = "{0:C0}")]

        [Range(0, 50000)]
        public double Amount { get; set;  }



    }
}
