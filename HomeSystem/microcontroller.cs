using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeSystem
{
    class microcontroller
    {
        private const byte VK_SCROLL = 0x91;
        private const byte VK_NUMLOCK = 0x90;
        private const byte VK_CAPITAL = 0x14;

        private const uint KEYEVENTF_KEYUP = 0x2;


        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "GetKeyState", SetLastError = true)]
        static extern short GetKeyState(uint nVirtKey);
        //-------------------------------------------------------Scroll
        public static void SetScrollLockKey(bool newState)
        {
            bool scrollLockSet = GetKeyState(VK_SCROLL) != 0;
            if (scrollLockSet != newState)
            {
                keybd_event(VK_SCROLL, 0, 0, 0);
                keybd_event(VK_SCROLL, 0, KEYEVENTF_KEYUP, 0);
            }
        }
        public static bool GetScrollLockState()
        {
            return GetKeyState(VK_SCROLL) != 0;
        }
        //-------------------------------------------------------NUM
        public static void SetNumLockKey(bool newState)
        {
            bool scrollLockSet = GetKeyState(VK_NUMLOCK) != 0;
            if (scrollLockSet != newState)
            {
                keybd_event(VK_NUMLOCK, 0, 0, 0);
                keybd_event(VK_NUMLOCK, 0, KEYEVENTF_KEYUP, 0);
            }
        }
        public static bool GetNumLockState()
        {
            return GetKeyState(VK_NUMLOCK) != 0;
        }
        //-------------------------------------------------------Caps
        public static void SetCapsLockKey(bool newState)
        {
            bool scrollLockSet = GetKeyState(VK_CAPITAL) != 0;
            if (scrollLockSet != newState)
            {
                keybd_event(VK_CAPITAL, 0, 0, 0);
                keybd_event(VK_CAPITAL, 0, KEYEVENTF_KEYUP, 0);
            }
        }
        public static bool GetCapsLockState()
        {
            return GetKeyState(VK_CAPITAL) != 0;
        }

    }
}
