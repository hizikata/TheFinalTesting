using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 普赛斯光功率计模块
    /// </summary>
    public class PssOPM : PssBase
    {
        #region Constants
        //定义错误信息
        public const ushort OPM_NO_E = 0x0000; //无错误
        public const ushort OPM_PCTX_CSTRING_FUNC_ER = 0x0001;//发送字符串函数未注册
        public const ushort OPM_PCRX_CSTRING_FUNC_ER = 0x0002;//接收字符串函数未注册
        public const ushort OPM_CARDID_ER = 0x0003;//CARDID错误
        public const ushort OPM_CHANNEL_ER = 0x0004;//配置通道号错误
        public const ushort OPM_AVERTIME_ER = 0x0005;//采样平均时间超限

        //定义通道
        public const int OPM_CHANNEL_NO = 0;
        public const int OPM_CHANNEL_1 = 1;
        public const int OPM_CHANNEL_2 = 2;




        #endregion

        #region DllImport
        /// <summary>
        /// 注册字符串发送函数
        /// </summary>
        /// <param name="intPtr">串口发送函数的指针</param>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMPCTxCStringFuncReg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void OPMPCTxCStringFuncReg(IntPtr intPtr);

        /// <summary>
        /// 注册字符串接收函数
        /// </summary>
        /// <param name="intPtr">串口接收函数的指针</param>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMPCRxCStringFuncReg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void OPMPCRxCStringFuncReg(IntPtr intPtr);

        /// <summary>
        /// 获取dll 名称和版本信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void OPMGetDllInfo(byte[] name, byte[] version);

        /// <summary>
        /// 获取模块IDN
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="endSign"></param>
        /// <param name="idn"></param>
        /// <returns></returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMReadIDN", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint OPMReadIDN(uint cardID, uint endSign, byte[] idn);

        /// <summary>
        /// 设备复位
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <returns>函数执行的错误信息</returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMReset", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OPMReset(uint cardId, uint endSign);

        /// <summary>
        /// 配置波长
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum">选择通道</param>
        /// <param name="wavelength"></param>
        /// <returns></returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMConfWavelength", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OPMConfWavelength(uint cardId, uint endSign,uint channelNum,uint wavelength);

        
        /// <summary>
        /// 获取当前配置的波长
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="wavelength"></param>
        /// <returns></returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMReadWavelength", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OPMReadWavelength(uint cardId, uint endSign, uint channelNum, uint wavelength);

        /// <summary>
        /// 配置功率单位
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="powerUnit"></param>
        /// <returns></returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMConfUnit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OPMConfUnit(uint cardId, uint endSign, uint channelNum, byte[] powerUnit);

        /// <summary>
        /// 获取当前功率单位
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="powerUnit"></param>
        /// <returns></returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMReadUnit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int OPMReadUnit(uint cardId, uint endSign, uint channelNum, byte[] powerUnit);

        /// <summary>
        /// 读取功率值
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="powerUnit"></param>
        /// <returns></returns>
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMReadPower", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint OPMReadPower(uint cardId, uint endSign, uint channelNum, ref double power);
        #endregion
    }
}
