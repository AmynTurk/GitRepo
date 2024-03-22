using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneRedirectory.Models.Entity
{
    public class Filters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FilterTypeId { get; set; }
    }
}
