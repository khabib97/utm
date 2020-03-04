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

namespace UTM
{
    public partial class UTMForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UTMForm));

        //graph thread
        private Thread utmGraphThread;

        //These variables for 2% offset line
        private double slop_r = 0;
        private double[] _xAxisDataArray;
        private double[] _yAxisDataArray;
        private double interceptPoint = 0;

        //Save data restore variable
        private Hashtable materialData;

        //arduino communication
        private SerialPort serialPort;

        //Real data variables
        //all arduino data, parsed data point
        private List<GraphData> graphPlotDataList;
        //all data, string format
        private StringBuilder realTimeDataStorage;

        //area calculate form diameter = 0.25*3.1416*diameter^2
        private double area;
        //for differnet file saving
        private String time = null;
        //select graph type
        private int graphChooser = 0;

        //ultimate tensile stress
        double maxY = (double)Int32.MinValue;

        double initialDisplacementValue = 0;

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
            try
            {
                InitializeComponent();

                LoadGraphType();
                //graph plot type
                
                utm_chart.Series[0].ChartType = SeriesChartType.Line;
                //utm_chart.Series[1].ChartType = SeriesChartType.Line;

                //utm_chart.ChartAreas[0].AxisY.Maximum = Double.NaN;
                //utm_chart.ChartAreas[0].AxisX.Maximum = Double.NaN;
                //reload saved data
                LoadMaterialData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void UTMForm_Load(object sender, EventArgs e) { }

        //Thread Function
        private void GetGraphData()
        {
            while (true)
            {
                //if()
                graphPlotDataList = new List<GraphData>();

                if (Variables.filter == (int)Variables.Filtering.AverageWithSavitzkyGolay)
                    CalculateGraphDataWithAvg();
                else
                    CalculateGraphDataWithoutAvg();

                if (utm_chart.IsHandleCreated)
                    this.Invoke((MethodInvoker)delegate { UpdateUTMChart(); });

                //Thread.Sleep(1000);
            }
        }


        public void SetGraphAxisLabel()
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

 

        private void SaveExperiment()
        {
            Directory.CreateDirectory("Experiment_Data_" + time);

            Util.SaveExperimentData(realTimeDataStorage, time);
            realTimeDataStorage.Clear();

            Variables.uts = maxY;
            String picFilePath = "Experiment_Data_" + time + "/" + "Image-" + time + ".jpge";
            Util.SaveChartAsImage(picFilePath, utm_chart, time);
            Thread.Sleep(1000);
            Util.SaveGraphImage(((System.Collections.Generic.KeyValuePair<int, string>)(graph_combo_box.SelectedItem)).Value + " Graph", picFilePath, time);
            
            Util.ShowInfo(message_label, "Message: Stop successfully!");
            Variables.selectedDataFile = null;
        }

        private void LoadDataFromSettingForm()
        {
            if (Variables.portName == null || Variables.portName.Equals(""))
            {
                Variables.portName = Util.AutodetectArduinoPort();
            }

            Variables.diameter = (float)System.Convert.ToDouble(A.Text);
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

        public double CalculateOutputForce(double forceValue)
        {
            return (6.25E-6 * forceValue * forceValue) - (0.112 * forceValue) + 459;
        }

        public double CalculateDisplacemnt(double displacement)
        {
            return Math.Abs(displacement - initialDisplacementValue) * Variables.displacementPerPulse; ;
        }

        public double PairedDataArrayToDisplacementValue(string[] pairedDataArray)
        {
            return pairedDataArray[0] != null && !pairedDataArray[0].Equals("") ? (float)(System.Convert.ToDouble(pairedDataArray[0].Split(',')[1])) : 0;
        }

        public void GraphDataSelectorForDrawingGraph(out double X, out double Y, double displacement, double force)
        {
            X = 0;
            Y = 0;
            switch (graphChooser)
            {
                case 1:
                    X = displacement / Variables.lenght;
                    Y = force / (area * 1000);
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
        }

        public void CalculateGraphDataWithAvg()
        {
            try
            {
                string utmDataString = realTimeDataStorage.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');

                initialDisplacementValue = PairedDataArrayToDisplacementValue(pairedDataArray);
                double initialForceValue = pairedDataArray[0] != null && !pairedDataArray[0].Equals("") ? (float)(System.Convert.ToDouble(pairedDataArray[0].Split(',')[0])) : 0;
                double displacementValueForGrouping = initialDisplacementValue;
                double sumOfForceValueForSingleDisplacement = 0;
                int numOfForceValue = 0;
                GraphData gData = null;
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
                            double meanForceValueForSingleDisplcamentValue = sumOfForceValueForSingleDisplacement / numOfForceValue;

                            double displacement = CalculateDisplacemnt(displacementValueForGrouping);//Math.Abs(displacementValueForGrouping - initialDisplacementValue) * Variables.displacementPerPulse;
                            //float force = (float)averageForceValueForSingleDisplcamentValue /*firstValue*/ * Variables.forceConversionFactor;
                            double outputForce = CalculateOutputForce(currentForceValue);

                            double X = 0;
                            double Y = 0;

                            GraphDataSelectorForDrawingGraph(out X, out Y, displacement, outputForce);

                            //ignore negative value for better ploting
                            if (Y >= 0)
                            {
                                gData = new GraphData();
                                gData.x = X;
                                gData.y = Y;
                                gData.displacementSensorReading = displacementValueForGrouping;
                                gData.forceSensorReading = meanForceValueForSingleDisplcamentValue;

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
        }

        public void CalculateGraphDataWithoutAvg()
        {
            try
            {
                string utmDataString = realTimeDataStorage.ToString();
                string[] pairedDataArray = utmDataString.Split('\n');

                initialDisplacementValue = PairedDataArrayToDisplacementValue(pairedDataArray);
                GraphData gData = null;
                foreach (string pairedData in pairedDataArray)
                {
                    //check blank string 
                    if (pairedData != "")
                    {
                        string[] utmDataArray = pairedData.Split(',');
                        float currentForceValue = (float)Convert.ToDouble(utmDataArray[0]);
                        float currentDisplacmentValue = (float)System.Convert.ToDouble(utmDataArray[1]);

                        double displacement = CalculateDisplacemnt(currentDisplacmentValue);//Math.Abs(currentDisplacmentValue - initialDisplacementValue) * Variables.displacementPerPulse;
                        //float force = (float)currentForceValue /*firstValue*/ * Variables.forceConversionFactor;
                        double force = CalculateOutputForce(currentForceValue);

                        double X = 0;
                        double Y = 0;

                        GraphDataSelectorForDrawingGraph(out X, out Y, displacement, force);

                        //ignore negative value for better ploting
                        if (Y >= 0 || X >= 0)
                        {
                            gData = new GraphData();
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

        private void GraphDataToArray(out double[] X, out double[] Y)
        {
            int length = graphPlotDataList.Count;
            X = new double[length];
            Y = new double[length];
            int index = 0;
            foreach (GraphData gData in graphPlotDataList)
            {
                X[index] = gData.x;
                Y[index] = gData.y;
                index++;
            }
        }

        private void FilteringData()
        {
            try
            {
                _yAxisDataArray = DataAnalysis(_yAxisDataArray);

                //replace negative with 0;
                for(int i = 0; i < _yAxisDataArray.Length ; i++)
                {
                    
                    if(_yAxisDataArray[i] < 0)
                        _yAxisDataArray[i] = 0;

                    if (_yAxisDataArray[i] > maxY) {
                        maxY = _yAxisDataArray[i];
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Filer Exceptio:", ex);
                throw ex;
            }
        }
        private double[] DataAnalysis(double[] data)
        {
            SavitzkyGolayFilter filter = new SavitzkyGolayFilter(100, 3);
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

            if (openFileDialog.ShowDialog() == DialogResult.OK){
                Variables.selectedDataFile = openFileDialog.FileName;
            }
        }

        private void GetYieldPoint()
        {
            slop_r = 0;
            interceptPoint = 0;

            int chunkSize = 100;

            double[] xAxisDataChunked = new double[chunkSize];
            double[] yAxisDataChunked = new double[chunkSize];

            double max_r = 0;
            double slop = 0;
            double intercept = 0;

            double rSquared = 0;
            int index = 0;

            //_xAxisDataArray = XValConverter();
            for (int i = 0; i < graphPlotDataList.Count / 2;)
            {
                Array.Copy(_xAxisDataArray, i, xAxisDataChunked, 0, chunkSize);
                Array.Copy(_yAxisDataArray, i, yAxisDataChunked, 0, chunkSize);

                Util.LinearRegression(xAxisDataChunked, yAxisDataChunked, out rSquared, out intercept, out slop);

                if (slop > 0)
                {
                    if (rSquared > max_r)
                    {
                        max_r = rSquared;
                        slop_r = slop;
                        interceptPoint = intercept;
                        index = i;
                    }
                }
                i = i + 10;
            }

            interceptPoint = -slop_r * 0.002;
            Console.WriteLine("Slop(m) : " + slop_r + " Intercept point(c) : " + interceptPoint);
        }


        private void ReDrawInit() {
            utm_chart.Series[0].Points.Clear();
            utm_chart.Series[0].Enabled = false;
            chartArea.AxisX.LabelStyle.Format = "{0.000}";

            if (graphChooser == 1){
                utm_chart.Series.Add(new Series("2% offset plot"));
                utm_chart.Series[1].BorderWidth = 1;
                utm_chart.Series[1].ChartType = SeriesChartType.Line;

                utm_chart.Series.Add(new Series("Filtered Graph"));
                utm_chart.Series[2].BorderWidth = 1;
                utm_chart.Series[2].ChartType = SeriesChartType.Line;
            }
            else {
                utm_chart.Series.Add(new Series("Filtered Graph"));
                utm_chart.Series[1].BorderWidth = 1;
                utm_chart.Series[1].ChartType = SeriesChartType.Line;
            }
            
        }
        private void ReDrawFilteredData()
        {
            if(graphChooser == 1)
                utm_chart.Series[2].Points.DataBindXY(_xAxisDataArray, _yAxisDataArray);
            else
                utm_chart.Series[1].Points.DataBindXY(_xAxisDataArray, _yAxisDataArray);
        }

        private void Draw2PercentOffset()
        {

            int Index = 40;
            double[] X = new double[Index];
            double[] Y = new double[Index];


            //utm_chart.Series[1].Points.Clear();

            double _x = 0.002;
            double _y = 0;
            for (int i = 0; i < Index; i++)
            {
                //float number = Random. (0.002f, 0.020f);
                _y = slop_r * _x + interceptPoint;
                //utm_chart.Series[1].Points.AddXY(_x, _y);
                X[i] = _x; Y[i] = _y;

                _x = _x + 0.0001;
            }
            utm_chart.Series[1].Points.DataBindXY(X, Y);
        }



        private void guide_menu_item_Click(object sender, EventArgs e)
        {

        }

        private void about_menu_item_Click(object sender, EventArgs e)
        {

        }


        //invoke when start_button_click
        private void start_button_Click(object sender, EventArgs e)
        {
            utm_chart.Series.Clear();
            Util.ShowInfo(message_label, "Message: Experiment Running...");

            utm_chart.Series.Add(new Series("Unfiltered Data Plot"));
            utm_chart.Series[0].BorderWidth = 1;
            utm_chart.Series[0].ChartType = SeriesChartType.Line;

            //utm_chart.Series[0].Enabled = true;
            graphPlotDataList = new List<GraphData>();
            _xAxisDataArray = null;
            _yAxisDataArray = null;
            string errorMsg = null;
            try
            {
                slop_r = 0;
                interceptPoint = 0;
                time = DateTime.Now.ToFileTime().ToString();
                
                graphChooser = ((System.Collections.Generic.KeyValuePair<int, string>)(graph_combo_box.SelectedItem)).Key;

                SetGraphAxisLabel();

                //reset old values
                realTimeDataStorage = new StringBuilder();

                //Load previoulsy save data
                Util.LoadSettingData();
                LoadDataFromSettingForm();
                Variables.diameter = Variables.diameter / 1000;
                area = 0.25 * 3.1416 * Variables.diameter * Variables.diameter;

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
            GraphDataToArray(out _xAxisDataArray, out _yAxisDataArray);
            string errorMsg = null;
            try
            {

                utmGraphThread.Abort();
                Thread.Sleep(1000);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //throw ex;
            }

            try
            {
                //Directory.CreateDirectory("Experiment_Data_" + time);
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
                        //throw ex;
                    }
                }

                try
                {
                    ReDrawInit();
                    ReProcessing();
                }
                catch (Exception ex)
                {
                    errorMsg = "Message: Graph thread error!";
                    log.Error(ex);
                    throw ex;
                }

                //realTimeDataStorage.Clear();
                //Variables.selectedDataFile = null;
                SaveExperiment();
                Util.ShowInfo(message_label, "Finish - UTS: "+maxY.ToString("F")+", Slop: "+slop_r.ToString("F"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Util.ShowError(message_label, errorMsg);
            }
        }

        private void ReProcessing()
        {
            
            Thread.Sleep(1000);
            FilteringData();
            if (graphChooser == 1)
            {
                Thread.Sleep(100);
                GetYieldPoint();
                Thread.Sleep(100);
                Draw2PercentOffset();
            }
            Thread.Sleep(100);
            ReDrawFilteredData();
        }
    }
}
