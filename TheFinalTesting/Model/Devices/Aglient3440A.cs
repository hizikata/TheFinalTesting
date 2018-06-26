using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
