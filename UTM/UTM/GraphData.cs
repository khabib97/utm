using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTM
{
    public class GraphData
    {
        public float x {  get; set; }
        public float y {  get; set; }
        public float displacementSensorReading { get; set; }
        public float forceSensorReading { get; set; }
        public long timer { get; set; }
    }
}
