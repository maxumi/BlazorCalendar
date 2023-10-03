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
                        command.CommandText = "INSERT INTO Birthday (bname, bdate) VALUES (@Name, @Date)";
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
                    command.CommandText = "SELECT bname, bdate FROM Birthday";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["bname"].ToString();
                            DateTime date = (DateTime)reader["bdate"];

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

        public void Update(string ID, string date)
        {
            // Parse the date string into a DateTime object
            if (DateTime.TryParse(date, out DateTime parsedDate) && int.TryParse(ID, out int parsedID))
            {
                using (var connection = new SqlConnection("Data Source=192.168.23.202,1433;Initial Catalog=BirthdayDB;User ID=max;Password=Passw0rd;Encrypt=False"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        string sqlFormattedDate = parsedDate.ToString("yyyy-MM-dd");
                        // Use parameterized query to prevent SQL injection
                        command.CommandText = "UPDATE Birthday SET bdate = @Date WHERE BirthdayID = @ID";
                        command.Parameters.AddWithValue("@Date", sqlFormattedDate); // Use the parsed date
                        command.Parameters.AddWithValue("@ID", parsedID);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }




        public void Delete(string Id)
        {
            using (var connection = new SqlConnection("Data Source=192.168.23.202,1433;Initial Catalog=BirthdayDB;User ID=max;Password=Passw0rd;Encrypt=False"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Birthday WHERE BirthdayID = @ID";
                    command.Parameters.AddWithValue("@ID", Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
