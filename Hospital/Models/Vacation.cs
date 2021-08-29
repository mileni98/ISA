using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Vacation : BaseModel
    {
        public string WorkerId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Today;
        public DateTime EndTime { get; set; } = DateTime.Today.AddDays(1);
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false;
        public bool HasBeenReviewed { get; set; } = false;
        public DateTime ApprovalTime { get; set; }

        public override string ToString()
        {
            return "Vacation: " + Id.ToString();
        }
    }
}
