using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    public partial class MainOverlay : ContentView
    {
        
        public MainOverlay()
        {
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
 

            var rootPage = new NavigationPage();
            await Navigation.PushAsync(rootPage);
        }
    }
}
