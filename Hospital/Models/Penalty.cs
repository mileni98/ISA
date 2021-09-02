using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Penalty : BaseModel
    {
        public string PatientId { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return "PatientId: " + PatientId.ToString();
        }
    }
}
