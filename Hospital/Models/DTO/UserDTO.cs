using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }

        public UserDTO(string email, string role)
        {
            this.Email = email;
            this.Role = role;
        }
    }
}
