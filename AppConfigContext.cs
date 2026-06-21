using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace VisionGuard
{
    internal class AppConfigContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;

        private System.Windows.Forms.Timer _timer;

        private string _baloonTipMessage = "Пора отдохнуть! Посмотри на 20 метров вдаль в течение 20 секунд.";
        public AppConfigContext() 
        {
            _trayIcon = new NotifyIcon();

            ContextMenuStrip contextMenu = new();
            ToolStripMenuItem exitItem = new("Exit");
            ToolStripMenuItem autostartItem = new("Execute on start");

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 20 * 60 * 1000;

            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisionGuard.app_icon.ico")) 
            {
                if (stream != null) 
                {
                    _trayIcon.Icon = new Icon(stream);
                }
                else 
                {
                    _trayIcon.Icon = SystemIcons.Application;
                }
            }

            _trayIcon.Text = "Vision Guard";
            _trayIcon.Visible = true;

            exitItem.Click += (sender, e) => 
            {
                _trayIcon.Visible = false;
                Application.Exit();
            };

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false)) 
            {
                autostartItem.Checked = key?.GetValue("VisionGuard") != null;
            }

            autostartItem.Click += (sender, e) => 
            {
                autostartItem.Checked = !autostartItem.Checked;

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    
                    if (key != null) 
                    {
                        if (autostartItem.Checked) 
                        {
                            key.SetValue("VisionGuard", $"\"{Application.ExecutablePath}\"");
                        }
                        else 
                        {
                            key.DeleteValue("VisionGuard", false);
                        }
                    }
                }
            };

            contextMenu.Items.Add(autostartItem);

            contextMenu.Items.Add(exitItem);

            _trayIcon.ContextMenuStrip = contextMenu;

            _timer.Tick += (sender, e) => 
            {
                _trayIcon.ShowBalloonTip(3000, _trayIcon.Text, _baloonTipMessage, ToolTipIcon.Info);
            };

            _timer.Start();
        }
    }
}
