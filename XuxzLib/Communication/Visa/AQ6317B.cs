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
    }
}
