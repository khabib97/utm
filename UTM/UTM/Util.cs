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
            message_label.ForeColor = Color.Green;
            message_label.Text = msg;
            //logger.Info(msg, "Info");
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

        public static String LoadSettingData()
        {
            Hashtable data;
            try
            {
                data = Util.LoadData("DataFile.dat");

                if (data != null)
                {
                    Variables.displacementPerPulse = System.Convert.ToDouble(data["displacementPerPulse"]);
                    Variables.forceConversionFactor = System.Convert.ToDouble(data["forceConversionFactor"]);
                    Variables.restZeroDisplacement = System.Convert.ToDouble(data["restZeroDisplacement"]);
                    Variables.restZeroDisforcement = System.Convert.ToDouble(data["restZeroDisforcement"]);

                    Variables.noOfSpecieme = System.Convert.ToDouble(data["noOfSpeciemen"]);
                    Variables.forceConversionFactor = System.Convert.ToDouble(data["specimenFractured"]);

                    Variables.portName = (String)data["portName"];
                    Variables.baundRate = System.Convert.ToInt32(data["baudRate"]);
                }
                //Variables.portName = Util.AutodetectArduinoPort();

                return "Settings data load successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Setting Load Error" + ex);
                //logger.Error(ex, "Setting Load Error:");
                log.Error(ex);
                return "Error : In settings data.";
            }
        }

        public static void SaveImageCapture(String imageFileName, System.Drawing.Image image)
        {
            try
            {
                if (!Directory.Exists("images"))
                {
                    Directory.CreateDirectory("images");
                }
                if(image != null)
                    image.Save("images/" + imageFileName + ".jpg", ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Image save exception: ", ex.Message);
                //logger.Error(ex, "Image save exception:");
                log.Error(ex);
            }
        }

        public static void SaveExperimentData(StringBuilder experimentData,String time) {
            string path = "Experiment_Data_"+time+"/"+"RawData-" + time + ".txt";

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                File.WriteAllText(path, experimentData.ToString());
            }

            // if it is not deleted.
            File.AppendAllText(path, experimentData.ToString());
        }

        public static StringBuilder LoadExperimentDataFromFile() {

            return null;
        }

    }
}
