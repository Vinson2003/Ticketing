namespace TicketingSystem.Models
{
    public class Datatable
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public List<Dictionary<string, string>>? Order { get; set; }

        public List<Dictionary<string, string>>? Columns { get; set; }
    }
}
