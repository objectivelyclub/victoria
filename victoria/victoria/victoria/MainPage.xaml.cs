using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    public partial class MainPage : ContentPage
    {
        ZXingScannerPage scanPage;
        MidiPlayer midiplayer;
        int scanningfreq = 150;

        public MainPage()
        {
            InitializeComponent();
            midiplayer = new MidiPlayer();
        }

        void OnRefreshRateChanged(object sender, ValueChangedEventArgs args)
        {
            scanningfreq = (int) (args.NewValue * 3000 + 100);
            refreshRateLabel.Text = scanningfreq.ToString("F3") + " ms";
        }

        void TestMidi(object sender, EventArgs args)
        {
            midiplayer.testMidiPlayer();
        }

        async void StartContinuousScanning(object sender, EventArgs args)
        {
            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = scanningfreq;

            scanPage = new ZXingScannerPage(opt);
            scanPage.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(() =>
                   midiplayer.addToQRValidatorQueue(result));


            var rootPage = new NavigationPage(scanPage);
            await Navigation.PushAsync(rootPage);
        }
    }
}
