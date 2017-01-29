using Android.App;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using System.IO;
using System;

namespace master
{
    [Activity(Label = "master", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button playNoteButton = FindViewById<Button>(Resource.Id.playNoteButton);
            Button printButton = FindViewById<Button>(Resource.Id.printButton);
            Button cindysButton = FindViewById<Button>(Resource.Id.cindysButton);
            TextView txt = FindViewById<TextView>(Resource.Id.textView1);
            Button singleScanButton = FindViewById<Button>(Resource.Id.singlescan);
            Button continousScanButton = FindViewById<Button>(Resource.Id.contscan);

            AudioSynth.AudioSynth audiosynth = new AudioSynth.AudioSynth();
            Scanner.Scanner scanner = new Scanner.Scanner(continousScanButton, Application);

           
            singleScanButton.Click += async (sender, e) => {

                // Initialize the scanner first so it can track the current context
                MobileBarcodeScanner.Initialize(Application);

                var scan = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scan.Scan();

                if (result != null)

                    txt.SetText((result.Text).ToCharArray(), 0, 5);
            };
            
            

            

        }

        
    }
}

