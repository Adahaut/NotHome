using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBuffer
{
    private float[] buffer;
    private int writeIndex = 0;
    private int readIndex = 0;
    private int bufferSize;
    private int count = 0;

    public CircularBuffer(int size)
    {
        buffer = new float[size];
        bufferSize = size;
    }

    public void Write(float[] data)
    {
        foreach(float sample in data)
        {
            buffer[writeIndex++] = sample;
            writeIndex = (writeIndex + 1) % bufferSize;
            if(count < bufferSize)
            {
                count++;
            }
            else
            {
                readIndex = (readIndex + 1) % bufferSize;
            }
        }
    }

    public int Read(float[] data, int samples)
    {
        int avaibleSamples = Mathf.Min(samples, count);
        for (int i = 0; i < avaibleSamples; i++)
        {
            data[i] = buffer[readIndex];
            readIndex = (readIndex + 1) % bufferSize;
        }
        count -= avaibleSamples;
        return avaibleSamples;
    }

    public int AvaibleSamples()
    {
        return count;
    }

}
