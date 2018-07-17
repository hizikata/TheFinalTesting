using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 普赛斯RS232(串口)驱动
    /// </summary>
    public class PssRS232Driver
    {
        #region DllImportMethods
        [DllImport("RS232_DLL.dll", EntryPoint = "WriteRs232DataStr", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint WriteRs232DataStr(IntPtr buffer);
        [DllImport("RS232_DLL.dll", EntryPoint = "ReadRs232DataStr", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ReadRs232DataStr(IntPtr buffer, uint length);
        [DllImport("RS232_DLL.dll", EntryPoint = "RS232Init", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint RS232Init(string comName, uint rate);
        [DllImport("RS232_DLL.dll", EntryPoint = "RS232TimeOutConfig", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void RS232TimeOutConfig(uint intervalTimeout, uint readTimeout, uint writeTimeout);
        /// <summary>
        /// 串口关闭函数
        /// </summary>
        /// <returns>函数执行的错误信息</returns>
        [DllImport("RS232_DLL.dll", EntryPoint = "RS232Close", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint RS232Close();
        #endregion

    }
}
