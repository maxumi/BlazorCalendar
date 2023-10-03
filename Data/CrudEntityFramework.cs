using BlazorCalendar.Data.Models;

namespace BlazorCalendar.Data
{
    public class CrudEntityFramework : ICrud
    {
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
        public void Update(string ID, string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate) && int.TryParse(ID, out int parsedID))
            {
                string newFormattedDateString = parsedDate.ToString("yyyy-MM-dd");
                using (var dbContext = new DataContext())
                {
                    var birthdayToUpdate = dbContext.Birthday.FirstOrDefault(b => b.Id == parsedID);
                    if (birthdayToUpdate != null)
                    {
                        birthdayToUpdate.Date = parsedDate;
                        dbContext.SaveChanges();
                    }
                }
            }
        }


        public void Delete(string BirthdayId)
        {
            if (int.TryParse(BirthdayId, out int parsedID))
            {
                using (var dbContext = new DataContext())
                {
                    var DeleteBirthday = dbContext.Birthday.FirstOrDefault(b => b.Id == parsedID);

                    if (DeleteBirthday != null)
                    {
                        dbContext.Birthday.Remove(DeleteBirthday); // Remove the entity from the context
                        dbContext.SaveChanges(); // Save changes to delete the entity from the database
                    }
                }
            }
        }

    }
}
