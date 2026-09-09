using System.Data;
using TicketingSystem.Models;

namespace TicketingSystem.Helper
{
    public class Support
    {
        public static void ProccessFilter(Datatable datatable, out string? col, out int colIndex, out string? sort)
        {
            sort = "";
            colIndex = 0;
            col = "";


            if (datatable.Order != null)
            {
                sort = datatable.Order.Select(i => i["dir"]).FirstOrDefault();
                colIndex = Convert.ToInt16(datatable.Order.Select(i => i["column"]).FirstOrDefault());
            }
            if (datatable.Columns != null)
            {
                col = datatable.Columns[colIndex]["data"].ToString().ToLower();
            }
        }
    }
}
