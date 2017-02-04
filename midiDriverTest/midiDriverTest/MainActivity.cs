using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using Org.Billthefarmer.Mididriver;
using System.Threading;
using Android.Views;
using System;
using Android.Util;

namespace midiDriverTest
{
    [Activity(Label = "midiDriverTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, MidiDriver.IOnMidiStartListener, View.IOnTouchListener
    {
        private MidiDriver midiDriver;
        private byte[] even;
        private int[] config;
        private Button buttonPlayNote;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            buttonPlayNote = FindViewById<Button>(Resource.Id.button1);
            buttonPlayNote.SetOnTouchListener(this);

            // Instantiate the driver.
            midiDriver = new MidiDriver();
            // Set the listener.
            midiDriver.SetOnMidiStartListener(this);
            playNote();
        }

        protected void onResume()
        {
            base.OnResume();
            midiDriver.Start();

            // Get the configuration.
            config = midiDriver.Config();

            // Print out the details.
            
            //Log.Debug(this.Class.Name, "maxVoices: " + config[0]);
            //Log.Debug(this.Class.Name, "numChannels: " + config[1]);
            //Log.Debug(this.Class.Name, "sampleRate: " + config[2]);
            //Log.Debug(this.Class.Name, "mixBufferSize: " + config[3]);
        }

        protected void onPause()
        {
            base.OnPause();
            midiDriver.Stop();
        }

        public void OnMidiStart()
        {
            //Log.Debug(this.Class.Name, "onMidiStart()");
        }

        private void playNote()
        {

        // Construct a note ON message for the middle C at maximum velocity on channel 1:
        even = new byte[3];
        even[0] = (byte) (0x90 | 0x00);  // 0x90 = note On, 0x00 = channel 1
        even[1] = (byte) 0x3C;  // 0x3C = middle C
        even[2] = (byte) 0x7F;  // 0x7F = the maximum velocity (127)

        // Internally this just calls write() and can be considered obsoleted:
        //midiDriver.queueEvent(event);

        // Send the MIDI event to the synthesizer.
        midiDriver.Write(even);

        }

        private void stopNote()
        {

        // Construct a note OFF message for the middle C at minimum velocity on channel 1:
        even = new byte[3];
        even[0] = (byte) (0x80 | 0x00);  // 0x80 = note Off, 0x00 = channel 1
        even[1] = (byte) 0x3C;  // 0x3C = middle C
        even[2] = (byte) 0x00;  // 0x00 = the minimum velocity (0)

        // Send the MIDI event to the synthesizer.
        midiDriver.Write(even);

        }

        public Boolean OnTouch(View v, MotionEvent even) {

            //Log.Debug(this.Class.Name, "Motion event: " + even);
            
        if (v.Id == (Resource.Id.button1)) {
            if (even.ActionMasked == MotionEventActions.Down) {
            //Log.Debug(this.Class.Name, "MotionEvent.ACTION_DOWN");
            playNote();
        }
            if (even.ActionMasked == MotionEventActions.Up) {
            //Log.Debug(this.Class.Name, "MotionEvent.ACTION_UP");
            stopNote();
        }
        }
        
        return false;
    }





        /*
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            midi = new MidiDriver();

            midi.SetOnMidiStartListener(this);
            //player = MediaPlayer.Create(this, Resource.Raw.ants);
            //player.Start();
            //Thread.Sleep(23000);
            //sendMidi(0xc0, 6);
            //Thread.Sleep(300);
            //sendMidi(0x90, 48, 63);
            //Thread.Sleep(300);
            //sendMidi(0x80, 59, 0);
            //Thread.Sleep(300);
            //onResume();


            //sendMidi(0xc0, 6);
            //Thread.Sleep(300);
            //sendMidi(0x90, 48, 63);
            //Thread.Sleep(300);
            //sendMidi(0x80, 59, 0);
            //Thread.Sleep(300);
            //sendMidi(0x92, 0x10, 0x20);
            playC();

        }

        protected void onResume()
        {
            base.OnResume();

            // Start midi

            if (midi != null)
                midi.Start();
        }

        protected void onPause()
        {
            base.OnPause();

            // Stop midi

            if (midi != null)
                midi.Stop();

            // Stop player

            if (player != null)
                player.Stop();
        }


        public void playC()
        {
            sendMidi(0x90, 48, 63);
            sendMidi(0x90, 52, 63);
            sendMidi(0x90, 55, 63);
        }



        public void OnMidiStart()
        {
            sendMidi(0xc0, 6);

            int[] config = midi.Config();

        }


        protected void sendMidi(int m, int p)
        {
            byte[] msg = new byte[2];

            msg[0] = (byte)m;
            msg[1] = (byte)p;

            midi.Write(msg);
        }
        protected void sendMidi(int m, int n, int v)
        {
            byte[] msg = new byte[3];

            msg[0] = (byte)m;
            msg[1] = (byte)n;
            msg[2] = (byte)v;

            midi.Write(msg);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            throw new NotImplementedException();
        }

    */
    }
}

