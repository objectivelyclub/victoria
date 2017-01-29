using Android.App;
using Android.Widget;
using ZXing.Mobile;

namespace Scanner
{
    public class Scanner
    {
        protected Application app;
        protected TextView txt;

        public Scanner( Button Continuous, Application app)
        {
            this.app = app;


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

