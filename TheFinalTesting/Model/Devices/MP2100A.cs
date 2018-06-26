using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// MP2100A眼图仪
    /// </summary>
    internal class MP2100A : DeviceBase
    {
        /// <summary>
        /// MP2100A 眼图仪
        /// </summary>
        /// <param name="add"></param>
        public MP2100A(string add) : base(add)
        {
            DeviceName = "Anritsu MP2100A";
        }
        /// <summary>
        /// 自动调整画面
        /// </summary>
        public void AutoScale()
        {
            SelectMod("5");
            Status = visa32.viPrintf(Vi, ":DISP:WIND:AUTO\n");
        }
        /// <summary>
        /// 获取ER
        /// </summary>
        /// <returns></returns>
        public string GetER()
        {
            SelectMod("5");
            Status = visa32.viPrintf(Vi, ":FETC:AMPL:EXTR?\n");
            CheckStatus(Vi, Status);
            return ReadCommand();

        }
        /// <summary>
        /// 获取Crossing
        /// </summary>
        /// <returns></returns>
        public string GetCrossing()
        {
            SelectMod("5");
            Status = visa32.viPrintf(Vi, ":FETC:AMPL:CROS?\n");
            CheckStatus(Vi, Status);
            return ReadCommand();
        }
        /// <summary>
        /// 获取误码率
        /// </summary>
        /// <returns></returns>
        public string GetErrorRate()
        {
            SelectMod("2");
            Status = visa32.viPrintf(Vi, "ER?\n");
            CheckStatus(Vi, Status);
            return ReadCommand();
        }
        /// <summary>
        /// 选择眼图仪当前操作模块
        /// </summary>
        /// <param name="modelNum"></param>
        public void SelectMod(string modelNum)
        {
            Status = visa32.viPrintf(Vi, ":MOD:ID?\n");
            CheckStatus(Vi, Status);
            string result = ReadCommand();
            if (result != modelNum)
            {
                Status = visa32.viPrintf(Vi, string.Format(":MOD:ID {0}\n", modelNum));
                CheckStatus(Vi, Status);
            }              
        }
        /***
         * 如何判定AutoScale 已经完成？
         *  *OPC? 操作完成查询
         *  *WAI  等待操作完成
         * ***/
    }
}
