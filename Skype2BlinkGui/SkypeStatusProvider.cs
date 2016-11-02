using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skype2Blink;

namespace Skype2BlinkGui
{
    class SkypeStatusProvider
    {
        private const string PROCESS_NAME = "skype.exe";

        ProcessWatcher skypeWatcher = new ProcessWatcher(PROCESS_NAME);

        public bool IsRunning { get; private set; }
        public uint Instances { get; private set; }
        public uint ProcessId { get; private set; }

        uint instances = 0;

        public SkypeStatusProvider()
        {
            skypeWatcher.Started += new ProcessWatcher.StartedEventHandler(OnStarted);
            skypeWatcher.Terminated += new ProcessWatcher.TerminatedEventHandler(OnTerminated);
        }

        private void OnStarted(object sender, EventArgs e)
        {
            
            CountInstances();
            Console.WriteLine("Started.");
        }

        private void OnTerminated(object sender, EventArgs e)
        {
            CountInstances();
            Console.WriteLine("Stopped.");
        }

        private void CountInstances()
        {

        }


    }
}
