using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeatherApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            appView.Title = "Local Weather";
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var position = await LocationManager.GetPosition();
                RootObject myWeatherImperial = await OpenWeatherMapProxy.GetWeatherImperial(position.Coordinate.Latitude, position.Coordinate.Longitude);
                RootObject myWeatherMetric = await OpenWeatherMapProxy.GetWeatherMetric(position.Coordinate.Latitude, position.Coordinate.Longitude);

                string icon = String.Format("ms-appx:///Assets/Weather/{0}.png", myWeatherImperial.weather[0].icon);
                ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
                TempTextBlock.Text = ((int)myWeatherImperial.main.temp).ToString() + "°F / " + ((int)myWeatherMetric.main.temp).ToString() + "°C";
                FeelsLikeTextBlock.Text = "(Feels Like " + ((int)myWeatherImperial.main.feels_like).ToString() + "°F / " + ((int)myWeatherMetric.main.feels_like).ToString() + "°C)";
                CloudTextBlock.Text = "Cloud Cover: " + myWeatherImperial.clouds.all.ToString() + "%";
                DescriptionTextBlock.Text = myWeatherImperial.weather[0].description;
                LocationTextBlock.Text = myWeatherImperial.name;
                LatLongTextBlock.Text = position.Coordinate.Latitude.ToString("F4") + ", " + position.Coordinate.Longitude.ToString("F4");
                WeatherStackPanel.Background = new SolidColorBrush(getBackgroundColor(myWeatherImperial.main.temp));
            }
            catch
            {
                LocationTextBlock.Text = "Unable to get weather at this time";
            }

        }

        private Color getBackgroundColor(double temp)
        {
            Color tempColorCase;

            if (temp < 33)
            {
                tempColorCase = Color.FromArgb(255, 95, 158, 160);
            }
            else if (temp >= 33 && temp < 60)
            {
                tempColorCase = Color.FromArgb(255, 224, 255, 255);
            }
            else if (temp >= 60 && temp < 80)
            {
                tempColorCase = Color.FromArgb(255, 238, 232, 170);
            }
            else if (temp >= 80 && temp < 100)
            {
                tempColorCase = Color.FromArgb(255, 255, 165, 0);
            }
            else if (temp >= 100)
            {
                tempColorCase = Color.FromArgb(255, 255, 69, 0);
            }
            else
            {
                tempColorCase = Color.FromArgb(255, 211, 211, 211);
            }

            return tempColorCase;
        }
    }
}
