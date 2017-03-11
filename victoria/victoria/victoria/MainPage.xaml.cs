using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    public partial class MainPage : ContentPage
    {

        ZXingScannerPage scanPage;

        public MainPage()
        {
            InitializeComponent();

            var opt = new MobileBarcodeScanningOptions();

            scanPage = new ZXingScannerPage(opt);
            scanPage.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(() => {
                    //midiplayer.testMidiPlayer();

                });


            var rootPage = new NavigationPage(scanPage);
            Navigation.PushAsync(rootPage);


        }

        void OnRefreshRateChanged(object sender, ValueChangedEventArgs args)
        {
           // scanningfreq = (int) (args.NewValue * 3000 + 100);
            //refreshRateLabel.Text = scanningfreq.ToString("F3") + " ms";
        }

        void TestMidi(object sender, EventArgs args)
        {
            //midiplayer.testMidiPlayer();
        }

        async void StartContinuousScanning(object sender, EventArgs args)
        {
            await startScanner();
        }
        
        async Task startScanner()
        {
            var opt = new MobileBarcodeScanningOptions();
            
            scanPage = new ZXingScannerPage(opt);
            scanPage.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(() => {
                    //midiplayer.testMidiPlayer();
                    //qrprocessor.addToQRValidatorQueue(result);

                });


            var rootPage = new NavigationPage(scanPage);
            await Navigation.PushAsync(rootPage);
        }
    }
}
