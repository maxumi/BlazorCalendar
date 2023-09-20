namespace BlazorCalendar.Data
{
    public interface ICrud
    {

        public void Create(string NewBirthdayName, string NewBirthdayDate);
        public void Read(List<Holiday> holidays);
        public void Update();
        public void Delete();

    }
}
