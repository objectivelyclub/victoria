
namespace victoria
{
    public interface iMidiDevice
    {
        void Start();
        void Stop();
        void Write(byte[] v);

        void MIDIQueuer(int time, byte[] b);
    }
}
