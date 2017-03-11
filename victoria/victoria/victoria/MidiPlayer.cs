using System.IO;
using Xamarin.Forms;

namespace victoria
{
    class MidiPlayer
    {
        private iMidiDevice midi = DependencyService.Get<iMidiDevice>();
        private iUtils utils = DependencyService.Get<iUtils>();

        public MidiPlayer()
        {
            onResume();
        }
        
        public void processChannels(byte[] data)
        {
            for (int i = 3; i<19; i++)
            {
                byte[] pc = new byte[] { (byte)((12 << 4) | i), data[i] };
                midi.Write(pc);
                utils.Sleep(25);
            }
        }
        
        public void processRawBytes(BinaryReader b)
        {
            int numOfMsgs = b.ReadByte();
            for (int i = 0; i < numOfMsgs ; i++)
            {
                int ms = (0xFF & b.ReadByte()) | ((0xFF & b.ReadByte()) << 8);
                byte b1 = b.ReadByte();
                byte[] msg;
                if (((b1 | 0x10) & 0xF0) == 0xD0)
                {
                    msg = new byte[] { b1, b.ReadByte() };
                } else
                {
                    msg = new byte[] {b1, b.ReadByte(), b.ReadByte() };
                }
                midi.MIDIQueuer(ms, msg);
            }
        }

        public void resetPlayer()
        {
            onPause();
            onResume();
        }

        public void onPause()
        {
            if (midi != null)
                midi.Stop();
        }

        public void onResume()
        {
            if (midi !=null)
                midi.Start();
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

        public void testMidiPlayer()
        {
            //Set channels
            //midi.Write(new byte[] { 0xC0, 75 });
            /*midi.Write(new byte[] { 0xC1, 75 });
            midi.Write(new byte[] { 0xC2, 75 });
            midi.Write(new byte[] { 0xC3, 75 });
            midi.Write(new byte[] { 0xC4, 75 });
            midi.Write(new byte[] { 0xC5, 75 });
            midi.Write(new byte[] { 0xC6, 75 });
            midi.Write(new byte[] { 0xC7, 75 });
            midi.Write(new byte[] { 0xC8, 75 });
            midi.Write(new byte[] { 0xC9, 75 });
            midi.Write(new byte[] { 0xCA, 75 });
            midi.Write(new byte[] { 0xCB, 75 });
            midi.Write(new byte[] { 0xCC, 75 });
            midi.Write(new byte[] { 0xCD, 75 });
            midi.Write(new byte[] { 0xCE, 75 });
            midi.Write(new byte[] { 0xCF, 75 });*/
            //byte[] inst = new byte[] { };

            
            byte pc = 0xC0;
            for (byte i = 0; i<15; i++)
            {
                byte str = (byte) ( 18 + i);
                midi.Write(new byte[] { pc, str });
                sendMidi(0x90, 48, 63);
                utils.Sleep(100);
                sendMidi(0x90, 52, 63);
                //utils.Sleep(1000);
                sendMidi(0x80, 48, 63);
                //utils.Sleep(100);
                sendMidi(0x80, 52, 63);
            }
            utils.Sleep(100);
            //sendMidi(0x90, 70, 63);
           /* sendMidi(0x91, 52, 63);
            sendMidi(0x92, 55, 63);
            sendMidi(0x93, 48, 63);
            sendMidi(0x94, 52, 63);
            sendMidi(0x95, 55, 63);
            sendMidi(0x96, 48, 63);
            sendMidi(0x97, 52, 63);
            sendMidi(0x98, 55, 63);
            sendMidi(0x99, 48, 63);
            sendMidi(0x9A, 52, 63);
            sendMidi(0x9B, 55, 63);
            sendMidi(0x9C, 48, 63);
            sendMidi(0x9D, 52, 63);
            sendMidi(0x9E, 55, 63);
            sendMidi(0x9F, 48, 63);
            sendMidi(0x90, 52, 63);
            sendMidi(0x90, 55, 63);
            */
            //utils.Sleep(1000);
            /*
            sendMidi(0x90, 48, 63);
            sendMidi(0x91, 52, 63);
            sendMidi(0x92, 55, 63);
            sendMidi(0x93, 48, 63);
            sendMidi(0x94, 52, 63);
            sendMidi(0x95, 55, 63);
            sendMidi(0x96, 48, 63);
            sendMidi(0x97, 52, 63);
            sendMidi(0x98, 55, 63);
            sendMidi(0x99, 48, 63);
            sendMidi(0x9A, 52, 63);
            sendMidi(0x9B, 55, 63);
            sendMidi(0x9C, 48, 63);
            sendMidi(0x9D, 52, 63);
            sendMidi(0x9E, 55, 63);
            sendMidi(0x9F, 48, 63);
            sendMidi(0x90, 52, 63);
            sendMidi(0x90, 55, 63);
            */


        }

        protected void sendMidi(int m, int n, int v)
        {
            utils.Sleep(0);
            midi.Write(new byte[] { (byte)m, (byte)n, (byte)v });
        }
    }
}