using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MegaDeskMVC.Models
{
    public class Quote
    {
        
        public int Id { get; set; }


        public string Date { get; set; }


        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        public double deskWidth { get; set;  }
        public double deskLength { get; set; }
        public int drawers { get; set;  }
        public string matrial { get; set;  }


        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double materialPrice { get; set;  }

        public int shippingDays { get; set; }

        [DataType(DataType.Currency)]
        public double shippingPrice { get; set;  }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Range(0, 50000)]
        public double Amount { get; set;  }



    }
}
