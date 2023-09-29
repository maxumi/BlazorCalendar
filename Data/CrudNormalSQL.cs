using Microsoft.Data.SqlClient;

namespace BlazorCalendar.Data
{
    public class CrudNormalSQL : ICrud
    {
        public void Create(string NewBirthdayName, string NewBirthdayDate)
        {
            if (DateTime.TryParse(NewBirthdayDate, out DateTime BirthDate))
            {
                using (var connection = new SqlConnection("Data Source=192.168.23.202,1433;Initial Catalog=BirthdayDB;User ID=max;Password=Passw0rd;Encrypt=False"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Birthday (Name, Date) VALUES (@Name, @Date)";
                        command.Parameters.AddWithValue("@Name", $"{NewBirthdayName}'s fødselsdag");
                        command.Parameters.AddWithValue("@Date", BirthDate);

                        command.ExecuteNonQuery();
                        Console.WriteLine($"{NewBirthdayName}'s birthday is on {BirthDate:D}");
                    }
                }
            }
        }

        public void Read(List<Holiday> holidays)
        {
            using (var connection = new SqlConnection("Data Source=192.168.23.202,1433;Initial Catalog=BirthdayDB;User ID=max;Password=Passw0rd;Encrypt=False"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Name, Date FROM Birthday";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            DateTime date = (DateTime)reader["Date"];

                            bool IfExists = false;
                            foreach (var holiday in holidays)
                            {
                                if (name == holiday.Name || date == holiday.Date)
                                {
                                    IfExists = true;
                                    break;
                                }
                            }

                            if (!IfExists)
                            {
                                holidays.Add(new Holiday { Name = name, Date = date });
                            }
                        }
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
