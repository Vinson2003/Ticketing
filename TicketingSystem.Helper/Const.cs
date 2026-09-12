namespace TicketingSystem.Helper
{
    public class Const
    {
        public static readonly string PAGING_SORT_ASC = "asc";
        public static readonly string PAGING_SORT_DESC = "desc";

        public static readonly int TICKET_STATUS_OPEN = 1;
        public static readonly int TICKET_STATUS_IN_PROGRESS = 2;
        public static readonly int TICKET_STATUS_RESOLVED = 3;
        public static readonly int TICKET_STATUS_CLOSED = 4;

        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_DEVELOPER = "DEVELOPER";
        public const string ROLE_USER = "USER";
        public const string ROLE_SUPPORT = "SUPPORT";
    }
}
