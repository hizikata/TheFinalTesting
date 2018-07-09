using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 频谱分析仪(2)
    /// </summary>
    public class AQ6317B:DeviceBase
    {
        public AQ6317B(string add):base(add)
        {
            DeviceName = "AQ6317B Optical Specturm Analyzer";
        } 
        public void SetSingle()
        {
            WriteCommand("SGL");
        }
        public string GetData()
        {
            return WriteAndRead("ANA?");
        }
        /// <summary>
        /// OSA初始化
        /// </summary>
        /// <param name="ldType">光源类型</param>
        /// <param name="centerLength">中心波长</param>
        /// <param name="span">扫描范围</param>
        public void Initialize(string ldType,string centerLength,string span)
        {
            
        }
        //SWEEP?   SWEEP AUTO|RPT|SGL   SWEEP SMEAS
        //SGL ANA?  命令格式一
        //:init:smode 1 :init :calc:data?  命令格式二
    }
}
