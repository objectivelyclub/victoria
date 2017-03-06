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
using System.Collections.Concurrent;
using System.Threading;

[assembly: Dependency(typeof(MidiDevice_Android))]

namespace victoria.Droid
{
    public class MidiDevice_Android : iMidiDevice
    {
        private MidiDriver midi;
        BlockingCollection<int> timearray = new BlockingCollection<int>();
        BlockingCollection<byte[]> notes = new BlockingCollection<byte[]>();
        public MidiDevice_Android()
        {
            midi = new MidiDriver();
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(timearray.Take());
                    midi.Write(notes.Take());
                }
            });
            t.Start();
        }

        public void MIDIQueuer(int time, byte[] b)
        {
            timearray.Add(time);
            notes.Add(b);
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