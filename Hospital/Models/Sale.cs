using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Sale : BaseModel
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(7);
        public int Percentage { get; set; } = 10;

        public override string ToString()
        {
            return "Sale: " + Id.ToString();
        }
    }
}
