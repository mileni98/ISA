﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PharmacyId { get; set; }
        public Guid PatientId { get; set; }
        public Guid MedicalWorkerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Rating { get; set; }
        public double Price { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
