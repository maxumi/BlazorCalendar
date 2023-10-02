using BlazorCalendar.Data.Models;

namespace BlazorCalendar.Data
{
    public class FakeCrud : ICrud
    {
        public FakeCrud()
        {
            holidays.Add(new Holiday() { Name = "Max", Date = new System.DateTime(2020, 12, 12) });
            holidays.Add(new Holiday() { Name = "Tom", Date = new System.DateTime(2020, 12, 12) });
            holidays.Add(new Holiday() { Name = "Jenny", Date = new System.DateTime(2020, 12, 12) });
        }
        List<Holiday> holidays { get; set; } = new List<Holiday>();
        public void Create(string NewBirthdayName, string NewBirthdayDate)
        {
            throw new NotImplementedException();
        }
        public void Read(List<Holiday> holidays)
        {
            throw new NotImplementedException();
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void DeleteSqlUser(string BirthdayName)
        {
            //This is a mockup method that is trying to delete a user from the database
            //This is not possible with the current database setup
            //This will will have a fake list and remove the user from the list if it exists
            //This is not a good way to do it, but it is the only way to do it with the current setup

            //This is removing the name from the list if it exists.
            if (holidays.Exists(x => x.Name == BirthdayName))
            {
                holidays.Remove(holidays.Find(x => x.Name == BirthdayName));
            }

        }

    }
}
