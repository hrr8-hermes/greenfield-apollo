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
    public sealed partial class Create : Page
    {
        public Create()
        {
            this.InitializeComponent();
        }

        private async void addHabit_Click_1(object sender, RoutedEventArgs e) {
            var data = await sendHabitRequest(this.textBox.Text, DateTimeOffset.Parse(this.remind.Time.ToString()).ToString(), DateTimeOffset.Parse(this.due.Time.ToString()).ToString());
            //var data = await sendHabitRequest("", "", "");
            if (data != null){
                Frame.Navigate(typeof(MainPage));
            }
        }
        public async Task<String> sendHabitRequest(String name, String remind, String due)
        {
            using (var client = new HttpClient() { BaseAddress = new Uri(MainPage.BaseAddress) })
            {
               // try
                //{
                    JsonObject o1 = new JsonObject { { "habitName", JsonValue.CreateStringValue(name) }, { "reminderTime", JsonValue.CreateStringValue(remind) }, {"dueTime", JsonValue.CreateStringValue(due) } };
                    HttpResponseMessage result;
                    Debug.WriteLine(o1);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"]);
                    result = await client.PostAsync("/api/users/habits", new StringContent(o1.ToString(), Encoding.UTF8, "application/json"));
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
