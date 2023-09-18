
using BlazorCalendar.Data.Models;

namespace BlazorCalendar.Data
{
    public class Crud
    {

        public void Readrow(List<Holiday> holidays)
        {
            using (var dbContext = new DataContext())
            {
                var birthdays = dbContext.Birthday.ToList();

                foreach (var birthday in birthdays)
                {
                    Console.WriteLine(birthday.Name);
                    bool IfExists = false;
                    foreach (var holiday in holidays)
                    {
                        if (birthday.Name == holiday.Name)
                            IfExists = true;
                        else if (birthday.Date == holiday.Date)
                            IfExists = true;
                    }
                    if (!IfExists)
                    {
                        holidays.Add(new Holiday { Name = birthday.Name, Date = birthday.Date });
                    }
                }
            }
        }
        public void AddRow(string NewBirthdayName, string NewBirthdayDate)
        {
            if (DateTime.TryParse(NewBirthdayDate, out DateTime BirthDate))
            {
                using (var dbContext = new DataContext())
                {
                    var birthday = new Birthday
                    {
                        Name = NewBirthdayName,
                        Date = BirthDate
                    };
                    Console.WriteLine($"{birthday.Name}'s birthday is on {birthday.Date:D}");
                    dbContext.Birthday.Add(birthday);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
