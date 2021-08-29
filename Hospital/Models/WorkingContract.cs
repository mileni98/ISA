using Hospital.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class WorkingContract : BaseModel
    {
        public string WorkerId { get; set; }
        public Guid PharmacyId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan WorkTimeStart { get; set; }
        public TimeSpan WorkTimeEnd { get; set; }

        public WorkingContract()
        {
            this.StartTime = DateTime.Now;
            this.EndTime = DateTime.Now.AddYears(1);
            this.WorkTimeStart = new TimeSpan(8, 0, 0);
            this.WorkTimeEnd = new TimeSpan(16, 0, 0);
        }

        public WorkingContract(WorkingContract wc)
        {
            WorkerId = wc.WorkerId;
            PharmacyId = wc.PharmacyId;
            StartTime = wc.StartTime;
            EndTime = wc.EndTime;
            WorkTimeStart = wc.WorkTimeStart;
            WorkTimeEnd = wc.WorkTimeEnd;
        }

        public WorkingContract(WorkingContractDTO workingContractDTO) : this((WorkingContract) workingContractDTO)
        {
            this.WorkTimeStart = new TimeSpan(workingContractDTO.WorkingTimeStart, 0, 0);
            this.WorkTimeEnd = new TimeSpan(workingContractDTO.WorkingTimeEnd, 0, 0);
        }

        public WorkingContract(IdentityUser user, Pharmacy pharmacy, 
            DateTime? contractStart = null, DateTime? contractEnd = null,
            TimeSpan? workingTimeStart = null, TimeSpan? workingTimeEnd = null)
        {
            this.WorkerId = user.Id;
            this.PharmacyId = pharmacy.Id;
            this.StartTime = contractStart.HasValue ? contractStart.Value : DateTime.Now;
            this.EndTime = contractEnd.HasValue ? contractEnd.Value : DateTime.Now.AddYears(1);
            this.WorkTimeStart = workingTimeStart.HasValue ? workingTimeStart.Value : new TimeSpan(8, 0, 0);
            this.WorkTimeEnd = workingTimeEnd.HasValue ? workingTimeEnd.Value : new TimeSpan(16, 0, 0);
        }

        public override string ToString()
        {
            return "WorkerId: " + WorkerId.ToString() + " PharmacyId: " + PharmacyId.ToString();
        }
    }
}
