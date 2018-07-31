using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HobbyLobbySBView.ViewModels
{
    public class IndexFitLocationViewModel
    {
        [Display(Name = "Location Length")]
        public double LocL { get; set; }

        [Display(Name = "Location Width")]
        public double LocW { get; set; }

        [Display(Name = "Location Height")]
        public double LocH { get; set; }

        [Display(Name = "Box Length")]
        public double BoxL { get; set; }

        [Display(Name = "Box Width")]
        public double BoxW { get; set; }

        [Display(Name = "Box Height")]
        public double BoxH { get; set; }
    }
}