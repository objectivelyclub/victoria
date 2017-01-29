'''
print(type(bytes))
with open(fileout, 'w') as f:
	all_bytes += 
'''
import mido
from mido import MidiFile
import sys
import array
import struct

fileconv = MidiFile('rey.mid')
fileout = open('out.mid', 'wb')

ticks_per_beat = fileconv.ticks_per_beat
tempo = 500000
seconds_per_beat = tempo / 1000000.0
seconds_per_tick = seconds_per_beat / float(ticks_per_beat)

print("MIDI file type: {}".format(fileconv.type))
print("MIDI file ticks_per_beat: {}".format(fileconv.ticks_per_beat))
print("MIDI file length: {}".format(fileconv.length))

all_bytes = []

for i, track in enumerate(fileconv.tracks):
    print('Track {}: {}'.format(i, track.name))
    for message in track:
    	if (message.bytes()[0] == 144):
			out_time = (int)(message.time * seconds_per_tick * 1000)
			all_bytes.append(out_time)
			all_bytes.append(message.bytes())
			fileout.write(struct.pack('IBBB',out_time,message.bytes()[0],message.bytes()[1],message.bytes()[2]))
			all_bytes = []

		



