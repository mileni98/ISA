using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Pharmacy : BaseModel
    {
        public string Name { get; set; }
        public string Location { get; set; }

        public override string GetName()
        {
            return "Pharmacy: " + Name + ", " + Location;
        }
    }
}
