using System;
using System.Runtime.InteropServices;
using static SDL2.SDL;
using System.IO;
using OpenGL;
using ImGuiNET;
using System.Threading;
using PZgba;

namespace PZgbaEmulator
{
    class PZgbaEmulator
    {
        static int width = 240;
        static int height = 160;
        static int channels = 3;
        static byte[] image = new byte[width * height * channels];
        static IntPtr Renderer;
        static SDL_Rect rect = new SDL_Rect();

        static IntPtr window = IntPtr.Zero;
        static IntPtr glcontext;
        static GBA Gba;
        static SDL_AudioSpec want, have;
        static uint AudioDevice;
        static ImGuiIOPtr ImGuiIO;
        static private System.Numerics.Vector2 _scaleFactor = System.Numerics.Vector2.One;

        public static void Main(string[] args)
        {
            new Thread(() =>
            {
                using (Game game = newGame(1600, 900, "PZ GBA"))
                {
                    game.Vsync = OpenTK.VSyncMode.On;
                    game.Run(60.0, 60.0);
                }
            }).Start();
        }
    }
}