using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(victoria.iOS.MidiPlayer_iOS))]

namespace victoria.iOS
{
    class MidiPlayer_iOS : iMidiDevice
    {
        public MidiPlayer_iOS()
        {

        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] v)
        {
            throw new NotImplementedException();
        }
    }
}
