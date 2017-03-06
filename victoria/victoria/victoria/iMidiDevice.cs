using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace victoria
{
    public interface iMidiDevice
    {
        void Start();
        void Write(byte[] v);

        void MIDIQueuer(int time, byte[] b);
    }
}
