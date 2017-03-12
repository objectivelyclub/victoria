using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    public partial class App : Application
    {
        private int scanningfreq = 225;
        private MidiPlayer midiplayer;
        private QRprocessor qrprocessor;
        private ContentView overlay;
        private ZXingScannerPage scanPage;

        public App()
        {
            InitializeComponent();
            overlay = new MainOverlay();
            midiplayer = new MidiPlayer();
            
            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = scanningfreq;
            scanPage = new ZXingScannerPage(opt, overlay);

            qrprocessor = new QRprocessor(midiplayer, scanPage);
            
            scanPage.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(() => {
                    qrprocessor.addToQRValidatorQueue(result);
                });

            scanPage.Title = "QRphony";
            MainPage = new NavigationPage(scanPage);
            
        }

      /*  private void processScan(ZXing.Result r)
        {
            qrprocessor.addToQRValidatorQueue(r);
            if (!displayActive)
            {
                displayActive = true;
                scanPage.DisplayAlert("new qr", "so excitin", "not");
            }

        }
        */
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
