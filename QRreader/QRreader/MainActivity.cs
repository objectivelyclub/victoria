using Android.App;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using System.IO;
using System;
using ZXing;
using Android.Media;

namespace QRreader
{
    [Activity(Label = "QRreader", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.button1);

            var txt = FindViewById<TextView>(Resource.Id.textView1);

            int reyid = sp.Load("/storage/AE2D-8B85/rey.mid", 1);
            System.Threading.Thread.Sleep(1000);

            sp.Play(reyid, 1, 1, 2, 0, 1f);

            bool temp = true;

            //continuousScan();

            button.Click += async (sender, e) => {

                // Initialize the scanner first so it can track the current context
                MobileBarcodeScanner.Initialize(Application);

                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scanner.Scan();

                if (result != null)
                    
                    txt.SetText((result.Text).ToCharArray(), 0 , 16);
            };
        }

        protected void continuousScan()
        {
            MobileBarcodeScanner.Initialize(Application);
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = 5000;

            //Start scanning
            scanner.ScanContinuously(opt, HandleScanResult);
            scanner.BottomText = "Wait for the barcode to automatically scan!";
        }

        private void HandleScanResult(ZXing.Result obj)
        {


            //SoundPool sp1 = new SoundPool(2, Android.Media.Stream.Music, 0);


            //byte[] bytes = System.IO.File.ReadAllBytes("/storage/AE2D-8B85/rey.mid");


            //File outputDir = context.getCacheDir();

            //System.IO.File.WriteAllBytes()



            
            System.Threading.Thread.Sleep(1000);

            sp.Play(1, 1, 1, 2, 0, 1f);
            bool temp = true;
        }

        protected void playMSG(ZXing.Result r)
        {
            byte[] bytes = System.IO.File.ReadAllBytes("/storage/AE2D-8B85/rey.mid");
            //File outputDir = context.getCacheDir();

            //System.IO.File.WriteAllBytes()

            //File.WriteAllBytes()
            //r.RawBytes
        }

        SoundPool sp = new SoundPool(10, Android.Media.Stream.Music, 0);

    }
}

