using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTBackUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Display();
        }

        private async void Display()
        {
            var data = await GetData();
            Temperature.Text = data.main.temp.ToString();
            Humdity.Text = data.main.humidity.ToString();
        }

        public async Task<OpenWeatherMapData> GetData()
        {
            var httpClient = new HttpClient();
            
            var APPID = "ReplaceWithYourVaildKey";
            //Since I first gave this talk OpenWeatherMap have changed their API policy 
            //Therefore if you want to run this sample, sign up for a free account and replace the APPID
            // http://openweathermap.org/faq#error401

            var response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=Melbourne&units=metric&APPID=" + APPID);
            return JsonConvert.DeserializeObject<OpenWeatherMapData>(await response.Content.ReadAsStringAsync());
        }
    }
}
