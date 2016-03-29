using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Autofac;
using Autofac.Core;
using SparkFun;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTBackUp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public IContainer Container { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            var builder = new ContainerBuilder();

            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.IoT")
            {
                builder.RegisterType<SparkFunWeatherSheild>().As<ISparkFunWeatherSheild>();
            }
            else
            {
                builder.RegisterType<DesktopWeather>().As<ISparkFunWeatherSheild>();
            }

            Container = builder.Build();

            Display();
        }

        private async void Display()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var sheild = scope.Resolve<ISparkFunWeatherSheild>();

                if (await sheild.Setup())
                {
                    Temperature.Text = sheild.Temperature.ToString();
                    Humdity.Text = sheild.Humidity.ToString();
                }
            }
        }

    }
}
