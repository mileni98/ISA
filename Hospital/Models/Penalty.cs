using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Penalty
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
