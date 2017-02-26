using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace victoria
{
    [Activity(Label = "victoria", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Vibrator vib = (Vibrator)this.ApplicationContext.GetSystemService(Context.VibratorService);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button playNoteButton = FindViewById<Button>(Resource.Id.playNoteButton);
            Button printButton = FindViewById<Button>(Resource.Id.printButton);
            Button cindysButton = FindViewById<Button>(Resource.Id.cindysButton);
            TextView txt = FindViewById<TextView>(Resource.Id.textView1);
            Button singleScanButton = FindViewById<Button>(Resource.Id.singlescan);
            Button continousScanButton = FindViewById<Button>(Resource.Id.contscan);

            MidiPlayer.MidiPlayer midiPlayer = new MidiPlayer.MidiPlayer();
            playNoteButton.Click += (sender, e) =>
            {
                midiPlayer.playCNote();
            };

            Scanner.Scanner scanner = new Scanner.Scanner(singleScanButton, continousScanButton, txt, Application, midiPlayer, vib);

        }


    }
}

