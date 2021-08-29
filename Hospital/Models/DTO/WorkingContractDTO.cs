using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models.DTO
{
    public class WorkingContractDTO : WorkingContract
    {
        public int WorkingTimeStart { get; set; } = 8;
        public int WorkingTimeEnd { get; set; } = 16;
    }
}
