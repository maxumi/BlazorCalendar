
using BlazorCalendar.Data.Models;
using System.Globalization;

namespace BlazorCalendar.Data
{
    public class Crud : ICrud
    {
        //Creates new row of data
        public void Create(string NewBirthdayName, string NewBirthdayDate)
        {
            if (DateTime.TryParse(NewBirthdayDate, out DateTime BirthDate))
            {
                using (var dbContext = new DataContext())
                {
                    
                    var birthday = new Birthday
                    {
                        Name = $"{NewBirthdayName}'s fødselsdag",
                        Date = BirthDate
                    };
                    Console.WriteLine($"{birthday.Name}'s birthday is on {birthday.Date:D}");
                    dbContext.Birthday.Add(birthday);
                    dbContext.SaveChanges();
                }
            }
        }

        //Reads the contents of the database and adds the content to the database
        public void Read(List<Holiday> holidays)
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

        public void Update()
        {
            throw new NotImplementedException();
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
