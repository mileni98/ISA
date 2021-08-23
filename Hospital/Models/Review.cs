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
        public int Id { get; set; }
        public DateTime DateTime { get; private set; }
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        //Ranging from 1 to 5
        public int Rate { get; set; }
        
        public Review()
        {
            this.DateTime = DateTime.Now;
        }
    }
}
