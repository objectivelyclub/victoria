using Xamarin.Forms;
using victoria.Droid;
using Org.Billthefarmer.Mididriver;
using System.Collections.Concurrent;
using System.Threading;
using System.Timers;

[assembly: Dependency(typeof(MidiDevice_Android))]

namespace victoria.Droid
{
    public class MidiDevice_Android : iMidiDevice
    {
        private MidiDriver midi;
        private BlockingCollection<int> timearray = new BlockingCollection<int>();
        private BlockingCollection<byte[]> notes = new BlockingCollection<byte[]>();

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

        public void Stop()
        {
            midi.Stop();
            while (notes.Count > 0)
            {
                notes.Take();
            }
            while (timearray.Count > 0)
            {
                timearray.Take();
            }
        }

        public void Write(byte[] v)
        {
            midi.Write(v);
        }
    }
}