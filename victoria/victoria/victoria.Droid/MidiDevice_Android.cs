using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using victoria.Droid;
using Org.Billthefarmer.Mididriver;

[assembly: Dependency(typeof(MidiDevice_Android))]

namespace victoria.Droid
{
    public class MidiDevice_Android : iMidiDevice
    {
        private MidiDriver midi;
        public MidiDevice_Android()
        {
            midi = new MidiDriver();            
        }

        public void Start()
        {
            midi.Start();
        }

        public void Write(byte[] v)
        {
            midi.Write(v);
        }
    }
}