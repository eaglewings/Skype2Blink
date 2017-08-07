using System;
using Skype2Blink;
using System.Diagnostics;

namespace Skype2BlinkGui
{
    class SkypeStatusProvider
    {
        private const string PROCESS_NAME = "skype.exe";

        ProcessWatcher skypeWatcher = new ProcessWatcher(PROCESS_NAME);

        public bool IsRunning { get; private set; }
        public int Instances { get; private set; }
        public uint ProcessId { get; private set; }


        Process[] _processes;
        public Process[] Processes
        {
            get { return _processes; }

            private set {
                if(_processes == null)
                {
                    _processes = value;
                }
            }
        }
        

        int instances = 0;

        public SkypeStatusProvider()
        {
            skypeWatcher.Started += new ProcessWatcher.StartedEventHandler(OnStarted);
            skypeWatcher.Terminated += new ProcessWatcher.TerminatedEventHandler(OnTerminated);
        }

        private void OnStarted(object sender, EventArgs e)
        {
            
            MaintainInstances();
            Console.WriteLine("Started.");
        }

        private void OnTerminated(object sender, EventArgs e)
        {
            MaintainInstances();
            Console.WriteLine("Stopped.");
        }

        private void MaintainInstances()
        {
            Processes = Process.GetProcessesByName(PROCESS_NAME);
            
        }


    }
}
