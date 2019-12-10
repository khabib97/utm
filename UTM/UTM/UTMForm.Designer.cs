namespace UTM
{
    partial class UTMForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.start_button = new System.Windows.Forms.Button();
            this.stop_button = new System.Windows.Forms.Button();
            this.port_name = new System.Windows.Forms.ComboBox();
            this.baund_rate = new System.Windows.Forms.TextBox();
            this.set_up = new System.Windows.Forms.Button();
            this.utm_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.force_conversion_factor = new System.Windows.Forms.TextBox();
            this.A = new System.Windows.Forms.TextBox();
            this.displacement_per_puls = new System.Windows.Forms.TextBox();
            this.L = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.message_label = new System.Windows.Forms.Label();
            this.guide = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.utm_chart)).BeginInit();
            this.SuspendLayout();
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(1129, 51);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(150, 40);
            this.start_button.TabIndex = 0;
            this.start_button.Text = "start button";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // stop_button
            // 
            this.stop_button.Location = new System.Drawing.Point(1129, 97);
            this.stop_button.Name = "stop_button";
            this.stop_button.Size = new System.Drawing.Size(150, 36);
            this.stop_button.TabIndex = 1;
            this.stop_button.Text = "stop button";
            this.stop_button.UseVisualStyleBackColor = true;
            this.stop_button.Click += new System.EventHandler(this.stop_button_Click);
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
            this.port_name.Location = new System.Drawing.Point(826, 81);
            this.port_name.Name = "port_name";
            this.port_name.Size = new System.Drawing.Size(170, 24);
            this.port_name.TabIndex = 2;
            // 
            // baund_rate
            // 
            this.baund_rate.Location = new System.Drawing.Point(826, 111);
            this.baund_rate.Name = "baund_rate";
            this.baund_rate.Size = new System.Drawing.Size(170, 22);
            this.baund_rate.TabIndex = 3;
            this.baund_rate.Text = "115200";
            // 
            // set_up
            // 
            this.set_up.Location = new System.Drawing.Point(1025, 51);
            this.set_up.Name = "set_up";
            this.set_up.Size = new System.Drawing.Size(98, 83);
            this.set_up.TabIndex = 5;
            this.set_up.Text = "Set Up";
            this.set_up.UseVisualStyleBackColor = true;
            this.set_up.Click += new System.EventHandler(this.set_up_Click);
            // 
            // utm_chart
            // 
            chartArea1.Name = "ChartArea1";
            this.utm_chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.utm_chart.Legends.Add(legend1);
            this.utm_chart.Location = new System.Drawing.Point(12, 137);
            this.utm_chart.Name = "utm_chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "UTM Graph";
            this.utm_chart.Series.Add(series1);
            this.utm_chart.Size = new System.Drawing.Size(1267, 495);
            this.utm_chart.TabIndex = 6;
            this.utm_chart.Text = "utm chart";
            // 
            // force_conversion_factor
            // 
            this.force_conversion_factor.Location = new System.Drawing.Point(537, 81);
            this.force_conversion_factor.Name = "force_conversion_factor";
            this.force_conversion_factor.Size = new System.Drawing.Size(154, 22);
            this.force_conversion_factor.TabIndex = 7;
            // 
            // A
            // 
            this.A.Location = new System.Drawing.Point(537, 109);
            this.A.Name = "A";
            this.A.Size = new System.Drawing.Size(154, 22);
            this.A.TabIndex = 8;
            // 
            // displacement_per_puls
            // 
            this.displacement_per_puls.Location = new System.Drawing.Point(179, 83);
            this.displacement_per_puls.Name = "displacement_per_puls";
            this.displacement_per_puls.Size = new System.Drawing.Size(183, 22);
            this.displacement_per_puls.TabIndex = 9;
            // 
            // L
            // 
            this.L.Location = new System.Drawing.Point(179, 111);
            this.L.Name = "L";
            this.L.Size = new System.Drawing.Size(183, 22);
            this.L.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Displacement Per Puls";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "Lenght";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(368, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Force Conversion Factor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(493, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Area";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(777, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(728, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 17);
            this.label6.TabIndex = 16;
            this.label6.Text = "Baund Rate";
            // 
            // message_label
            // 
            this.message_label.AutoSize = true;
            this.message_label.Location = new System.Drawing.Point(23, 49);
            this.message_label.Name = "message_label";
            this.message_label.Size = new System.Drawing.Size(69, 17);
            this.message_label.TabIndex = 17;
            this.message_label.Text = "Message:";
            // 
            // guide
            // 
            this.guide.AutoSize = true;
            this.guide.Location = new System.Drawing.Point(23, 9);
            this.guide.Name = "guide";
            this.guide.Size = new System.Drawing.Size(408, 17);
            this.guide.TabIndex = 18;
            this.guide.Text = "Guide : 1. Fill up data 2. Click Set Up 3. Click Start 4. Click Stop ";
            // 
            // UTMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 644);
            this.Controls.Add(this.guide);
            this.Controls.Add(this.message_label);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.L);
            this.Controls.Add(this.displacement_per_puls);
            this.Controls.Add(this.A);
            this.Controls.Add(this.force_conversion_factor);
            this.Controls.Add(this.utm_chart);
            this.Controls.Add(this.set_up);
            this.Controls.Add(this.baund_rate);
            this.Controls.Add(this.port_name);
            this.Controls.Add(this.stop_button);
            this.Controls.Add(this.start_button);
            this.Name = "UTMForm";
            this.Text = "Universal Tesitng Machine";
            this.Load += new System.EventHandler(this.UTMForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.utm_chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Button stop_button;
        private System.Windows.Forms.ComboBox port_name;
        private System.Windows.Forms.TextBox baund_rate;
        private System.Windows.Forms.Button set_up;
        private System.Windows.Forms.DataVisualization.Charting.Chart utm_chart;
        private System.Windows.Forms.TextBox force_conversion_factor;
        private System.Windows.Forms.TextBox A;
        private System.Windows.Forms.TextBox displacement_per_puls;
        private System.Windows.Forms.TextBox L;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label message_label;
        private System.Windows.Forms.Label guide;
    }
}

