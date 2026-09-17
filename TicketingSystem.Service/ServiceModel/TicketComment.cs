using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class AddTicketCommentRequest
    {
        [Range(1, int.MaxValue)]
        public int TicketId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Comment { get; set; } = null!;
    }

    public class TicketCommentResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Comment { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string CreatedAtText { get; set; } = null!;
    }
}
