using System;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    public partial class App : Application
    {
        private int scanningfreq = 225;
        private bool alertActive = false;
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
                    processScan(result);
                });

            scanPage.Title = "QRphony";
            MainPage = new NavigationPage(scanPage);
            
        }

        private void processScan(ZXing.Result r)
        {
            if (r == null || r.Text == null)
                return;
            if (r.Text.StartsWith("QRMI"))
                qrprocessor.addToQRValidatorQueue(r);
            else
                openDisplayAlert(r.Text);
        }

        private void openDisplayAlert(string str)
        {
            if (!alertActive)
            {
                alertActive = true;
                Device.BeginInvokeOnMainThread(() => displayAlert(str));
            }
        }

        private async void displayAlert(string str)
        {
            if (Uri.IsWellFormedUriString(str, UriKind.Absolute))
            {
                if (await scanPage.DisplayAlert("Navigate to Link?", str, "Yes", "No"))
                {
                    Device.OpenUri(new Uri(str));
                }
            }
            else
            {
                await scanPage.DisplayAlert("Invalid QR Data", str, "Close");
            }
            alertActive = false;
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
