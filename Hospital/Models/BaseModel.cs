using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public abstract class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public abstract string GetName();
    }
}
