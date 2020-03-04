using System;
using System.Drawing;
using System.Management;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Drawing.Imaging;
using log4net;
using System.Text;
using OfficeOpenXml.Drawing;
using OfficeOpenXml;

namespace UTM
{
    public static class Util
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Util));

        public static void ShowError(System.Windows.Forms.Label message_label, string msg)
        {
            message_label.ForeColor = Color.Red;
            message_label.Text = msg;

        }

        public static void ShowInfo(System.Windows.Forms.Label message_label, string msg)
        {
            message_label.Text = "";
            message_label.ForeColor = Color.Green;
            message_label.Text = msg;
        }

        // if this method failed to detect port name, then user can manually update port
        public static string AutodetectArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    log.Debug(item);
                    if (item["Description"].ToString().Contains("Serial"))
                        return item["DeviceID"].ToString();
                }
                log.Info("Arduino detect successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error :", e);
                log.Error(e);
            }

            return "";
        }

        public static void SaveData(Hashtable data, string fileName)
        {
            // To serialize the hashtable and its key/value pairs,   
            // you must first open a stream for writing.  
            // In this case, use a file stream.
            FileStream fs = new FileStream(fileName, FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                //logger.Error(e, "Failed to serialize.Reason:");
                log.Error(e);
            }
            finally
            {
                fs.Close();
            }
        }


        public static Hashtable LoadData(string fileName)
        {
            // Declare the hashtable reference.
            Hashtable data = null;
            FileStream fs;
            try
            {
                // Open the file containing the data that you want to deserialize.
                fs = new FileStream(fileName, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Deserialize the hashtable from the file and  
                    // assign the reference to the local variable.
                    data = (Hashtable)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    //logger.Error(e, "Failed to deserialize. Reason:");
                    log.Error(e);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
                log.Error(ex);
            }
            return data;
        }

        public static void LoadSettingData()
        {
            Hashtable data;
            try
            {
                data = Util.LoadData("DataFile.dat");

                if (data != null)
                {
                    Variables.displacementPerPulse = (float)System.Convert.ToDouble(data["displacementPerPulse"]);
                    Variables.forceConversionFactor = (float)System.Convert.ToDouble(data["forceConversionFactor"]);
                    Variables.restZeroDisplacement = (float)System.Convert.ToDouble(data["restZeroDisplacement"]);
                    Variables.restZeroDisforcement = (float)System.Convert.ToDouble(data["restZeroDisforcement"]);

                    Variables.noOfSpecieme = System.Convert.ToInt16(data["noOfSpeciemen"]);
                    Variables.fracturedLoadDrop = (float)System.Convert.ToDouble(data["specimenFractured"]);

                    Variables.portName = (String)data["portName"];
                    Variables.baundRate = System.Convert.ToInt32(data["baudRate"]);


                    Variables.filter = System.Convert.ToInt16(data["filter"]);
                    Variables.points = System.Convert.ToInt16(data["points"]);
                    Variables.degree = System.Convert.ToInt16(data["degree"]);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Setting Load Error" + ex);
                log.Error(ex);
            }
        }

        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="rSquared">The r^2 value of the line.</param>
        /// <param name="yIntercept">The y-intercept value of the line (i.e. y = ax + b, yIntercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        public static void LinearRegression(
            double[] xVals,
            double[] yVals,
            out double rSquared,
            out double yIntercept,
            out double slope)
        {
            if (xVals.Length != yVals.Length)
            {
                throw new Exception("Input values should be with the same length.");
            }

            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            for (var i = 0; i < xVals.Length; i++)
            {
                var x = xVals[i];
                var y = yVals[i];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            var count = xVals.Length;
            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }

        public static void SaveImageCapture(String imageFileName, System.Drawing.Image image)
        {
            try
            {
                if (!Directory.Exists("images"))
                {
                    Directory.CreateDirectory("images");
                }
                if (image != null)
                    image.Save("images/" + imageFileName + ".jpg", ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Image save exception: ", ex.Message);
                //logger.Error(ex, "Image save exception:");
                log.Error(ex);
            }
        }

        public static void SaveExperimentData(StringBuilder experimentData, String time)
        {
            string path = "Experiment_Data_" + time + "/" + "RawData-" + time + ".txt";

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                File.WriteAllText(path, experimentData.ToString());
            }
            experimentData.Clear();
            // if it is not deleted.
            //File.AppendAllText(path, experimentData.ToString());
        }

        public static void SaveChartAsImage(String picFilePath, System.Windows.Forms.DataVisualization.Charting.Chart utm_chart, String time)
        {
            utm_chart.SaveImage(picFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static void SaveGraphImage(string graphTypeName, string picFilePath, string time)
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
                wsSheet1.Cells["B4"].Value = Variables.lenght + " mm";

                wsSheet1.Cells["A5"].Value = "Area ";
                wsSheet1.Cells["B5"].Value = Variables.diameter + " mm";

                wsSheet1.Cells["A6:D6"].Merge = true;
                wsSheet1.Cells["A6:D6"].Value = "Displacement Per Pulse";
                wsSheet1.Cells["E6"].Value = Variables.displacementPerPulse;

                wsSheet1.Cells["A7:D7"].Merge = true;
                wsSheet1.Cells["A7:D7"].Value = "ultimate tensile stress";
                wsSheet1.Cells["E7"].Value = Variables.uts;

                wsSheet1.Cells["D9:F9"].Merge = true;
                wsSheet1.Cells["D9:F9"].Value = graphTypeName;
                wsSheet1.Cells["D9:F9"].Style.Font.Size = 12;
                wsSheet1.Cells["D9:F9"].Style.Font.Bold = true;

                int rowIndex = 10;
                int colIndex = 0;
                Image img = Image.FromFile(picFilePath);
                ExcelPicture pic = wsSheet1.Drawings.AddPicture("UTMDataImage", img);
                pic.SetPosition(rowIndex, 0, colIndex, 0);
                pic.SetSize(Variables.excelImageWidth, Variables.excelImageHeight);
                wsSheet1.Protection.IsProtected = false;
                wsSheet1.Protection.AllowSelectLockedCells = false;
                ExcelPkg.SaveAs(new FileInfo("Experiment_Data_" + time + "/" + "Report-" + time + ".xlsx"));
            }
        }

        public static StringBuilder LoadExperimentDataFromFile()
        {

            return null;
        }

    }
}
