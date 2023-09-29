
using BlazorCalendar.Data.Models;
using System.Globalization;

namespace BlazorCalendar.Data
{
    public class Crud : ICrud
    {
        public int SwitchNumber { get; set; }
        public Crud(int switchNumber)
        {
            this.SwitchNumber = SwitchNumber;
        }
        //Creates new row of data
        public void Create(string NewBirthdayName, string NewBirthdayDate)
        {
            switch (SwitchNumber)
            {
                case 0:
                    CrudEntityFramework crudEntityFramework = new CrudEntityFramework();
                    crudEntityFramework.Create(NewBirthdayName, NewBirthdayDate);
                    break;
                case 1:
                    CrudNormalSQL crudNormalSQL = new CrudNormalSQL();
                    crudNormalSQL.Create(NewBirthdayName, NewBirthdayDate);
                    break;
            }
        }
        public void Read(List<Holiday> holidays)
        {
            switch (SwitchNumber)
            {
                case 0:
                    CrudEntityFramework crudEntityFramework = new CrudEntityFramework();
                    crudEntityFramework.Read(holidays);
                    break;
                case 1:
                    CrudNormalSQL crudNormalSQL = new CrudNormalSQL();
                    crudNormalSQL.Read(holidays);
                    break;
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
