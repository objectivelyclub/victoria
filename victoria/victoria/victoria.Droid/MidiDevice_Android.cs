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
        private bool midiStarted = false;
        private Thread eventDriverTH;
        private BlockingCollection<int> timearray = new BlockingCollection<int>();
        private BlockingCollection<byte[]> notes = new BlockingCollection<byte[]>();

        public MidiDevice_Android()
        {
            midi = new MidiDriver();
        }

        private void startEventDriver()
        {
            eventDriverTH = new Thread(() =>eventDriver());
            eventDriverTH.Start();
        }

        private void stopEventDriver()
        {
            eventDriverTH.Abort();
        }

        private void eventDriver()
        {
            while (midiStarted)
            {
                Thread.Sleep(timearray.Take());
                midi.Write(notes.Take());
            }
        }

        public void MIDIQueuer(int time, byte[] b)
        {
            if (midiStarted)
            {
                timearray.Add(time);
                notes.Add(b);
            }
        }

        public void Start()
        {
            midi.Start();
            midiStarted = true;
            startEventDriver();
        }

        public void Stop()
        {
            midi.Stop();
            midiStarted = false;
            stopEventDriver();
            notes = new BlockingCollection<byte[]>();
            timearray = new BlockingCollection<int>();
        }

        public void Write(byte[] v)
        {
            midi.Write(v);
        }
    }
}