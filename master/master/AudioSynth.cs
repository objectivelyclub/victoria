using System.Threading;
using Android.Media;
using System.Collections.Generic;

namespace AudioSynth
{
    public class AudioSynth
    {

        private bool sp1Active = false;
        private SoundPool sp1;
        private SoundPool sp2;

        List<int> sp1IDs = new List<int>();
        List<int> sp2IDs = new List<int>();


        private int dormantSP = 1;


        private List<SoundPool> spArray = new List<SoundPool>();
        private List<List<int>> spIDArray = new List<List<int>>();


        public AudioSynth()
        {
            //Thread prepData = new Thread(new ThreadStart(PrepData));
            Thread MidiPlayer = new Thread(new ThreadStart(MidiQueue));
            
            sp1 = new SoundPool(10, Android.Media.Stream.Music, 0);
            sp2 = new SoundPool(10, Android.Media.Stream.Music, 0);

            spArray.Insert(0, sp1);
            spArray.Insert(1, sp2);
        }

        public void addMidis(string[] midis)
        {
            spIDArray[dormantSP].Clear();
            foreach (string url in midis)
            {
                sp1IDs.Add(spArray[dormantSP].Load(url, 1));
            }
        }

        protected void PrepData()
        {
            int i = 0;
            while (true)
            {
                Thread.Sleep(300);
                i++;
            }
        }

        protected void MidiQueue()
        {
            while (true)
            {
                Thread.Sleep(2000);
                foreach (int id in sp1IDs)
                {
                    spArray[dormantSP].Play(id, 1, 1, 2, 0, 1f);
                }
                SwitchDormantSP();
            }
        }

        protected void SwitchDormantSP()
        {
            dormantSP = (dormantSP == 1) ? 0 : 1;
        }

    }

}