using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HabitTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Auth : Page {

        Uri baseAddress = new Uri(MainPage.BaseAddress);

        public Auth()
        {
            this.InitializeComponent();
        }

        private async void signIn_Click(object sender, RoutedEventArgs e) {
            var data = await sendAuthRequest(this.usernameBlock.Text, this.passwordBlock.Password, true);
            Debug.WriteLine(data);
            if(data != null){
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"] = data;
                
                Frame.Navigate(typeof(MainPage));
            } else {
                //Todo display error
            }

           
        }


        private async void signUp_Click(object sender, RoutedEventArgs e)  {
            var data = await sendAuthRequest(this.usernameBlock.Text, this.passwordBlock.Password, false);
            if (data != null)
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"] = data;

                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                //Todo display error
            }
        }

        public async Task<String> sendAuthRequest(String username, String password, Boolean signin) {
            using (var client = new HttpClient() { BaseAddress = baseAddress })
            {
                try
                {
                    JsonObject o1 = new JsonObject { { "username", JsonValue.CreateStringValue(username) }, { "password", JsonValue.CreateStringValue(password) } };
                    HttpResponseMessage result;
                    if (signin)
                    {
                        result = await client.PostAsync("authenticate/signin", new StringContent(o1.ToString(), Encoding.UTF8, "application/json"));

                    } else {
                        result = await client.PostAsync("authenticate/signup", new StringContent(o1.ToString(), Encoding.UTF8, "application/json"));
                    }
                    JsonObject returnToken = JsonObject.Parse(result.Content.ReadAsStringAsync().Result);
                    return returnToken.GetNamedString("token");
                } catch(Exception)
                {

                }
            }
            return null;
        }

        private void form_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }


   
}
