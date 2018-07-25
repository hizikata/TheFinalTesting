using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PssHighLowTemperature.Model
{
    public class TestingParameters
    {
        //SN,PF,ER,CROSS,SEN,RX PF10,RX PF20,RX PF30,Temp,Bias,Final,time,chtemp,producttype
        public string SN { get; set; }
        public string Power { get; set; }
        public string ExRatio { get; set; }
        public string Crossing { get; set; }
        public string Sensitivity { get; set; }
        public string RxPoint1 { get; set; }
        public string RxPoint2 { get; set; }
        public string RxPoint3 { get; set; }
        public string Temperature { get; set; }
        public string Bias { get; set; }
        public bool FinalResult { get; set; }
        public string Time { get; set; }
        public string TempLevel { get; set; }
        public string ProductType { get; set; }
        public TestingParameters()
        {
            this.SN = this.Power = this.ExRatio = this.Crossing = this.Sensitivity = this.RxPoint1 = this.RxPoint2 = this.RxPoint3 =
                this.Temperature = this.Bias = this.ProductType = string.Empty;
        }
        public override string ToString()
        {
            return this.SN + "," + this.Power + "," + this.ExRatio + "," + this.Crossing + "," + this.Sensitivity + ","
                + this.RxPoint1 + "," + this.RxPoint2 + "," + this.RxPoint3 + "," + this.Temperature + "," + this.Bias + ","
                + this.FinalResult + "," + this.Time + "," + this.TempLevel + "," + this.ProductType;
        }
        public void Clear()
        {
            this.Power = this.ExRatio = this.Crossing = this.Sensitivity = this.RxPoint1 = this.RxPoint2 = this.RxPoint3 =
            this.Temperature = this.Bias = this.ProductType = string.Empty;
        }
    }
}
