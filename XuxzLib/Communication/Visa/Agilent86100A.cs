﻿using System;
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
        /// <summary>
        /// 查询模组温度
        /// </summary>
        /// <returns></returns>
        public double QueryModuleTemp(string chanNum)
        {
            string command = string.Format(":CALibrate:MODule:TIME? CHAN{0}", chanNum);
            string msg=WriteAndRead(command);
            msg = msg.Replace("\n", "").Replace("C","");
            string[] result = msg.Split(' ');
            if (result.Length == 5)
            {
                string data = result[4];
                if(double.TryParse(data.Trim(),out double temp))
                {
                    return temp;
                }
                else
                {
                    throw new Exception("查询眼图仪温度失败，请联系工程师");
                }
            }
            else if(result.Length==6)
            {
                string data = result[4] + result[5];
                if(double.TryParse(data, out double temp))
                {
                    return temp;
                }
                else
                {
                    throw new Exception("查询眼图仪温度失败,请联系工程师");
                }

            }
            else
            {
                throw new Exception("眼图仪返回温度查询字符串格式错误");
            }
        } 
        //crossing :MEAS:CGR:CROS? CHAN3  (选择channle)
        //average power :MEAS:APOW? DEC|WATT,CHAN3  单位：分贝|uW
        //Ext.ratio(消光比) :MEAS:CGR:ERAT? DEC|Ratio,CHAN3 单位:分贝|比例
        //AUTOSCALE    :AUT
        //CAL 校验模组    :CAL:MOD:VERT LMOD|RMOD  
        //CAL     :CAL:CONT 继续    :CAL:CANC 取消
        //Extinction ratio cal 校验通道   :CAL:ERAT:STAR CHAN3
        //MODEL temperature  :CALibrate:MODule:TIME? {LMODule | RMODule | CHANnel <N>}
    }
}
