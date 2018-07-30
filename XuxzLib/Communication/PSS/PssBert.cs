using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 普赛斯误码模块
    /// </summary>
    public class PssBert : PssBase
    {
        #region Constants
        //定义错误信息
        public const uint BERT_SUCESS = 0x00000000;  //设备操作成功
        public const uint BERT_WR_REGEIST_ERROR = 0x10000001;  //设备读写函数没有注册
        public const uint BERT_RD_ERROR = 0x10000002;  //设备读取数据超时或者数据错误
        public const uint BERT_PATTER_ERROR = 0x10000003;  //没找到匹配码型，码型配置失败     
        public const uint BERT_SPEED_ERROR = 0x10000004;  //没找到匹配速率，速率配置失败  
        public const uint BERT_LEVEL_ERROR = 0x10000005;  //没找到匹配幅度，幅度配置失败
        public const uint BERT_INVERT_ERROR = 0x10000006;  //没找到匹配极性反转，极性反转配置失败
        public const uint BERT_RESULT_ERROR = 0x20000007;  //数据结果错误

        // 通道定义
        public const uint CHANAL_0 = 0;
        public const uint CHANAL_1 = 1;
        public const uint CHANAL_2 = 2;
        public const uint CHANAL_3 = 3;

        // 同步/失步状态定义
        public const int BERT_LOSS = 0;
        public const int BERT_SYNC = 1;
        public const int BERT_SYNNONE = 2;
        // 有误码/无误码状态定义
        public const int BERT_ERR = 0;
        public const int BERT_NOERR = 1;
        public const int BERT_ERRNONE = 2;
        // 速率定义
        public const int RATE_0G125 = 0;
        public const int RATE_0G155 = 1;
        public const int RATE_0G200 = 2;
        public const int RATE_0G622 = 3;
        public const int RATE_1G0625 = 4;
        public const int RATE_1G228 = 5;
        public const int RATE_1G244 = 6;
        public const int RATE_1G25 = 7;
        public const int RATE_1G536 = 8;
        public const int RATE_2G125 = 9;
        public const int RATE_2G457 = 10;
        public const int RATE_2G488 = 11;
        public const int RATE_2G5 = 12;
        public const int RATE_3G07 = 13;
        public const int RATE_3G125 = 14;
        public const int RATE_4G25 = 15;
        public const int RATE_5G = 16;
        public const int RATE_6G14 = 17;
        public const int RATE_6G25 = 18;
        public const int RATE_7G5 = 19;
        public const int RATE_8G5 = 20;
        public const int RATE_9G95 = 21;
        public const int RATE_10G = 22;
        public const int RATE_10G31 = 23;
        public const int RATE_10G52 = 24;
        public const int RATE_10G7 = 25;
        public const int RATE_11G09 = 26;
        public const int RATE_11G32 = 27;
        public const int RATE_15G = 28;
        public const int RATE_16G = 29;
        public const int RATE_2G67 = 30;
        public const int RATE_4G915 = 31;
        public const int RATE_11G1 = 32;
        // 码型定义			  
        public const int STYLE_PRB7 = 0;
        public const int STYLE_PRB9 = 1;
        public const int STYLE_PRB15 = 2;
        public const int STYLE_PRB23 = 3;
        public const int STYLE_PRB31 = 4;
        public const int STYLE_PRB58 = 5;
        public const int STYLE_CJTPAT = 6;
        public const int STYLE_CRPAT = 7;
        public const int STYLE_CSPAT = 8;
        public const int STYLE_USER = 9;
        public const int STYLE_CRPATL = 10;
        public const int STYLE_CRPATH = 11;
        //幅度定义
        public const int LEVEL_400 = 0;
        public const int LEVEL_500 = 1;
        public const int LEVEL_600 = 2;
        public const int LEVEL_700 = 3;
        public const int LEVEL_800 = 4;
        public const int LEVEL_900 = 5;
        public const int LEVEL_1000 = 6;
        public const int LEVEL_1100 = 7;
        public const int LEVEL_1200 = 8;
        public const int LEVEL_1300 = 9;
        public const int LEVEL_1400 = 10;
        public const int LEVEL_1500 = 11;
        public const int LEVEL_1600 = 12;
        public const int LEVEL_1700 = 13;
        public const int LEVEL_1800 = 14;
        #endregion
        #region DllImport
        /// <summary>
        /// 读写函数注册
        /// </summary>
        /// <param name="ptrWrite">写函数指针</param>
        /// <param name="ptrRead">读函数指针</param>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertWRRegist", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void BertWRRegist(IntPtr ptrWrite, IntPtr ptrRead);

        /// <summary>
        /// 获取dll 名称和版本信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void BertGetDllInfo(byte[] name, byte[] version);

        /// <summary>
        /// 获取模块IDN
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="endSign"></param>
        /// <param name="idn"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertIDNGet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertIDNGet(uint cardID, uint endSign, byte[] idn);

        /// <summary>
        /// 设备复位
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <returns>函数执行的错误信息</returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertReset", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertReset(uint cardId, uint endSign);

        /// <summary>
        /// 码型配置
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="patter"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertPatterSet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertPatterSet(uint cardId, uint endSign, uint channelNum, uint patter);

        /// <summary>
        /// 码型查询
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="patter"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertPatterGet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertPatterGet(uint cardId, uint endSign, uint channelNum, ref uint patter);

        /// <summary>
        /// 速率配置
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertSpeedSet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertSpeedSet(uint cardId, uint endSign, uint channelNum, uint speed);
        /// <summary>
        /// 速率查询
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertSpeedGet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertSpeedGet(uint cardId, uint endSign, uint channelNum, ref uint speed);
        /// <summary>
        /// 幅度配置
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertLevelSet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertLevelSet(uint cardId, uint endSign, uint channelNum, uint level);

        /// <summary>
        /// 幅度查询
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertLevelGet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertLevelGet(uint cardId, uint endSign, uint channelNum, ref uint level);

        /// <summary>
        /// 测试时间配置
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertTimeSet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertTimeSet(uint cardId, uint endSign, uint channelNum, uint time);

        /// <summary>
        /// 测试时间查询
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertTimeGet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertTimeGet(uint cardId, uint endSign, uint channelNum, ref uint time);

        /// <summary>
        /// 码型启动
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertPGStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertPGStart(uint cardId, uint endSign, uint channelNum);

        /// <summary>
        /// 码型停止
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertPGStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertPGStop(uint cardId, uint endSign, uint channelNum);

        /// <summary>
        /// 码型比对启动
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertEDStart", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertEDStart(uint cardId, uint endSign, uint channelNum);


        /// <summary>
        /// 码型启动停止
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertEDStop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertEDStop(uint cardId, uint endSign, uint channelNum);

        /// <summary>
        /// 状态清除命令
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertClr", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertClr(uint cardId, uint endSign, uint channelNum);

        /// <summary>
        /// 状态查询命令
        /// </summary>
        /// <param name="cardId">模块卡号</param>
        /// <param name="endSign">结束位</param>
        /// <param name="channelNum">通道号</param>
        /// <param name="syncState">同步状态</param>
        /// <param name="errorState">有/无误码(仪)</param>
        /// <param name="errCount">误码数</param>
        /// <param name="all">码数</param>
        /// <param name="ber">误码率</param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertResult", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertResult(uint cardId, uint endSign, uint channelNum, out uint syncState, out uint errorState, out double errCount, out double all, out double ber);

        /// <summary>
        /// 测试时间查询
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="channelNum"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertCurrentTimeGet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertCurrentTimeGet(uint cardId, uint endSign, uint channelNum, ref uint time);

        /// <summary>
        /// 光开关功能
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="onOrOff">模块加电控制‘0’表示打开 ‘1’表示关闭</param>
        /// <returns></returns>
        [DllImport("PSS_BERT_15G_DLL.dll", EntryPoint = "BertPowerSet", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern uint BertPowerSet(uint cardId, uint endSign, uint onOrOff);

        #endregion
        #region Methods
        /// <summary>
        /// Bert初始化
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="endSign"></param>
        /// <param name="bertChannel"></param>
        static void Initialize(uint cardId, uint endSign, uint bertChannel)
        {

        }
        #endregion
    }
}
