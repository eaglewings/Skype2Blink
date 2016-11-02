using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;

namespace Skype2Blink
{
    public class TitleTracker
    {
        readonly EventHook _hook;

        public TitleTracker(uint idProcess)
        {
            _hook = new EventHook(OnObjectNameChange, EventHook.EVENT_OBJECT_NAMECHANGE, EventHook.EVENT_OBJECT_NAMECHANGE, idProcess);
        }

        ~TitleTracker()
        {
            _hook.Stop();
        }

        static void OnObjectNameChange(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            int windowHandle = hWnd.ToInt32();
            Console.WriteLine("hWinEventHook: {0:x8}, eventType: {1:x4}, hWnd: {2:x8}, idObject: {3:x8}, idChild: {4:x8}, dwEventThread: {5:x8}, dwmsEventTime: {6:x8}"
                , new object[]{ hWinEventHook.ToInt32(), eventType, hWnd.ToInt32(), idObject, idChild, dwEventThread, dwmsEventTime});
        }
    }

    public class EventHook
    {
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
           hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
           uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public const uint EVENT_OBJECT_NAMECHANGE = 0x800C;
        public const uint WINEVENT_OUTOFCONTEXT = 0;

        readonly WinEventDelegate _procDelegate;
        readonly IntPtr _hWinEventHook;

        public EventHook(WinEventDelegate handler, uint eventMin, uint eventMax, uint idProcess)
        {
            _procDelegate = handler;
            _hWinEventHook = SetWinEventHook(eventMin, eventMax, IntPtr.Zero, handler, idProcess, 0, WINEVENT_OUTOFCONTEXT);
        }

        public EventHook(WinEventDelegate handler, uint eventMin)
              : this(handler, eventMin, eventMin, 0)
        {
        }

        public void Stop()
        {
            UnhookWinEvent(_hWinEventHook);
        }


        static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine("Foreground changed to {0:x8}", hwnd.ToInt32());
        }
    }
}