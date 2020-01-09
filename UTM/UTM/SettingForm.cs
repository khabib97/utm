using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace UTM
{
    public partial class SettingForm : Form
    {
        Hashtable data;

        public SettingForm()
        {
            InitializeComponent();
            Thread.Sleep(1000);

            //try to load
            data = Util.LoadData("DataFile.dat");
            port_name.Text = Util.AutodetectArduinoPort();
            Variables.portName = port_name.Text;

            if (data != null)
            {
                correction_factor_displacement.Text = (string)data["displacementPerPulse"];
                force_factor_load.Text = (string)data["forceConversionFactor"];
                displacement_reset_zero.Text = (string)data["restZeroDisplacement"];
                deformetion_reset_zero.Text = (string)data["restZeroDisforcement"];
                no_of_speciemen.Text = (string)data["noOfSpeciemen"];
                specimen_fractured.Text = (string)data["specimenFractured"];
                if (port_name.Text == null || port_name.Text.Equals(""))
                {
                    port_name.Text = (string)data["portName"];
                    Variables.portName = port_name.Text;
                }
                baud_rate.Text = (string)data["baudRate"];
            }
        }

        private void SaveSetting()
        {
            try
            {
                Hashtable localData = new Hashtable();

                localData.Add("displacementPerPulse", correction_factor_displacement.Text);
                localData.Add("forceConversionFactor", force_factor_load.Text);
                localData.Add("restZeroDisplacement", displacement_reset_zero.Text);
                localData.Add("restZeroDisforcement", deformetion_reset_zero.Text);
                localData.Add("noOfSpeciemen", no_of_speciemen.Text);
                localData.Add("specimenFractured", specimen_fractured.Text);
                localData.Add("portName", port_name.Text);
                localData.Add("baudRate", baud_rate.Text);

                Util.SaveData(localData, "DataFile.dat");
                Util.ShowInfo(message_label, "Message : Data save done!");
            }
            catch (Exception ex) {
                Util.ShowError(message_label, "Message : Data save error!");
            }
        }

        private void setup_save_button_setting_page_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

    }
}
