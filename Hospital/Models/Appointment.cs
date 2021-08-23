using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public int PatientId { get; set; }
        public int MedicalWorkerId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Rating { get; set; }
    }
}
