using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Service.ServiceModel
{
    public class RequestPermsRoleList
    {
        public int RoleId { get; set; }
        public string Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public string Description { get; set; } = null!;
        public string Display { get; set; } = null!;
    }

    public class RolePermissionItem
    {
        public int PermissionId { get; set; }

        public string Description { get; set; } = null!;
        public string Display { get; set; } = null!;

        public bool IsChecked { get; set; }
    }

    public class SaveRolePermissionRequest
    {
        public int RoleId { get; set; }

        public List<int> PermissionIds { get; set; } = [];
    }
}
