using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 电源供应器
    /// </summary>
    public class AglientE3631A:DeviceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="add">GPIB地址</param>
        public AglientE3631A(string add) : base(add)
        {

        }
        /// <summary>
        /// 读取电压
        /// </summary>
        /// <returns></returns>
        public string GetCurrent()
        {
            string command = " MEAS:CURR? P6V\n";
            return WriteAndRead(command);
        }
        /// <summary>
        /// 读取电压
        /// </summary>
        /// <returns></returns>
        public string GetVoltage()
        {
            string command = " MEAS:VOLT? P6V\n";
            return WriteAndRead(command);
        }
        /// <summary>
        /// 打开设备
        /// </summary>
        public void Open()
        {
            Status = visa32.viPrintf(Vi, ":OUTP ON\n");
            CheckStatus(Vi, Status);
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        public void Close()
        {
            Status = visa32.viPrintf(Vi, ":OUTP OFF\n");
            CheckStatus(Vi, Status);
        }
        /// <summary>
        /// 设置电源供应器输出
        /// </summary>
        /// <param name="range">量程(6,25)</param>
        /// <param name="volage">电压(形如3.3)单位V</param>
        /// <param name="current">电流(形如1.0)单位A</param>
        /// <returns></returns>
        public bool SetOutput(string range,string volage,string current)
        {
            string command = string.Format("APPL P{0}V,{1},{2}", range, volage, current);
            return WriteCommand(command);
        }
        //电流电压读取 MEAS:CURR? P6V MEAS:VOLT? P6V
    }
}
