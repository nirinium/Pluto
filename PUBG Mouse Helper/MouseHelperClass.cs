using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace PUBG_Mouse_Helper
{
    public static class MouseHelperClass
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        const uint MOUSEEVENTF_LEFTUP = 0x04;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        const uint MOUSEEVENTF_RIGHTUP = 0x10;
        const uint MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const uint MOUSEEVENTF_MIDDLEUP = 0x40;
        const uint MOUSEEVENTF_MOVE = 0x01;
        

        public static void LeftClickDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
        }

        public static void LeftClickUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        public static void MouseMove(int dx, int dy, uint sleep = 10)
        {
            int stepx, stepy;

            stepx = (dx < 0) ?  -1 : 1;
            for (int cnt = 1; cnt <= Math.Abs(dx); cnt++)
            {
                mouse_event(MOUSEEVENTF_MOVE, stepx, 0, 0, UIntPtr.Zero);
                Thread.Sleep((int)sleep / 2);
            }

            stepy = (dy < 0) ?  -1 : 1;
            for (int cnt = 1; cnt <= Math.Abs(dy); cnt++)
            {
                mouse_event(MOUSEEVENTF_MOVE, 0, stepy, 0, UIntPtr.Zero);
                Thread.Sleep((int)sleep / 2);
            }
        }

        public static void FastLoot() // CONTROLLED IN Poller.cs [XButton1]
        {
            
            Console.WriteLine("Fastlooting!");
            for (int i = 0; i < 10; i++)
            {
                LeftClickUp();
                LeftClickDown();
                Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_MOVE, 600, 0, 0, UIntPtr.Zero);
                Thread.Sleep(20);
                LeftClickUp();
                mouse_event(MOUSEEVENTF_MOVE, -600, 0, 0, UIntPtr.Zero);
            }
        }
        public static void Rapidfire() // CONTROLLED IN Poller.cs [XButton2]
        {
            Console.WriteLine("Rapidfire!");
            for (int i = 0; i < 10; i++)
            {
                int[] x = new int[] { };
                int[] y = { 1, 1, 2, 2 };

                LeftClickUp();
                LeftClickDown();
                Thread.Sleep(2);
                mouse_event(MOUSEEVENTF_MOVE, 0, 0, 0, UIntPtr.Zero);
                LeftClickUp();
                //mouse_event(MOUSEEVENTF_MOVE, -600, 0, 0, UIntPtr.Zero);
            }
        }
    }
}