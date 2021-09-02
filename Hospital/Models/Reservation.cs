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
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public string UserId { get; set; }
        public Guid PharmacyId { get; set; }

        public override string ToString()
        {
            return "Reservation with starting time: " + StartTime.ToString();
        }
    }
}
