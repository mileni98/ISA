using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        public double Price { get; set; }
        public Guid Pharmacy { get; set; }
        public DateTime CreationTime = DateTime.Now;
        public Guid ItemId { get; set; }
        public bool IsAvailable { get; set; }

        public Item(bool isAvailable = true)
        {
            this.IsAvailable = isAvailable;
        }
    }
}
