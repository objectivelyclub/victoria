using Android.Util;
using Plugin.Vibrate;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace victoria
{
    class MidiPlayer
    {
        //QR Data Analysis Variables
        int QR_Num = 45;
        int current_song = 200;
        byte[] lastdata = new byte[] { };
        Plugin.Vibrate.Abstractions.IVibrate v = CrossVibrate.Current;

        //Midi player Varaibles
        private iMidiDevice midi = DependencyService.Get<iMidiDevice>();
        private iUtils utils = DependencyService.Get<iUtils>();

        public MidiPlayer()
        {
            midi.Start();
            utils.startNewThreadPool("MidiEventThread");
            utils.startNewThreadPool("QRValidatorThread");
        }

        public void addToQRValidatorQueue(ZXing.Result r)
        {
            utils.addToThreadPool("QRValidatorThread", new Action(() => QRValidator(r)));
        }

        private void processChannels(BinaryReader reader)
        {
            reader.ReadBytes(2);
            if (current_song == reader.PeekChar())
            {
                reader.ReadBytes(17);
                return;
            }
            current_song = reader.Read();
            for (int i = 0; i<16; i++)
            {
                byte[] pc = new byte[] { (byte)((12 << 4) | i), reader.ReadByte() };
                midi.Write(pc);
            }
            
        }

        private void QRValidator(ZXing.Result r)
        {
            if (r == null)
                return;

            byte[] data = System.Convert.FromBase64String(r.ToString());
            int this_Num = (data[0] << 8) | data[1];
            if (QR_Num == this_Num && current_song == data[3])
                return;

            BinaryReader reader;
            
            if (current_song != data[2])
            {
                v.Vibration(45);
                QR_Num = this_Num + 2;
                current_song = data[2];
                reader = new BinaryReader(new MemoryStream(data));
                processChannels(reader);
            }

            if (QR_Num != this_Num)
            {
                reader = new BinaryReader(new MemoryStream(data));
                QR_Num = this_Num;
                reader.ReadBytes(19);
                int datalength = reader.Read();
                //utils.startNewSelfTerminatingThread(processRawBytes, reader, datalength);
                processRawBytes(reader, datalength);
            }
        }

        private void processRawBytes(BinaryReader b, int numOfMsgs)
        {   
            for (int i = 0; i < numOfMsgs ; i++)
            {
                int ms = (0xFF & b.ReadByte()) | ((0xFF & b.ReadByte()) << 8) | ((0xFF & b.ReadByte()) << 16) | (0xFF & b.ReadByte() << 24);
                byte[] msg = b.ReadBytes(3);
                /*utils.addToThreadPool("MidiEventThread", new Action(() => {
                    utils.Sleep(ms);
                    midi.Write(msg);
                }));*/
                midi.MIDIQueuer(ms, msg);

            }
                
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
            //midi.Write(new byte[] { (byte)((12 << 4) | 0), });
            midi.Write(new byte[] { 0xC1, 35});
            midi.Write(new byte[] { 0xC2, 49 });
            midi.Write(new byte[] { 0xC3, 75 });
            midi.Write(new byte[] { 0xC4, 52 });
            midi.Write(new byte[] { 0xC5, 48 });
            midi.Write(new byte[] { 0xC6, 19 });
            midi.Write(new byte[] { 0xC7, 81});
            midi.Write(new byte[] { 0xC8, 30});
            //midi.Write(new byte[] { 0xC9, 0 });
            midi.Write(new byte[] { 0xCA, 42 });
            midi.Write(new byte[] { 0xCB, 26 });
            midi.Write(new byte[] { 0xCC, 60 });
            midi.Write(new byte[] { 0xCD, 11 });
            //midi.Write(new byte[] { (byte)((12 << 4) | 0), });
            //midi.Write(new byte[] { (byte)((12 << 4) | 0), });

            utils.Sleep(100);
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

            //utils.Sleep(1000);
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


        }

        protected void sendMidi(int m, int n, int v)
        {
            utils.Sleep(0);
            midi.Write(new byte[] { (byte)m, (byte)n, (byte)v });
        }
    }
}