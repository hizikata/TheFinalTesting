using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// 测高低压万用表
    /// </summary>
    internal class Aglient34401A:DeviceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="add">GPIB地址</param>
        public  Aglient34401A(string add) : base(add)
        {

        }
        /// <summary>
        /// 读取电压
        /// </summary>
        public string GetVoltage()
        {
            string command = "INIT";
            WriteCommand(command);
            Thread.Sleep(200); 
            command = "READ?";
            return WriteAndRead(command);
        }
        //READ?
    }
}
