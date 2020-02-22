using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using log4net;
using System.IO;
//using MathNet.Filtering;
using Filtering;
using System.Threading.Tasks;

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
        String time = null;
        //bool isCalculationDone = false;
        private int graphChooser = 0;

        private double[] _x;
        private double[] _y;
        //DoubleVector graphData = null;


        float minY = (float)Int32.MaxValue;
        //Set Up serial communication
        private void SetUpComPort()
        {
            try
            {
                serialPort = new SerialPort(Variables.portName, Variables.baundRate);
                serialPort.DtrEnable = false;
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

        private void LoadGraphType()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add((int)Variables.Experiment.StressVsStrain, "Stress Vs Strain");
            comboSource.Add((int)Variables.Experiment.LoadVsTime, "Load Vs Time");
            comboSource.Add((int)Variables.Experiment.LoadVsDisplacement, "Load Vs Displacement");

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
        }

        private void UTMForm_Load(object sender, EventArgs e) { }

        //Thread Function
        private void GetGraphData()
        {
            while (true)
            {
                graphPlotDataList = new List<GraphData>();
                if (Variables.filter == (int)Variables.Filtering.AverageWithSavitzkyGolay) CalculateGraphData();
                else iCalculateGraphData();
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
                    switch (graphChooser)
                    {
                        case (int)Variables.Experiment.StressVsStrain:
                            utm_chart.Series[0].Points.AddXY(gData.x.ToString("F"), gData.y);
                            break;
                        case (int)Variables.Experiment.LoadVsTime:
                            utm_chart.Series[0].Points.AddXY(gData.timer, gData.y);
                            break;
                        case (int)Variables.Experiment.LoadVsDisplacement:
                            utm_chart.Series[0].Points.AddXY(gData.x.ToString("F"), gData.y);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                log.Error("Graph plot, data missing during parsing");
            }
        }

        private void SetGraphAxisLabel()
        {
            switch (graphChooser)
            {
                case 1:
                    chartArea.AxisX.Title = "Strain(mm/mm)";
                    chartArea.AxisY.Title = "Stress(MPa)";
                    break;
                case 2:
                    chartArea.AxisX.Title = "Time(s)";
                    chartArea.AxisY.Title = "Load(kN)";
                    break;
                case 3:
                    chartArea.AxisX.Title = "Displacement(mm)";
                    chartArea.AxisY.Title = "Load(kN)";
                    break;
            }
        }

        //invoke when start_button_click
        private void start_button_Click(object sender, EventArgs e)
        {
            Util.ShowInfo(message_label, "Message: Experiment Running...");
            string errorMsg = null;
            //isCalculationDone = false;
            try
            {
                time = DateTime.Now.ToFileTime().ToString();
                graphChooser = ((System.Collections.Generic.KeyValuePair<int, string>)(graph_combo_box.SelectedItem)).Key;

                SetGraphAxisLabel();

                //reset old values
                minY = (float)Int32.MaxValue;
                realTimeDataStorage = new StringBuilder();

                //Load previoulsy save data
                Util.LoadSettingData();
                LoadDataFromSettingForm();


                //read data from arduino
                try
                {
                    if (Variables.selectedDataFile == null)
                    {
                        try
                        {
                            //old code neet to replace
                            
                            //init arduino
                            SetUpComPort();

                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            errorMsg = "Message: Please set up form with correct data";
                            log.Error(ex);
                            throw;
                        }

                        serialPort.Write("sbem");
                    }
                    else
                    {
                        string textFile = Variables.selectedDataFile;
                        if (File.Exists(textFile))
                        {
                            // Read a text file line by line.
                            string lines = File.ReadAllText(textFile);
                            realTimeDataStorage.Clear();
                            realTimeDataStorage.Append(lines);
                        }
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Please set up form with correct data";
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
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Util.ShowError(message_label, errorMsg);

            }
        }

        //invoke when stop_button_click
        private void stop_button_Click(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            string errorMsg = null;

            try
            {
                Directory.CreateDirectory("Experiment_Data_" + time);
                if (Variables.selectedDataFile == null)
                {
                    try
                    {
                        serialPort.Write("sxem");
                        serialPort.Close();
                    }
                    catch (Exception ex)
                    {
                        errorMsg = "Message: Port closed error";
                        log.Error(ex);
                        throw;
                    }
                }

                try
                {
                    Thread.Sleep(1000);
                    ReFilteringData();

                    
                    utmGraphThread.Abort();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Graph thread error";
                    log.Error(ex);
                    throw;
                }

                

                realTimeDataStorage.Clear();
                Variables.selectedDataFile = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Util.ShowError(message_label, errorMsg);
            }
        }

        private void SaveExperiment() {
            Util.SaveExperimentData(realTimeDataStorage, time);
            String picFilePath = "Experiment_Data_" + time + "/" + "Image-" + time + ".jpge";
            Util.SaveChartAsImage(picFilePath, utm_chart, time);
            Thread.Sleep(1000);
            Util.SaveGraphImage(((System.Collections.Generic.KeyValuePair<int, string>)(graph_combo_box.SelectedItem)).Value + " Graph", picFilePath, time);
            Util.ShowInfo(message_label, "Message: Stop successfully!");
        }

        private void LoadDataFromSettingForm()
        {
            if (Variables.portName == null || Variables.portName.Equals(""))
            {
                Variables.portName = Util.AutodetectArduinoPort();
            }

            Variables.area = (float)System.Convert.ToDouble(A.Text);
            Variables.lenght = (float)System.Convert.ToDouble(L.Text);

            SaveMaterialData();
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

        public void CalculateGraphData()
        {
            try
            {
                string utmDataString = realTimeDataStorage.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');

                float initialDisplacementValue = pairedDataArray[0] != null && !pairedDataArray[0].Equals("") ? (float)(System.Convert.ToDouble(pairedDataArray[0].Split(',')[1])) : 0;
                float initialForceValue = pairedDataArray[0] != null && !pairedDataArray[0].Equals("") ? (float)(System.Convert.ToDouble(pairedDataArray[0].Split(',')[0])) : 0;
                float displacementValueForGrouping = initialDisplacementValue;
                float sumOfForceValueForSingleDisplacement = 0;
                int numOfForceValue = 0;

                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        string[] utmDataArray = pairedData.Split(',');
                        float currentForceValue = (float)Convert.ToDouble(utmDataArray[0]);
                        float currentDisplacmentValue = (float)System.Convert.ToDouble(utmDataArray[1]);

                        if (displacementValueForGrouping == currentDisplacmentValue)
                        {
                            numOfForceValue++;
                            sumOfForceValueForSingleDisplacement = sumOfForceValueForSingleDisplacement + currentForceValue;
                        }
                        else
                        {

                            float averageForceValueForSingleDisplcamentValue = (float)sumOfForceValueForSingleDisplacement / numOfForceValue;

                            float displacement = Math.Abs(displacementValueForGrouping - initialDisplacementValue) * Variables.displacementPerPulse;
                            float force = (float)averageForceValueForSingleDisplcamentValue /*firstValue*/ * Variables.forceConversionFactor;


                            float X = 0;
                            float Y = 0;

                            switch (graphChooser)
                            {
                                case 1:
                                    X = displacement / Variables.lenght;
                                    Y = force / Variables.area;
                                    break;
                                case 2:
                                    X = 0;
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
                                gData.displacementSensorReading = displacementValueForGrouping;
                                gData.forceSensorReading = averageForceValueForSingleDisplcamentValue;

                                graphPlotDataList.Add(gData);

                                {//reset all for next calculation
                                    sumOfForceValueForSingleDisplacement = currentForceValue;
                                    displacementValueForGrouping = currentDisplacmentValue;
                                    numOfForceValue = 1;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //isCalculationDone = true;
        }

        public void iCalculateGraphData()
        {
            try
            {
                string utmDataString = realTimeDataStorage.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');

                float initialDisplacementValue = pairedDataArray[0] != null && !pairedDataArray[0].Equals("") ? (float)(System.Convert.ToDouble(pairedDataArray[0].Split(',')[1])) : 0;

                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        string[] utmDataArray = pairedData.Split(',');
                        float currentForceValue = (float)Convert.ToDouble(utmDataArray[0]);
                        float currentDisplacmentValue = (float)System.Convert.ToDouble(utmDataArray[1]);

                        float displacement = Math.Abs(currentDisplacmentValue - initialDisplacementValue) * Variables.displacementPerPulse;
                        float force = (float)currentForceValue /*firstValue*/ * Variables.forceConversionFactor;


                        float X = 0;
                        float Y = 0;

                        switch (graphChooser)
                        {
                            case 1:
                                X = displacement / Variables.lenght;
                                Y = force / Variables.area;
                                break;
                            case 2:
                                X = 0;
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
                            gData.displacementSensorReading = currentDisplacmentValue;
                            gData.forceSensorReading = currentForceValue;

                            graphPlotDataList.Add(gData);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            //isCalculationDone = true;
        }

        private void ReFilteringData()
        {
            int  lenght = graphPlotDataList.Count;
            _y = new double[lenght];
            int index = 0;
            foreach (GraphData gData in graphPlotDataList)
            {
                _y[index] = gData.y;
                index++;
            }
            _y = DataAnalysis(_y);
            index = 0;
            foreach (GraphData gData in graphPlotDataList)
            {
                gData.y = (float)_y[index];
                index++;
            }
        }
        private double[] DataAnalysis(double[] data)
        {
            SavitzkyGolayFilter filter = new SavitzkyGolayFilter(101, 3);
            return filter.Process(data);
        }

        private void setting_menu_item_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            settingForm.Show();
        }

        private void load_data_menu_item_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Variables.selectedDataFile = openFileDialog.FileName;
            }



        }

        private void guide_menu_item_Click(object sender, EventArgs e)
        {

        }

        private void about_menu_item_Click(object sender, EventArgs e)
        {

        }
    }
}
