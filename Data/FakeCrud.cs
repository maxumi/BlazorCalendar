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
        public void Update(string Name, string Date)
        {
            //Update the date of the user inside the list

            if (holidays.Exists(x => x.Name == Name))
            {
                holidays.Find(x => x.Name == Name).Date = System.DateTime.Parse(Date);
            }
        }
        public void Delete(string BirthdayName)
        {
            //This is a mockup method that is trying to delete a user from the database
            //This is removing the name from the list if it exists.
            if (holidays.Exists(x => x.Name == BirthdayName))
            {
                holidays.Remove(holidays.Find(x => x.Name == BirthdayName));
            }
        }
    }
}
