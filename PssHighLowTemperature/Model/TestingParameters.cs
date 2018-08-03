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
        public bool? IsPowerPass { get; set; }
        public string ExRatio { get; set; }
        public bool? IsExRatioPass { get; set; }
        public string Crossing { get; set; }
        public bool? IsCrossPass { get; set; }
        public string Sensitivity { get; set; }
        public bool? IsSensitivity { get; set; }
        public string RxPoint1 { get; set; }
        public bool? IsRxPoint1Pass { get; set; }
        public string RxPoint2 { get; set; }
        public bool? IsRxPoint2Pass { get; set; }
        public string RxPoint3 { get; set; }
        public bool? IsRxPoint3Pass { get; set; }
        public string Temperature { get; set; }
        public bool? IsTempPass { get; set; }
        public string Bias { get; set; }
        public bool? IsBiasPass { get; set; }
        public bool? FinalResult { get; set; }
        public string Time { get; set; }
        public string TempLevel { get; set; }
        public string ProductType { get; set; }
        public TestingParameters()
        {
            this.SN = this.Power = this.ExRatio = this.Crossing = this.Sensitivity = this.RxPoint1 = this.RxPoint2 = this.RxPoint3 =
                this.Temperature = this.Bias = this.ProductType = string.Empty;
            this.IsBiasPass = this.IsCrossPass = this.IsExRatioPass = this.IsPowerPass = this.IsRxPoint1Pass = this.IsRxPoint2Pass =
                this.IsRxPoint3Pass = this.IsSensitivity=this.IsTempPass =this.FinalResult= false;
        }
        public override string ToString()
        {
            if (this.FinalResult == true)
            {
                return this.SN + "," + this.Power + "," + this.ExRatio + "," + this.Crossing + "," + this.Sensitivity + ","
                + this.RxPoint1 + "," + this.RxPoint2 + "," + this.RxPoint3 + "," + this.Temperature + "," + this.Bias + ","
                + "Pass"+ "," + this.Time + "," + this.TempLevel + "," + this.ProductType;
            }
            else
            {
                return this.SN + "," + this.Power + "," + this.ExRatio + "," + this.Crossing + "," + this.Sensitivity + ","
                + this.RxPoint1 + "," + this.RxPoint2 + "," + this.RxPoint3 + "," + this.Temperature + "," + this.Bias + ","
                + "Fail" + "," + this.Time + "," + this.TempLevel + "," + this.ProductType;
            }
            
        }
        public void Clear()
        {
            this.Power = this.ExRatio = this.Crossing = this.Sensitivity = this.RxPoint1 = this.RxPoint2 = this.RxPoint3 =
            this.Temperature = this.Bias = this.ProductType = string.Empty;
            this.IsBiasPass = this.IsCrossPass = this.IsExRatioPass = this.IsPowerPass =this.IsTempPass= this.IsRxPoint1Pass =
                this.IsRxPoint2Pass = this.IsRxPoint3Pass = this.IsSensitivity  =this.FinalResult= false;
        }
    }
}
