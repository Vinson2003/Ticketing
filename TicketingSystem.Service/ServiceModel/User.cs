using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
    }

    public class UpdateUserRequest
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
    }

    public class UserListResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtText { get; set; } = null!;
    }

    public class UserFilter
    {
        public string? Search { get; set; }

        public int? RoleId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UserDetailResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtText { get; set; } = null!;
    }
}
