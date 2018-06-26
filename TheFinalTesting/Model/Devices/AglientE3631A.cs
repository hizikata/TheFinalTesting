using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// 电源供应器
    /// </summary>
    internal class AglientE3631A:DeviceBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="add">GPIB地址</param>
        public AglientE3631A(string add) : base(add)
        {

        }
    }
}
