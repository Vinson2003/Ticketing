using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class ProfileResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string RoleName { get; set; } = null!;
    }

    public class ChangePasswordRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = null!;

        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
