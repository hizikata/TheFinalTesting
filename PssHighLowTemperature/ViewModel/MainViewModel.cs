using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using XuxzLib;
using PssHighLowTemperature.Model;
using System.IO.Ports;
using System.Runtime.InteropServices;
using XuxzLib.Communication;
using System.Threading;
using System.Windows;

namespace PssHighLowTemperature.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private uint State = 0;
        byte[] Serbuf = new byte[50];
        #endregion
        #region Properties
        private string _displayInfo;

        public string DisplayInfo
        {
            get { return _displayInfo; }
            set { _displayInfo = value; RaisePropertyChanged(() => DisplayInfo); }
        }

        public string[] ComArray { get; set; }
        private string _currentCom;

        public string CurrentCom
        {
            get { return _currentCom; }
            set { _currentCom = value; RaisePropertyChanged(() => CurrentCom); }
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        public Instru[] InstruList { get; set; }

        private string _sn;
        public string SN
        {
            get { return _sn; }
            set { _sn = value; RaisePropertyChanged(() => SN); }
        }
        private double _power;
        /// <summary>
        /// pf
        /// </summary>
        public double Power
        {
            get { return _power; }
            set { _power = value; RaisePropertyChanged(() => Power); }
        }

        private double _exRatio;
        /// <summary>
        /// 消光比
        /// </summary>
        public double ExRatio
        {
            get { return _exRatio; }
            set { _exRatio = value; RaisePropertyChanged(() => ExRatio); }
        }
        /// <summary>
        /// Crossing
        /// </summary>
        private double _crossing;

        public double Crossing
        {
            get { return _crossing; }
            set { _crossing = value; RaisePropertyChanged(() => Crossing); }
        }

        private double _sen;
        /// <summary>
        /// 灵敏度
        /// </summary>
        public double Sen
        {
            get { return _sen; }
            set { _sen = value; RaisePropertyChanged(() => Sen); }
        }
        #region IIC参数
        private double _rxPoint1;
        /// <summary>
        /// RxPoint1
        /// </summary>
        public double RxPoint1
        {
            get { return _rxPoint1; }
            set { _rxPoint1 = value; RaisePropertyChanged(() => RxPoint1); }
        }

        private double _rxPoint2;

        public double RxPoint2
        {
            get { return _rxPoint2; }
            set { _rxPoint2 = value; RaisePropertyChanged(() => RxPoint2); }
        }

        private double _rxPoint3;

        public double RxPoint3
        {
            get { return _rxPoint3; }
            set { _rxPoint3 = value; RaisePropertyChanged(() => RxPoint3); }
        }

        private double _temp;
        /// <summary>
        /// 温度
        /// </summary>
        public double Temp
        {
            get { return _temp; }
            set { _temp = value; RaisePropertyChanged(() => Temp); }
        }
        private double _bias;

        public double Bias
        {
            get { return _bias; }
            set { _bias = value; RaisePropertyChanged(() => Bias); }
        }

        #endregion

        #endregion
        #region Construtors
        public MainViewModel()
        {
            InstruList = new Instru[]
            {
                new Instru("PPG-4","-1","码型发生器模块",InstType.PssInstru),
                new Instru("BERT15G","-1","误码模块",InstType.PssInstru),
                new Instru("DOA","-1","光衰减器模块",InstType.PssInstru),
                new Instru("OPM","-1","光功率计模块",InstType.PssInstru),
                new Instru("Ag86100A","2","眼图仪",InstType.PssInstru)
            };
            ComArray = SerialPort.GetPortNames();
        }
        #endregion
        #region Commands
        public RelayCommand Initialize
        {
            get
            {
                return new RelayCommand(() => ExecuteInitialize());
            }
        }
        private void ExecuteInitialize()
        {
            try
            {
                //注册发送函数
                DelPCTxCStringFunc delTx = new DelPCTxCStringFunc(PCTxCStringFunc);
                IntPtr ptrTx = Marshal.GetFunctionPointerForDelegate(delTx);
                PssDOA.DOAPCTxCStringFuncReg(ptrTx);
                PssOPM.OPMPCTxCStringFuncReg(ptrTx);

                //注册接收函数
                DelPCRxCStringFunc delRx = new DelPCRxCStringFunc(PCRxCStringFunc);
                IntPtr ptrRx = Marshal.GetFunctionPointerForDelegate(delRx);
                PssDOA.DOAPCRxCStringFuncReg(ptrRx);
                PssOPM.OPMPCRxCStringFuncReg(ptrRx);
                //串口初始化
                Rs232LinkInitial(CurrentCom.Trim(), 115200);

                //Bert和PPG-4配置
                //Bert 配置
                //码型
                uint patter = PssBert.STYLE_PRB7;
                //幅值
                uint level = PssBert.LEVEL_800;
                //速率
                uint speed = PssBert.RATE_1G25;
                //时间0连续  非0 设备运行时间段 在时间到达后会自动关闭？
                uint time = 0;

                //PPG
                uint cardPPG = PssBase.CARDID_2;
                //PSS-BERT-O 1通道为光光通道 2 为光电通道。指的是两个不同的型号，这边选择光电通道
                uint channelPPG = PssBert.CHANAL_2;
                //4PPG
                uint card4PPG = PssBase.CARDID_1;
                uint channel4PPG = PssBert.CHANAL_1;
                //配置BERT15G-O
                //patter set/get
                State = PssBert.BertPatterSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, patter);
                Thread.Sleep(200);
                CheckState(State,cardPPG);
                patter = 0;
                State = PssBert.BertPatterGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref patter);
                Thread.Sleep(200);
                DisplayInfo += string.Format("码型:{0}", patter);
                //level set/get
                State = PssBert.BertLevelSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, level);
                Thread.Sleep(200);
                level = 0;
                State = PssBert.BertLevelGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref level);
                Thread.Sleep(200);
                DisplayInfo += string.Format("level：{0}", level);
                //speed set/get
                State = PssBert.BertSpeedSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, speed);
                Thread.Sleep(3000);
                speed = 0;
                State = PssBert.BertSpeedGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref speed);
                Thread.Sleep(200);
                DisplayInfo += string.Format("speed:{0}", speed);
                //time set/get
                State = PssBert.BertTimeSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, time);
                Thread.Sleep(200);
                time = 0;
                State = PssBert.BertTimeGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref time);
                Thread.Sleep(200);
                DisplayInfo += string.Format("time:{0}", time);



                //PPG、ED配置
                DisplayInfo += "开始配置PPG,ED Start";
                PssBert.BertPGStart(cardPPG, PssBase.ENDSIGN_1, channelPPG);
                Thread.Sleep(500);
                PssBert.BertEDStart(cardPPG, PssBase.ENDSIGN_1, channelPPG);
                Thread.Sleep(500);
                //PPG-15G-4
                //4PPG配置
                //轮询配置4通道
                DisplayInfo += "开始配置4PPG";
                for (uint i = 0; i < 4; i++)
                {
                    //patter set/get
                    PssBert.BertPatterSet(card4PPG, PssBase.ENDSIGN_1, i, patter);
                    Thread.Sleep(200);
                    PssBert.BertPatterGet(card4PPG, PssBase.ENDSIGN_1, i, ref patter);
                    Thread.Sleep(200);
                    DisplayInfo += string.Format("patter:{0}", patter);
                    //level set/get
                    PssBert.BertLevelSet(card4PPG, PssBase.ENDSIGN_1, i, level);
                    Thread.Sleep(200);
                    PssBert.BertLevelGet(card4PPG, PssBase.ENDSIGN_1, i, ref level);
                    Thread.Sleep(200);
                    DisplayInfo += string.Format("level：{0}", level);
                }
                //速率
                PssBert.BertSpeedSet(card4PPG, PssBase.ENDSIGN_1, channel4PPG, speed);
                Thread.Sleep(3000);
                PssBert.BertSpeedGet(card4PPG, PssBase.ENDSIGN_1, channel4PPG, ref speed);
                Thread.Sleep(200);
                DisplayInfo += string.Format("speed:{0}", speed);
                //4通道轮询配置PG
                for (uint i = 0; i < 4; i++)
                {
                    PssBert.BertPGStart(card4PPG, PssBase.ENDSIGN_1, i);
                    Thread.Sleep(200);
                }
                //获取DOA idn 信息
                State = PssDOA.DOAReadIDN(PssBase.CARDID_3, PssBase.ENDSIGN_1, Serbuf);
                CheckState(State, PssBase.CARDID_3);
                DisplayInfo += (Encoding.ASCII.GetString(Serbuf));
                //获取Opm idn 信息
                State = PssOPM.OPMReadIDN(PssBase.CARDID_10, PssBase.ENDSIGN_1, Serbuf);
                CheckState(State, PssBase.CARDID_10);
                DisplayInfo += (Encoding.ASCII.GetString(Serbuf));
                //获取Bert idn 信息
                State = PssBert.BertIDNGet(PssBase.CARDID_2, PssBase.ENDSIGN_1, Serbuf);
                CheckState(State, PssBase.CARDID_2);
                DisplayInfo += (Encoding.ASCII.GetString(Serbuf));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
            

        }
        /// <summary>
        /// 测试命令
        /// </summary>
        public RelayCommand Test
        {
            get
            {
                return new RelayCommand(() => ExecuteTest());
            }
        }
        private void ExecuteTest()
        {
            //pf
            State = PssOPM.OPMReadPower(PssBase.CARDID_10, PssBase.ENDSIGN_1, PssOPM.OPM_CHANNEL_1, ref _power);
            RaisePropertyChanged(() => Power);

        }

        #endregion
        #region Methods
        private void CheckState(uint state,uint cardId)
        {
            if (state == 0)
                return;
            else
            {
                throw new Exception(string.Format("查询设备时出错,State:{0},CardAdd:{1}",state,cardId));
            }
        }
        /// <summary>
        /// 获取Sen
        /// </summary>
        /// <returns></returns>
        private double GetSensitivity(uint cardId)
        {
            double bert = 0;
            uint syncState = 0, errorState = 0;
            double errCount = 0, all = 0;
            State = PssBert.BertResult(cardId, PssBase.ENDSIGN_1, PssBert.CHANAL_2,ref syncState,ref errorState,ref errCount,ref all,ref bert);
            CheckState(State, cardId);
            bert = Math.Log10(bert);
            return bert;
        }
        #region Com Methods
        public static uint Rs232LinkInitial(string comName, uint rate)
        {
            uint error = 0;
            error = PssRS232Driver.RS232Init(comName, rate);
            PssRS232Driver.RS232TimeOutConfig(0, 1000, 1000);
            return error;
        }
        private static uint PCTxCStringFunc(IntPtr buffer, uint length, uint endSign)
        {
            string msg = Marshal.PtrToStringAnsi(buffer);
            uint errorMessage = 0;
            errorMessage = PssRS232Driver.WriteRs232DataStr(buffer);
            if (errorMessage != 0)
                return errorMessage;

            if (endSign == PssBase.ENDSIGN_1)
            {
                errorMessage = PssRS232Driver.WriteRs232DataStr(Marshal.StringToHGlobalAnsi("\n"));
                return errorMessage;
            }
            if (endSign == PssBase.ENDSIGN_2)
            {
                errorMessage = PssRS232Driver.WriteRs232DataStr(Marshal.StringToHGlobalAnsi("\r"));
                return errorMessage;
            }
            return 0X00;
        }
        private static uint PCRxCStringFunc(IntPtr buffer, uint length, uint endSign)
        {
            uint errorMessage = 0;
            errorMessage = PssRS232Driver.ReadRs232DataStr(buffer, length);
            string msg = Marshal.PtrToStringAnsi(buffer);
            return errorMessage;
        }
        #endregion

        #region RegisterFunction
        //注册DOA发送函数
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint DelPCTxCStringFunc(IntPtr buffer, uint length, uint endSign);
        //注册DOA接收函数
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint DelPCRxCStringFunc(IntPtr buffer, uint length, uint endSign);
        #endregion
        #endregion

    }
}
