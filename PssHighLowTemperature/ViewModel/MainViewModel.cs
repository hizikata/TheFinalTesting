#define Ag86100
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PssHighLowTemperature.Model;
using System.IO.Ports;
using System.Runtime.InteropServices;
using XuxzLib.Communication;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using GalaSoft.MvvmLight.Messaging;

namespace PssHighLowTemperature.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        readonly XDocument XDoc;
        private string dirPath = @"E:\temp";
        private static bool IsComReady = false;
        public const int LowTemp = 0;
        public const int HighTemp = 1;
        private const uint ENDSIGN = PssBase.ENDSIGN_1;
        private uint State = 0;
        //声明发送函数
        DelPCTxCStringFunc delTx = new DelPCTxCStringFunc(PCTxCStringFunc);
        //声明接收函数
        DelPCRxCStringFunc delRx = new DelPCRxCStringFunc(PCRxCStringFunc);

        Agilent86100A Ag8610;
        #endregion
        #region Properties
        /// <summary>
        /// 设备地址
        /// </summary>
        public string[] DcaChannelArray { get; set; }
        private string _dcaChannel;

        public string DcaChannle
        {
            get { return _dcaChannel; }
            set { _dcaChannel = value; RaisePropertyChanged(() => DcaChannle); }
        }

        #region Calibration Paras
        private static double _coefficient;
        /// <summary>
        /// 光功率计校验系数
        /// </summary>
        public double Coefficient
        {
            get { return _coefficient; }
            set { _coefficient = value; RaisePropertyChanged(() => Coefficient); }
        }

        private double _standardPower;
        /// <summary>
        /// 基准键功率
        /// </summary>
        public double StandardPower
        {
            get { return _standardPower; }
            set { _standardPower = value; RaisePropertyChanged(() => StandardPower); }
        }
        private double _actualPower;
        /// <summary>
        /// 实际测量功率
        /// </summary>
        public double ActualPower
        {
            get { return _actualPower; }
            set { _actualPower = value; RaisePropertyChanged(() => ActualPower); }
        }

        private static double _calOfDOA;
        /// <summary>
        /// 光衰减Cal
        /// </summary>
        public double CalOfDOA
        {
            get { return _calOfDOA; }
            set { _calOfDOA = value; RaisePropertyChanged(() => CalOfDOA); }
        }
        private double _standardAtt;

        public double StandardAtt
        {
            get { return _standardAtt; }
            set { _standardAtt = value; RaisePropertyChanged(() => StandardAtt); }
        }
        private double _actualAtt;

        public double ActualAtt
        {
            get { return _actualAtt; }
            set { _actualAtt = value; RaisePropertyChanged(() => ActualAtt); }
        }
        private double _opmAtt;

        public double OpmAtt
        {
            get { return _opmAtt; }
            set { _opmAtt = value; RaisePropertyChanged(() => OpmAtt); }
        }


        #endregion
        /// <summary>
        /// 通道列表
        /// </summary>
        public string[] ChannelArray { get; set; }

        /// <summary>
        /// 波长选择列表
        /// </summary>
        public uint[] WavelengthArray { get; set; }
        /// <summary>
        /// 高低温选择
        /// </summary>
        public string[] TempLevelArray { get; set; }
        /// <summary>
        /// 码型列表
        /// </summary>
        public string[] PatterArray { get; set; }
        /// <summary>
        /// 幅值选择
        /// </summary>
        public string[] LevelArray { get; set; }
        /// <summary>
        /// 速率选择
        /// </summary>
        public string[] SpeedArray { get; set; }


        public uint OpmChannel { get; set; }

        public string[] ProductTypeArray { get; set; }
        private string _producdtType;
        /// <summary>
        /// 测试产品型号
        /// </summary>
        public string ProductType
        {
            get { return _producdtType; }
            set { _producdtType = value; RaisePropertyChanged(() => ProductType); }
        }

        //Tx波长(产品发射光)
        private uint _txWavelength;

        public uint TxWavelength
        {
            get { return _txWavelength; }
            set { _txWavelength = value; RaisePropertyChanged(() => TxWavelength); }
        }

        private uint _rxWavelength;
        /// <summary>
        /// Rx波长(产品接收光) 
        /// </summary>

        public uint RxWavelength
        {
            get { return _rxWavelength; }
            set { _rxWavelength = value; RaisePropertyChanged(() => RxWavelength); }
        }
        //码型
        private uint _patter;

        public uint Patter
        {
            get { return _patter; }
            set { _patter = value; RaisePropertyChanged(() => Patter); }
        }

        //幅值

        private uint _level;

        public uint Level
        {
            get { return _level; }
            set { _level = value; RaisePropertyChanged(() => Level); }
        }

        //速率

        private uint _speed;

        public uint Speed
        {
            get { return _speed; }
            set { _speed = value; RaisePropertyChanged(() => Speed); }
        }

        #region TestingCondition
        private string _tempLevel;
        /// <summary>
        /// 温度测试条件
        /// </summary>
        public string TempLevel
        {
            get { return _tempLevel; }
            set { _tempLevel = value; RaisePropertyChanged(() => TempLevel); }
        }

        private double _pfMax;

        public double PfMax
        {
            get { return _pfMax; }
            set { _pfMax = value; RaisePropertyChanged(() => PfMax); }
        }
        private double _pfMin;

        public double PfMin
        {
            get { return _pfMin; }
            set { _pfMin = value; RaisePropertyChanged(() => PfMin); }
        }
        private double _crosMax;

        public double CrosMax
        {
            get { return _crosMax; }
            set { _crosMax = value; RaisePropertyChanged(() => CrosMax); }
        }
        private double _crosMin;

        public double CrosMin
        {
            get { return _crosMin; }
            set { _crosMin = value; RaisePropertyChanged(() => CrosMin); }
        }

        private double _erMax;

        public double ErMax
        {
            get { return _erMax; }
            set { _erMax = value; RaisePropertyChanged(() => ErMax); }
        }
        private double _erMin;

        public double ErMin
        {
            get { return _erMin; }
            set { _erMin = value; RaisePropertyChanged(() => ErMin); }
        }
        private double _rxMax;

        public double RxMax
        {
            get { return _rxMax; }
            set { _rxMax = value; RaisePropertyChanged(() => RxMax); }
        }
        private double _rxMin;

        public double RxMin
        {
            get { return _rxMin; }
            set { _rxMin = value; RaisePropertyChanged(() => RxMin); }
        }

        private double _tempMax;

        public double TempMax
        {
            get { return _tempMax; }
            set { _tempMax = value; RaisePropertyChanged(() => TempMax); }
        }
        private double _tempMin;

        public double TempMin
        {
            get { return _tempMin; }
            set { _tempMin = value; RaisePropertyChanged(() => TempMin); }
        }

        private double _biasMax;

        public double BiasMax
        {
            get { return _biasMax; }
            set { _biasMax = value; RaisePropertyChanged(() => BiasMax); }
        }
        private double _biasMin;

        public double BiasMin
        {
            get { return _biasMin; }
            set { _biasMin = value; RaisePropertyChanged(() => BiasMin); }
        }

        private int _rx1Set;

        public int Rx1Set
        {
            get { return _rx1Set; }
            set { _rx1Set = value; RaisePropertyChanged(() => Rx1Set); }
        }
        private int _rx2Set;

        public int Rx2Set
        {
            get { return _rx2Set; }
            set { _rx2Set = value; RaisePropertyChanged(() => Rx2Set); }
        }
        private int _rx3Set;

        public int Rx3Set
        {
            get { return _rx3Set; }
            set { _rx3Set = value; RaisePropertyChanged(() => Rx3Set); }
        }
        private double _senSet;

        public double SenSet
        {
            get { return _senSet; }
            set { _senSet = value; RaisePropertyChanged(() => SenSet); }
        }
        private double _erSet;

        public double ErSet
        {
            get { return _erSet; }
            set { _erSet = value; RaisePropertyChanged(() => ErSet); }
        }

        #endregion
        /// <summary>
        /// 指示初始化是否完成
        /// </summary>
        private bool _isIniReady;

        public bool IsIniReady
        {
            get { return _isIniReady; }
            set { _isIniReady = value; RaisePropertyChanged(() => IsIniReady); }
        }

        private bool _isTestEnable;
        /// <summary>
        /// 是否可测试
        /// </summary>
        public bool IsTestEnable
        {
            get { return _isTestEnable; }
            set { _isTestEnable = value; RaisePropertyChanged(() => IsTestEnable); }
        }


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
        /// 测试参数
        /// </summary>
        private TestingParameters _testingParas;

        public TestingParameters TestParas
        {
            get { return _testingParas; }
            set { _testingParas = value; RaisePropertyChanged(() => TestParas); }
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

        private string _sen;
        /// <summary>
        /// 灵敏度
        /// </summary>
        public string Sen
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
            ProductTypeArray = new string[] { "ETP69966-7TB4-F2" };
            ProductType = ProductTypeArray[0];
            DcaChannelArray = new string[] { "1", "2", "3", "4" };
            IsIniReady = true;
            //初始化测试参数实例
            TestParas = new TestingParameters();
            ChannelArray = new string[] { "Chan0", "Chan1", "Chan2", "Chan3" };
            WavelengthArray = new uint[] { 850, 1270, 1310, 1330, 1490, 1550, 1577 };
            TempLevelArray = new string[] { "-40","25", "85" };
            PatterArray = new string[]{ "STYLE_PRB7", "STYLE_PRB9" , "STYLE_PRB15" , "STYLE_PRB23", "STYLE_PRB31", "STYLE_PRB58", "STYLE_CJTPAT",
            "STYLE_CRPAT","STYLE_CSPAT","STYLE_USER","STYLE_CRPATL","STYLE_CRPATH"};
            LevelArray = new string[] { "LEVEL_400", "LEVEL_500", "LEVEL_600", "LEVEL_700", "LEVEL_800", "LEVEL_900", "LEVEL_1000" ,
            "LEVEL_1100","LEVEL_1200","LEVEL_1300","LEVEL_1400","LEVEL_1500","LEVEL_1600","LEVEL_1700","LEVEL_1800"};
            SpeedArray = new string[] { "RATE_0G125","RATE_0G155", "RATE_0G200" , "RATE_0G622" , "RATE_1G0625" , "RATE_1G228" , "RATE_1G244" ,
            "RATE_1G25","RATE_1G536","RATE_2G125","RATE_2G457","RATE_2G488","RATE_2G5","RATE_3G07","RATE_3G125","RATE_4G25",
            "RATE_5G","RATE_6G14","RATE_6G25","RATE_7G5","RATE_8G5","RATE_9G95","RATE_10G ","RATE_10G31","RATE_10G52","RATE_10G7",
            "RATE_11G09","RATE_11G32","RATE_15G","RATE_16G","RATE_2G67","RATE_4G915","RATE_11G1"};

            //信号类型设置
            Coefficient = 0;
            OpmChannel = 1;
            StandardPower = 5.86;
            TempLevel = TempLevelArray[1];
            StandardAtt = -20;
            TxWavelength = WavelengthArray[1];
            RxWavelength = WavelengthArray[5];
            Patter = PssBert.STYLE_PRB31;
            Level = PssBert.LEVEL_800;
            Speed = PssBert.RATE_9G95;
            Rx1Set = -10;
            Rx2Set = -19;
            Rx3Set = -28;
            PfMax = 10;
            PfMin = 1;
            RxMax = 2;
            RxMin = 2;
            SenSet = -28;
            ErSet = -3;
            BiasMin = 0;
            BiasMax = 20;
            TempMin = 10;
            TempMax = 70;
            CrosMin = 45;
            CrosMax = 55;
            ErMin = 5;
            ErMax = 15;
            InstruList = new Instru[]
            {
                new Instru("PPG-4","-1","码型发生器模块",InstType.PssInstru),
                new Instru("BERT15G","-1","误码模块",InstType.PssInstru),
                new Instru("DOA","-1","光衰减器模块",InstType.PssInstru),
                new Instru("OPM","-1","光功率计模块",InstType.PssInstru),
                new Instru("Ag86100A","7","眼图仪",InstType.PssInstru)
            };
            //读取opm/doa 修正系数
            XDoc = XDocument.Load("Configure.xml");
            XElement xEle = XDoc.Root.Element("CalOfOpm");
            XElement xEleOpm = XDoc.Root.Element("CalOfDoa");
            Coefficient = Convert.ToDouble(xEle.Value.Trim());
            CalOfDOA = Convert.ToDouble(xEleOpm.Value.Trim());
            //获取Com列表
            ComArray = SerialPort.GetPortNames();
            if (ComArray.Length > 0)
            {

                CurrentCom = ComArray[0];
            }

        }
        #endregion
        #region Commands
        /// <summary>
        /// 读取功率计
        /// </summary>
        public RelayCommand ReadOPM
        {
            get
            {
                return new RelayCommand(() => ExecuteReadOPM());
            }
        }
        private void ExecuteReadOPM()
        {
            if (IsComReady == false)
            {
                MessageBox.Show("请先初始化Com", "系统提示");
                return;
            }
            State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref _actualPower);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            ActualPower = _actualPower;
        }
        /// <summary>
        /// Doa读取光功率计
        /// </summary>
        public RelayCommand ReadOPMAtDoa
        {
            get
            {
                return new RelayCommand(() => ExecuteReadOPMAtDoa());
            }
        }
        private void ExecuteReadOPMAtDoa()
        {
            if (IsComReady == false)
            {
                MessageBox.Show("请先初始化Com", "系统提示");
                return;
            }
            //设置波长
            State = PssOPM.OPMConfWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, RxWavelength);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref _opmAtt);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            OpmAtt = _opmAtt;
        }
        /// <summary>
        /// 光衰减器内部读取
        /// </summary>
        public RelayCommand ReadDoa
        {
            get
            {
                return new RelayCommand(() => ExecuteReadDoa());
            }
        }
        private void ExecuteReadDoa()
        {
            if (IsComReady == false)
            {
                MessageBox.Show("请先初始化Com", "系统提示");
                return;
            }
            State = PssDOA.DOAConfWavelength(PssBase.CARDID_3, ENDSIGN, RxWavelength);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            State = PssDOA.DOAReadWavelength(PssBase.CARDID_3, ENDSIGN, ref _rxWavelength);
            State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, StandardAtt);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            State = PssDOA.DOAReadPower(PssBase.CARDID_3, ENDSIGN, ref _actualAtt);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            ActualAtt = _actualAtt;

        }
        /// <summary>
        /// 初始化命令
        /// </summary>
        public RelayCommand ComInit
        {
            get
            {
                return new RelayCommand(() => ExecuteComInit());
            }
        }
        private void ExecuteComInit()
        {
            //注册发送函数

            IntPtr ptrTx = Marshal.GetFunctionPointerForDelegate(delTx);
            PssDOA.DOAPCTxCStringFuncReg(ptrTx);
            PssOPM.OPMPCTxCStringFuncReg(ptrTx);

            //注册接收函数

            IntPtr ptrRx = Marshal.GetFunctionPointerForDelegate(delRx);
            PssDOA.DOAPCRxCStringFuncReg(ptrRx);
            PssOPM.OPMPCRxCStringFuncReg(ptrRx);
            //bert 注册发送和接收函数
            PssBert.BertWRRegist(ptrTx, ptrRx);
            //串口初始化
            State = Rs232LinkInitial(CurrentCom.Trim(), 115200);
            if (State != 0)
            {
                IsComReady = false;
                MessageBox.Show("串口初始化失败", "系统提示");
            }
            else
            {
                IsComReady = true;
            }
        }
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
                IsIniReady = false;
                IsTestEnable = false;
                Thread thread = new Thread(() =>
                  {
                      //注册发送/接收函数、串口初始化
                      ExecuteComInit();

                      //DOA
                      State = PssDOA.DOAConfWavelength(PssBase.CARDID_3, ENDSIGN, RxWavelength);
                      Thread.Sleep(200);
                      CheckState(State, PssBase.CARDID_3);
                      //0pm
                      State = PssOPM.OPMConfWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, TxWavelength);
                      Thread.Sleep(200);
                      CheckState(State, PssBase.CARDID_4);



                      #region 4PPG配置
                      //Bert 配置
                      //码型

                      //时间0连续  非0 设备运行时间段 在时间到达后会自动关闭？
                      uint time = 0;
                      //PPG
                      uint cardPPG = PssBase.CARDID_2;
                      //PSS-BERT-O 1通道为光光通道, 2 为光电通道。指的是两个不同的型号，这边选择光电通道
                      uint channelPPG = PssBert.CHANAL_2;
                      //4PPG
                      uint card4PPG = PssBase.CARDID_1;
                      uint channel4PPG = PssBert.CHANAL_1;

                      //轮询配置4通道
                      DisplayInfo += "开始配置4PPG";
                      for (uint i = 0; i < 4; i++)
                      {
                          //patter set/get
                          State = PssBert.BertPatterSet(card4PPG, ENDSIGN, i, Patter);
                          Thread.Sleep(200);
                          CheckState(State, card4PPG);
                          State = PssBert.BertPatterGet(card4PPG, ENDSIGN, i, ref _patter);
                          Thread.Sleep(200);
                          CheckState(State, card4PPG);
                          //level set/get
                          State = PssBert.BertLevelSet(card4PPG, ENDSIGN, i, Level);
                          Thread.Sleep(200);
                          CheckState(State, card4PPG);
                          State = PssBert.BertLevelGet(card4PPG, ENDSIGN, i, ref _level);
                          Thread.Sleep(200);
                          CheckState(State, card4PPG);
                          DisplayInfo += string.Format("level：{0}", _level);
                      }
                      //速率
                      State = PssBert.BertSpeedSet(card4PPG, ENDSIGN, channel4PPG, Speed);
                      Thread.Sleep(4000);
                      CheckState(State, card4PPG);
                      State = PssBert.BertSpeedGet(card4PPG, ENDSIGN, channel4PPG, ref _speed);
                      Thread.Sleep(200);
                      CheckState(State, card4PPG);
                      DisplayInfo += string.Format("speed:{0}", _speed);
                      //4通道轮询配置PG
                      for (uint i = 0; i < 4; i++)
                      {
                          State = PssBert.BertPGStart(card4PPG, ENDSIGN, i);
                          Thread.Sleep(200);
                          CheckState(State, card4PPG);
                      }
                      #endregion 4PPG配置

                      #region Bert 15G-0 配置
                      //配置BERT15G-O

                      //patter set/get
                      State = PssBert.BertPatterSet(cardPPG, ENDSIGN, channelPPG, Patter);
                      Thread.Sleep(200);
                      CheckState(State, cardPPG);
                      State = PssBert.BertPatterGet(cardPPG, ENDSIGN, channelPPG, ref _patter);
                      Thread.Sleep(200);
                      DisplayInfo += string.Format("码型:{0}", _patter);
                      //level set/get
                      State = PssBert.BertLevelSet(cardPPG, ENDSIGN, channelPPG, Level);
                      Thread.Sleep(200);
                      State = PssBert.BertLevelGet(cardPPG, ENDSIGN, channelPPG, ref _level);
                      Thread.Sleep(200);
                      DisplayInfo += string.Format("level：{0}", _level);
                      //speed set/get
                      State = PssBert.BertSpeedSet(cardPPG, ENDSIGN, channelPPG, Speed);
                      Thread.Sleep(4000);
                      State = PssBert.BertSpeedGet(cardPPG, ENDSIGN, channelPPG, ref _speed);
                      Thread.Sleep(200);
                      DisplayInfo += string.Format("speed:{0}", _speed);
                      //time set/get
                      State = PssBert.BertTimeSet(cardPPG, ENDSIGN, channelPPG, time);
                      Thread.Sleep(200);
                      time = 0;
                      State = PssBert.BertTimeGet(cardPPG, ENDSIGN, channelPPG, ref time);
                      Thread.Sleep(200);
                      DisplayInfo += string.Format("time:{0}", time);



                      //PPG、ED配置
                      DisplayInfo += "开始配置PPG,ED Start";
                      PssBert.BertPGStart(cardPPG, ENDSIGN, channelPPG);
                      Thread.Sleep(500);
                      PssBert.BertEDStart(cardPPG, ENDSIGN, channelPPG);
                      Thread.Sleep(500);
                      #endregion

                      State = PssBert.BertClr(PssBert.CARDID_3, ENDSIGN, channelPPG);
                      Thread.Sleep(300);
#if Ag86100
                      //眼图仪初始化
                      Ag8610 = new Agilent86100A("7");
                      string idnAg8610 = Ag8610.GetIdn();
                      if (string.IsNullOrEmpty(idnAg8610.Trim()))
                      {
                          MessageBox.Show("眼图仪初始化失败", "系统提示");
                      }
#endif
                      MessageBox.Show("初始化成功", "系统提示");
                      //关闭显示正在初始化
                      IsTestEnable = true;
                      IsIniReady = true;
                  });
                thread.IsBackground = true;
                thread.Start();



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
            try
            {

                Thread thread = new Thread(() =>
                {
                    if (IsTestEnable == false)
                    {
                        MessageBox.Show("系统未初始化", "系统提示");
                        return;
                    }
                    IsTestEnable = false;
                    TestParas.Clear();
                    RaisePropertyChanged(() => TestParas);
#if Ag86100
                    Ag8610.AutoScale();
#endif
                    byte[] serbuf = new byte[50];
                    double atten = SenSet;
                    double attRead = 0;
                    State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, atten);
                    Thread.Sleep(300);
                    CheckState(State, PssBase.CARDID_3);
                    State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref attRead);
                    Thread.Sleep(200);
                    CheckState(State, PssBase.CARDID_3);

                    //获取SN 12位
                    uint count = 0xC;
                    State = PssDOA.ReadDDM(PssBase.CARDID_3, ENDSIGN, 0xA1, 68, count, serbuf);
                    Thread.Sleep(200);
                    if (serbuf[0] == 0 && serbuf[1] == 0)
                    {
                        Thread.Sleep(1000);
                        State = PssDOA.ReadDDM(PssBase.CARDID_3, ENDSIGN, 0xA1, 68, count, serbuf);
                        Thread.Sleep(200);
                    }

                    //if (State != 0)
                    //{
                    //    MessageBox.Show("查询SN失败,请重试！", "系统提示");
                    //    IsTestEnable = true;
                    //    return;
                    //}

                    string sn = Encoding.ASCII.GetString(serbuf);
                    sn = sn.Substring(0, 35);
                    Console.WriteLine(sn);

                    string[] datas = sn.Split(' ');


                    byte[] byteSn = new byte[count];
                    for (int i = 0; i < count; i++)
                    {
                        byteSn[i] = Convert.ToByte(datas[i].Trim(), 16);
                    }
                    SN = (Encoding.ASCII.GetString(byteSn));
                    TestParas.SN = SN;
                    RaisePropertyChanged(() => TestParas);



                    //pf power
                    State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref _power);
                    Thread.Sleep(200);
                    Power = _power - Coefficient;
                    TestParas.Power = Power.ToString("f2");
                    if (Power >= PfMin && Power <= PfMax)
                    {
                        TestParas.IsPowerPass = true;
                    }

                    else
                    {
                        MessageBox.Show("功率范围内，请重试!", "系统提示");
                        TestParas.IsPowerPass = false;
                        IsTestEnable = true;
                        TestParas.FinalResult = false;
                        return;
                    }
                    RaisePropertyChanged(() => TestParas);

                    

                    //Rx Points
                    //Rx  Point1
                    atten = Rx1Set;
                    State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, atten);
                    Thread.Sleep(1000);
                    CheckState(State, PssBase.CARDID_3);
                    //State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref attRead);
                    //Thread.Sleep(200);
                    //MessageBox.Show(string.Format("当前Atten:{0}", attRead));
                    //CheckState(State, PssBase.CARDID_3);
                    State = PssDOA.ReadDDM_RxPower(PssBase.CARDID_3, ENDSIGN, 0XA3, ref _rxPoint1);
                    Thread.Sleep(300);
                    RxPoint1 = 10 * Math.Log10(_rxPoint1);
                    TestParas.RxPoint1 = RxPoint1.ToString("f2");
                    RaisePropertyChanged(() => TestParas);
                    if ((RxPoint1 >= Rx1Set - RxMin) && (RxPoint1 <= Rx1Set + RxMax))
                    {
                        TestParas.IsRxPoint1Pass = true;
                    }
                    else
                    {
                        TestParas.IsRxPoint1Pass = false;
                    }
                    //Rx point 2
                    atten = Rx2Set;
                    State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, atten);
                    Thread.Sleep(1000);
                    CheckState(State, PssBase.CARDID_3);
                    //State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref attRead);
                    //Thread.Sleep(200);
                    //MessageBox.Show(string.Format("当前Atten:{0}", attRead));
                    State = PssDOA.ReadDDM_RxPower(PssBase.CARDID_3, ENDSIGN, 0XA3, ref _rxPoint2);
                    Thread.Sleep(200);
                    RxPoint2 = 10 * Math.Log10(_rxPoint2);
                    TestParas.RxPoint2 = RxPoint2.ToString("f2");
                    RaisePropertyChanged(() => TestParas);
                    if ((RxPoint2 >= Rx2Set - RxMin) && (RxPoint2 <= Rx2Set + RxMax))
                    {
                        TestParas.IsRxPoint2Pass = true;

                    }
                    else
                    {
                        TestParas.IsRxPoint2Pass = false;
                    }
                    //Rx point3
                    atten = Rx3Set;
                    State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, atten);
                    Thread.Sleep(1000);
                    CheckState(State, PssBase.CARDID_3);
                    //State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref attRead);
                    //Thread.Sleep(200);
                    //MessageBox.Show(string.Format("当前Atten:{0}", attRead));
                    State = PssDOA.ReadDDM_RxPower(PssBase.CARDID_3, ENDSIGN, 0XA3, ref _rxPoint3);
                    Thread.Sleep(200);
                    RxPoint3 = 10 * Math.Log10(_rxPoint3);
                    TestParas.RxPoint3 = RxPoint3.ToString("f2");
                    RaisePropertyChanged(() => TestParas);
                    if ((RxPoint3 >= Rx3Set - RxMin) && (RxPoint3 <= Rx3Set + RxMax))
                    {
                        TestParas.IsRxPoint3Pass = true;

                    }
                    else
                    {
                        TestParas.IsRxPoint3Pass = false;
                    }

                    //设置测试时衰减
                    State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, atten);
                    Thread.Sleep(300);
                    CheckState(State, PssBase.CARDID_3);
                    State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref attRead);
                    Thread.Sleep(200);
                    CheckState(State, PssBase.CARDID_3);
                    //temp
                    State = PssDOA.ReadDDM_Temperature(PssBase.CARDID_3, ENDSIGN, 0XA3, ref _temp);
                    Thread.Sleep(200);
                    CheckState(State, PssBase.CARDID_3);
                    Temp = _temp;
                    TestParas.Temperature = Temp.ToString("f2");
                    switch (TempLevel)
                    {
                        case "-40":
                            if (Temp < -15)
                            {
                                TestParas.IsTempPass = true;
                            }
                            else
                            {
                                TestParas.IsTempPass = false;
                            }
                            break;
                        case "25":
                            TestParas.IsTempPass = true;
                            break;
                        case "85":
                            if (Temp < 0)
                            {
                                MessageBox.Show("温度测试条件选择失败", "系统提示");
                                return;
                            }
                            if (Temp < 80)
                            {
                                TestParas.IsTempPass = true;
                            }
                            else
                            {
                                TestParas.IsTempPass = false;
                            }
                            break;

                        default:
                            break;
                    }
                    RaisePropertyChanged(() => TestParas);
                    //Bias
                    State = PssDOA.ReadDDM_BiasTx(PssBase.CARDID_3, ENDSIGN, 0XA3, ref _bias);
                    Thread.Sleep(200);
                    CheckState(State, PssBase.CARDID_3);
                    Bias = _bias / 1000;
                    TestParas.Bias = Bias.ToString("f2");
                    if (Bias >= BiasMin && Bias <= BiasMax)
                    {
                        TestParas.IsBiasPass = true;

                    }
                    else
                    {
                        TestParas.IsBiasPass = false;
                    }
                    RaisePropertyChanged(() => TestParas);
                    //Sensitivity

                    double sen = GetSensitivity(PssBase.CARDID_2);
                    Sen = sen.ToString("E");
                    TestParas.Sensitivity = Math.Log10(sen).ToString("f2");
                    if (Math.Log10(sen) <= (ErSet))
                    {
                        TestParas.IsSensitivity = true;

                    }
                    else
                    {
                        TestParas.IsSensitivity = false;
                    }

                    RaisePropertyChanged(() => TestParas);
                    //Thread.Sleep(5000);
#if Ag86100
                    //Crossing

                    Crossing = Ag8610.GetCrossing(DcaChannle.Trim());
                    TestParas.Crossing = Crossing.ToString("f2");
                    if (Crossing >= CrosMin && Crossing <= CrosMax)
                    {
                        TestParas.IsCrossPass = true;

                    }
                    else
                    {
                        TestParas.IsCrossPass = false;
                    }
                    RaisePropertyChanged(() => TestParas);

                    //ExRatio
                    ExRatio = Ag8610.GetExRatio(DcaChannle.Trim());
                    if (ExRatio >= ErMin && ExRatio <= ErMax)
                    {
                        TestParas.IsExRatioPass = true;
                        TestParas.ExRatio = ExRatio.ToString("f2");
                    }
                    else
                    {
                        TestParas.IsExRatioPass = false;
                    }
                    RaisePropertyChanged(() => TestParas);
                    
#endif
                    IsTestEnable = true;
                    //导出数据
                    TestParas.Time = DateTime.Now.ToString();
                    TestParas.TempLevel = TempLevel;
                    if (ProductType == null || ProductType == string.Empty)
                    {
                        TestParas.ProductType = "defult";
                    }
                    if (TestParas.SN == string.Empty)
                    {
                        TestParas.SN = "000000000000";
                    }
                    bool isEmpty = false;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string filePath = dirPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + TempLevel + ".txt";
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string output = null;
                            if ((output = sr.ReadLine()) == null)
                            {
                                isEmpty = true;
                            }
                            else
                            {
                                isEmpty = false;
                            }
                        }
                    }
                    using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            if (isEmpty == true)
                            {
                                sw.WriteLine("SN,PF,ER,CROSS,SEN,RX PF10,RX PF20,RX PF30,Temp,Bias,Final,time,chtemp,producttype");
                            }
                            sw.WriteLine(TestParas.ToString());                            
                            TestParas.SN = string.Empty;
                            RaisePropertyChanged(() => TestParas);
                        }
                    }
                    //final result
                    if (TestParas.IsBiasPass == true && TestParas.IsCrossPass == true && TestParas.IsExRatioPass == true && TestParas.IsPowerPass == true
                    && TestParas.IsRxPoint1Pass == true && TestParas.IsRxPoint2Pass == true && TestParas.IsRxPoint3Pass == true &&
                    TestParas.IsSensitivity == true && TestParas.IsTempPass == true)
                    {
                        TestParas.FinalResult = true;
                    }
                    else
                    {
                        TestParas.FinalResult = false;
                        MessageBox.Show("产品测量不合格，请重试", "系统提示");
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
                throw;
            }
        }
        /// <summary>
        /// 光功率模块校验
        /// </summary>
        public RelayCommand OpmCalibration
        {
            get
            {
                return new RelayCommand(() => ExecuteOpmCalibration());
            }
        }
        private void ExecuteOpmCalibration()
        {
            if (IsComReady == false)
            {
                MessageBox.Show("请先初始化串口！", "系统提示");
                return;
            }
            if (StandardPower == 0)
            {
                MessageBox.Show("请先设置基准键光功率", "系统提示");
                return;
            }
            uint readData = 0;

            MessageBox.Show("请将Tx端光纤线连接到光功率计模块", "系统提示");
            //设置波长
            State = PssOPM.OPMConfWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, TxWavelength);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            State = PssOPM.OPMReadWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref readData);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            if (readData != TxWavelength)
            {
                MessageBox.Show("波长设置失败,请重新设置", "系统设置");
                return;
            }
            State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref _actualPower);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            ActualPower = _actualPower * 1;
            double diff = ActualPower - StandardPower;


            if (Math.Abs(diff) > 5)
            {
                MessageBox.Show("修正系数超出预期，请联系工程师解决", "系统提示");
                IsTestEnable = false;
                return;
            }
            Coefficient = diff;
            XElement xele = XDoc.Root.Element("CalOfOpm");
            xele.SetValue(Coefficient);
            XDoc.Save("Configure.xml");
        }


        /// <summary>
        /// 设定衰减器Att
        /// </summary>
        public RelayCommand SetDoaAtt
        {
            get
            {
                return new RelayCommand(() => ExecuteSetDoaAtt());
            }
        }
        private void ExecuteSetDoaAtt()
        {
            uint readData = 0;
            double att = 0;
            //设置波长
            State = PssOPM.OPMConfWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, RxWavelength);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            State = PssOPM.OPMReadWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref readData);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            if (readData != RxWavelength)
            {
                MessageBox.Show("波长设置失败,请重新设置", "系统设置");
                return;
            }
            //设置Att
            State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, StandardAtt);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref att);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            if (att != StandardAtt)
            {
                MessageBox.Show("衰减模块衰减设置失败,请重试", "系统提示");
                return;
            }
        }
        /// <summary>
        /// 光衰减模块修正
        /// </summary>
        public RelayCommand DoaCalibration
        {
            get
            {
                return new RelayCommand(() => ExecuteDoaCalibration());
            }
        }
        private void ExecuteDoaCalibration()
        {
            if (IsComReady == false)
            {
                MessageBox.Show("请先初始化串口", "系统提示");
                return;
            }
            if (StandardAtt >= 0)
            {
                MessageBox.Show("请设置衰减值", "系统提示");
            }
            uint readData = 0;
            MessageBox.Show("请将Rx端光纤连接到光功率计模块", "系统提示");
            //设置波长
            State = PssOPM.OPMConfWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, RxWavelength);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            State = PssOPM.OPMReadWavelength(PssBase.CARDID_4, ENDSIGN, OpmChannel, ref readData);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            if (readData != RxWavelength)
            {
                MessageBox.Show("波长设置失败,请重新设置", "系统设置");
                return;
            }
            double cal = 0;
            double att = 0;
            //读取初始Calibration
            State = PssDOA.DOAReadCalibration(PssBase.CARDID_3, ENDSIGN, ref cal);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            //设置Att
            State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, StandardAtt);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref att);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            if (att != StandardAtt)
            {
                MessageBox.Show("衰减模块衰减设置失败,请重试", "系统提示");
                return;
            }
            //使用光衰减器和设定值比对
            State = PssDOA.DOAReadPower(PssBase.CARDID_3, ENDSIGN, ref _actualAtt);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_4);
            ActualAtt = _actualAtt;
            double diff = ActualAtt - StandardAtt;
            if (Math.Abs(diff) > 0.2)
            {
                State = PssDOA.DOAConfCalibration(PssBase.CARDID_3, ENDSIGN, cal + diff);
                Thread.Sleep(200);
                CheckState(State, PssBase.CARDID_3);
            }
            State = PssDOA.DOAReadCalibration(PssBase.CARDID_3, ENDSIGN, ref _calOfDOA);
            CalOfDOA = _calOfDOA;
            XDoc.Root.Element("CalOfDoa").SetValue(CalOfDOA);
            XDoc.Save("Configure.xml");
        }
        /// <summary>
        /// 系统校验命令
        /// </summary>
        //public RelayCommand Calibration
        //{
        //    get
        //    {
        //        return new RelayCommand(() => ExecuteCalibration());
        //    }
        //}
        //private void ExecuteCalibration()
        //{
        //    try
        //    {
        //        if (IsComReady == false)
        //        {
        //            MessageBox.Show("请先初始化系统", "系统设置");
        //            return;
        //        }
        //        double calDoa = 0;
        //        double diff = 0;
        //        //Tx端校验   光功率计 使用系数
        //        MessageBox.Show("请将Tx光纤连接到光功率计", "系统提示");
        //        if (double.TryParse(Interaction.InputBox("请输入基准键功率:", "系统校验", "5.86"), out double result))
        //        {
        //            double actualPower = 0;
        //            State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, PssOPM.OPM_CHANNEL_1, ref actualPower);
        //            Thread.Sleep(200);
        //            CheckState(State, PssBase.CARDID_4);
        //            diff = actualPower - result;
        //            if (Math.Abs((result - actualPower)) > 0.1)
        //            {
        //                Coefficient = result / actualPower;
        //            }
        //            else
        //            {

        //            }
        //        }
        //        //Rx端校验  光衰减器使用差值
        //        double atten = -10;
        //        double attenOpm = 0;
        //        double readAtten = 0;
        //        MessageBox.Show("请将衰减器直接连接到光功率计");
        //        State = PssDOA.DOAConfAtten(PssBase.CARDID_3, ENDSIGN, atten);
        //        Thread.Sleep(300);
        //        CheckState(State, PssBase.CARDID_3);
        //        State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref readAtten);
        //        Thread.Sleep(200);
        //        CheckState(State, PssBase.CARDID_3);
        //        State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, PssOPM.OPM_CHANNEL_1, ref attenOpm);
        //        Thread.Sleep(300);
        //        diff = attenOpm * Coefficient - readAtten;
        //        if (diff > 0.3)
        //        {
        //            State = PssDOA.DOAReadCalibration(PssBase.CARDID_3, ENDSIGN, ref calDoa);
        //            Thread.Sleep(300);
        //            CheckState(State, PssBase.CARDID_3);
        //            calDoa += diff;
        //            State = PssDOA.DOAConfCalibration(PssBase.CARDID_3, ENDSIGN, calDoa);
        //            Thread.Sleep(300);
        //            CheckState(State, PssBase.CARDID_3);

        //            State = PssOPM.OPMReadPower(PssBase.CARDID_4, ENDSIGN, PssOPM.OPM_CHANNEL_1, ref attenOpm);
        //            Thread.Sleep(300);
        //        }
        //        else
        //        {

        //        }

        //        MessageBox.Show("校验完成", "系统提示");



        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message, "系统提示");
        //    }
        //}
        #endregion
        #region Methods
        private void CheckState(uint state, uint cardId)
        {
            if (state == 0)
                return;
            else
            {
                throw new Exception(string.Format("查询设备时出错,State:{0},CardAdd:{1}", state, cardId));
            }
        }
        /// <summary>
        /// 获取Sen
        /// </summary>
        /// <returns></returns>
        private double GetSensitivity(uint cardId)
        {
            double att = 0;
            State = PssDOA.DOAReadAtten(PssBase.CARDID_3, ENDSIGN, ref att);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            uint patter = 0, level = 0, speed = 0;
            State = PssBert.BertPatterGet(PssBase.CARDID_1, ENDSIGN, PssBert.CHANAL_2, ref patter);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            State = PssBert.BertLevelGet(PssBase.CARDID_1, ENDSIGN, PssBert.CHANAL_2, ref level);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            //检测速率为0
            State = PssBert.BertSpeedGet(PssBase.CARDID_1, ENDSIGN, PssBert.CHANAL_1, ref speed);
            Thread.Sleep(200);
            CheckState(State, PssBase.CARDID_3);
            State = PssBert.BertClr(cardId, ENDSIGN, PssBert.CHANAL_2);
            Thread.Sleep(3000);
            CheckState(State, cardId);
            double bert = 0;
            uint syncState = 0, errorState = 0;
            double errCount = 0, all = 0;
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(300);
                State = PssBert.BertResult(cardId, ENDSIGN, PssBert.CHANAL_2, out syncState, out errorState, out errCount, out all, out bert);
                Thread.Sleep(300);
                CheckState(State, cardId);
                State = PssBert.BertResult(cardId, ENDSIGN, PssBert.CHANAL_2, out syncState, out errorState, out errCount, out all, out bert);
                Thread.Sleep(3000);
                CheckState(State, cardId);
                if (bert < 1E-2)
                    break;
            }

            //bert = Math.Log10(bert);
            return bert;
        }
        #region Com Methods
        public static uint Rs232LinkInitial(string comName, uint rate)
        {
            uint error = 0;
            //先关闭串口 然后重新初始化
            error = PssRS232Driver.RS232Close();
            error = PssRS232Driver.RS232Init(comName, rate);
            PssRS232Driver.RS232TimeOutConfig(0, 1000, 1000);
            return error;
        }
        private static uint PCTxCStringFunc(IntPtr buffer, uint length, uint endSign)
        {
#warning  测试发送命令，完成后注释
            string msg = Marshal.PtrToStringAnsi(buffer);
            uint errorMessage = 0;
            errorMessage = PssRS232Driver.WriteRs232DataStr(buffer);
            if (errorMessage != 0)
                return errorMessage;

            if (endSign == ENDSIGN)
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
