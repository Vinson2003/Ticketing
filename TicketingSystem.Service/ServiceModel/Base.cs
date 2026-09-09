using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class BaseResponse<T>
    {
        public T? Result { get; set; }
        public string? Message { get; set; }
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
    }

    public class BasePaging
    {
        public string? SortBy { get; set; }
        public string? Column { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }
    }
}
