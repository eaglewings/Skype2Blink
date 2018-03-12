using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace Skype2Blink
{
    public class ProcessWatcher
    {
        public EventHandler Started = null;
        public EventHandler Terminated = null;

        // WMI event watcher
        private ManagementEventWatcher eventWatcher;

        //Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(ProcessWatcher));

        public ProcessWatcher(string processName) : this(processName, 1.0) { }

        public ProcessWatcher(string processName, double sPollingIntervall)
        {

            string eventQuery = "SELECT * FROM __InstanceOperationEvent " + 
                "WITHIN  " + sPollingIntervall +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = '" + processName + ".exe'";

            string managementScope = @"\\.\root\CIMV2";

            eventWatcher = new ManagementEventWatcher(managementScope, eventQuery);

            eventWatcher.EventArrived += new EventArrivedEventHandler(this.OnEventArrived);

        }

        public void Start()
        {
            eventWatcher.Start();
            log.Debug("ProcessWatcher started: " + eventWatcher.Query);
        }

        public void Dispose()
        {
            log.Debug("ProcessWatcher disposed: " + eventWatcher.Query);
            eventWatcher.Stop();
            eventWatcher.Dispose();
        }

        private void OnEventArrived(object sender, System.Management.EventArrivedEventArgs e)
        {
            log.Debug("EventArrived: " + e);
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
                log.Debug("EventArrived Exception", ex);
            }
        }
    }
}
