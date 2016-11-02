using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Management;

namespace Skype2Blink
{
    class ProcessWatcher
    {
        public delegate void StartedEventHandler(object sender, EventArgs e);
        public delegate void TerminatedEventHandler(object sender, EventArgs e);

        public StartedEventHandler Started = null;
        public TerminatedEventHandler Terminated = null;

        // WMI event watcher
        private ManagementEventWatcher eventWatcher;

        public ProcessWatcher(string processName) : this(processName, 1.0) { }

        public ProcessWatcher(string processName, double sPollingIntervall)
        {
            string eventQuery = "SELECT * FROM __InstanceOperationEvent " + 
                "WITHIN  " + sPollingIntervall +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + processName + "'";

            string managementScope = @"\\.\root\CIMV2";

            eventWatcher = new ManagementEventWatcher(managementScope, eventQuery);

            eventWatcher.EventArrived += new EventArrivedEventHandler(this.OnEventArrived);
            eventWatcher.Start();
        }

        public void Dispose()
        {
            eventWatcher.Stop();
            eventWatcher.Dispose();
        }

        private void OnEventArrived(object sender, System.Management.EventArrivedEventArgs e)
        {
            try
            {
                string eventName = e.NewEvent.ClassPath.ClassName;

                if (eventName.CompareTo("__InstanceCreationEvent") == 0)
                {
                    if (Started != null)
                        Started(this, e);
                }
                else if (eventName.CompareTo("__InstanceDeletionEvent") == 0)
                {
                    if (Terminated != null)
                        Terminated(this, e);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
