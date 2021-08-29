using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Review : BaseModel
    {
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public string ReviewedId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        //Ranging from 1 to 5
        public int Rating { get; set; }

        public override string ToString()
        {
            return "Review: " + Id.ToString();
        }
    }
}
