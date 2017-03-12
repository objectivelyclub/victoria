using Android.Util;
using Plugin.Vibrate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace victoria
{
    class QRprocessor
    {
        private byte[] data;
        private int lastQR = 45;
        private int songID = 200;

        private Page page;
        private MidiPlayer midiplayer;
        private iUtils utils = DependencyService.Get<iUtils>();
        private bool alertActive = false;
        private Plugin.Vibrate.Abstractions.IVibrate v = CrossVibrate.Current;

        public QRprocessor(MidiPlayer midiplayer, Page page)
        {
            this.page = page;
            this.midiplayer = midiplayer;
            utils.startNewThreadPool("QRValidatorThread");
            utils.newTimer("ClearNotes", QRTimeout, 2500);
        }

        public void addToQRValidatorQueue(ZXing.Result r)
        {
            utils.addToThreadPool("QRValidatorThread", new Action(() => QRValidator(r)));
        }
        
        private void openDisplayAlert(String str)
        {
            if (!alertActive)
            {
                alertActive = true;
                Device.BeginInvokeOnMainThread(() => displayAlert(str));
            }
        }

        private async void displayAlert(String str)
        {
            if (Uri.IsWellFormedUriString(str, UriKind.Absolute))
            {
                if (await page.DisplayAlert("Navigate to Link?", str, "Yes", "No"))
                {
                    Device.OpenUri(new Uri(str));
                }
            }
            else
            {
                await page.DisplayAlert("Invalid QR Data", str, "Close");
            }
            alertActive = false;
        }

        private void QRValidator(ZXing.Result r)
        {
            if (r == null)
                return;

            try {
                data = System.Convert.FromBase64String(r.ToString());
            } catch (Exception e) {
                openDisplayAlert(r.Text);
                return;
            }

            if (data[0] != 0x41 || data[1] != 0x13 || data[2] != 0x08 || data.Length < 20)
            {
                openDisplayAlert(r.Text);
                return;
            }

            int currentQR = (data[3] << 8) | data[4];
            if (lastQR == currentQR && songID == data[5])
                return;
            
            if (songID != data[5]) {
                songID = data[5];
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
            reader.ReadBytes(22);
            midiplayer.processRawBytes(reader);
        }
    }
}
