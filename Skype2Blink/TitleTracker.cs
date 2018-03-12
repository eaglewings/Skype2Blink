using log4net;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Skype2Blink
{
    public class TitleTracker
    {
        readonly EventHook _hook;

        //Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(TitleTracker));
        public uint ProcessId { get; }

        public delegate void TitleChangedEventHandler(object sender, TitleChangedEventArgs e);
        public event TitleChangedEventHandler TitleChanged;

        public TitleTracker(uint idProcess)
        {
            ProcessId = idProcess;
            _hook = new EventHook(OnObjectNameChange, EventHook.EVENT_OBJECT_NAMECHANGE, EventHook.EVENT_OBJECT_NAMECHANGE, idProcess);
        }

        ~TitleTracker()
        {
            _hook.Stop();
        }

        public string Title
        {
            get
            {
                var p = new Process().MainWindowTitle;
                return Process.GetProcessById((int)ProcessId).MainWindowTitle;
            }
        }

        protected virtual void OnObjectNameChange(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            int windowHandle = hWnd.ToInt32();
            log.InfoFormat("hWinEventHook: {0:x8}, eventType: {1:x4}, hWnd: {2:x8}, idObject: {3:x8}, idChild: {4:x8}, dwEventThread: {5:x8}, dwmsEventTime: {6:x8}"
                , new object[]{ hWinEventHook.ToInt32(), eventType, hWnd.ToInt32(), idObject, idChild, dwEventThread, dwmsEventTime});

            if (hWnd.ToInt32() != 0)
            {
                StringBuilder sb = new StringBuilder(50);
                EventHook.GetWindowText(hWnd.ToInt32(), sb, sb.Capacity);
                log.Info(sb.ToString());

                if(TitleChanged != null)
                {
                    TitleChanged(this, new TitleChangedEventArgs(hWnd.ToInt32(), sb.ToString()));
                }
            }
        }
    }

    public class TitleChangedEventArgs : EventArgs
    {
        public TitleChangedEventArgs(int hWnd, string title)
        {
            this.hWnd = hWnd;
            this.title = title;
        }

        private int hWnd;
        private string title;

        public int HWnd{
            get { return hWnd; }
        }

        public string Title
        {
            get { return title; }
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

        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder title, int size);

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