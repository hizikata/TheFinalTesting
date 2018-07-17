using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 普赛斯光衰减模块
    /// </summary>
    public class PssDOA:PssBase
    {
        #region DllImport
        /// <summary>
        /// 注册字符串发送函数
        /// </summary>
        /// <param name="intPtr">串口发送函数的指针</param>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAPCTxCStringFuncReg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void DOAPCTxCStringFuncReg(IntPtr intPtr);

        /// <summary>
        /// 注册字符串接收函数
        /// </summary>
        /// <param name="intPtr">串口接收函数的指针</param>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAPCRxCStringFuncReg", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void DOAPCRxCStringFuncReg(IntPtr intPtr);

        /// <summary>
        /// 获取dll 名称和版本信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void DOAGetDllInfo(byte[] name, byte[] version);

        /// <summary>
        /// 获取模块IDN
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="endSign"></param>
        /// <param name="idn"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadIDN", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint DOAReadIDN(uint cardID, uint endSign, byte[] idn);

        /// <summary>
        /// 设备复位
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <returns>函数执行的错误信息</returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReset", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAReset(uint cardId, uint endSign);

        /// <summary>
        /// 配置波长
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="wavelength"></param>
        /// <returns>函数执行的错误信息</returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAConfWavelength", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAConfWavelength(uint cardId, uint endSign,uint wavelength);

        /// <summary>
        /// 获取当前波长
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="wavelength"></param>
        /// <returns>函数执行的错误信息</returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadWavelength", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAReadWavelength(uint cardId, uint endSign,ref uint wavelength);

        /// <summary>
        /// 配置衰减量
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="atten"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAConfAtten", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint DOAConfAtten(uint cardId, uint endSign, double atten);

        /// <summary>
        /// 获取当前衰减量
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="atten"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadAtten", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint DOAReadAtten(uint cardId, uint endSign, ref double atten);

        /// <summary>
        /// 配置校准量(cal)
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="calibration"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAConfCalibration", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAConfCalibration(uint cardId, uint endSign, double calibration);

        /// <summary>
        /// 获取当前校准值(cal)
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="atten"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadCalibration", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAReadCalibration(uint cardId, uint endSign, ref double atten);

        /// <summary>
        /// 读取输出端功率
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadPower", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAReadPower(uint cardId, uint endSign, ref double power);

        /// <summary>
        ///设置输出光功率
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="configPower">设置的光功率</param>
        /// <param name="readPower">存储返回的实际光功率</param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAConfPower", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int DOAConfPower(uint cardId, uint endSign,double configPower, ref double readPower);


        #region DDM Methods
        /// <summary>
        /// 查询DDM温度
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="slaveAdd">地址 取值“A2"或者"B2"</param>
        /// <param name="temps"></param>
        /// <returns></returns>
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "ReadDDM_Temperature", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ReadDDM_Temperature(uint cardId, uint endSign, byte slaveAdd, ref double temp);

        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "ReadDDM", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ReadDDM(uint cardId, uint endSign, byte slaveAdd,byte dateAdd,uint dataLength, byte[] data);


        #endregion






        #endregion
    }
}
