using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Item : BaseModel
    {
        public double Price { get; set; }
        public Guid Pharmacy { get; set; }
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public Guid ItemId { get; set; }
        public bool IsAvailable { get; set; }

        public Item(bool isAvailable = true)
        {
            this.IsAvailable = isAvailable;
        }

        public override string GetName()
        {
            return "Item: " + Id.ToString();
        }
    }
}
