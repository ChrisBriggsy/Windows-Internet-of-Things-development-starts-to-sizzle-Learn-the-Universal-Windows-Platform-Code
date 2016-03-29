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
            var sheild = new SparkFun.SparkFunWeatherSheild();
            if (await sheild.Setup()) { 
            Temperature.Text = sheild.Temperature.ToString();
                Humdity.Text = sheild.Humidity.ToString();
            }
        }

    }
}
