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

namespace BlazorCalendar.Pages
{
    public partial class Index
    {
        static public string apiKey = "d";
        static public string country = "DK";
        static int year = 2022;
        static string BaseApiUrl = $"https://holidayapi.com/v1/holidays?key={apiKey}&country={country}&year={year}";

        // The selected date from the calendar
        private string selectedDate = "";


        public class Holiday
        {
            public string? Name { get; set; }
            public DateTime? Date { get; set; }
        }
        public List<Holiday>? holidays { get; set; }

        public class Weekday
        {
            public DateTime? Date { get; set; }
            public string? Name { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            // Create a method to get the holidays from the API
            holidays = await GetHolidaysAsync();
        }

        async Task<List<Holiday>> GetHolidaysAsync()
        {
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
                        weekday.Name = holiday.Name;
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

    }
}