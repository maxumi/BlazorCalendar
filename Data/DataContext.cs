using BlazorCalendar.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlazorCalendar.Pages;
using BlazorCalendar.Data;

namespace BlazorCalendar.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Birthday>? Birthday { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=192.168.23.202,1433;Initial Catalog=BirthdayDB;User ID=max;Password=Passw0rd;Encrypt=False");
        }
    }
}
