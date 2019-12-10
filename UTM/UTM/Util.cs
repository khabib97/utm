using System;
using System.Drawing;
using System.Management;

namespace UTM
{
    public static class Util
    {

        public static void ShowError(System.Windows.Forms.Label message_label, string msg)
        {
            message_label.ForeColor = Color.Red;
            message_label.Text = msg;
        }

        public static void ShowInfo(System.Windows.Forms.Label message_label, string msg)
        {
            message_label.ForeColor = Color.Green;
            message_label.Text = msg;
        }
        // if this method failed to detect port name, then user can manually update port
        public static string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    if (item["Description"].ToString().Contains("Serial"))
                        return item["DeviceID"].ToString();
                }
            }
            catch (Exception e)
            {
                /* Do Nothing */
            }

            return "";
        }
    }
}
