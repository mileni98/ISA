using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Appointment : BaseModel
    {
        public Guid PharmacyId { get; set; }
        public string PatientId { get; set; } = "";
        public string MedicalWorkerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Rating { get; set; }
        public double Price { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return "Appointment with starting time: " + StartTime.ToString();
        }
    }
}
