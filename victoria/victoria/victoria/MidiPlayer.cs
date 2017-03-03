using Plugin.Vibrate;
using System;
using Xamarin.Forms;

namespace victoria
{
    class MidiPlayer
    {
        //QR Data Analysis Variables
        private iBlockingCollections dataAnalysisQueue;
        string[] msgs;
        byte lastProcessedQrId = 213;
        byte[] b0;
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

            string[] msgs = r.ToString().Split('\n');
            b0 = System.Convert.FromBase64String(msgs[0]);
            if (b0[0] != lastProcessedQrId)
            {
                lastProcessedQrId = b0[0];
                utils.startNewSelfTerminatingThread(processRawBytes, msgs);
                v.Vibration(45);
            }
        }

        private void processRawBytes(string[] s)
        {
            for (int i = 1; i < s.GetLength(0) - 1; i++)
            {
                byte[] b = System.Convert.FromBase64String(s[i]);
                utils.addToThreadPool("MidiEventQueue", new Action(() => {
                    utils.Sleep((0xFF & b[0]) | ((0xFF & b[1]) << 8) | ((0xFF & b[2]) << 16) | (0xFF & b[3] << 24));
                    midi.Write(new byte[] { b[4], b[5], b[6] });
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