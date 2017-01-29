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


            /*
            JetPlayer jetPlayer = JetPlayer.GetJetPlayer();
            //jetPlayer.LoadJetFile("/storage/AE2D-8B85/jone.jet");
            JetPlayer jetPlayer2 = JetPlayer.GetJetPlayer();
            jetPlayer2.LoadJetFile("/storage/AE2D-8B85/jone2.jet");
            sbyte segmentId = 0;

           // bool exists = File.Exists("/storage/AE2D-8B85/jone.jet");

            while (false)
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

    */
            bool exists = File.Exists("/storage/AE2D-8B85/rey.mid");
            bool exists2 = File.Exists("/storage/AE2D-8B85/ww.mp3");

            AudioAttributes attributes = new AudioAttributes.Builder()
    .SetUsage(AudioUsageKind.Game)
    .SetContentType(AudioContentType.Music)
    .Build();

          //  SoundPool sp = new SoundPool.Builder().SetAudioAttributes(attributes).Build();


            SoundPool sp = new SoundPool(2, Android.Media.Stream.Music, 0);


            SoundPool sp1 = new SoundPool(2, Android.Media.Stream.Music, 0);


            byte[] bytes = System.IO.File.ReadAllBytes("/storage/AE2D-8B85/rey.mid");


            File outputDir = context.getCacheDir();

            System.IO.File.WriteAllBytes()



            int reyid = sp.Load("/storage/AE2D-8B85/rey.mid",1);
            int wwid = sp1.Load("/storage/AE2D-8B85/ww.mp3", 1);
            System.Threading.Thread.Sleep(1000);
            //sp.Play(wwid, 1, 1, 1, 0, 1f);
            sp.Play(reyid, 1, 1, 2, 0, 1f);

            System.Threading.Thread.Sleep(2000);
            sp.Pause(reyid);
            sp1.Play(wwid, 1, 1, 2, 0, 1f);
            bool here1 = true;

            while (true ){
                System.Threading.Thread.Sleep(1800);
            }

        }
    }
}

