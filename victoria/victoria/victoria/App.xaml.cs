using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    public partial class App : Application
    {
        //public MidiPlayer midiplayer;
        private int scanningfreq = 150;
        private MidiPlayer midiplayer;
        private QRprocessor qrprocessor;
        private Grid customOverlay;
        private BoxView b;

        public App()
        {
            InitializeComponent();
            midiplayer = new MidiPlayer();
            qrprocessor = new QRprocessor(midiplayer);

            
            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = scanningfreq;
            generateOverlay();
            //new ZXingScannerPage(opt, customOverlay);
            ZXingScannerPage scanPage = new ZXingScannerPage(opt, customOverlay);
            scanPage.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(() => {
                    qrprocessor.addToQRValidatorQueue(result);
                });
            
            MainPage = new NavigationPage(scanPage);
        }



        private void generateOverlay()
        {
            customOverlay = new Grid
            {
                RowDefinitions = {
                                new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star)},
                                new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                                new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star)}
                            },
            };

            customOverlay.Children.Add(new BoxView { BackgroundColor = Color.Black, Opacity = 0.5 }, 0, 0);
            customOverlay.Children.Add(new BoxView { BackgroundColor = Color.Black, Opacity = 0.5 }, 0, 2);
            b = new BoxView { BackgroundColor = Color.FromHex("004990"), VerticalOptions = LayoutOptions.Center, HeightRequest = 2, Opacity = 0.8 };
            customOverlay.Children.Add(b, 0, 1);

            customOverlay.Children.Add(new Label { Text = "Start scanning the animated QR", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, TextColor = Color.White }, 0, 0);
            customOverlay.Children.Add(new Label { Text = "Tap to Focus", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, TextColor = Color.White }, 0, 2);

        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            midiplayer.onPause();
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            midiplayer.onResume();
            // Handle when your app resumes
        }
    }
}
