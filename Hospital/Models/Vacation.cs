﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Vacation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public bool? IsApproved
        {
            get
            {
                return IsApproved;
            }
            set
            {
                ApprovalTime = DateTime.Now;
                IsApproved = value;
            }
        }
        public DateTime ApprovalTime { get; private set; }

        public Vacation()
        {
            IsApproved = null;
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
