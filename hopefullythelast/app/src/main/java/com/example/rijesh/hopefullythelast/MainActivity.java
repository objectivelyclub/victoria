package com.example.rijesh.hopefullythelast;

import org.apache.commons.codec.binary.Base64;

import android.os.Looper;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.LinkedList;
import com.google.zxing.Result;
import org.billthefarmer.mididriver.MidiDriver;
import me.dm7.barcodescanner.zxing.ZXingScannerView;
import android.os.Handler;
import android.os.Message;


public class MainActivity extends AppCompatActivity implements ZXingScannerView.ResultHandler  {
    private ZXingScannerView mScannerView;
    private MidiDevice midi1;

    private long timeSinceLastProcess = System.currentTimeMillis();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        midi1 = new MidiDevice();
        midi1.start();
        setContentView(R.layout.activity_main);
    }

    public void PlayTone(View v){
        midi1.playCNote();
    }

    public void QrScanner(View view){

        mScannerView = new ZXingScannerView(this);   // Programmatically initialize the scanner view
        setContentView(mScannerView);

        mScannerView.setResultHandler(this); // Register ourselves as a handler for scan results.
        mScannerView.startCamera();         // Start camera

    }

    @Override
    public void handleResult(Result rawResult) {
        long tmp = System.currentTimeMillis();
        if ((tmp - timeSinceLastProcess) > 2000){
            timeSinceLastProcess = System.currentTimeMillis();
            byte[] fby = Base64.decodeBase64(rawResult.toString().getBytes());

            midi1.addToQueue(fby);
            //Log.e("handler", rawResult.getText());
        }
        mScannerView.resumeCameraPreview(this);
        // If you would like to resume scanning, call this method below:

    }

    @Override
    protected void onResume()
    {
        super.onResume();
    }


    @Override
    public void onPause() {
        super.onPause();
        mScannerView.stopCamera();           // Stop camera on pause
    }

}
