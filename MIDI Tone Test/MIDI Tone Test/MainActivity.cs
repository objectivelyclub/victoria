using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.IO;

namespace MIDI_Tone_Test
{
    [Activity(Label = "MIDI_Tone_Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            JetPlayer jetPlayer = JetPlayer.GetJetPlayer();
            //jetPlayer.LoadJetFile("/storage/AE2D-8B85/jone.jet");
            JetPlayer jetPlayer2 = JetPlayer.GetJetPlayer();
            jetPlayer2.LoadJetFile("/storage/AE2D-8B85/jone2.jet");
            sbyte segmentId = 0;

           // bool exists = File.Exists("/storage/AE2D-8B85/jone.jet");

            while (true)
            {
                jetPlayer2 = JetPlayer.GetJetPlayer();
                jetPlayer2.LoadJetFile("/storage/AE2D-8B85/jone2.jet");
                jetPlayer2.QueueJetSegment(1, -1, 10, -1, 0, segmentId++);
                jetPlayer2.Play();
                System.Threading.Thread.Sleep(2000);
                jetPlayer2.Pause();
                jetPlayer2.Release();

                jetPlayer2 = JetPlayer.GetJetPlayer();
                jetPlayer2.LoadJetFile("/storage/AE2D-8B85/jone2.jet");
                jetPlayer2.QueueJetSegment(1, -1, 10, -1, 0, segmentId++);
                jetPlayer2.Play();
                System.Threading.Thread.Sleep(2000);
                jetPlayer2.Pause();
                jetPlayer2.Release();
            }

            bool here = false;


            //jetPlayer2.LoadJetFile("/storage/AE2D-8B85/jone.jet");
            //jetPlayer2.QueueJetSegment(4, -1, 10, -1, 0, segmentId++);
            //jetPlayer.Pause();
            //jetPlayer2.Play();


        }
    }
}

