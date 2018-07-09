using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace XuxzLib.Communication
{
    /// <summary>
    /// PST3202 三通道电源供应器(15)
    /// </summary>
    public class PST3202:DeviceBase
    {
        /// <summary>
        /// PS3202 电源供应器构造函数
        /// </summary>
        /// <param name="add"></param>
        public PST3202(string add):base(add)
        {
            DeviceName = "PST-3202";
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
        /// <summary>
        /// 获取电压
        /// </summary>
        /// <param name="channelNum">通道编号(1,2,3)</param>
        public string GetVolage(string channelNum)
        {
            string command = string.Format(":CHAN{0}:MEAS:VOLT\n",channelNum);
            return WriteAndRead(command);
        }
        /// <summary>
        /// 获取电流
        /// </summary>
        /// <param name="channelNum"></param>
        /// <returns></returns>
        public string GetCurrent(string channelNum)
        {
            string command = string.Format(":CHAN{0}:MEAS:CURR?\n", channelNum);
            return WriteAndRead(command);
        }
        /// <summary>
        /// 设置电压
        /// </summary>
        /// <param name="channelNum">选择通道</param>
        /// <param name="volage">电压值</param>
        public bool SetVolage(string channelNum,string volage)
        {
            string command = string.Format(":CHAN{0}:VOLT {1}", channelNum, volage);
            return WriteCommand(command);
        }
        /// <summary>
        /// 设置电流
        /// </summary>
        /// <param name="current"></param>
        public bool SetCurrent(string channelNum,string current)
        {
            string command = string.Format(":CHAN{0}:CURR {1}", channelNum, current);
            return WriteCommand(command);
        }
    }
}
