using AudioUnit;
using AVFoundation;
using Foundation;
using System;
using System.Collections.Concurrent;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(victoria.iOS.MidiDevice_iOS))]

namespace victoria.iOS
{
    class MidiDevice_iOS : iMidiDevice
    {
        BlockingCollection<int> timearray = new BlockingCollection<int>();
        BlockingCollection<byte[]> notes = new BlockingCollection<byte[]>();

        Synth s;

        public MidiDevice_iOS()
        {
            s = new Synth();
            startProcessingThread();
        }

        public void startProcessingThread()
        {
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(timearray.Take());
                    byte[] note = notes.Take();
                    Write(note);
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
            
        }

        public void Write(byte[] v)
        {
            s.Write(v);     
        }
    }
}
