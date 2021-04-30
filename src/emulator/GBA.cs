public delegate void Callback();

namespace PZgba
{
  class GBA
  {
    public ARM7 ARM7;
    public Memory Memory;
    public Audio Audio;

    public uint[] registers = new uint[16];
    public Callback AudioCallback;
    public ARM7 Arm7;
    public Memory Mem;
    public GBA(Callback audioCallback)
    {
      Arm7 = new ARM7(this);
      Mem = new Memory(this);
      Audio = new Audio(this);
      AudioCallback = audioCallback;
    }
    public uint Run() {
      Arm7.Execute();
      return 8;
    }

    void Tick(uint cycles) {
      Audio.Tick(cycles);
    }
  }
}