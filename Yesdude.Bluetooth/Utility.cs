using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yesdude.Bluetooth
{
    internal class Utility
    {

        public static Bitmap GetResourceIcon(Boolean enabled)
        {

            System.Drawing.Bitmap icnTask;
            System.IO.Stream? st;
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
    
            if(enabled)
                st = a.GetManifestResourceStream("Yesdude.Bluetooth.TrayIcon_Blue.png");
            else
                st = a.GetManifestResourceStream("Yesdude.Bluetooth.TrayIcon_Grey.png");

            if (st is not null)
                icnTask = new System.Drawing.Bitmap(st);
            else
            {
                //failed somehow, paint it red
                icnTask = new System.Drawing.Bitmap(16, 16);
                using Graphics g = Graphics.FromImage(icnTask);
                g.Clear(Color.Red);
            }

            return icnTask;
        }

    }
}
