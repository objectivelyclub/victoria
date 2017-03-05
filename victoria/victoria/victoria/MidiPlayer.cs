using Android.Util;
using Plugin.Vibrate;
using System;
using System.IO;
using Xamarin.Forms;

namespace victoria
{
    class MidiPlayer
    {
        //QR Data Analysis Variables
        string[] msgs;
        byte lastProcessedQrId = 213;
        byte[] b0;
        int QR_Num = 45;
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

        private void QRValidator(ZXing.Result r)
        {
            if (r == null)
                return;

            byte[] dat = System.Convert.FromBase64String(r.ToString());
            int this_Num = (dat[0] << 8) | dat[1];

            if (QR_Num != this_Num)
            {
                QR_Num = this_Num;
                utils.startNewSelfTerminatingThread(processRawBytes, dat);
                v.Vibration(45);
            }
        }

        private void processRawBytes(byte[] b)
        {
            //b.Length - 2
            int j = 0;
            int bps = 1000000 / 571429;

            for (int i = 2; i < b.Length - 7; i+=7)
            {
                j++;
                int ms = (0xFF & b[i]) | ((0xFF & b[i + 1]) << 8) | ((0xFF & b[i + 2]) << 16) | (0xFF & b[i+3] << 24);
                Log.Debug("timing", "NOTE " + Convert.ToString(j) + " is " + Convert.ToString(ms));
                utils.addToThreadPool("MidiEventThread", new Action(() => {
                    utils.Sleep(ms);
                    midi.Write(new byte[] { b[i + 4], b[i + 5], b[i + 6] });
                }));

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

        protected void sendMidi(int m, int n, int v)
        {
            midi.Write(new byte[] { (byte)m, (byte)n, (byte)v });
        }
    }
}