using Skype2Blink;
using System.Diagnostics;

namespace Skype2BlinkGui
{
    public class Test
    {
        private const string PROCESS_NAME = "skype";
        private Process[] _processes;
        private Process skype;
        private TitleTracker titleTracker;

        public Test()
        {
            _processes = Process.GetProcessesByName(PROCESS_NAME);
            
            if(_processes.Length > 0)
            {
                skype = _processes[0];
            }

            titleTracker = new TitleTracker((uint)skype.Id);

        }
    }
}
