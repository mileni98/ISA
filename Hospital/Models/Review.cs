using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreationTime = DateTime.Now;
        public Guid ReviewedId { get; set; }
        public Guid UserId { get; set; }
        //Ranging from 1 to 5
        public int Rate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
