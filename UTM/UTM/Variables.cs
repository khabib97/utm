﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTM
{
    public static class Variables
    {
        // public static 
        public static string portName; 
        public static int baundRate = 115200;

        public static float diameter;
        public static float lenght;
        public static float displacementPerPulse;
        public static float forceConversionFactor;
        //not clear
        public static float restZeroDisplacement;
        //not clear
        public static float restZeroDisforcement;
        public static double uts = 0;

        public static int noOfSpecieme = 0;
        public static float fracturedLoadDrop = 0;

        public static int excelImageHeight = 350;
        public static int excelImageWidth = 700;

        public enum Experiment{
            StressVsStrain = 1,
            LoadVsTime = 2,
            LoadVsDisplacement =3
        }

        public static int points = 50;
        public static int degree = 3;
        public static int filter = 1;

        public enum Filtering
        {
            AverageWithSavitzkyGolay = 1,
            SavitzkyGolay = 2
        }

        public static string selectedDataFile = null;
    }
}
