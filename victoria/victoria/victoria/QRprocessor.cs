using Android.Util;
using Plugin.Vibrate;
using System;
using System.IO;
using Xamarin.Forms;

namespace victoria
{
    class QRprocessor
    {
        private byte[] data;
        private int lastQR = 45;
        private int songID = 200;

        private MidiPlayer midiplayer;
        private iUtils utils = DependencyService.Get<iUtils>();
        private Plugin.Vibrate.Abstractions.IVibrate v = CrossVibrate.Current;

        public QRprocessor(MidiPlayer midiplayer)
        { 
            this.midiplayer = midiplayer;
            utils.startNewThreadPool("QRValidatorThread");
            utils.newTimer("ClearNotes", QRTimeout, 2500);
        }

        public void addToQRValidatorQueue(ZXing.Result r)
        {
            utils.addToThreadPool("QRValidatorThread", new Action(() => QRValidator(r)));
        }

        private void QRValidator(ZXing.Result r)
        {
            if (r == null)
                return;

            try {
                data = System.Convert.FromBase64String(r.ToString());
            } catch (Exception e) {
                if (Uri.IsWellFormedUriString(r.Text, UriKind.Absolute))
                {
                    //DisplayAlert("Found A URL", r.Text, "OK");
                    Log.Info("Data", "GOTAURLGOTAURLGOTAURLGOTAURL");
                }
                else
                {
                    v.Vibration(90);

                }
                return;
            }
            
            if (data.Length < 20)
            {
                Log.Info("Data", "ERROR/ERROR/ERROR/ERROR/ERROR/ERROR/ERROR/ERROR/");
                return;
            }
            int currentQR = (data[0] << 8) | data[1];
            if (lastQR == currentQR && songID == data[2])
                return;
            
            if (songID != data[2]) {
                songID = data[2];
                newSong(data);
                lastQR = currentQR++;
            }
            if (lastQR != currentQR) {
                utils.resetTimer("ClearNotes");
                lastQR = currentQR;
                newQRCode(data);
            }
            data = new byte[] { };
        }

        private void newSong(byte[] data)
        {
            v.Vibration(45);
            midiplayer.resetPlayer();
            midiplayer.processChannels(data);
        }

        private void QRTimeout()
        {
            midiplayer.resetPlayer();
            songID++;
        }

        private void newQRCode(byte[] data)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(data));
            reader.ReadBytes(19);
            midiplayer.processRawBytes(reader);
        }
    }
}
