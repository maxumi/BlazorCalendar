using System.Globalization;

namespace BlazorCalendar.Data
{
    public class Holiday
    {
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string MonthYear => Date?.ToString("MMMM-yyyy", CultureInfo.InvariantCulture) ?? "";

    }
}
