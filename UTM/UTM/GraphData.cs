using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTM
{
    public class GraphData
    {
        public double x {  get; set; }
        public double y {  get; set; }
        public double displacementSensorReading { get; set; }
        public double forceSensorReading { get; set; }
        public long timer { get; set; }
        public double displacement { get; set; }
    }
}
