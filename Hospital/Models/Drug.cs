using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Drug
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //csv formatted string
        public string Ingredients { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
