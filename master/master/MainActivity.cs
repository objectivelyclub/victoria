using Android.App;
using Android.Widget;
using Android.OS;
//using AudioSynth;

namespace master
{
    [Activity(Label = "master", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Button playNoteButton = FindViewById<Button>(Resource.Id.playNoteButton);
            Button printButton = FindViewById<Button>(Resource.Id.printButton);
            Button cindysButton = FindViewById<Button>(Resource.Id.cindysButton);

            AudioSynth.AudioSynth audiosynth = new AudioSynth.AudioSynth();

            



            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }

        
    }
}

