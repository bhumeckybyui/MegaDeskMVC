using System;
using MegaDeskMVC.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Web;

namespace MegaDeskMVC.Models
{
    public class QuotesViewModel
    {

        public List<Quote> myQuotes { get; set; }
        public string search { get; set; }
        public string sort { get; set; }
        public string sortOrder { get; set;  }


    }
}
