using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Reservation : BaseModel
    {
        public Guid ItemId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public Guid UserId { get; set; }

        public override string GetName()
        {
            return "Reservation with starting time: " + StartTime.ToString();
        }
    }
}
