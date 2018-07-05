using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 电源供应器Keithley
    /// </summary>
    public class Keithley:DeviceBase
    {
        //int DefRM = 0;
        //int Vi = 0;
        //int Status;
        //string DeviceConn;
        /// <summary>
        /// Keithley 构造函数
        /// </summary>
        /// <param name="add"></param>
        public Keithley(string add) : base(add)
        {
            DeviceName = "Keithley";
        }
        public string GetMeasuredOnlyVoltage()
        {
            return string.Empty;

        }
        /// <summary>
        /// 用作万用表测量电压时的参数设置
        /// </summary>
        /// <param name="voltageRange">电压量程(形如:10)</param>
        public void SetMeasureVoltageOnlyPara(string voltageRange)
        {
            //用作万用表测量电压是，要将Source 电流设置为0
            WriteCommand(":SOUR:FUNC CURR");
            WriteCommand(":SOUR:CURR:MODE FIX");
            WriteCommand(":SOUR:CURR:RANG 0.2");            
            WriteCommand(":SOUR:CURR:LEV 0");
            //设置电压测量参数
            WriteCommand(":SENS:FUNC \"VOLT\"");
            //WriteCommand(":SENS:VOL:PROT 21");
            WriteCommand(string.Format(":SENS:VOLT:RANG {0}",voltageRange));
        }
        /// <summary>
        /// 读取Keithley数据(Read?)
        /// </summary>
        /// <returns>读取的数据</returns>
        public string ReadData()
        {
            return WriteAndRead("READ?");
        }
        /// <summary>
        /// 打开Keithley
        /// </summary>
        public void Open()
        {
            WriteCommand(":OUTP ON");
        }
        /// <summary>
        /// 关闭Keithley
        /// </summary>
        public void Close()
        {
            WriteCommand(":OUTP OFF");
        }
        /******
         * *RST
         * :SOUR:FUNC VOLT
         * :SOUR:VOLT:MODE FIX
         * :SOUR:VOLT:RANG 10
         * :SOUR
         * ****/
        /**
         *   viError = visa32.viPrintf(seesion, ":SOUR:FUNC CURR\n");
           viError = visa32.viPrintf(seesion, ":SOUR:CURR:MODE FIX\n");
           viError = visa32.viPrintf(seesion, ":SOUR:CURR:RANG 0.2\n");
           viError = visa32.viPrintf(seesion, ":SOUR:CURR:LEV 0\n");
           viError = visa32.viPrintf(seesion, ":SENS:FUNC \"VOLT\"\n");
           //viError = visa32.viPrintf(seesion, ":SENS:VOL:PROT 21\n");
           viError = visa32.viPrintf(seesion, ":SENS:VOLT:RANG 10\n");
           viError = visa32.viPrintf(seesion, ":OUTP ON\n");
           viError = visa32.viPrintf(seesion, "READ?\n");
           viError = visa32.viRead(seesion, out result, 100);
         * ***/

    }
}
