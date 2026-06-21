using System;
using System.Collections.Generic;
using System.Text;

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

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 20 * 60 * 1000;

            _trayIcon.Icon = SystemIcons.Application;
            _trayIcon.Text = "Vision Guard";
            _trayIcon.Visible = true;

            exitItem.Click += (sender, e) => 
            {
                _trayIcon.Visible = false;
                Application.Exit();
            };

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
