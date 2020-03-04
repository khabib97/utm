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
            chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.WebCamCapture = new WebCam_Capture.WebCamCapture();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.start_button = new System.Windows.Forms.Button();
            this.stop_button = new System.Windows.Forms.Button();
            this.utm_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.A = new System.Windows.Forms.TextBox();
            this.L = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.message_label = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setting_menu_item = new System.Windows.Forms.ToolStripMenuItem();
            this.load_data_menu_item = new System.Windows.Forms.ToolStripMenuItem();
            this.guide_menu_item = new System.Windows.Forms.ToolStripMenuItem();
            this.about_menu_item = new System.Windows.Forms.ToolStripMenuItem();
            this.graph_combo_box = new System.Windows.Forms.ComboBox();
            this.graph_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utm_chart)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // WebCamCapture
            // 
            this.WebCamCapture.CaptureHeight = 240;
            this.WebCamCapture.CaptureWidth = 320;
            this.WebCamCapture.FrameNumber = ((ulong)(0ul));
            this.WebCamCapture.Location = new System.Drawing.Point(0, 0);
            this.WebCamCapture.Name = "WebCamCapture";
            this.WebCamCapture.Size = new System.Drawing.Size(342, 252);
            this.WebCamCapture.TabIndex = 0;
            this.WebCamCapture.TimeToCapture_milliseconds = 100;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 50);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // start_button
            // 
            this.start_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_button.ForeColor = System.Drawing.Color.Green;
            this.start_button.Location = new System.Drawing.Point(1118, 42);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(150, 40);
            this.start_button.TabIndex = 0;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // stop_button
            // 
            this.stop_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stop_button.ForeColor = System.Drawing.Color.Red;
            this.stop_button.Location = new System.Drawing.Point(1118, 88);
            this.stop_button.Name = "stop_button";
            this.stop_button.Size = new System.Drawing.Size(150, 36);
            this.stop_button.TabIndex = 1;
            this.stop_button.Text = "Filter Data";
            this.stop_button.UseVisualStyleBackColor = true;
            this.stop_button.Click += new System.EventHandler(this.stop_button_Click);
            // 
            // utm_chart
            // 
            chartArea.Name = "ChartArea";
            this.utm_chart.ChartAreas.Add(chartArea);
            legend1.Name = "Legend1";
            this.utm_chart.Legends.Add(legend1);
            this.utm_chart.Location = new System.Drawing.Point(12, 141);
            this.utm_chart.Name = "utm_chart";
            series1.ChartArea = "ChartArea";
            series1.Legend = "Legend1";
            series1.Name = "Unfiltered Graph";
            this.utm_chart.Series.Add(series1);
            this.utm_chart.Size = new System.Drawing.Size(1267, 491);
            this.utm_chart.TabIndex = 6;
            this.utm_chart.Text = "utm chart";
            // 
            // A
            // 
            this.A.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.A.Location = new System.Drawing.Point(108, 86);
            this.A.Name = "A";
            this.A.Size = new System.Drawing.Size(197, 26);
            this.A.TabIndex = 8;
            // 
            // L
            // 
            this.L.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L.Location = new System.Drawing.Point(108, 46);
            this.L.Name = "L";
            this.L.Size = new System.Drawing.Size(197, 26);
            this.L.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Lenght";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Diameter";
            // 
            // message_label
            // 
            this.message_label.AutoSize = true;
            this.message_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message_label.Location = new System.Drawing.Point(431, 46);
            this.message_label.Name = "message_label";
            this.message_label.Size = new System.Drawing.Size(0, 20);
            this.message_label.TabIndex = 17;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setting_menu_item,
            this.load_data_menu_item,
            this.guide_menu_item,
            this.about_menu_item});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1291, 35);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // setting_menu_item
            // 
            this.setting_menu_item.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.setting_menu_item.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.setting_menu_item.Margin = new System.Windows.Forms.Padding(1);
            this.setting_menu_item.Name = "setting_menu_item";
            this.setting_menu_item.Size = new System.Drawing.Size(85, 29);
            this.setting_menu_item.Text = "Setting";
            this.setting_menu_item.Click += new System.EventHandler(this.setting_menu_item_Click);
            // 
            // load_data_menu_item
            // 
            this.load_data_menu_item.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.load_data_menu_item.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.load_data_menu_item.Margin = new System.Windows.Forms.Padding(1);
            this.load_data_menu_item.Name = "load_data_menu_item";
            this.load_data_menu_item.Size = new System.Drawing.Size(111, 29);
            this.load_data_menu_item.Text = "Load Data";
            this.load_data_menu_item.Click += new System.EventHandler(this.load_data_menu_item_Click);
            // 
            // guide_menu_item
            // 
            this.guide_menu_item.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.guide_menu_item.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.guide_menu_item.Margin = new System.Windows.Forms.Padding(1);
            this.guide_menu_item.Name = "guide_menu_item";
            this.guide_menu_item.Size = new System.Drawing.Size(76, 29);
            this.guide_menu_item.Text = "Guide";
            this.guide_menu_item.Click += new System.EventHandler(this.guide_menu_item_Click);
            // 
            // about_menu_item
            // 
            this.about_menu_item.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.about_menu_item.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.about_menu_item.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.about_menu_item.Margin = new System.Windows.Forms.Padding(1);
            this.about_menu_item.Name = "about_menu_item";
            this.about_menu_item.Size = new System.Drawing.Size(77, 29);
            this.about_menu_item.Text = "About";
            this.about_menu_item.Click += new System.EventHandler(this.about_menu_item_Click);
            // 
            // graph_combo_box
            // 
            this.graph_combo_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graph_combo_box.ForeColor = System.Drawing.Color.Blue;
            this.graph_combo_box.FormattingEnabled = true;
            this.graph_combo_box.Location = new System.Drawing.Point(456, 90);
            this.graph_combo_box.Name = "graph_combo_box";
            this.graph_combo_box.Size = new System.Drawing.Size(223, 28);
            this.graph_combo_box.TabIndex = 20;
            // 
            // graph_label
            // 
            this.graph_label.AutoSize = true;
            this.graph_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graph_label.ForeColor = System.Drawing.Color.DarkBlue;
            this.graph_label.Location = new System.Drawing.Point(376, 96);
            this.graph_label.Name = "graph_label";
            this.graph_label.Size = new System.Drawing.Size(55, 20);
            this.graph_label.TabIndex = 21;
            this.graph_label.Text = "Graph";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(311, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 18);
            this.label1.TabIndex = 22;
            this.label1.Text = "mm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(311, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "mm";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(722, 84);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(82, 22);
            this.radioButton1.TabIndex = 24;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Tension";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(722, 112);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(119, 22);
            this.radioButton2.TabIndex = 25;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Compression";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // UTMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 644);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.graph_label);
            this.Controls.Add(this.graph_combo_box);
            this.Controls.Add(this.message_label);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.L);
            this.Controls.Add(this.A);
            this.Controls.Add(this.utm_chart);
            this.Controls.Add(this.stop_button);
            this.Controls.Add(this.start_button);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "UTMForm";
            this.Text = "Universal Tesitng Machine";
            this.Load += new System.EventHandler(this.UTMForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utm_chart)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea;
        private WebCam_Capture.WebCamCapture WebCamCapture;
        private System.Windows.Forms.PictureBox pictureBox;

        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Button stop_button;
        private System.Windows.Forms.DataVisualization.Charting.Chart utm_chart;
        private System.Windows.Forms.TextBox A;
        private System.Windows.Forms.TextBox L;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label message_label;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ComboBox graph_combo_box;
        private System.Windows.Forms.Label graph_label;
        private System.Windows.Forms.ToolStripMenuItem setting_menu_item;
        private System.Windows.Forms.ToolStripMenuItem load_data_menu_item;
        private System.Windows.Forms.ToolStripMenuItem guide_menu_item;
        private System.Windows.Forms.ToolStripMenuItem about_menu_item;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        //private System.Windows.Forms.Button camera_button;
    }
}

