using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorCalendar;
using BlazorCalendar.Shared;
using BlazorCalendar.Data;

namespace BlazorCalendar.Pages //Currently a mockup of the update page. It has no database yet so it does nothing
{
    public partial class UpdateRazor
    {
        public string SelectedName { get; set; } = "";
        public void DeleteUser()
        {
            FakeCrud crud = new FakeCrud();
            crud.DeleteSqlUser(SelectedName);
        }
    }
}