using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCalendar.Data.Models
{
    public class Birthday
    {
        [Key] // Specify the primary key
        [Column("BirthdayID")] // Map to the 'BirthdayID' column in the database
        public int Id { get; set; }

        [Column("bname")] // Map to the 'bname' column in the database
        public string Name { get; set; }

        [Column("bdate")] // Map to the 'bdate' column in the database
        public DateTime Date { get; set; }
    }
}
