using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HabitTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public static string BaseAddress = "https://habit-trainer.herokuapp.com";
        Uri baseAddress = new Uri(MainPage.BaseAddress);
        List<UIElement> inGrid = new List<UIElement>();
       

        public MainPage()
        {
            this.InitializeComponent();
            getHabits();



            //setupToastNotification();

            //ToastNotification toast = new ToastNotification(toastXml);
            
            //ToastNotificationManager.CreateToastNotifier().Show(toast);


        }
        public XmlDocument setupToastNotification(string text)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "http://www.contoso.com/redWide.png");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");
            return toastXml;

        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        public async void getHabits() {


            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress }) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"]);
                try
                {
                    var result = await client.GetStringAsync("/api/users/habits");


                    parseHabitsResponce(result);
                } catch(Exception)
                {
                    this.Frame.Navigate(typeof(Auth));
                }
                
                //result.EnsureSuccessStatusCode();
            }
        }

        public void parseHabitsResponce(String responce){
            Debug.WriteLine("Before Count: "+ this.mainGrid.Children.Count);
            foreach(UIElement inGRID in inGrid)
            {
                this.mainGrid.Children.Remove(inGRID);
            }
            Debug.WriteLine("After Count: " + this.mainGrid.Children.Count);
            this.mainGrid.UpdateLayout();

            int count;
            int limit;
            JsonArray array;
            JsonObject jobj = JsonObject.Parse(responce);
            
            JsonObject o = jobj.GetObject();
            count = (int)o.GetNamedNumber("habitCount");
            limit = (int)o.GetNamedNumber("habitLimit");
            array = o.GetNamedArray("habits");
            int loop = 1;
            Debug.WriteLine(array.Count);
            foreach(JsonValue habbitValue in array) {
                JsonObject habbit = habbitValue.GetObject();
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(25);
                this.mainGrid.RowDefinitions.Add(rowDef);
                //Check if active
                Debug.WriteLine(habbit);
               if(habbit.GetNamedBoolean("active")) {
                    
                    Button button = new Button();
                    button.Click += async (sender, args) => {
                        await sendRequest("/api/records/" + habbit.GetNamedString("_id"), "");
                        getHabits();
                    };


                    button.HorizontalAlignment = HorizontalAlignment.Center;
                    
                    button.VerticalAlignment = VerticalAlignment.Center;
                    button.Height = 25;
                    button.FontSize = 8;
                    switch(habbit.GetNamedString("status")) {
                        case "pending":
                            button.Background = new SolidColorBrush(Colors.Gray);
                            button.Content = "Did-it!";
                            break;
                        case "completed":
                            button.Background = new SolidColorBrush(Colors.Green);
                            button.Content = "Completed";
                            break;
                        case "failed":
                            button.Background = new SolidColorBrush(Colors.Red);
                            button.Content = "Failed";
                            break;
                        default:
                            button.Content = habbit.GetNamedString("status");
                            break;
                    }

                    //button.Content = "Checkin";
                    Grid.SetColumn(button, 1);
                    Grid.SetRow(button, loop);
                    this.mainGrid.Children.Add(button);
                    inGrid.Add(button);


                    //Habbit name
                    TextBlock habbitName = new TextBlock();
                    habbitName.Text = habbit.GetNamedString("habitName");
                    habbitName.HorizontalAlignment = HorizontalAlignment.Center;
                    habbitName.VerticalAlignment = VerticalAlignment.Center;
                    habbitName.FontSize = 10.667;
                    Grid.SetColumn(habbitName, 2);
                    Grid.SetRow(habbitName, loop);
                    this.mainGrid.Children.Add(habbitName);
                    inGrid.Add(habbitName);
                    //Debug.WriteLine(habbitName.Text + "" + loop);

                    //Reminder time
                    TextBlock RTime = new TextBlock();
                    DateTime rtime = System.DateTime.Parse(habbit.GetNamedString("reminderTime"));
                    RTime.Text = rtime.ToString("h:mm tt");
                    RTime.HorizontalAlignment = HorizontalAlignment.Center;
                    RTime.VerticalAlignment = VerticalAlignment.Center;
                    RTime.FontSize = 10.667;
                    Grid.SetColumn(RTime, 3);
                    Grid.SetRow(RTime, loop);
                    this.mainGrid.Children.Add(RTime);
                    inGrid.Add(RTime);
                    //Debug.WriteLine(RTime.Text + "" + loop);

                    //Setup notification
                    var time = DateTimeOffset.Parse(rtime.ToLocalTime().ToString());
                    if(time.ToUnixTimeSeconds() > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        Debug.WriteLine("Scheduled Toast");
                        Debug.WriteLine(time);
                        var xml = setupToastNotification(habbitName.Text+" is due by "+ System.DateTime.Parse(habbit.GetNamedString("reminderTime")).ToString("h:mm tt"));
                        ToastNotificationManager.CreateToastNotifier().AddToSchedule(new ScheduledToastNotification(xml, time));
                    } else
                    {
                        Debug.WriteLine("No need to schedule Toast");
                        Debug.WriteLine(time);
                    }
                    

                    //Due time
                    TextBlock DTime = new TextBlock();
                    DateTime dtime = System.DateTime.Parse(habbit.GetNamedString("dueTime"));
                    DTime.Text = dtime.ToString("h:mm tt");
                    DTime.HorizontalAlignment = HorizontalAlignment.Center;
                    DTime.VerticalAlignment = VerticalAlignment.Center;
                    DTime.FontSize = 10.667;
                    //fGrid.SetColumn(RTime, 4);
                    Grid.SetColumn(DTime, 4);
                    Grid.SetRow(DTime, loop);
                    this.mainGrid.Children.Add(DTime);
                    inGrid.Add(DTime);
                    //Debug.WriteLine(DTime.Text + "" + loop);

                    //Current Streak
                    TextBlock currentStreak = new TextBlock();
                    currentStreak.Text = "" + habbit.GetNamedNumber("streak");
                    currentStreak.HorizontalAlignment = HorizontalAlignment.Center;
                    currentStreak.VerticalAlignment = VerticalAlignment.Center;
                    currentStreak.FontSize = 10.667;
                    Grid.SetColumn(currentStreak, 5);
                    //Grid.SetColumn(currentStreak, 5);
                    Grid.SetRow(currentStreak, loop);
                    this.mainGrid.Children.Add(currentStreak);
                    inGrid.Add(currentStreak);
                    //Debug.WriteLine(currentStreak.Text + "" + loop);

                    //Record Streak
                    TextBlock recordStreak = new TextBlock();
                    recordStreak.Text = "" + habbit.GetNamedNumber("streakRecord");
                    recordStreak.HorizontalAlignment = HorizontalAlignment.Center;
                    recordStreak.VerticalAlignment = VerticalAlignment.Center;
                    recordStreak.FontSize = 10.667;
                    Grid.SetColumn(recordStreak, 6);
                    Grid.SetRow(recordStreak, loop);
                    this.mainGrid.Children.Add(recordStreak);
                    inGrid.Add(recordStreak);

                    //Edit
                    Button edit = new Button();
                    edit.Click += (sender, args) =>
                        Frame.Navigate(typeof(HabbitEdit), habbit);
                    edit.Content = "Edit!";
                    edit.HorizontalAlignment = HorizontalAlignment.Center;
                    edit.VerticalAlignment = VerticalAlignment.Center;
                    edit.Height = 25;
                    edit.FontSize = 8;
                    edit.Background = new SolidColorBrush(Colors.LightBlue);
                    Grid.SetColumn(edit, 0);
                    Grid.SetRow(edit, loop);
   
                    this.mainGrid.Children.Add(edit);
                    inGrid.Add(edit);
                    loop++;

                }
            }
        }

        public async Task<String> sendRequest(String request, String data)
        {
            using (var client = new HttpClient() { BaseAddress = baseAddress })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"]);
                var result = await client.PostAsync(request, new StringContent( data));

                Debug.WriteLine(result);
                return result.Content.ReadAsStringAsync().Result;
            }

        }

        private void login_Click(object sender, RoutedEventArgs e) {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["tokin"] = "";
            this.Frame.Navigate(typeof(Auth));
        }


        private void create_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(Create));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            getHabits();
        }
    }
}
