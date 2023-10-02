namespace BlazorCalendar.Data
{
    public interface ICrud
    {

        public void Create(string NewBirthdayName, string NewBirthdayDate);
        public void Read(List<Holiday> holidays);
        public void Update(string Name, string Date);
        public void Delete(string BirthdayName);

    }
}
