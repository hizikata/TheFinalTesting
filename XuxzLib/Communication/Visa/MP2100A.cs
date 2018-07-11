using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace XuxzLib.Communication
{
    /// <summary>
    /// MP2100A眼图仪(16)
    /// </summary>
    public class MP2100A : DeviceBase
    {
        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="add"></param>
        public MP2100A(string add) : base(add)
        {
            DeviceName = "Anritsu MP2100A";
        }
        public MP2100A(string addStr, ConnectionType type) : base(addStr, type)
        {

        }
        #endregion  
        /// <summary>
        /// 自动调整画面
        /// </summary>
        public void AutoScale()
        {
            SelectMod("5");
            Status = visa32.viPrintf(Vi, ":DISP:WIND:AUTO\n");
        }
        /// <summary>
        /// 获取Extinction Ratio (消光比)
        /// </summary>
        /// <returns></returns>
        public double GetExRatio()
        {
            SelectMod("5");
            Status = visa32.viPrintf(Vi, ":FETC:AMPL:EXTR?\n");
            CheckStatus(Vi, Status);
            string data= ReadCommand().Trim();
            return DataAnalysis(data.Split(','));
        }
        /// <summary>
        /// 获取Crossing
        /// </summary>
        /// <returns></returns>
        public double GetCrossing()
        {
            SelectMod("5");
            Status = visa32.viPrintf(Vi, ":FETC:AMPL:CROS?\n");
            CheckStatus(Vi, Status);
            string data= ReadCommand().Trim();
            return DataAnalysis(data.Split(','));
        }

        /// <summary>
        /// 获取Jitter
        /// </summary>
        /// <returns></returns>
        public double GetJitter()
        {
            SelectMod("5");
            //Jitter RMS
            //string command = string.Format(":FETCh:TIME:JITT:RMS?");
            //Jitter P-P
            string command = string.Format(":FETCh:TIME:JITTer:PPeak?");
            string data= WriteAndRead(command);
            return DataAnalysis(data.Split(','));
        }
        /// <summary>
        /// 获取MaskMargin
        /// </summary>
        /// <returns></returns>
        public double GetMaskMargin()
        {
            SelectMod("5");
            string command = string.Format(":MEAS:MASK:MARG?");
            string data= WriteAndRead(command);
            if(double.TryParse(data.Trim(),out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取误码率
        /// </summary>
        /// <returns></returns>
        public double GetErrorRate()
        {
            SelectMod("2");
            Status = visa32.viPrintf(Vi, "ER?\n");
            CheckStatus(Vi, Status);
            string data = ReadCommand();
            string str = data.Substring(2);
            if (double.TryParse(str.Trim(), out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 设置持续测量
        /// </summary>
        /// <returns></returns>
        public bool SetSampRun()
        {
            SelectMod("5");
            string command = string.Format(":SAMP:STAT RUN\n");
            return WriteCommand(command);
        }
        /// <summary>
        /// 设置眼图仪当前操作模块
        /// </summary>
        /// <param name="modelNum"></param>
        public void SelectMod(string modelNum)
        {
            //Status = visa32.viPrintf(Vi, ":MOD:ID?\n");
            //CheckStatus(Vi, Status);
            //string result = ReadCommand();
            //if (result != modelNum)
            //{
            Status = visa32.viPrintf(Vi, string.Format(":MOD:ID {0}\n", modelNum));
            CheckStatus(Vi, Status);
            //}              
        }

        private double DataAnalysis(string[] dataBefore)
        {
            double result;
            List<double> listData = new List<double>();
            foreach (string item in dataBefore)
            {
                if(double.TryParse(item.Trim(),out result))
                {
                    if (result > 0)
                    {
                        listData.Add(result);
                    }
                }
            }
            //返回数组中最大的值，后续改善
            return listData.Max();
        }
        /***
         * 如何判定AutoScale 已经完成？
         *  *OPC? 操作完成查询
         *  *WAI  等待操作完成
         * ***/
    }
}
