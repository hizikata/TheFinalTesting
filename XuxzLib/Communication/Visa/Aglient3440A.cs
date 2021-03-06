﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 测高低压万用表(10)
    /// </summary>
    public class Aglient34401A:DeviceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="add">GPIB地址</param>
        public  Aglient34401A(string add) : base(add)
        {
            DeviceName = "Aglient34401A";
        }
        /// <summary>
        /// 读取电压
        /// </summary>
        public double GetVoltage()
        {
            string command = "INIT";
            WriteCommand(command);
            Thread.Sleep(200); 
            command = "READ?";
            string data= WriteAndRead(command);
            if(double.TryParse(data,out double result))
            {
                return result;
            }
            else
            {
                return 0.00;
            }
        }
        //READ?
    }
}
