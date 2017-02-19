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
    public class MainActivity : Activity, View.IOnTouchListener
    {
        private MidiDriver midiDriver;
        private byte[] even;
        private Button buttonPlayNote;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            buttonPlayNote = FindViewById<Button>(Resource.Id.button1);
            buttonPlayNote.SetOnTouchListener(this);

            // Instantiate the driver.
            midiDriver = new MidiDriver();
            midiDriver.Start();
            playNote();
        }

        protected void onResume()
        {
            base.OnResume();
        }

        protected void onPause()
        {
            base.OnPause();
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
    }
}

