using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models.DTO
{
    public class WorkerDTO
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public string Profession { get; set; }

        public string ToString()
        {
            return Profession + " - " + Name;
        }
    }
}
