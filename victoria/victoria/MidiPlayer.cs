using System;

using System.Threading;
using Org.Billthefarmer.Mididriver;
using System.Collections.Concurrent;
using Android.Util;

namespace MidiPlayer
{
    public class MidiPlayer
    {
        protected MidiDriver midi;
        BlockingCollection<Action> midiEventQueue = new BlockingCollection<Action>();

        public MidiPlayer()
        {
            midi = new MidiDriver();
            midi.Start();
            Thread thread = new Thread(() => midiEventThread());
            thread.Start();
        }

        private void midiEventThread()
        {
            while (true)
            {
                midiEventQueue.Take()();
            }
        }

        private void processRawBytes(string[] s)
        {
            for (int i = 1; i < s.GetLength(0) - 1; i++) {
                byte[] b = System.Convert.FromBase64String(s[i]);
                Log.Info("msg length", ((Java.Lang.Integer)b.GetLength(0)).ToString());
                MidiEvent meve = new MidiEvent(b, midi);
                midiEventQueue.Add(meve.runEvent);
            }
        }

        public void addToQueue(string[] s)
        {
            Thread ByteProcessor = new Thread(() => processRawBytes(s));
            ByteProcessor.Start();
        }

        public void playCNote()
        {
            sendMidi(0x90, 48, 63);
            sendMidi(0x90, 52, 63);
            sendMidi(0x90, 55, 63);

            sendMidi((int)0x90 + 1, 48, 63);
            sendMidi((int)0x90 + 1, 52, 63);
            sendMidi((int)0x90 + 1, 55, 63);
        }


        protected void sendMidi(int m, int n, int v)
        {
            midi.Write(new byte[] { (byte)m, (byte)n, (byte)v });
        }

        public class MidiEvent
        {
            private int time;
            private byte[] msg;
            private MidiDriver midi;
            public MidiEvent(byte[] b, MidiDriver midi)
            {
                this.midi = midi;
                this.time = (0xFF & b[0]) | ((0xFF & b[1]) << 8) | ((0xFF & b[2]) << 16) | (0xFF & b[3] << 24);
                this.msg = new byte[] { b[4], b[5], b[6] };
            }

            public void runEvent()
            {
                Thread.Sleep(this.time);
                midi.Write(msg);                                             
            }
        }
    }
}