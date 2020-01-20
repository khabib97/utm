using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using log4net;

using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Drawing;
using System.Drawing;

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
        String time = DateTime.Now.ToFileTime().ToString();
        private double timer = 0;
        private int graphChooser = 0;

        double minY = Double.MaxValue;
        int counter = 0;

        //Set Up serial communication
        private void SetUpComPort()
        {
            try
            {
                serialPort = new SerialPort(Variables.portName, Variables.baundRate);
                serialPort.DtrEnable = false;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                serialPort.Open();
                //serialPort.DtrEnable = true;
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

        private void LoadGraphType()
        {
            Dictionary<int,string> comboSource = new Dictionary<int,string>();
            comboSource.Add(1, "Stress Vs Strain");
            //comboSource.Add(2, "Load Vs Time");
            comboSource.Add(3, "Load Vs Displacement");
           
            graph_combo_box.DataSource = new BindingSource(comboSource, null);
            graph_combo_box.DisplayMember = "Value";
            graph_combo_box.ValueMember = "Key";
        }

        public UTMForm()
        {
            InitializeComponent();

            LoadGraphType();
            //graph plot type
            utm_chart.Series[0].ChartType = SeriesChartType.Line;
            //reload saved data
            LoadMaterialData();
            //image capture init
            //InitImageCapture();
        }
         
        private void UTMForm_Load(object sender, EventArgs e) { }

        //Thread Function
        private void GetGraphData()
        {
            while (true)
            {
                timer = timer + 0.2;

                graphPlotDataList = new List<GraphData>();
                //CalculateData(realTimeDataStorage);
                ReDrawGraph();
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
                    switch (graphChooser) {
                        case 1:
                            utm_chart.Series[0].Points.AddXY(gData.x, gData.y);
                            break;
                        case 2:
                            utm_chart.Series[0].Points.AddXY(gData.timer, gData.y );
                            break;
                        case 3:
                            utm_chart.Series[0].Points.AddXY(gData.x, gData.y);
                            break;

                    }
                

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
                time = DateTime.Now.ToFileTime().ToString();
                timer = DateTime.Now.ToFileTime();
                graphChooser = ((System.Collections.Generic.KeyValuePair<int,string>)(graph_combo_box.SelectedItem)).Key;

                
                switch (graphChooser) {
                    case 1:
                        chartArea3.AxisX.Title = "Strain(mm/mm)";
                        chartArea3.AxisY.Title = "Stress(MPa)";
                        break;
                    case 2:
                        chartArea3.AxisX.Title = "Time(s)";
                        chartArea3.AxisY.Title = "Load(kN)";
                        break;
                    case 3:
                        chartArea3.AxisX.Title = "Displacement(mm)";
                        chartArea3.AxisY.Title = "Load(kN)";
                        break;
                }
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
                    /*string textFile = @"C:\Users\kawse\Desktop\Experiment_Data_132236422881759293-20200119T034127Z-001\Experiment_Data_132236422881759293\RawData-132236422881759293.txt";
                    if (File.Exists(textFile))
                    {
                        // Read a text file line by line.
                        string[] lines = File.ReadAllLines(textFile);
                        foreach (string line in lines)
                            realTimeDataStorage.Append(line+"\n");
                    }*/

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
                Directory.CreateDirectory("Experiment_Data_" + time);

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
                Util.SaveExperimentData(realTimeDataStorage,time);
                SaveChartAsImage();
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
                double adjustData = pairedDataArray[0] != null && pairedDataArray[0].Equals("") ? (System.Convert.ToDouble(pairedDataArray[0].Split(',')[1])): 0;
                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        _pairedData = pairedData;
                        string[] utmDataArray = pairedData.Split(',');

                        var displacement = Math.Abs((System.Convert.ToDouble(utmDataArray[1]) - adjustData) * Variables.displacementPerPulse);
                        var force = (System.Convert.ToDouble(utmDataArray[0]) * Variables.forceConversionFactor);


                        double X = 0;
                        double Y = 0; 

                        switch (graphChooser) {
                            case 1:
                                X = displacement / Variables.lenght; 
                                Y = force / Variables.area;
                                break;
                            case 2:
                                X = displacement;
                                Y = force;
                                break;
                            case 3:
                                X = displacement;
                                Y = force;
                                break;


                        }

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
                            gData.timer = DateTime.Now.ToFileTime()- timer;

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

        public void ReDrawGraph() {
            string _pairedData = "";
            try
            {
                
                string utmDataString = realTimeDataStorage.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');
                double adjustData = pairedDataArray[0] != null && !pairedDataArray[0].Equals("") ? (System.Convert.ToDouble(pairedDataArray[0].Split(',')[1])) : 0;

                double secondBufferedValue = adjustData;
                ArrayList firstBufferedList = new ArrayList(); 

                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        _pairedData = pairedData;
                        string[] utmDataArray = pairedData.Split(',');
                        var firstValue = System.Convert.ToDouble(utmDataArray[0]);
                        var secondValue = System.Convert.ToDouble(utmDataArray[1]);

                        if (secondBufferedValue == secondValue)
                        {
                            firstBufferedList.Add(firstValue);
                        }
                        else
                        {
                            
                            double sum = 0;
                            foreach (double i in firstBufferedList)
                            {
                                sum = sum + i;
                            }
                            sum = sum / firstBufferedList.Count;
                            firstBufferedList = new ArrayList();
                            firstBufferedList.Add(firstValue);

                            var displacement = Math.Abs(secondValue - adjustData) * Variables.displacementPerPulse;
                            var force = sum /*firstValue*/ * Variables.forceConversionFactor;

                            secondBufferedValue = secondValue;
                            double X = 0;
                            double Y = 0;

                            switch (graphChooser)
                            {
                                case 1:
                                    X = displacement / Variables.lenght;
                                    Y = force / Variables.area;
                                    break;
                                case 2:
                                    X = displacement;
                                    Y = force;
                                    break;
                                case 3:
                                    X = displacement;
                                    Y = force;
                                    break;
                            }

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
                                gData.timer = DateTime.Now.ToFileTime() - timer;

                                graphPlotDataList.Add(gData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
            }
        }
        /*private void InitImageCapture()
        {
            this.WebCamCapture.CaptureHeight = this.pictureBox.Height;
            this.WebCamCapture.CaptureWidth = this.pictureBox.Width;
            this.WebCamCapture.TimeToCapture_milliseconds = 20;
            this.WebCamCapture.Start(0);
        }*/

        /*private void camera_button_Click(object sender, EventArgs e)
        {
            cameraSwitch = !cameraSwitch;

            if (cameraSwitch)
                this.camera_button.ForeColor = System.Drawing.Color.Green;
            else
                this.camera_button.ForeColor = System.Drawing.Color.Red;
        }*/

        private void SaveChartAsImage()
        {
           
            String picFilePath = "Experiment_Data_" + time+"/"+"Image-" + time + ".jpge";
            //String excelPath = "Excel-" + time + ".xlsx";

            this.utm_chart.SaveImage(picFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            Thread.Sleep(2000);
            SaveGraphImage(picFilePath);


        }

        private void SaveGraphImage(String picFilePath)
        {
            using (ExcelPackage ExcelPkg = new ExcelPackage())
            {
                ExcelWorksheet wsSheet1 = ExcelPkg.Workbook.Worksheets.Add("UMTResultOutput");
                using (ExcelRange Rng = wsSheet1.Cells["A1:G2"])
                {
                    Rng.Value = "Amar Source UTM Result";
                    Rng.Merge = true;  
                    Rng.Style.Font.Size = 14;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Italic = false;
                }

                wsSheet1.Cells["A4"].Value = "Length ";
                wsSheet1.Cells["B4"].Value = Variables.lenght;

                wsSheet1.Cells["A5"].Value = "Area ";
                wsSheet1.Cells["B5"].Value = Variables.area;

                wsSheet1.Cells["A6:D6"].Merge = true;
                wsSheet1.Cells["A6:D6"].Value = "Displacement Per Pulse";
                wsSheet1.Cells["E6"].Value = Variables.displacementPerPulse;

                wsSheet1.Cells["A7:D7"].Merge = true;
                wsSheet1.Cells["A7:D7"].Value = "Force Convertion Factor";
                wsSheet1.Cells["E7"].Value = Variables.forceConversionFactor;

                wsSheet1.Cells["D9:F9"].Merge = true;
                wsSheet1.Cells["D9:F9"].Value = ((System.Collections.Generic.KeyValuePair<int, string>)(graph_combo_box.SelectedItem)).Value+" Graph";
                wsSheet1.Cells["D9:F9"].Style.Font.Size = 12;
                wsSheet1.Cells["D9:F9"].Style.Font.Bold = true;

                int rowIndex = 10;
                int colIndex = 0;
                int PixelTop = 88;
                int PixelLeft = 129;
                //int Height = 491;
                //int Width = 1267;
                Image img = Image.FromFile(picFilePath);
                ExcelPicture pic = wsSheet1.Drawings.AddPicture("UTMDataImage", img);
                pic.SetPosition(rowIndex, 0, colIndex, 0);
                //pic.SetPosition(PixelTop, PixelLeft);  
                pic.SetSize(700, 350);
                //pic.SetSize(40);  
                wsSheet1.Protection.IsProtected = false;
                wsSheet1.Protection.AllowSelectLockedCells = false;
                ExcelPkg.SaveAs(new FileInfo("Experiment_Data_" + time+"/"+"Report-" +time+".xlsx"));
            }
        }
    }
}
