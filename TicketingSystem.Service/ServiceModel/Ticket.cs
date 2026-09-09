using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class TicketFilter
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }

    public class ResponseListTicket
    {
        public int Id { get; set; }
        public string TicketNo { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int PriorityId { get; set; }
        public string? PriorityName { get; set; }

        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }

        public int? AssignedTo { get; set; }
        public string? AssignedToName { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedAtText { get; set; } = string.Empty;
    }

    public class DetailTicket
    {
        public int Id { get; set; }
        public string TicketNo { get; set; } = null!; 
        public string Title { get; set; } = null!; 
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!; 

        public int PriorityId { get; set; }
        public string PriorityName { get; set; } = null!; 

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!; 

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = null!; 

        public int? AssignedTo { get; set; }
        public string? AssignedToName { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedAtText { get; set; } = null!;
    }

    public class CreateTicketRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int PriorityId { get; set; }

        public int? AssignedTo { get; set; }
    }

    public class UpdateTicketRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int PriorityId { get; set; }

        [Range(1, int.MaxValue)]
        public int StatusId { get; set; }

        public int? AssignedTo { get; set; }
    }

    public class DropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class TicketDropdownResponse
    {
        public List<DropdownItem> Categories { get; set; } = [];
        public List<DropdownItem> Priorities { get; set; } = [];
        public List<DropdownItem> Statuses { get; set; } = [];
        public List<DropdownItem> Users { get; set; } = [];
    }
}
