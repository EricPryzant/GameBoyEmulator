using System;

namespace PZgba
{
  public class Memory
  {
    // Reference - https://problemkaputt.de/gbatek.htm#gbamemorymap
    public Memory(GBA gba)
    {
      
    }
    public byte[] Bios = new byte[16384]; // Bios - 16KB
    public byte[] Ewram = new byte[262144]; // On-board Work RAM - 256KB
    public byte[] Iwram = new byte[32768]; // On-chip Work RAM - 32KB
    public byte[] Rom = new byte[33554432]; // Game Pak ROM - 32MB

    public byte Read(uint addr)
    {
      // GBA Bios
      if(addr >= 0x00000000 && addr <= 0x00003FFF)
      {
        return Bios[addr - 0x00000000];
      }
      // 00004000 - 01FFFFFF Not used
      // On board WRAM
      else if (addr >= 0x02000000 && addr <= 0x0203FFFF)
      {
        return Ewram[addr-0x02000000];
      }
      // 0x02040000 - 02FFFFFF Not used
      // Internal Work Ram
      else if (addr >= 0x03000000 && addr <= 0x02007FFF)
      {
        return Iwram[addr - 0x03000000];
      }
      // 0300800 - 03FFFFFF Not used
      // I/O Registers
      else if (addr >= 0x04000000 && addr <= 0x040003FE)
      {
        return ReadHWIO(addr);
      }
      // Internal Display Memory
      // 0x05000000 - 0x050003FF BG/OBJ Palette RAM 1kb
      // 0x06000000 - 0x06127FFF VRAM - Video RAM 96kb
      // 0x07000000 - 0x070003FF OAM - OBJ Attributes 1kb

      // External Memory ROM
      else if (addr >= 0x08000000 && addr <= 0x09FFFFFF)
      {
        return Rom[addr - 0x08000000];
      }
      
      // TODO - Open Bus

      else 
      {
        throw new Exception($"Memory out of bounds - {addr.ToString("X8")}");
      }
    }

    public byte ReadHWIO(uint addr)
    {
      throw new Exception($"Hardware IO Read - {addr.ToString("X8")}");
    }
  }
}