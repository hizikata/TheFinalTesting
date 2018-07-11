using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFinalTesting.Model
{
    public class FinalTestingParas
    {
        #region Properties

        public string SN { get; set; }
        public string SupplyCurrent { get; set; }
        public string OutputPower { get; set; }
        public string ExtioRatio { get; set; }
        public string Crossing { get; set; }
        public string Jitter { get; set; }
        public string MaskMargin { get; set; }
        public string CenterWavelength { get; set; }
        public string SMSR { get; set; }
        public string WavelengthDiff { get; set; }
        public bool TxDisable { get; set; }
        public string Sensitivity { get; set; }
        public string SD_Asserted { get; set; }
        public string SD_Desserted { get; set; }
        public string Hysteresis { get; set; }
        public string SD_High { get; set; }
        public string SD_Low { get; set; }
        public bool Saturation { get; set; }
        public string RxPoint1 { get; set; }
        public string RxPoint2 { get; set; }
        public string RxPoint3 { get; set; }
        public string TxPower { get; set; }
        public string Vcc { get; set; }
        public string Temp { get; set; }
        public string Bias { get; set; }
        public bool IsAwPass { get; set; }
        #endregion
        #region Constructor
        public FinalTestingParas()
        {
            this.SN = "N/A";
            this.SupplyCurrent = this.OutputPower = this.ExtioRatio = this.Crossing = this.Jitter = "N/A";
            this.MaskMargin = this.CenterWavelength = this.SMSR = this.WavelengthDiff = this.Sensitivity = "N/A";
            this.SD_Asserted = this.SD_Desserted = this.Hysteresis = this.SD_High = this.SD_Low = "N/A";
            this.RxPoint1 = this.RxPoint2 = this.RxPoint3 = this.TxPower = this.Vcc = this.Temp = this.Bias = "N/A";
        }
        #endregion
        public override string ToString()
        {
            string info = this.SN +","+ this.SupplyCurrent + "," + this.OutputPower + "," + this.ExtioRatio + "," + this.Crossing + "," + this.Jitter + "," +
                this.MaskMargin + "," + this.CenterWavelength + "," + this.SMSR + "," + this.WavelengthDiff + "," + this.TxDisable + "," + this.Sensitivity + "," +
                this.SD_Asserted + "," + this.SD_Desserted + "," + this.Hysteresis + "," + this.SD_High + "," + this.SD_Low + "," + this.Saturation + "," + this.RxPoint1 
                + "," + this.RxPoint2 + "," + this.RxPoint3 + "," + this.TxPower + "," + this.Vcc + "," + this.Temp + "," + this.Bias + "," + this.IsAwPass + "," + DateTime.Now;
            return info;
        }
    }
}
