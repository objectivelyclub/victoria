using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using ZXing.Mobile;


namespace Scanner
{
    public class Scanner
    {
        protected Application app;
        protected TextView txt;
        protected MidiPlayer.MidiPlayer midiplayer;
        private BlockingCollection<string[]> dataAnalysisQueue;
        Vibrator vib;
        string[] msgs;
        byte current_byte = 213;
        byte[] b0;

        public Scanner(Button Single, Button Continuous, TextView txt, Application app, MidiPlayer.MidiPlayer midiplayer, Vibrator vib)
        {
            this.vib = vib;
            this.app = app;
            this.midiplayer = midiplayer;
            dataAnalysisQueue = new BlockingCollection<string[]>();

            Thread DataSender = new Thread(() => uniqueDataSender());
            DataSender.Start();

            /*
            Single.Click += async (sender, e) => {

                // Initialize the scanner first so it can track the current context
                MobileBarcodeScanner.Initialize(app);

                var scan = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scan.Scan();

                if (result != null)
                {
                    string s = result.ToString();

                    string[] msgs = s.Split('\n');
                    byte[] b0 = System.Convert.FromBase64String(msgs[0]);
                    if (b0 != current_byte)
                    {
                        current_byte = b0;
                        midiplayer.addToQueue(msgs);
                    }
                }

            };
            */
            Continuous.Click +=  (sender, e) =>
            {
                MobileBarcodeScanner.Initialize(app);
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                var opt = new MobileBarcodeScanningOptions();
                opt.DelayBetweenContinuousScans = 100;
                
                scanner.TopText = "Wait for the barcode to automatically scan!";
                scanner.BottomText = "Tap to Focus";

                //Start scanning
                scanner.ScanContinuously(opt, HandleScanResult);
            };

        }

        private void HandleScanResult(ZXing.Result r)
        {
            if (r != null)
            {
                dataAnalysisQueue.Add(r.ToString().Split('\n'));
                //this.vib.Vibrate(25);
                /*msgs = r.ToString().Split('\n');
                b0 = System.Convert.FromBase64String(msgs[0]);
                if (b0[0] != current_byte)
                {
                    current_byte = b0[0];
                    midiplayer.addToQueue(msgs);
                }
                this.vib.Vibrate(25);*/
            }
        }

        private void uniqueDataSender()
        {
            while (true)
            {
                msgs = dataAnalysisQueue.Take();
                b0 = System.Convert.FromBase64String(msgs[0]);
                if (b0[0] != current_byte)
                {
                    current_byte = b0[0];
                    midiplayer.addToQueue(msgs);
                    this.vib.Vibrate(55);
                }
                this.vib.Vibrate(25);
            }
        }
            

}
}

