package com.example.rijesh.hopefullythelast;

import org.billthefarmer.mididriver.MidiDriver;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;


/**
 * Created by Rijesh on 1/29/2017.
 */

public class MidiDevice  extends Thread implements MidiDriver.OnMidiStartListener{
    protected MidiDriver midi;

    public Handler mHandler;
    boolean queueThreadStarted = false;

    public MidiDevice(){
        midi = new MidiDriver();
        midi.setOnMidiStartListener(this);
        midi.start();
    }

    @Override
    public void run() {
        Looper.prepare();

        mHandler = new Handler(){
            public void handleMessage(Message msg){
                Bundle bun = msg.getData();
                try {
                    Thread.sleep(bun.getInt("time"));
                } catch (InterruptedException e) {
                    e.printStackTrace();
                };
                midi.write(bun.getByteArray("midiData"));
            }
        };
        Looper.loop();
    }

    public void addToQueue(final byte[] b) {
        if (queueThreadStarted){
            return;
        }
        queueThreadStarted = true;
        Thread thread = new Thread() {
            @Override
            public void run() {
                Message msg = Message.obtain();
                Bundle bun = new Bundle();
                for(int i=0;i < b.length - 9; i+=9){
                    bun.putInt("time", ((0xFF & b[i])  | ((0xFF & b[i+1]) << 8) |
                            ((0xFF & b[i+2]) << 16) | (0xFF & b[i+3] << 24)));
                    bun.putByteArray("midiData",new byte[] {b[i+4], b[i+5], b[i+6] });
                    msg.setData(bun);
                    mHandler.handleMessage(msg);
                }
                queueThreadStarted = false;
            }
        };
        thread.start();
    }

    public void pause() {
        // Clear MIDI Queue
    }

    public void playCNote(){
        sendMidi(0x90, 48, 63);
        sendMidi(0x90, 52, 63);
        sendMidi(0x90, 55, 63);
    }

    public void onMidiStart()
    {
        // Program change - harpsicord
        //sendMidi(0xc0, 6);
        // Get the config

        int config[] = midi.config();
    }

    // Send a midi message

    protected void sendMidi(int m, int p)
    {
        byte msg[] = new byte[2];

        msg[0] = (byte) m;
        msg[1] = (byte) p;

        midi.write(msg);
    }

    // Send a midi message

    protected void sendMidi(int m, int n, int v)
    {
        byte msg[] = new byte[3];

        msg[0] = (byte) m;
        msg[1] = (byte) n;
        msg[2] = (byte) v;

        midi.write(msg);
    }

}
