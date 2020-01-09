namespace UTM
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.correction_factor_displacement = new System.Windows.Forms.TextBox();
            this.displacement_reset_zero = new System.Windows.Forms.TextBox();
            this.force_factor_load = new System.Windows.Forms.TextBox();
            this.deformetion_reset_zero = new System.Windows.Forms.TextBox();
            this.no_of_speciemen = new System.Windows.Forms.TextBox();
            this.specimen_fractured = new System.Windows.Forms.TextBox();
            this.port_name = new System.Windows.Forms.ComboBox();
            this.save_button_setting_page = new System.Windows.Forms.Button();
            this.baud_rate = new System.Windows.Forms.TextBox();
            this.message_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Calibration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Correction factor for displacement:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(280, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Displacement reset zero when load is (KN):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(270, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Force/Pressure/Correction factor for load:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(264, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Deformation rest zero when load is (KN):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "No of speciemen:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 17);
            this.label7.TabIndex = 6;
            this.label7.Text = "Specimen is fractured when load drop:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Sepcieme";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 261);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Serial Setup";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 299);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 17);
            this.label10.TabIndex = 9;
            this.label10.Text = "Port No:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(41, 329);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 17);
            this.label11.TabIndex = 10;
            this.label11.Text = "Baud Rate:";
            // 
            // correction_factor_displacement
            // 
            this.correction_factor_displacement.Location = new System.Drawing.Point(417, 40);
            this.correction_factor_displacement.Name = "correction_factor_displacement";
            this.correction_factor_displacement.Size = new System.Drawing.Size(264, 22);
            this.correction_factor_displacement.TabIndex = 11;
            this.correction_factor_displacement.Text = "0.0";
            // 
            // displacement_reset_zero
            // 
            this.displacement_reset_zero.Location = new System.Drawing.Point(417, 69);
            this.displacement_reset_zero.Name = "displacement_reset_zero";
            this.displacement_reset_zero.Size = new System.Drawing.Size(264, 22);
            this.displacement_reset_zero.TabIndex = 12;
            this.displacement_reset_zero.Text = "0.0";
            // 
            // force_factor_load
            // 
            this.force_factor_load.Location = new System.Drawing.Point(417, 101);
            this.force_factor_load.Name = "force_factor_load";
            this.force_factor_load.Size = new System.Drawing.Size(264, 22);
            this.force_factor_load.TabIndex = 13;
            this.force_factor_load.Text = "0.0";
            // 
            // deformetion_reset_zero
            // 
            this.deformetion_reset_zero.Location = new System.Drawing.Point(417, 129);
            this.deformetion_reset_zero.Name = "deformetion_reset_zero";
            this.deformetion_reset_zero.Size = new System.Drawing.Size(264, 22);
            this.deformetion_reset_zero.TabIndex = 14;
            this.deformetion_reset_zero.Text = "0.0";
            // 
            // no_of_speciemen
            // 
            this.no_of_speciemen.Location = new System.Drawing.Point(417, 193);
            this.no_of_speciemen.Name = "no_of_speciemen";
            this.no_of_speciemen.Size = new System.Drawing.Size(264, 22);
            this.no_of_speciemen.TabIndex = 15;
            this.no_of_speciemen.Text = "0.0";
            // 
            // specimen_fractured
            // 
            this.specimen_fractured.Location = new System.Drawing.Point(417, 227);
            this.specimen_fractured.Name = "specimen_fractured";
            this.specimen_fractured.Size = new System.Drawing.Size(264, 22);
            this.specimen_fractured.TabIndex = 16;
            this.specimen_fractured.Text = "0.0";
            // 
            // port_name
            // 
            this.port_name.FormattingEnabled = true;
            this.port_name.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12"});
            this.port_name.Location = new System.Drawing.Point(417, 281);
            this.port_name.Name = "port_name";
            this.port_name.Size = new System.Drawing.Size(264, 24);
            this.port_name.TabIndex = 17;
            // 
            // save_button_setting_page
            // 
            this.save_button_setting_page.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_button_setting_page.Location = new System.Drawing.Point(210, 400);
            this.save_button_setting_page.Name = "save_button_setting_page";
            this.save_button_setting_page.Size = new System.Drawing.Size(333, 53);
            this.save_button_setting_page.TabIndex = 19;
            this.save_button_setting_page.Text = " Save";
            this.save_button_setting_page.UseVisualStyleBackColor = true;
            this.save_button_setting_page.Click += new System.EventHandler(this.setup_save_button_setting_page_Click);
            // 
            // baud_rate
            // 
            this.baud_rate.Location = new System.Drawing.Point(417, 323);
            this.baud_rate.Name = "baud_rate";
            this.baud_rate.Size = new System.Drawing.Size(264, 22);
            this.baud_rate.TabIndex = 21;
            this.baud_rate.Text = "115200";
            // 
            // message_label
            // 
            this.message_label.AutoSize = true;
            this.message_label.Location = new System.Drawing.Point(16, 366);
            this.message_label.Name = "message_label";
            this.message_label.Size = new System.Drawing.Size(77, 17);
            this.message_label.TabIndex = 22;
            this.message_label.Text = "Message : ";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 491);
            this.Controls.Add(this.message_label);
            this.Controls.Add(this.baud_rate);
            this.Controls.Add(this.save_button_setting_page);
            this.Controls.Add(this.port_name);
            this.Controls.Add(this.specimen_fractured);
            this.Controls.Add(this.no_of_speciemen);
            this.Controls.Add(this.deformetion_reset_zero);
            this.Controls.Add(this.force_factor_load);
            this.Controls.Add(this.displacement_reset_zero);
            this.Controls.Add(this.correction_factor_displacement);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox correction_factor_displacement;
        private System.Windows.Forms.TextBox displacement_reset_zero;
        private System.Windows.Forms.TextBox force_factor_load;
        private System.Windows.Forms.TextBox deformetion_reset_zero;
        private System.Windows.Forms.TextBox no_of_speciemen;
        private System.Windows.Forms.TextBox specimen_fractured;
        private System.Windows.Forms.ComboBox port_name;
        private System.Windows.Forms.Button save_button_setting_page;
        private System.Windows.Forms.TextBox baud_rate;
        private System.Windows.Forms.Label message_label;
    }
}