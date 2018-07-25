using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XuxzLib.Communication
{
    public class Agilent86100A:DeviceBase
    {
        /// <summary>
        /// 安捷伦眼图仪(地址:7)
        /// </summary>
        /// <param name="add"></param>
        public Agilent86100A(string add):base(add)
        {
            DeviceName = "Agilent86100A";
        }
        public double GetCrossing(string chanNum)
        {
            string command = string.Format(":MEAS:CGR:CROS? CHAN{0}", chanNum);
            string msg= WriteAndRead(command);
            if(double.TryParse(msg.Trim(),out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        public double GetPower(string chanNum)
        {
            string command = string.Format(":MEAS:APOW? DEC,CHAN{0}", chanNum);
            string msg = WriteAndRead(command);
            if (double.TryParse(msg.Trim(), out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        public double GetExRatio(string chanNum)
        {
            string command = string.Format(":MEAS:CGR:ERAT? DEC,CHAN{0}", chanNum);
            string msg = WriteAndRead(command);
            if (double.TryParse(msg.Trim(), out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 开始AutoScale
        /// </summary>
        public void AutoScale()
        {
            string command = ":AUT";
            WriteCommand(command);
        }
        //crossing :MEAS:CGR:CROS? CHAN3  (选择channle)
        //average power :MEAS:APOW? DEC|WATT,CHAN3  单位：分贝|uW
        //Ext.ratio(消光比) :MEAS:CGR:ERAT? DEC|Ratio,CHAN3 单位:分贝|比例
        //AUTOSCALE    :AUT
        //CAL     :CAL:MOD:VERT LMOD|RMOD  
        //CAL     :CAL:CONT 继续    :CAL:CANC 取消
        //Extinction ratio cal   :CAL:ERAT:STAR CHAN3
        //MODEL temperature  :CALibrate:MODule:TIME? {LMODule | RMODule | CHANnel <N>}
    }
}
