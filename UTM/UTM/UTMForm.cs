using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using log4net;

namespace UTM
{
    public partial class UTMForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UTMForm));

        Hashtable materialData;
        SerialPort serialPort;
        private Thread utmGraphThread;
        private List<GraphData> graphPlotDataList;
        private StringBuilder realTimeDataStorage;
        private bool cameraSwitch = false;

        double minY = Double.MaxValue;
        int counter = 0;

        //Set Up serial communication
        private void SetUpComPort()
        {
            try
            {
                serialPort = new SerialPort(Variables.portName, Variables.baundRate);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                serialPort.Open();
            }
            catch (Exception e)
            {
                log.Error(e);
                Console.WriteLine(e);
            }
        }

        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            realTimeDataStorage.Append(sp.ReadExisting());
        }

        private void LoadMaterialData()
        {
            materialData = Util.LoadData("MaterialFile.dat");
            if (materialData != null)
            {
                A.Text = (string)materialData["a"];
                L.Text = (string)materialData["l"];
            }
        }

        public UTMForm()
        {
            InitializeComponent();
            //graph plot type
            utm_chart.Series[0].ChartType = SeriesChartType.Line;
            //reload saved data
            LoadMaterialData();
            //image capture init
            InitImageCapture();
        }
         
        private void UTMForm_Load(object sender, EventArgs e) { }

        //Thread Function
        private void GetGraphData()
        {
            while (true)
            {
                graphPlotDataList = new List<GraphData>();
                CalculateData(realTimeDataStorage);
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

                    if (counter == 10000)
                    {
                        if (cameraSwitch)
                            Util.SaveImageCapture(gData.raw_x + "-" + DateTime.Now.ToFileTime(), this.pictureBox.Image);
                        counter = 0;
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                log.Error("Graph plot, data missing during parsing");
            }
        }



        //invoke when start_button_click
        private void start_button_Click(object sender, EventArgs e)
        {
            string errorMsg = null;
            try
            {
                //reset old values
                minY = Double.MaxValue;
                realTimeDataStorage = new StringBuilder();

                //Load previoulsy save data
                Util.LoadSettingData();

                try
                {
                    //old code neet to replace
                    LoadDataFromSettingForm();
                    //init arduino
                    SetUpComPort();
                    //Util.ShowInfo(message_label, "Message: Set Up Done!");

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    //RefreshFormSettingData();
                    errorMsg = "Message: Please set up form with correct data";
                    //Util.ShowError(message_label, "Message: Please set up form with correct data");
                    log.Error(ex);
                    throw;
                }

                //read data from arduino
                try
                {
                    serialPort.Write("sbem");
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Please set up form with correct data";
                    //Util.ShowError(message_label, "Message: Port Closed");
                    log.Error(ex);
                    throw;
                }

                //plot graph from incoming data
                try
                {
                    utmGraphThread = new Thread(new ThreadStart(this.GetGraphData));
                    utmGraphThread.IsBackground = true;
                    utmGraphThread.Start();
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Graph thread error";
                    log.Error(ex);
                    throw;
                }
                Util.ShowInfo(message_label, "Message: Experiment Running...");
            }
            catch (Exception ex)
            {
                Util.ShowError(message_label, errorMsg);
                //log.Error("Erro in start button :", ex);
            }
        }

        //invoke when stop_button_click
        private void stop_button_Click(object sender, EventArgs e)
        {
            string errorMsg = null;
            try
            {
                try
                {
                    if (cameraSwitch)
                        this.WebCamCapture.Stop();
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Webcam shutdown error";
                    //Util.ShowError(message_label, "Message: Webcam shutdown error");
                    log.Error(ex);
                }

                try
                {
                    serialPort.Write("sxem");
                    serialPort.Close();
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Port closed error";
                    //Util.ShowError(message_label, "Message: Port closed error");
                    log.Error(ex);
                }

                try
                {
                    utmGraphThread.Abort();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Graph thread error";
                    //Util.ShowError(message_label, "Message: Graph thread Error");
                    log.Error(ex);
                }
                Util.SaveExperimentData(realTimeDataStorage);
                Util.ShowInfo(message_label, "Message: Experiment stop successfully!");
            }
            catch (Exception ex)
            {
                //log.Error(ex);
                Util.ShowError(message_label, errorMsg);
            }
        }

        private void LoadDataFromSettingForm()
        {
            if (Variables.portName == null || Variables.portName.Equals(""))
            {
                Variables.portName = Util.AutodetectArduinoPort();
            }

            Variables.area = System.Convert.ToDouble(A.Text);
            Variables.lenght = System.Convert.ToDouble(L.Text);

            SaveMaterialData();
        }

        private void CalculateData(StringBuilder utmStorageData)
        {
            string _pairedData = "";
            try
            {
                string utmDataString = utmStorageData.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');
                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        _pairedData = pairedData;
                        string[] utmDataArray = pairedData.Split(',');

                        var X = Math.Abs((System.Convert.ToDouble(utmDataArray[1])) * Variables.displacementPerPulse) / Variables.lenght;
                        var Y = (System.Convert.ToDouble(utmDataArray[0]) * Variables.forceConversionFactor) / Variables.area;

                        //ignore negative value for better ploting
                        if (X >= 0 || Y >= 0)
                        {
                            if (Y <= minY)
                                minY = Y;

                            Y = Y - minY;

                            GraphData gData = new GraphData();

                            gData.x = X;
                            gData.y = Y;
                            gData.raw_x = System.Convert.ToDouble(utmDataArray[0]);
                            gData.raw_y = System.Convert.ToDouble(utmDataArray[1]);

                            graphPlotDataList.Add(gData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ignore data parsing exception
                log.Error("Data parsing exception! , Data : " + _pairedData);
            }
        }

        private void SaveMaterialData()
        {
            Hashtable localData = new Hashtable();

            localData.Add("a", A.Text);
            localData.Add("l", L.Text);

            Util.SaveData(localData, "MaterialFile.dat");
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            settingForm.Show();
        }

        //invoke when set_up button click
        private void WebCamCapture_ImageCaptured(object source, WebCam_Capture.WebcamEventArgs e)
        {
            // set the picturebox picture
            this.pictureBox.Image = e.WebCamImage;
        }

        private void InitImageCapture()
        {
            this.WebCamCapture.CaptureHeight = this.pictureBox.Height;
            this.WebCamCapture.CaptureWidth = this.pictureBox.Width;
            this.WebCamCapture.TimeToCapture_milliseconds = 20;
            this.WebCamCapture.Start(0);
        }

        private void camera_button_Click(object sender, EventArgs e)
        {
            cameraSwitch = !cameraSwitch;

            if (cameraSwitch)
                this.camera_button.ForeColor = System.Drawing.Color.Green;
            else
                this.camera_button.ForeColor = System.Drawing.Color.Red;
        }
    }
}
