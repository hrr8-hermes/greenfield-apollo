using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HabitTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HabbitEdit : Page
    {

        public JsonObject habbitData;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            habbitData = (JsonObject)e.Parameter;
            String habbitName = habbitData.GetNamedString("habitName");
            this.habitName.Text = habbitName;
        }
        public HabbitEdit()
        {
            this.InitializeComponent();
        }

        private async void update_Click(object sender, RoutedEventArgs e) {
            
            Debug.WriteLine(DateTimeOffset.Parse(this.due.Time.ToString()));
            Debug.WriteLine(DateTimeOffset.Now.ToUniversalTime().UtcDateTime.ToUniversalTime());
            await parseUpdate(habitName.Text, DateTimeOffset.Parse(this.remind.Time.ToString()).ToString(), DateTimeOffset.Parse(this.due.Time.ToString()).ToString(), true);
            Frame.Navigate(typeof(MainPage));
        }

        private async void deactivate_Click(object sender, RoutedEventArgs e) {
            await parseUpdate(habitName.Text, new DateTime(this.remind.Time.Ticks).ToUniversalTime().ToString(), new DateTime(this.due.Time.Ticks).ToUniversalTime().ToString(), false);
            Frame.Navigate(typeof(MainPage));
        }

        public async Task<String> parseUpdate(String name, String remind, String due, Boolean active) {
            using (var client = new HttpClient() { BaseAddress = new Uri(MainPage.BaseAddress) })
            {
                // try
                //{
                JsonObject o1 = new JsonObject { { "habitName", JsonValue.CreateStringValue(name) }, { "reminderTime", JsonValue.CreateStringValue(remind) }, { "dueTime", JsonValue.CreateStringValue(due) }, { "active", JsonValue.CreateBooleanValue(active) } };
                HttpResponseMessage result;
                Debug.WriteLine(o1);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"]);
                result = await client.PutAsync("/api/users/habits/" + habbitData.GetNamedString("_id"), new StringContent(o1.ToString(), Encoding.UTF8, "application/json"));
                Debug.WriteLine(result);
                JsonObject returned = JsonObject.Parse(result.Content.ReadAsStringAsync().Result);
                return returned.ToString();
                //}
                //catch (Exception e)
                //{
                //    
                //}
            }
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
