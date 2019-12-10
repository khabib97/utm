﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO.Ports;
using System.Management;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UTM
{
    public partial class UTMForm : Form
    {
        SerialPort serialPort;
        private Thread utmGraphThread;
        private List<GraphData> graphPlotDataList;
        private StringBuilder realTimeStorage;

        string portName;
        int baundRate = 115200;
        double area;
        double lenght;
        double displacementPerPulse;
        double forceConversionFactor;
        double minY = Double.MaxValue;

        //Set Up serial communication
        private void SetUpComPort()
        {
            try
            {
                serialPort = new SerialPort(portName, baundRate);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                serialPort.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            realTimeStorage.Append(sp.ReadExisting());
        }

        public UTMForm() {
            InitializeComponent();

            utm_chart.Series[0].ChartType = SeriesChartType.Line;

            port_name.Text = AutodetectArduinoPort(); 
        }

        private void UTMForm_Load(object sender, EventArgs e) { }

        //Thread Function
        private void GetGraphData()
        {
            while (true)
            {
                graphPlotDataList = new List<GraphData>();
                CalculateData(realTimeStorage);
                if (utm_chart.IsHandleCreated)
                    this.Invoke((MethodInvoker)delegate { UpdateUTMChart(); });

                Thread.Sleep(1000);
            }
        }

        private void UpdateUTMChart()
        {
            try
            {
                utm_chart.Series[0].Points.Clear();
                foreach (GraphData gData in graphPlotDataList)
                {
                    utm_chart.Series[0].Points.AddXY(gData.x, gData.y);
                }
            }
            catch (Exception ex) {
                //ignore 
            }
        }

        //invoke when set_up button click
        private void set_up_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDataFromSettingForm();
                ShowInfo("Message: Set Up Done!");

                SetUpComPort();
            }
            catch (Exception ex)
            {
                //RefreshFormSettingData();
                ShowError("Message: Please set up form with correct data");
                Console.WriteLine(ex);
            }
        }

        //invoke when start_button_click
        private void start_button_Click(object sender, EventArgs e)
        {
            serialPort.Write("sbem");
            ShowInfo("Message: Experiment Running...");
            Thread.Sleep(100);
            
            realTimeStorage = new StringBuilder();
            utmGraphThread = new Thread(new ThreadStart(this.GetGraphData));
            utmGraphThread.IsBackground = true;
            utmGraphThread.Start();
        }

        //invoke when stop_button_click
        private void stop_button_Click(object sender, EventArgs e) {
            serialPort.Write("sxem");
            ShowInfo("Message: Experiment Cancel");
        }

        private void ShowError(string msg)
        {
            message_label.ForeColor = Color.Red;
            message_label.Text = msg;
        }

        private void ShowInfo(string msg)
        {
            message_label.ForeColor = Color.Green;
            message_label.Text = msg;
        }

        private void LoadDataFromSettingForm()
        {
            portName = port_name.Text;
            baundRate = System.Convert.ToInt32(baund_rate.Text);
            area = System.Convert.ToDouble(A.Text);
            lenght = System.Convert.ToDouble(L.Text);
            displacementPerPulse = System.Convert.ToDouble(displacement_per_puls.Text);
            forceConversionFactor = System.Convert.ToDouble(force_conversion_factor.Text);
        }

        private void RefreshFormSettingData()
        {
            //port_name.Text = "";
            baund_rate.Text = "";
            A.Text = "";
            L.Text = "";
            displacement_per_puls.Text = "";
            force_conversion_factor.Text = "";
        }

        private void CalculateData(StringBuilder utmStorageData)
        {
            try
            {
                string utmDataString = utmStorageData.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');
                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        string[] utmDataArray = pairedData.Split(',');

                        var X = (System.Convert.ToDouble(utmDataArray[0]) * displacementPerPulse) / lenght;
                        var Y = (System.Convert.ToDouble(utmDataArray[1]) * forceConversionFactor) / area;

                        //ignore negative value for better ploting
                        if (X >= 0 || Y >= 0)
                        {
                            if (Y <= minY)
                                minY = Y;

                            Y = Y - minY;

                            GraphData gData = new GraphData();
                            gData.x = X;
                            gData.y = Y;

                            graphPlotDataList.Add(gData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ignore data parsing exception
            }
        }


        // if this method failed to detect port name, then user can manually update port
        private string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    //string desc = item["Description"].ToString();
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