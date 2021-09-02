using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Offer : BaseModel
    {
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public string OfferedById { get; set; }
        public Guid DrugId { get; set; }
        public Guid PharmacyId { get; set; }
        public bool IsClosed { get; set; } = false;
        public string CreatedByUserId { get; set; }

        public override string ToString()
        {
            return "Offer: " + Id.ToString();
        }
    }
}
