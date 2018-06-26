using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// PST3202 三通道电源供应器
    /// </summary>
    internal class PST3202:DeviceBase
    {
        /// <summary>
        /// PS3202 电源供应器构造函数
        /// </summary>
        /// <param name="add"></param>
        public PST3202(string add):base(add)
        {
            DeviceName = "PST3202";
        }
        /// <summary>
        /// 打开设备
        /// </summary>
        public void Open()
        {
            this.WriteCommand("OUTP:STAT?\n");
            string result = this.ReadCommand();
            if (result == "0")
                this.WriteCommand("OUTP:STAT 1\n");
            
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        public void Close()
        {
            this.WriteCommand("OUTP:STAT?\n");
            string result = this.ReadCommand();
            if (result == "1")
                this.WriteCommand("OUTP:STAT 0\n");
        }

    }
}
