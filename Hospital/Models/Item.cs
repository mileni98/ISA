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
        public Guid PharmacyId { get; set; }
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public Guid ItemId { get; set; }
        public bool IsAvailable { get; set; } = true;

        public override string ToString()
        {
            return "Item: " + Id.ToString();
        }
    }
}
