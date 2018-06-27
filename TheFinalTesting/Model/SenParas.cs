using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// 计算灵敏度涉及的参数
    /// </summary>
    public class SenParas
    {
        /// <summary>
        /// R平方值
        /// </summary>
        public double RSquare { get; set; }
        /// <summary>
        /// 灵敏度
        /// </summary>
        public double Sensitive { get; set; }
        /// <summary>
        /// 回归系数A
        /// </summary>
        public double RCA { get; set; }
        /// <summary>
        /// 回归系数B(线性方程斜率)
        /// </summary>
        public double RCB { get; set; }
        /// <summary>
        /// 剩余平方和
        /// </summary>
        public double ResidualSS { get; set; }
        /// <summary>
        /// 回归平方和
        /// </summary>
        public double RegressionSS { get; set; }
    }
}
