using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mouse_Faking
{

    public class Win32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static POINT GetCursorPosition()
        {
            GetCursorPos(out POINT lpPoint);
            return lpPoint;
        }

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        public static void sendMouseRightclick(POINT p)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.x, (uint)p.y, 0, (UIntPtr)0);
        }

        public static void sendMouseLeftclick(POINT p)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, (UIntPtr)0);
        }

        public static void sendMouseDoubleClick(POINT p)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, (UIntPtr)0);

            Thread.Sleep(150);

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, (UIntPtr)0);
        }

        public static void sendMouseRightDoubleClick(POINT p)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.x, (uint)p.y, 0, (UIntPtr)0);

            Thread.Sleep(150);

            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.x, (uint)p.y, 0, (UIntPtr)0);
        }

        public static void sendMouseDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 50, 50, 0, (UIntPtr)0);
        }

        public static void sendMouseUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 50, 50, 0, (UIntPtr)0);
        }
    }

}
