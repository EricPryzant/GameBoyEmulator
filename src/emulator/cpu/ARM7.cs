using System;
using static PZgba.Bits;

namespace PZgba
{
  public class ARM7
  {
    // Reference - https://problemkaputt.de/gbatek.htm#armcpuregisterset
    GBA GBA;
    public uint R0; // R0-R12 General Purpose Registers
    public uint R1;
    public uint R2;
    public uint R3;
    public uint R4;
    public uint R5;
    public uint R6;
    public uint R7;
    public uint R8;
    public uint R9;
    public uint R10;
    public uint R11;
    public uint R12;
    public uint R13; // Stack Pointer in THUMB state, in ARM state the user may decide to use it as a stack pointer or general purpose
    public uint R14; // Link Register - called by a Branch with Link
    public uint R15; // Program counter

    public bool Sign = false;
    public bool Zero = false;
    public bool Carry = false;
    public bool Overflow = false;
    public bool Sticky = false;
    public bool IRQ_Disable = false;
    public bool FIQ_Disable = false;
    public bool Thumb_State = false;
    uint Mode = 0;
    GBA Gba;

    public ARM7(GBA gba)
    {
      Gba = gba;
      R15 = 0;
      R15 = 0x08000000;
    }

    public void Execute()
    {
      Console.WriteLine("R15: ${0:X}", R15);
      byte f0 = Gba.Mem.Read(R15++);
      byte f1 = Gba.Mem.Read(R15++);
      byte f2 = Gba.Mem.Read(R15++);
      byte f3 = Gba.Mem.Read(R15++);
      uint ins = (uint)((f3 << 24) | (f2 << 16 ) | (f1 << 8) | (f0 << 0));
      Console.WriteLine($"Instruction: ${ins:X}");
      Console.WriteLine($"Condition: ${ins >> 28:x}");
      uint condition = (ins >> 28) & 0x0000000F;
      bool conditionMet = false;
      switch(condition)
      {
        case 0x0000000E:
          conditionMet = true;
          break;
        default:
          throw new Exception($"Invalid condition${condition:X}");
      }

      if(conditionMet)
      {
        if ((ins & 0b1110000000000000000000000000) == 0b1010000000000000000000000000)
        {
          int offset = (int)(ins & 0b111111111111111111111111) << 2;
          // Signed with Two's complement
          if((offset & BIT_25) != 0)
          {
            Console.WriteLine("Backward branch");
            offset -= (int)BIT_26;
          }
          else
          {
            Console.WriteLine("Forward Branch");
          }
          // Link - store return address in R14
          if ((ins & BIT_24) != 0)
          {
            R14 = R15;
          }
          R15 = (uint)(R15 + offset + 4);
        }
        else if ((ins & 0b1100000000000000000000000000) == 0b0000000000000000000000000000)
        {
          Console.WriteLine("Data Processing / FSR Transfer");
          // ALU Operations - https://problemkaputt.de/gbatek.htm#armopcodesdataprocessingalu
          bool immediate = (ins & BIT_25) != 0;
          uint opcode = (ins >> 21) & 0xF;
          bool setCondition = (ins & BIT_20) != 0;
          uint operandReg = (ins >> 16) & 0xF;
          uint destReg = (ins >> 12) & 0xF;

          uint regShiftId = (ins >> 8) & 0b1111;
          uint immShift = (ins >> 7) & 0b11111;

          // Shift by immediate or shift by register
          bool shiftByReg = (ins & BIT_4) != 0;
          Console.WriteLine($"Shift by reg: {shiftByReg}");
          uint shiftBy;
        }
        else
        {
          throw new Exception("Unimplemented opcode");
        }
      }
    }
    public uint GetReg(int reg)
    {
      return 0;
    }

    public void SetReg(int reg, uint val)
    {

    }
  }
}