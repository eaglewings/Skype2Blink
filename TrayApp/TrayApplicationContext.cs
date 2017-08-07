using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Skype2Blink;

namespace TrayApp
{
    class TrayApplicationContext:ApplicationContext
    {
        private static readonly string IconFileName = "blink1.ico";
        private static readonly string DefaultTooltip = "Skype2Blink";

        private System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        private NotifyIcon notifyIcon;				            // the icon that sits in the system tray
        private ToolStripMenuItem startStopItem;
        private ProcessWatcher processWatcher;

        public TrayApplicationContext()
        {
            InitializeContext();
        }

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(IconFileName),
                Text = DefaultTooltip,
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.MouseUp += notifyIcon_MouseUp;

            startStopItem = new ToolStripMenuItem("Start process watcher", null, startStopItem_Click);
            notifyIcon.ContextMenuStrip.Items.Add(startStopItem);
            notifyIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("&Exit", null, exitItem_Click));
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            
        }

        /// <summary>
        /// When the application context is disposed, dispose things like the notify icon.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) { components.Dispose(); }
        }

        /// <summary>
        ///  Start and stop a process watcher.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void startStopItem_Click(object sender, EventArgs e)
        {
            if(processWatcher == null)
            {
                processWatcher = new ProcessWatcher("Skype.exe", 0.5);
                startStopItem.Text = "Stop process watcher";
            }
            else
            {
                processWatcher.Dispose();
                processWatcher = null;
                startStopItem.Text = "Start process watcher";
            }
            
        }

        /// <summary>
        /// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitItem_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

    }
}
