using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AudioToolbox;
using AudioUnit;
using CoreFoundation;
using CoreMidi;
using Foundation;
using UIKit;
using ObjCRuntime;

namespace Victoria.iOS
{
    /// <summary>
    /// A little class showing how to use iOS's AudioToolbox
    /// to synthesize audio dynamically and from MIDI files.
    /// </summary>
    public class PlayMidi : UIViewController
    {
        AUGraph graph;
        AudioUnit.AudioUnit samplerUnit;

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            CreateAudioGraph();

            LoadInstrument(1);

            //			PlayMidiSong ();

            await PlayDynamicSong();
        }

        void CreateAudioGraph()
        {
            graph = new AUGraph();

            var samplerNode = graph.AddNode(AudioComponentDescription.CreateMusicDevice(AudioTypeMusicDevice.Sampler));
            var ioNode = graph.AddNode(AudioComponentDescription.CreateOutput(AudioTypeOutput.Remote));

            graph.Open();
            graph.ConnnectNodeInput(samplerNode, 0, ioNode, 0);

            samplerUnit = graph.GetNodeInfo(samplerNode);

            graph.Initialize();
            graph.Start();
        }

        void LoadInstrument(int preset)
        {
            var soundFontPath = NSBundle.MainBundle.PathForResource("basic_midi", "sf2");
            var soundFontUrl = CFUrl.FromFile(soundFontPath);

            samplerUnit.LoadInstrument(new SamplerInstrumentData(soundFontUrl, InstrumentType.SF2Preset)
            {
                BankLSB = SamplerInstrumentData.DefaultBankLSB,
                BankMSB = SamplerInstrumentData.DefaultMelodicBankMSB,
                PresetID = (byte)preset,
            });
        }

        async Task PlayDynamicSong()
        {
            var dur = 2000;

            var t = 5 * 12 - 2;

            var progression = new[] {
                t,
                t,
                t,
                t,
                t + 5,
                t + 5,
                t,
                t,
                t + 7,
                t + 5,
                t,
                t,
            };
            //
            //			var progression = new [] {
            //				t,
            //				t + 5,
            //				t + 7,
            //				t + 7,
            //				t,
            //				t + 5,
            //				t + 7,
            //				t,
            //			};

            for (;;)
            {
                foreach (var i in progression)
                {
                    await Task.WhenAll(
                        PlayMajor7ChordNotesChords(i, dur),
                        PlayMajor7ChordNotes(i, dur / 8));

                    //					await PlayMinor7ChordNotes (i, dur);

                    //					await Task.Delay (dur);
                }
            }
        }

        async Task PlayMajor7ChordNotes(int tone, int duration, int velocity = 127)
        {
            await PlayNote(tone, duration, velocity * 0.6);
            await PlayNote(tone + 4, duration, velocity * 0.65);
            await PlayNote(tone + 7, duration, velocity * 0.7);
            await PlayNote(tone + 11, duration, velocity * 0.75);
            await PlayNote(tone + 12, duration, velocity * 0.8);
            await PlayNote(tone + 11, duration, velocity * 0.85);
            await PlayNote(tone + 7, duration, velocity * 0.9);
            await PlayNote(tone + 4, duration, velocity * 0.7);
        }

        async Task PlayMajor7DimChordNotes(int tone, int duration, int velocity = 127)
        {
            await PlayNote(tone, duration, velocity * 0.6);
            await PlayNote(tone + 4, duration, velocity * 0.65);
            await PlayNote(tone + 7, duration, velocity * 0.7);
            await PlayNote(tone + 10, duration, velocity * 0.75);
            await PlayNote(tone + 12, duration, velocity * 0.8);
            await PlayNote(tone + 10, duration, velocity * 0.85);
            await PlayNote(tone + 7, duration, velocity * 0.9);
            await PlayNote(tone + 4, duration, velocity * 0.7);
        }

        async Task PlayMajor7ChordNotesChords(int tone, int duration, int velocity = 127)
        {
            await PlayMajorChord(tone, duration / 4, velocity * 0.8);
            await PlayMajor7Chord(tone, duration * 3 / 4, velocity * 0.6);
            //			await PlayMajor7Chord (tone, duration, velocity*0.6);
            //			await PlayMajor7Chord (tone, duration, velocity*0.6);
        }

        async Task PlayMinor7ChordNotes(int tone, int duration, int velocity = 127)
        {
            await PlayNote(tone, duration, velocity * 0.6);
            await PlayNote(tone + 3, duration, velocity * 0.7);
            await PlayNote(tone + 7, duration, velocity * 0.8);
            await PlayNote(tone + 10, duration, velocity * 0.9);
            await PlayNote(tone + 12, duration, velocity);
            await PlayNote(tone + 10, duration, velocity * 0.9);
            await PlayNote(tone + 7, duration, velocity * 0.8);
            await PlayNote(tone + 3, duration, velocity * 0.7);
        }

        async Task PlayMajorChord(int tone, int duration, double velocity = 127)
        {
            await PlayNotes(new[] {
                tone,
                tone + 4,
                tone + 7,
            }, duration, velocity);
        }

        async Task PlayMajor7Chord(int tone, int duration, double velocity = 127)
        {
            await PlayNotes(new[] {
                tone,
                tone + 4,
                tone + 7,
                tone + 10,
            }, duration, velocity);
        }

        async Task PlayMinorChord(int tone, int duration, double velocity = 127)
        {
            await PlayNotes(new[] {
                tone,
                tone + 3,
                tone + 7,
            }, duration, velocity);
        }

        async Task PlayMinor7Chord(int tone, int duration, int velocity = 127)
        {
            await PlayNotes(new[] {
                tone,
                tone + 3,
                tone + 7,
                tone + 10,
            }, duration, velocity);
        }

        async Task PlayMajorScale(int tone, int duration, int velocity = 127)
        {
            await PlayNotes(new[] {
                tone,           // I
//				tone + 2,       // II
				tone + 4,       // III
//				tone + 5,       // IV
				tone + 7,       // V
//				tone + 9,       // VI
				tone + 11,      // VII
				tone + 12,
            }, duration, velocity);
        }

        async Task PlayMinorScale(int tone, int duration, int velocity = 127)
        {
            await PlayNotes(new[] {
                tone,
//				tone + 2,
				tone + 3,
//				tone + 5,
				tone + 7,
//				tone + 8,
//				tone + 10,
//				tone + 12,
			}, duration, velocity);
        }

        async Task PlayNotes(int[] notes, int duration, double velocity = 127)
        {
            await Task.WhenAll(notes.Select(x => PlayNote(x, duration, velocity)).ToArray());
        }

        async Task PlayNote(int note, int duration, double velocity = 127)
        {
            var channel = 0;
            var status = (9 << 4) | channel;
            samplerUnit.MusicDeviceMIDIEvent((byte)status, (byte)note, (byte)velocity);
            await Task.Delay(duration);
        }

        MidiClient midiClient;
        MusicPlayer player;

        void PlayMidiSong()
        {
            var midiPath = NSBundle.MainBundle.PathForResource("get_lucky", "mid");

            midiClient = new MidiClient("Midi Client");

            MidiError stat;
            var midiEndpoint = midiClient.CreateVirtualDestination("VEnd", out stat);
            //midiEndpoint.MessageReceived += (sender, e) => HandleMidiPackets(e.Packets);

            var s = new MusicSequence();
            s.LoadFile(NSUrl.FromFilename(midiPath), MusicSequenceFileTypeID.Midi);
            s.SetMidiEndpoint(midiEndpoint);

            player = new MusicPlayer();

            player.MusicSequence = s;
            player.Start();
        }
        /*

        unsafe void HandleMidiPackets(MidiPacket[] packets)
        {
            foreach (var p in packets)
            {
                var bytes = (byte*)p.Bytes;

                var status = bytes[0];
                var data1 = p.Length > 1 ? bytes[1] : 0u;
                var data2 = p.Length > 2 ? bytes[2] : 0u;

                var command = status >> 4;
                //
                if (command != 0x0F)
                {
                    //					var channel = status & 0x0F;
                    var note = (byte)(data1 & 0x7F);
                    var velocity = (byte)(data2 & 0x7F);
                    samplerUnit.MusicDeviceMIDIEvent(status, note, velocity);
                    //					Console.WriteLine (channel);
                }
                else
                {
                    //					var eox = bytes [p.Length - 1];
                    //					Console.WriteLine ("st={0:X2}", status);
                    MusicDeviceSysEx(samplerUnit.Handle, p.Bytes, p.Length);
                }
            }
        }*/

        [DllImport(Constants.AudioToolboxLibrary)]
        static extern AudioUnitStatus MusicDeviceSysEx(IntPtr inUnit, IntPtr inData, uint inLength);
    }
}

