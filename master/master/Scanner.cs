using Android.App;
using Android.Widget;
using ZXing.Mobile;

namespace Scanner
{
    public class Scanner
    {
        protected Application app;
        protected TextView txt;

        public Scanner(Button Single, Button Continuous, TextView txt, Application app, AudioSynth.AudioSynth audiosynth)
        {
            this.app = app;

            Single.Click += async (sender, e) => {

                // Initialize the scanner first so it can track the current context
                MobileBarcodeScanner.Initialize(app);

                var scan = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scan.Scan();

                if (result != null)
                    txt.SetText((result.Text).ToCharArray(), 0, result.Text.Length - 1);
                    audiosynth.PlaySingleQR(result.Text);
            };

        }


        public void ContinuousScan()
        {
            MobileBarcodeScanner.Initialize(app);
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = 5000;

            //Start scanning
            scanner.ScanContinuously(opt, HandleScanResult);
            scanner.BottomText = "Wait for the barcode to automatically scan!";
        }

        private void HandleScanResult(ZXing.Result r)
        {

        }


    }
}

