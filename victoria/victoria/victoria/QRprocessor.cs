﻿using Plugin.Vibrate;
using System;
using System.IO;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace victoria
{
    class QRprocessor
    {
        private byte[] data;
        private int lastQR = 45;
        private int songID = 200;

        private ZXingScannerPage page;
        private MidiPlayer midiplayer;
        private iUtils utils = DependencyService.Get<iUtils>();
        private Plugin.Vibrate.Abstractions.IVibrate v = CrossVibrate.Current;
        private MainOverlay overlay;

        public QRprocessor(MidiPlayer midiplayer, ZXingScannerPage page)
        {
            this.page = page;
            overlay = (MainOverlay)page.Overlay;
            this.midiplayer = midiplayer;
            utils.startNewThreadPool("QRValidatorThread");
            utils.newTimer("ClearNotes", QRTimeout, 3500);
        }

        public void addToQRValidatorQueue(ZXing.Result r)
        {
            utils.addToThreadPool("QRValidatorThread", new Action(() => QRValidator(r)));
        }
        
        
        private void QRValidator(ZXing.Result r)
        {
            try {
                data = Convert.FromBase64String(r.ToString());
            } catch (Exception e) {
                return;
            }


            if (data.Length < 25)
                return;

            int currentQR = (data[3] << 8) | data[4];
            if (lastQR == currentQR && songID == data[5])
                return;
            
            if (songID != data[5]) {
                v.Vibration(45);
                songID = data[5];
                newSong(data);
                lastQR = currentQR++;
            }
            if (lastQR != currentQR) {
                lastQR = currentQR;
                utils.resetTimer("ClearNotes");
                Device.BeginInvokeOnMainThread(() => overlay.newScan());
                newQRCode(data);
            }
            data = new byte[] { };
        }

        private void newSong(byte[] data)
        {
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
