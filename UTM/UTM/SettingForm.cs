using System;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace UTM
{
    public partial class SettingForm : Form
    {
        Hashtable data;

        public SettingForm()
        {
            InitializeComponent();
            LoadFilterType();

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

                Variables.filter = System.Convert.ToInt32(data["filter"]);
                if (Variables.filter == (int)Variables.Filtering.AverageWithSavitzkyGolay)
                {
                    filtering_combo_box.Text = "Average & Savitzky-Golay";
                }
                else {
                    filtering_combo_box.Text = "Savitzky-Golay";
                }
                numberOfPoints.Text = (string)data["points"];
                degree.Text = (string)data["degree"];

                //Variables.filter = ;
                if (port_name.Text == null || port_name.Text.Equals(""))
                {
                    port_name.Text = (string)data["portName"];
                    Variables.portName = port_name.Text;
                }
                baud_rate.Text = (string)data["baudRate"];
            }
        }

        private void LoadFilterType()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add((int)Variables.Filtering.AverageWithSavitzkyGolay, "Average & Savitzky-Golay");
            comboSource.Add((int)Variables.Filtering.SavitzkyGolay, "Savitzky-Golay");

            filtering_combo_box.DataSource = new BindingSource(comboSource, null);
            filtering_combo_box.DisplayMember = "Value";
            filtering_combo_box.ValueMember = "Key";
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
                localData.Add("filter", ((System.Collections.Generic.KeyValuePair<int, string>)(filtering_combo_box.SelectedItem)).Key);
                localData.Add("points", numberOfPoints.Text);
                localData.Add("degree",degree.Text);

                Util.SaveData(localData, "DataFile.dat");
                Util.ShowInfo(message_label, "Message : Data save done!");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                Util.ShowError(message_label, "Message : Data save error!");
            }
        }

        private void setup_save_button_setting_page_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
