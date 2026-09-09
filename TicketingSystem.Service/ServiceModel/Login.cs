using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class LoginResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public int RoleId { get; set; }

        public string RoleCode { get; set; } = null!;

        public string RoleName { get; set; } = null!;
    }
}
