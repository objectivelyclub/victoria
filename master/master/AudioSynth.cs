using System.Threading;
using Android.Media;
using System.Collections.Generic;
using System;
using Java.IO;
using Android.Content;

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

        public void PlaySingleQR(string str)
        {
            byte[] actualB = Convert.FromBase64String(str);
            File song = File.CreateTempFile("hjhki", "mid", null);
            System.IO.File.WriteAllBytes(song.AbsolutePath, actualB);
            //song.
            //System.IO.File.make
            //System.IO.File.WriteAllBytes("/storage/AE2D-8B85/adel.mid", actualB);
            //File rey = System.IO.File.



            int tmp = sp1.Load("/storage/AE2D-8B85/adel.mid", 1);

            Thread.Sleep(2000);
            sp1.Play(tmp,1,1,1,0,1);


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