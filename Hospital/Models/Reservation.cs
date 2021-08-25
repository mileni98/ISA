using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreationTime = DateTime.Now;
        public Guid UserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
