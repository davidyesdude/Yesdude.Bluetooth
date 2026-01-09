using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace Yesdude.Bluetooth
{
    //internal static class Program
    //{
    //    /// <summary>
    //    ///  The main entry point for the application.
    //    /// </summary>
    //    [STAThread]
    //    static void Main()
    //    {
    //        // To customize application configuration such as set high DPI settings or default font,
    //        // see https://aka.ms/applicationconfiguration.
    //        ApplicationConfiguration.Initialize();
    //        Application.Run(new Form1());
    //    }
    //}

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string processName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? "Yesdude.Bluetooth";
            string machineName = Environment.MachineName;

            using var mutex = new Mutex(false, machineName + "_" + processName);
            bool isAnotherInstanceInMemory = !mutex.WaitOne(TimeSpan.Zero);
            if (isAnotherInstanceInMemory) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new BluetoothToggler());

            mutex.ReleaseMutex();

        }
    }


    public class BluetoothToggler : ApplicationContext
    {
        private NotifyIcon? trayIcon;

        public BluetoothToggler()
        {
            AddTrayIcon();
        }

        private void AddTrayIcon()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon();

            Boolean enabled = BluetoothManager.IsBluetoothEnabledAsync().WaitAsync(new CancellationToken()).Result;

            trayIcon.Icon = Icon.FromHandle(Utility.GetResourceIcon(enabled).GetHicon());

            trayIcon.ContextMenuStrip = new ContextMenuStrip();

            trayIcon.ContextMenuStrip.Items.Add("On");
            trayIcon.ContextMenuStrip.Items.Add("Off");

            trayIcon.ContextMenuStrip.Items.Add("Exit");

            trayIcon.ContextMenuStrip.Items[0].Tag = 1;
            trayIcon.ContextMenuStrip.Items[0].Image = Utility.GetResourceIcon(true);

            trayIcon.ContextMenuStrip.Items[1].Tag = 0;
            trayIcon.ContextMenuStrip.Items[1].Image = Utility.GetResourceIcon(false);

            trayIcon.ContextMenuStrip.Items[2].Tag = -1;

            trayIcon.ContextMenuStrip.ItemClicked += ContextMenuStrip_Click;

            trayIcon.Visible = true;

        }

        private void ContextMenuStrip_Click(object? sender, ToolStripItemClickedEventArgs e)
        {

            if (!Int32.TryParse(e.ClickedItem?.Tag?.ToString(), out int bluetoothSetting))
                return;

            CancellationToken token = new();

            switch (bluetoothSetting)
            {
                case -1:
                    Exit(sender, e);
                    break;
                case 0:
                    BluetoothManager.SetBluetoothState(false).WaitAsync(token);
                    if (trayIcon is not null) trayIcon.Icon = Icon.FromHandle(Utility.GetResourceIcon(false).GetHicon()); ;
                    break;
                case 1:
                    BluetoothManager.SetBluetoothState(true).WaitAsync(token);
                    if (trayIcon is not null) trayIcon.Icon = Icon.FromHandle(Utility.GetResourceIcon(true).GetHicon());
                    break;
                default:
                    break;
            }

        }

        void Exit(object? sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            if (trayIcon is not null) trayIcon.Visible = false;
            Application.Exit();
        }
    }
}