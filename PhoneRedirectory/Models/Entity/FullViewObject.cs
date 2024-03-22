using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneRedirectory.Models.Entity
{
    public class FullViewObject
    {
        [Key]
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string PhoneType { get; set; }
        public string Number { get; set; }

        //[NotMapped]
        public string CountryName { get; set; }
        //[NotMapped]
        public string Displayer { get; set; }
    }
}
