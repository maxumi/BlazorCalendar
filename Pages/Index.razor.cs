using global::System;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorCalendar;
using BlazorCalendar.Shared;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using static BlazorCalendar.Pages.Index;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace BlazorCalendar.Pages
{
    public partial class Index
    {
        static public string apiKey = "FAKEKEY";
        static public string country = "DK";
        static int year = 2022;
        static string BaseApiUrl = $"https://holidayapi.com/v1/holidays?key={apiKey}&country={country}&year={year}";

        // The selected date from the calendar
        private string selectedDate = "";

        private string NewBirthdayName = "";
        private string NewBirthdayDate = "";




        public class Holiday
        {
            public string? Name { get; set; }
            public DateTime? Date { get; set; }
            public string MonthYear => Date?.ToString("MMMM-yyyy", CultureInfo.InvariantCulture) ?? "";

        }
        public List<Holiday>? holidays { get; set; }

        public class Weekday
        {
            public DateTime? Date { get; set; }
            public string? Name { get; set; }
        }
        // Changes the names from english to danish
        private Dictionary<string, string> ChangeHolidayName = new Dictionary<string, string>
        {
            { "New Year's Day", "Nytårsdag" },
            { "Fastelavn", "Fastelavn" },
            { "March Equinox", "Forårsjævndøgn" },
            { "Palm Sunday", "Palmesøndag" },
            { "Maundy Thursday", "Skærtorsdag" },
            { "Good Friday", "Langfredag" },
            { "Easter", "Påske" },
            { "Easter Monday", "2. Påskedag" },
            { "International Workers' Day", "Arbejdernes Internationale Kampdag" },
            { "Liberation Day", "Befrielsesdag" },
            { "Mother's Day", "Mors Dag" },
            { "General Prayer Day", "Store Bededag" },
            { "Feast of the Ascension of Jesus Christ", "Kristi Himmelfartsdag" },
            { "Constitution Day", "Grundlovsdag" },
            { "Father's Day", "Fars Dag" },
            { "Pentecost", "Pinsedag" },
            { "Pentecost Monday", " 2. Pinsedag" },
            { "June Solstice", "sommersolhverv" },
            { "September Equinox", "Efterårsjævndøgn" },
            { "Halloween", "Halloween" },
            { "December Solstice", "Vintersolhverv" },
            { "Christmas Eve", "Juleaften" },
            { "Christmas Day", "1. Juledag" },
            { "Day after Christmas", "2. Juledag" },
            { "New Year's Eve", "Nytårsaften" }
        };
        public Dictionary<string, List<Holiday>> GroupedHolidays { get; set; } = new Dictionary<string, List<Holiday>>();
        string GetTranslatedHolidayName(string englishName)
        {
            if (ChangeHolidayName.ContainsKey(englishName))
            {
                return ChangeHolidayName[englishName];
            }
            return englishName; // Return the original name if not found in the dictionary
        }

        protected override async Task OnInitializedAsync()
        {
            // Create a method to get the holidays from the API
            holidays = await GetHolidaysAsync();

            //When not using sql, comment this code
            //AddBirthdaysFromSQL();

            //This is for displaying all holidays.
            //They create a dictinary with each month being a key and a list for that month being within that
            //So there now exists 12 lists for all months showing holidays.
            GroupedHolidays = holidays
                .GroupBy(h => h.MonthYear)
                .OrderBy(g => DateTime.ParseExact(g.Key, "MMMM-yyyy", CultureInfo.InvariantCulture))
                .ToDictionary(g => g.Key, g => g.ToList());


            //New Data could put them out of order so these sorts the lists within the dictonary's value lists
            foreach (var monthYear in GroupedHolidays.Keys.ToList())
            {
                GroupedHolidays[monthYear] = GroupedHolidays[monthYear].OrderBy(h => h.Date).ToList();
            }

        }

        async Task<List<Holiday>> GetHolidaysAsync()
        {
            //Creates a temporary Client object.
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(BaseApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(json);
                    JArray? holidaysArray = jsonObject["holidays"] as JArray;
                    if (holidaysArray != null)
                    {
                        var holidayList = new List<Holiday>();
                        foreach (JObject holidayObject in holidaysArray)
                        {
                            string? name = holidayObject["name"]?.ToString();
                            DateTime? date = holidayObject["date"]?.ToObject<DateTime?>();
                            Holiday holiday = new Holiday()
                            {
                                Name = name,
                                Date = date
                            };
                            holidayList.Add(holiday);
                        }
                        return holidayList;
                    }
                    else
                    {
                        // Handle error here
                        Console.WriteLine("Error, HolidayArray null ");
                        return new List<Holiday>();
                    }
                }
                else
                {
                    // Handle error here
                    Console.WriteLine("Error, request Not Succesfull");
                    return new List<Holiday>();
                }
            }

        }
        public List<Weekday> GetWeekDays()
        {
            if (DateTime.TryParse(selectedDate, out DateTime selectedDateTime))
            {
                // Calculate the start of the week for the selected date
                // Inside AddDays it takes the enum of the day of the week(0 = Sunday, 1 = Monday, etc.) and makes it up to minus (Tuesday is 2, so it will be minus 2 days to sunday(2-2=0)
                // so wednesday will become -3 to sunday, but I add 1 to make it monday.
                DateTime startOfWeek = selectedDateTime.Date.AddDays(-(int)selectedDateTime.DayOfWeek);
                startOfWeek = startOfWeek.Date.AddDays(1);


                // Create a list of 7 days (Monday to Sunday) with the dates
                var weekdays = new List<Weekday>();
                for (int i = 0; i < 7; i++)
                {
                    DateTime currentDay = startOfWeek.AddDays(i);
                    weekdays.Add(new Weekday { Date = currentDay });
                }

                // Check if each day is a holiday and add the holiday name
                foreach (var weekday in weekdays)
                {
                    var holiday = holidays?.FirstOrDefault(h => h.Date == weekday.Date);
                    if (holiday != null)
                    {
                        // Check if the English holiday name is in the mapping, if so, use the Danish name
                        if (ChangeHolidayName.ContainsKey(holiday.Name))
                        {
                            weekday.Name = ChangeHolidayName[holiday.Name];
                        }
                        else
                        {
                            // If not found in the mapping, use the original English name
                            weekday.Name = holiday.Name;
                        }
                    }
                    else
                    {
                        // IF there exists no holiday, then the name will be No holiday
                        weekday.Name = "No holiday";
                    }
                }

                return weekdays;
            }
            else
            {
                // If the date is not valid, return an empty list
                return new List<Weekday>();
            }
        }

        public void IfBirthDayExists(string? name, DateTime? date)
        {
            bool IfExists = false;
            foreach (var holiday in holidays)
            {
                if (holiday.Name == name)
                {
                    IfExists = true;
                }
                else if (holiday.Date == date)
                {
                    IfExists = true;
                }
            }
            if (!IfExists)
            {
                holidays.Add(new Holiday { Name = name, Date = date });
            }
        }


        public void AddBirthdaysFromSQL()
        {
            Console.WriteLine("Hello, World!");
            var builder = new SqlConnectionStringBuilder();

            // Set the necessary properties for your SQL Server connection
            builder.DataSource = "192.168.23.202,1433"; // Replace with your SQL Server instance name or IP address.
            builder.InitialCatalog = "BirthdayDB"; // Replace with your database name.
            builder.UserID = "max"; // Replace with your SQL Server username.
            builder.Password = "Passw0rd"; // Replace with your SQL Server password (if any).
            builder.Encrypt = false; //This is needed or else the it will create an error form connection.


            // Convert the builder to a connection string
            string connectionString = builder.ToString();
            Console.WriteLine(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT bname, bdate FROM birthdayDB.dbo.Birthday";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? name = reader["bname"] as string;
                            DateTime? date = reader["bdate"] as DateTime?;

                            IfBirthDayExists(name, date);

                            Console.WriteLine($"Name: {name}, Date: {date}");
                        }
                    }
                }
            }
        }

        public void AddBirthday()
        {
            if (NewBirthdayDate != null || NewBirthdayName != null)
            {
                if (DateTime.TryParse(NewBirthdayDate, out DateTime BirthDate))
                {
                    var builder = new SqlConnectionStringBuilder();

                    // Set the necessary properties for your SQL Server connection
                    builder.DataSource = "192.168.23.202,1433"; // Replace with your SQL Server instance name or IP address.
                    builder.InitialCatalog = "BirthdayDB"; // Replace with your database name.
                    builder.UserID = "max"; // Replace with your SQL Server username.
                    builder.Password = "Passw0rd"; // Replace with your SQL Server password (if any).
                    builder.Encrypt = false; // This is needed or else the it will create an error from the connection.
                    string connectionString = builder.ToString();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        // Use parameterized query to avoid SQL injection and properly format string values.
                        string sqlQuery = "INSERT INTO Birthday(bname, bdate) VALUES(@Name, @Date)";
                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", NewBirthdayName);
                            command.Parameters.AddWithValue("@Date", BirthDate);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }


    }
}