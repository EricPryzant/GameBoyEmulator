using System;
using System.Runtime.InteropServices;
using System.IO;
using static SDL2.SDL;
using PZgba;
using static PZgba.Bits;

namespace PZgba
{
  class Audio
  {
    public GBA Gba;

    static byte[] SEVEN_BIT_NOISE = GenerateNoiseBuffer(true);
    static byte[] FIFTEEN_BIT_NOISE = GenerateNoiseBuffer(false);
    public static float[] DAC_TABLE = GenerateDACTable();
    public static int SAMPLE_RATE = 65536;
    public static int SAMPLE_TIME_MAX = 4194304 / SAMPLE_RATE;
    readonly double CAPACITATOR_FACTOR = Math.Pow(0.999958, (4194304 / SAMPLE_RATE));

    public static float[] GenerateDACTable()
    {
      float[] table = new float[32];
      for (int i = 0; i < 16; i++)
      {
        table[i] = ((i/15)*2)-1;
      }
      return table;
    }

    public static byte[] GenerateNoiseBuffer(bool putBitBack)
    {
      uint seed = 0xFF;
      byte[] waveTable = new byte[32768];
      for (int i = 0; i < 32768; i++)
      {
        waveTable[i] = (byte)((seed & 1) ^ 1);
        int bit = (int)((seed) ^ (seed >> 1));
        bit &= 1;
        seed = (uint)(seed >> 1) | (uint)(bit << 14);
        if (putBitBack == true)
        {
          seed &= ~BIT_7;
          seed |= (uint)(bit << 6);
        }
      }
      return waveTable;
    }

    public Audio(GBA gba) 
    {
      Gba = gba;
    }

    public float[] AudioQueue = new float[2048];
    public bool AudioReady = false;
    uint AudioQueuePointer = 0;
    uint SampleTimer = 0;
    
  }
}