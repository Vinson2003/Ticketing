using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Helper;
using TicketingSystem.Service.Interfaces;
using TicketingSystem.Service.ServiceModel;

namespace TicketingSystem.Controllers
{
    [Authorize]
    public class TicketCommentController(ITicketCommentService ticketCommentService) : Controller
    {
        private readonly ITicketCommentService _ticketCommentService = ticketCommentService;

        [HttpGet]
        public IActionResult GetComments(int id)
        {
            var data = _ticketCommentService.GetComments(id, User.Id(), User.RoleCode());

            if (data == null)
            {
                return Json(new
                {
                    result = false,
                    message = "Ticket not found or access denied."
                });
            }

            return Json(new
            {
                result = true,
                data
            });
        }

        [HttpPost]
        public IActionResult AddComment(AddTicketCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    result = false,
                    message = "Invalid comment data."
                });
            }

            var response = _ticketCommentService.AddComment(
                request, User.Id(), User.RoleCode()
            );

            return Json(response);
        }
    }
}