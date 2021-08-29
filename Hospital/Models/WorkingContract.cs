using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class WorkingContract : BaseModel
    {
        public Guid WorkerId { get; set; }
        public Guid PharmacyId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public override string ToString()
        {
            return "WorkerId: " + WorkerId.ToString() + " PharmacyId: " + PharmacyId.ToString();
        }
    }
}
