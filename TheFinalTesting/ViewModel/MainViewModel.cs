#define Common
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TheFinalTesting.Model;
using System.IO.Ports;
using XuxzLib.Communication;
using System.IO;

namespace TheFinalTesting.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Fields
        private string SavePath = @"E:\Xu\Test.txt";
        private FinalTestingParas TestingPara = new FinalTestingParas();
        /// <summary>
        /// 指示设备等的初始化是否完成
        /// </summary>
        private bool IsReady = false;
        private bool IsPortReady = false;
        SenParas SenPara = new SenParas();
        bool IsCountChanged;
        /// <summary>
        /// I2C通信串口
        /// </summary>
        SerialPort Port = new SerialPort();
        TransmitBase TranBase = new TransmitBase();
        byte[] SerBuf = new byte[200];
        /// <summary>
        /// 高低压万用表
        /// </summary>
        internal Aglient34401A Ag34401A;
        /// <summary>
        /// 基板电源供应器
        /// </summary>
        internal AglientE3631A AgE3631A;
        /// <summary>
        /// 光功率计
        /// </summary>
        internal HP8153A Hp8153A;
        /// <summary>
        /// 光衰减器
        /// </summary>
        internal HP8156A Hp8156A;
        /// <summary>
        /// 眼图仪
        /// </summary>
        internal MP2100A Mp2100A;
        /// <summary>
        /// 电源供应器
        /// </summary>
        internal PST3202 P3202;
        /// <summary>
        /// 频谱分析仪
        /// </summary>
        internal AQ6317B Aq6317B;
        internal Keithley Keith;
        #endregion
        #region Properties
        private string _sn;

        public string SN
        {
            get { return _sn; }
            set { _sn = value; RaisePropertyChanged(() => SN); }
        }
        #region Initialize Paras
        private double _iniAtt;
        /// <summary>
        /// 初始衰减
        /// </summary>
        public double IniAtt
        {
            get { return _iniAtt; }
            set { _iniAtt = value; RaisePropertyChanged(() => IniAtt); }
        }
        private string _attInSaturation;

        public string AttInSaturation
        {
            get { return _attInSaturation; }
            set { _attInSaturation = value;RaisePropertyChanged(() => AttInSaturation); }
        }
        private double _errorRateInSensitivity;

        public double ErrorRateInSensitivity
        {
            get { return _errorRateInSensitivity; }
            set { _errorRateInSensitivity = value; RaisePropertyChanged(() => ErrorRateInSensitivity); }
        }

        #endregion
        private bool _isTestEnable;

        public bool IsTestEnable
        {
            get { return _isTestEnable; }
            set { _isTestEnable = value; RaisePropertyChanged(() => IsTestEnable); }
        }

        private string _alarms;

        public string Alarms
        {
            get { return _alarms; }
            set { _alarms = value; RaisePropertyChanged(() => Alarms); }
        }

        private string _warnings;

        public string Warnings
        {
            get { return _warnings; }
            set { _warnings = value; RaisePropertyChanged(() => Warnings); }
        }
        private bool _isAWPass;

        public bool IsAWPass
        {
            get { return _isAWPass; }
            set { _isAWPass = value; RaisePropertyChanged(() => IsAWPass); }
        }

        private readonly string[] _strCom;
        public string SelectedCom { get; set; }
        /// <summary>
        /// 待初始化设备展示
        /// </summary>
        public DeviceInfo[] Devices { get; } = new DeviceInfo[]
        {
            new DeviceInfo(true,"Aglient34401A",10,"高低压万用表(地址:10)"),
            new DeviceInfo(false,"AglientE3631A",9,"基板电源供应器(地址:9)"),
            new DeviceInfo(false,"HP8153A",5,"光功率计(地址:5)"),
            new DeviceInfo(true,"HP8156A",22,"光衰减器(地址:22)"),
            new DeviceInfo(true,"MP2100A",16,"眼图仪(地址:2)"),
            new DeviceInfo(false,"PST3202",15,"电源供应器(地址:15)"),
            new DeviceInfo(false,"AQ6317B",2,"频谱分析仪(地址:2)"),
            new DeviceInfo(false,"Keithley",24,"Keithley")

        };
        /// <summary>
        /// 设备初始化信息展示
        /// </summary>
        public string DisplayInfo
        {
            get { return _DisplayInfo; }
            set
            {
                _DisplayInfo = value;
                RaisePropertyChanged(() => DisplayInfo);
            }
        }
        private string _DisplayInfo;
        #region TX Parameters

        private double _supplyCurrent;

        /// <summary>
        /// E3631 供应电流
        /// </summary>
        public double SupplyCurrent
        {
            get { return _supplyCurrent; }
            set { _supplyCurrent = value; RaisePropertyChanged(() => SupplyCurrent); }
        }

        private double _outputPower;
        /// <summary>
        /// 8153A 光功率
        /// </summary>
        public double OutputPower
        {
            get { return _outputPower; }
            set { _outputPower = value; RaisePropertyChanged(() => OutputPower); }
        }
        private bool _txDisable;

        public bool TxDisable
        {
            get { return _txDisable; }
            set { _txDisable = value; RaisePropertyChanged(() => TxDisable); }
        }

        private double _extiRatio;
        /// <summary>
        /// MP2100A
        /// </summary>
        public double ExtiRatio
        {
            get { return _extiRatio; }
            set { _extiRatio = value; RaisePropertyChanged(() => ExtiRatio); }
        }
        private double _crossingRate;
        /// <summary>
        /// MP2100A
        /// </summary>
        public double CrossingRate
        {
            get { return _crossingRate; }
            set { _crossingRate = value; RaisePropertyChanged(() => CrossingRate); }
        }
        private double _centerWavelength;
        /// <summary>
        /// 中心波长
        /// </summary>
        public double CenterWavelength
        {
            get { return _centerWavelength; }
            set { _centerWavelength = value; RaisePropertyChanged(() => CenterWavelength); }
        }
        private double _differenceWavelength;
        /// <summary>
        /// 频谱仪 波长差
        /// </summary>
        public double DifferenceWavelength
        {
            get { return _differenceWavelength; }
            set { _differenceWavelength = value; RaisePropertyChanged(() => DifferenceWavelength); }
        }

        private double _SMSR;

        public double SMSR
        {
            get { return _SMSR; }
            set { _SMSR = value; RaisePropertyChanged(() => SMSR); }
        }
        private double _jitter;
        /// <summary>
        /// MP2100A 
        /// </summary>
        public double Jitter
        {
            get { return _jitter; }
            set { _jitter = value; RaisePropertyChanged(() => Jitter); }
        }

        private double _maskMargin;

        public double MaskMargin
        {
            get { return _maskMargin; }
            set { _maskMargin = value; RaisePropertyChanged(() => MaskMargin); }
        }

        #endregion
        #region Rx Parameters
        private double _sensitivity;

        public double Sensitivity
        {
            get { return _sensitivity; }
            set { _sensitivity = value; RaisePropertyChanged(() => Sensitivity); }
        }
        private double _sdAsserted;

        public double SdAsserted
        {
            get { return _sdAsserted; }
            set { _sdAsserted = value; RaisePropertyChanged(() => SdAsserted); }
        }
        private double _sdDesserted;

        public double SdDesserted
        {
            get { return _sdDesserted; }
            set { _sdDesserted = value; RaisePropertyChanged(() => SdDesserted); }
        }
        private double _sdHigh;

        public double SDHigh
        {
            get { return _sdHigh; }
            set { _sdHigh = value; RaisePropertyChanged(() => SDHigh); }
        }
        private double _sdLow;

        public double SDLow
        {
            get { return _sdLow; }
            set { _sdLow = value; RaisePropertyChanged(() => SDLow); }
        }
        private double _hysteresis;

        public double Hysteresis
        {
            get { return _hysteresis; }
            set { _hysteresis = value;RaisePropertyChanged(() => Hysteresis); }
        }

        private bool _saturation;

        public bool Saturation
        {
            get { return _saturation; }
            set { _saturation = value; RaisePropertyChanged(() => Saturation); }
        }


        #endregion
        #region DDMI/A0H Parameters
        private double _rxPoint1;

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
        private double _txPower;

        public double TxPower
        {
            get { return _txPower; }
            set { _txPower = value; RaisePropertyChanged(() => TxPower); }
        }
        private double _vcc;

        public double Vcc
        {
            get { return _vcc; }
            set { _vcc = value; RaisePropertyChanged(() => Vcc); }
        }
        private double _temp;

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
        #region A/W Paremeters

        #endregion
        #endregion
        #region Consturctors
        public MainViewModel()
        {
            _strCom = SerialPort.GetPortNames();
            IsTestEnable = true;
            IniAtt = 16.0;
            AttInSaturation = "9";
            ErrorRateInSensitivity = -3;
        }
        #endregion
        #region Commands
        /// <summary>
        /// 设备初始化命令
        /// </summary>
        public RelayCommand InitializeCommand
        {
            get { return new RelayCommand(() => ExecuteInitialize()); }
        }
        /// <summary>
        /// 测试命令
        /// </summary>
        public RelayCommand TestCommand
        {
            get
            {
                return new RelayCommand(() => ExecuteTest());
            }
        }
        public RelayCommand ClosedCommand
        {
            get
            {
                return new RelayCommand(() => ExecuteClosed());
            }

        }

        #endregion
        #region CommandMethods
        private void ExecuteInitialize()
        {
            StringBuilder info = new StringBuilder();
            info.Append("程序正在初始化。。" + DateTime.Now.ToShortTimeString() + "\r\n");
            try
            {
                //串口初始化
                if (!string.IsNullOrEmpty(SelectedCom))
                {
                    info.Append("串口初始化中。。。");
                    if (Port.IsOpen == true)
                        Port.Close();
                    Port.PortName = SelectedCom.Trim();
                    Port.Parity = 0;
                    Port.BaudRate = 19200;
                    Port.StopBits = StopBits.Two;
                    Port.DataBits = 8;
                    Port.ReadTimeout = 100;
                    Port.WriteTimeout = 100;

                    Port.Open();
                    string msg = TranBase.IsPortReady(Port, SerBuf) + DateTime.Now.ToShortTimeString();
                    if (msg == null)
                    {
                        info.Append("I2C通信失败！");
                        DisplayInfo = info.ToString();
                    }
                    else
                    {
                        IsPortReady = true;
                        info.Append("初始化成功 \r\n");
                        info.Append("msg");
                        DisplayInfo = info.ToString();
                    }
                }
                else
                {
                    info.Append("未选择串口号！\r\n");
                    info.Append("串口初始化失败！\r\n");
                    DisplayInfo = info.ToString();
                }
                //设备初始化
                foreach (var item in Devices)
                {
                    if (item.IsSelected == true)
                    {
                        if (item.Address != 0)
                        {
                            string address = item.Address.ToString();
                            int index = Array.IndexOf(Devices, item);
                            switch (index)
                            {
                                case 0:
                                    if (Ag34401A != null)
                                        Ag34401A.Dispose();
                                    info.Append("Aglient34401A初始化中。。。\r\n");
                                    Ag34401A = new Aglient34401A(address);
                                    info.Append(Ag34401A.GetIdn());
                                    if (Ag34401A != null)
                                        info.Append(Ag34401A.DeviceName + "初始化OK \r\n");
                                    else
                                    {
                                        info.Append(Ag34401A.DeviceName + "初始化失败 \r\n");
                                    }
                                    DisplayInfo = info.ToString();

                                    break;
                                case 1:
                                    if (AgE3631A != null)
                                        AgE3631A.Dispose();
                                    info.Append("AglientE3631A初始化中。。。\r\n");
                                    AgE3631A = new AglientE3631A(address);
                                    info.Append(AgE3631A.GetIdn());
                                    if (AgE3631A != null)
                                    {
                                        AgE3631A.Open();
                                        info.Append(AgE3631A.DeviceName + "初始化OK \r\n");
                                    }
                                    else
                                    {
                                        info.Append(AgE3631A.DeviceName + "初始化失败 \r\n");
                                    }
                                    DisplayInfo = info.ToString();

                                    break;
                                case 2:
                                    if (Hp8153A != null)
                                        Hp8153A.Dispose();
                                    info.Append("Hp8153A初始化中。。。\r\n");
                                    Hp8153A = new HP8153A(address);
                                    info.Append(Hp8153A.GetIdn());
                                    if (Hp8153A != null)
                                    {
                                        info.Append(Hp8153A.DeviceName + "初始化OK \r\n");
                                    }

                                    else
                                    {
                                        info.Append(Hp8153A.DeviceName + "初始化失败 \r\n");
                                    }
                                    DisplayInfo = info.ToString();

                                    break;
                                case 3:
                                    if (Hp8156A != null)
                                        Hp8156A.Dispose();
                                    info.Append("Hp8156A初始化中。。。\r\n");
                                    Hp8156A = new HP8156A(address);
                                    info.Append(Hp8156A.GetIdn());
                                    if (Hp8156A != null)
                                    {
                                        Hp8156A.Open();
                                        info.Append(Hp8156A.DeviceName + "初始化OK \r\n");
                                    }
                                    else
                                    {
                                        info.Append(Hp8156A.DeviceName + "hp8156A初始化失败\r\n");

                                    }
                                    DisplayInfo = info.ToString();

                                    break;
                                case 4:
                                    if (Mp2100A != null)
                                        Mp2100A.Dispose();
                                    info.Append("MP2100A初始化中。。。\r\n");
                                    Mp2100A = new MP2100A(address);
                                    info.Append(Mp2100A.GetIdn());
                                    if (Mp2100A != null)
                                    {
                                        info.Append(Mp2100A.DeviceName + "初始化OK \r\n");

                                    }
                                    else
                                    {
                                        info.Append(Mp2100A.DeviceName + "初始化失败\r\n");
                                    }
                                    DisplayInfo = info.ToString();
                                    break;
                                case 5:
                                    if (P3202 != null)
                                        P3202.Dispose();
                                    info.Append("P3202初始化中。。。\r\n");
                                    P3202 = new PST3202(address);
                                    info.Append(P3202.GetIdn());
                                    if (P3202 != null)
                                    {
                                        P3202.Open();
                                        info.Append(P3202.DeviceName + "初始化OK \r\n");
                                    }

                                    else
                                    {
                                        info.Append(P3202.DeviceName + "初始化失败");
                                    }
                                    DisplayInfo = info.ToString();

                                    break;
                                case 6:
                                    if (Aq6317B != null)
                                        Aq6317B.Dispose();
                                    info.Append("AQ6317B初始化中。。。\r\n");
                                    Aq6317B = new AQ6317B(address);
                                    info.Append(Aq6317B.GetIdn());
                                    if (Aq6317B != null)
                                        info.Append(Aq6317B.DeviceName + "初始化OK \r\n");
                                    else
                                    {
                                        info.Append(Aq6317B.DeviceName + "初始化失败");
                                    }
                                    DisplayInfo = info.ToString();
                                    break;
                                case 7:
                                    if (Keith != null)
                                        Keith.Dispose();
                                    info.Append("Keithley初始化中。。。\r\n");
                                    Keith = new Keithley(address);
                                    info.Append(Keith.GetIdn());
                                    if (Keith != null)
                                    {
                                        info.Append(Keith.DeviceName + "初始化成功\r\n");
                                    }
                                    else
                                    {
                                        info.Append(Keith.DeviceName + "初始化失败\r\n");
                                    }
                                    DisplayInfo = info.ToString();
                                    break;
                                default:
                                    throw new Exception("该设备不存在");
                            }
                        }
                    }
                }
                IsReady = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
                IsReady = false;
            }
        }
        /// <summary>
        /// 终测命令
        /// </summary>
        private void ExecuteTest()
        {
            bool isSaveData = false;
            StringBuilder strBuild = new StringBuilder();
            IsTestEnable = false;
            try
            {
                //另一线程执行该方法
                Thread thread = new Thread(() =>
                  {
                      //SN
                      if (!string.IsNullOrEmpty(SN))
                      {
                          TestingPara.SN = SN;
                      }
                      //GPIB通信
                      if (IsReady)
                      {
                          double ini = IniAtt;
                          double span = 0.2;
                          double voltage;
                          double saturation = 0;
                          //Hp8156A.Open();
                          Mp2100A.AutoScale();
                          Aq6317B.SetSingle();

                          //Supply Current
                          SupplyCurrent = AgE3631A.GetCurrent();
                          TestingPara.SupplyCurrent = SupplyCurrent.ToString("F3");
                          //OutputPower
                          OutputPower = Hp8153A.ReadPower("2");
                          TestingPara.OutputPower = OutputPower.ToString("F3");

                          //SD_Desserted SD_High
                          for (int i = 0; i <= 40; i++)
                          {
                              
                              Hp8156A.SetAtt(ini.ToString());
                              Thread.Sleep(300);
                              voltage = Ag34401A.GetVoltage();
                              Thread.Sleep(100);
                              if (i == 0 && voltage >= 2.0)
                              {
                                  MessageBox.Show("初始即为高电压，请检查设备连接！", "系统提示");
                                  return;
                              }
                              if (voltage >= 2.0)
                              {
                                  SdDesserted = ini;
                                  TestingPara.SD_Desserted = SdDesserted.ToString();
                                  SDHigh = voltage;
                                  TestingPara.SD_High = SDHigh.ToString("F3");
                                  //SdDesserted SdHigh
                                  strBuild.Append(string.Format("SdDesserted:{0}", SdDesserted));
                                  strBuild.Append(string.Format("SdHigh:{0}", SDHigh));
                                  DisplayInfo = strBuild.ToString();
                                  break;
                              }
                              else
                              {
                                  ini += span;
                                  if (i == 40)
                                  {
                                      strBuild.Append("获取SdDesserted失败！");
                                      DisplayInfo = strBuild.ToString();
                                  }
                              }
                          };
                          //SD_Asserted,SD_Low
                          for (int i = 0; i <= 30; i++)
                          {
                              Hp8156A.SetAtt(ini.ToString());
                              Thread.Sleep(200);
                              voltage = Ag34401A.GetVoltage();
                              Thread.Sleep(100);
                              if (voltage < 0.5)
                              {
                                  SdAsserted = ini;
                                  TestingPara.SD_Asserted = SdAsserted.ToString();
                                  SDLow = voltage;
                                  TestingPara.SD_Low = SDLow.ToString();
                                  strBuild.Append(string.Format("SdAsserted:{0}", SdAsserted));
                                  strBuild.Append(string.Format("SdLow:{0}", SDLow));
                                  DisplayInfo = strBuild.ToString();
                                  break;
                              }
                              else
                              {
                                  ini -= span;
                                  if (i == 30)
                                  {
                                      strBuild.Append("获取SdAsserted失败");
                                      DisplayInfo = strBuild.ToString();
                                  }
                              }

                          }
                          //Hysteresis
                          Hysteresis = SdDesserted = SdAsserted;
                          TestingPara.Hysteresis = Hysteresis.ToString();
                          #region MP2100
                          Hp8156A.SetAtt(IniAtt.ToString());
                          //Exit.Ratio
                          ExtiRatio = Mp2100A.GetExRatio();
                          TestingPara.ExtioRatio = ExtiRatio.ToString("F3");
                          strBuild.Append(string.Format("Extinction Ratio:{0}", ExtiRatio));
                          DisplayInfo = strBuild.ToString();
                          //Crossing
                          CrossingRate = Mp2100A.GetCrossing();
                          TestingPara.Crossing = CrossingRate.ToString("F3");
                          strBuild.Append(string.Format("CrossingRate:{0}", CrossingRate));
                          DisplayInfo = strBuild.ToString();
                          //MaskMargin
                          MaskMargin = Mp2100A.GetMaskMargin();
                          TestingPara.MaskMargin = MaskMargin.ToString("F3");
                          strBuild.Append(string.Format("Mask Margin:{0}", MaskMargin));
                          DisplayInfo = strBuild.ToString();
                          //Jitter
                          Jitter = Mp2100A.GetJitter();
                          TestingPara.Jitter = Jitter.ToString("F3");
                          strBuild.Append(string.Format("Jitter:{0}", Jitter));
                          DisplayInfo = strBuild.ToString();
                          Thread.Sleep(2000);
                          //OSA
                          string osaData = Aq6317B.GetData();
                          string[] data = osaData.Split(',');
                          //CenterWavelength
                          if (double.TryParse(data[1].Trim(), out double data1))
                          {
                              CenterWavelength = data1;
                          }
                          else
                          {
                              CenterWavelength = 0;
                          }
                          TestingPara.CenterWavelength = CenterWavelength.ToString();
                          //SMSR
                          if (double.TryParse(data[4].Trim(), out double data4))
                          {
                              SMSR = data4;
                          }
                          else
                          {
                              SMSR = 0;
                          }
                          TestingPara.SMSR = SMSR.ToString();
                          //WavelengthDiff
                          if (double.TryParse(data[0].Trim(), out double data0))
                          {
                              DifferenceWavelength = data0;
                          }
                          else
                          {
                              DifferenceWavelength = 0;
                          }
                          TestingPara.WavelengthDiff = DifferenceWavelength.ToString();

                          strBuild.AppendLine(string.Format("中心波长:{0}", CenterWavelength));
                          strBuild.AppendLine(string.Format("Δλ:{0}", DifferenceWavelength));
                          strBuild.AppendLine(string.Format("SMSR:{0}", SMSR));
                          DisplayInfo = strBuild.ToString();
                          //Sensitivity
                          Sensitivity = Math.Round(GetSensitivity(), 2);
                          TestingPara.Sensitivity = Sensitivity.ToString();
                          strBuild.Append(string.Format("Sensitivity:{0}", Sensitivity));
                          DisplayInfo = strBuild.ToString();
                          #endregion
                          Thread.Sleep(300);

                          //Saturation
                          //测试饱和度 设置 衰减
                          Hp8156A.SetAtt(AttInSaturation);
                          Thread.Sleep(3000);
                          for (int i = 0; i < 5; i++)
                          {
                              Thread.Sleep(300);
                              double r = Mp2100A.GetErrorRate();
                              if (r >= 0)
                              {
                                  saturation = Math.Log10(r);
                                  if (saturation > -9)
                                  {
                                      Saturation = false;
                                      break;
                                  }
                              }
                              else
                              {
                                  MessageBox.Show("读取眼图仪误码率发生错误", "系统设置");
                                  break;
                              }
                              Saturation = true;
                          }
                          TestingPara.Saturation = Saturation;
                          strBuild.Append(string.Format("Saturation:{0}", Saturation));
                          DisplayInfo = strBuild.ToString();

                          //TxDisable
                          Thread.Sleep(200);
                          Hp8156A.Close();
                          P3202.SetVolage("1", "2.0");
                          Thread.Sleep(3000);
                          double txDisablePower = Hp8153A.ReadPower("2");
                          strBuild.Append(string.Format("Power@TxDisable:{0}", txDisablePower));
                          DisplayInfo = strBuild.ToString();
                          if (txDisablePower < -20.0 || Math.Log10(txDisablePower) > 10)
                          {
                              TxDisable = true;
                              TestingPara.TxDisable = true;
                          }
                          P3202.SetVolage("1", "0.8");
                          isSaveData = true;
                      }
                      else
                      {
                          MessageBox.Show("设备初始化未完成,将无法进行GPIB测试", "系统提示");
                      }
                      //IIC通信参数
                      if (IsPortReady)
                      {
                          I2CTest();
                          isSaveData = true;

                      }
                      else
                      {
                          MessageBox.Show("串口初始化未完成", "系统提示");
                      }
                      if (isSaveData)
                      {
                          //将数据存储到本地
                          MessageBoxResult result = MessageBox.Show("测试完成，是否储存数据？", "系统提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                          if (result == MessageBoxResult.OK)
                          {
                              bool isEmpty = false;
                              if (!Directory.Exists(@"E:\Xu"))
                              {
                                  Directory.CreateDirectory(@"E:\Xu");
                              }
                              using (FileStream fs = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
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
                              using (FileStream fs = new FileStream(SavePath, FileMode.Append, FileAccess.Write, FileShare.None))
                              {
                                  using (StreamWriter sw = new StreamWriter(fs))
                                  {
                                      if (isEmpty == true)
                                      {
                                          sw.WriteLine("SN,SupplyCurrent,OutputPower,ExRatio,Crossing,Jitter,MaskMargin," +
                                              "CenterWavelength,SMSR,WavelengthDiff,TxDisable,Sensitivity,SD_Asserted,SD_Desserted," +
                                              "Hysteresis,SD_High,SD_Low,Saturation,RxPoint1,RxPoint2,RxPoint3,TxPower," +
                                              "Vcc,Temp,Bias,IsAwPass,Date");
                                      }
                                      sw.WriteLine(TestingPara.ToString());
                                  }
                              }

                          }
                      }
                      IsTestEnable = true;
                  });
                thread.IsBackground = true;
                thread.Start();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
        }
        private void ExecuteClosed()
        {
            //Dispose resources
            if (Ag34401A != null)
                Ag34401A.Dispose();
            if (AgE3631A != null)
                AgE3631A.Dispose();
            if (Hp8153A != null)
                Hp8153A.Dispose();
            if (Hp8156A != null)
                Hp8156A.Dispose();
            if (Mp2100A != null)
                Mp2100A.Dispose();
            if (Aq6317B != null)
                Aq6317B.Dispose();
            if (P3202 != null)
                P3202.Dispose();
            if (Keith != null)
                Keith.Dispose();
        }
        #endregion
        #region Methods
        //I2C通信
        #region I2CMethods
        /// <summary>
        /// 获取Temp,Vcc,Bias,TxPower,RxPoint 1/2/3
        /// </summary>
        private void I2CTest()
        {

            Thread.Sleep(200);
            Hp8156A.SetAtt(IniAtt.ToString());
            Thread.Sleep(300);
            Hp8156A.Open();
            Thread.Sleep(200);
            //Temp，Vcc,Bias,TxPower
            this.GetParas();
            Thread.Sleep(200);
            //RxPoint 1
            Hp8156A.SetAtt("10");
            Thread.Sleep(1000);
            RxPoint1 = Math.Round(GetRxPower());
            TestingPara.RxPoint1 = RxPoint1.ToString();
#if Common
            //RxPoint 2
            Hp8156A.SetAtt("19");
            Thread.Sleep(1000);
            RxPoint2 = Math.Round(GetRxPower());
            TestingPara.RxPoint2 = RxPoint2.ToString();
            //RxPoint 3
            Hp8156A.SetAtt("28");
            Thread.Sleep(1000);
            RxPoint3 = Math.Round(GetRxPower());
            TestingPara.RxPoint3 = RxPoint3.ToString();

            //A/W
            Hp8156A.SetAtt("25");
            Thread.Sleep(300);
            IsAWPass= GetAlarmAndWarning();
            TestingPara.IsAwPass = IsAWPass;
#endif
            Hp8156A.SetAtt(IniAtt.ToString());
        }
        /// <summary>
        /// 获取Alarms和Warnings 信息
        /// </summary>
        bool GetAlarmAndWarning()
        {
            List<byte> alarms = TranBase.MyI2C_ReadA2HByte(SerBuf, Port, 112, 2);
            //转换为二进制
            string dataAlarm = Convert.ToString((ushort)(alarms[0] * 256 + alarms[1]), 2).PadLeft(16, '0');
            Alarms = dataAlarm.Substring(0, 10);
            foreach (char item in Alarms)
            {
                if (item == '1')
                    return false;
            }
            List<byte> warnings = TranBase.MyI2C_ReadA2HByte(SerBuf, Port, 116, 2);
            string dataWarning = (Convert.ToString((ushort)(warnings[0] * 256 + alarms[1]), 2)).PadLeft(16, '0');
            Warnings = dataWarning.Substring(0, 10);
            foreach (char item in Warnings)
            {
                if (item == '1')
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 通过I2C获取温度，Vcc,Bias,TxPower
        /// </summary>
        void GetParas()
        {
            //AOH 读取SN
            //A2H
            double temp, vcc, txPower, bais;
            short cache = 0;
            ushort ucache = 0;
            List<byte> data = TranBase.MyI2C_ReadA2HByte(SerBuf, Port, 96, 10);
            //Temp 96，97
            cache = DigitTransform(data[0], data[1]);
            temp = (double)cache / 256;
            Temp = temp;
            TestingPara.Temp = Temp.ToString();
            //Vcc 98，99
            ucache = UDigitTransform(data[2], data[3]);
            vcc = (double)ucache / 10000; //V
            Vcc = vcc;
            TestingPara.Vcc = Vcc.ToString();

            //Bais 100,101
            ucache = UDigitTransform(data[4], data[5]);
            bais = (double)ucache / 500;
            Bias = bais;
            TestingPara.Bias = Bias.ToString();
            //TxPower 102,103
            ucache = UDigitTransform(data[6], data[7]);
            txPower = (double)ucache / 10000; //mW
            //取两位有效数字
            TxPower = Math.Round((Math.Log10(txPower) * 10), 2);
            TestingPara.TxPower = TxPower.ToString();
        }
        //I2C获取RxPower
        double GetRxPower()
        {
            if (Port == null || Port.IsOpen == false)
            {
                MessageBox.Show("请先初始化COM口", "系统提示");
                return 0;
            }
            else
            {
                ushort ucache;
                double rxPower;
                List<byte> data = new List<byte>();
                data = TranBase.MyI2C_ReadA2HByte(SerBuf, Port, 104, 2);
                ucache = UDigitTransform(data[0], data[1]);
                rxPower = (double)ucache / 10000;
                return Math.Round((Math.Log10(rxPower) * 10), 2);

            }

        }
        /// <summary>
        /// 获取short数据
        /// </summary>
        /// <param name="msb">高八位</param>
        /// <param name="lsb">低八位</param>
        /// <returns></returns>
        short DigitTransform(byte msb, byte lsb)
        {
            short num = (short)((short)msb * 256 + (short)lsb);
            return num;
        }
        /// <summary>
        /// 获取ushort 数据
        /// </summary>
        /// <param name="msb">高八位</param>
        /// <param name="lsb">低八位</param>
        /// <returns></returns>
        ushort UDigitTransform(byte msb, byte lsb)
        {
            ushort num = (ushort)((ushort)msb * 256 + (ushort)lsb);
            return num;
        }

        #endregion
        //GPIB通信
        #region TxMethods
        /// <summary>
        /// 获取ExtioRatio和Crossing
        /// </summary>
        void GetTxParas()
        {
            //等待AutoScale完成
            ExtiRatio = Mp2100A.GetExRatio();
            CrossingRate = Mp2100A.GetCrossing();
            Jitter = Mp2100A.GetJitter();
            MaskMargin = Mp2100A.GetMaskMargin();
        }
        #endregion TxMethods
        #region RxMethods
        /// <summary>
        /// 计算灵敏度
        /// </summary>
        double GetSensitivity()
        {
            StringBuilder sb = new StringBuilder();
            List<Point> listPoint = new List<Point>();
            try
            {

                //测量数量 衰减初始值 步进
                int testCount = 12;
                //灵敏度
                double initialValue = IniAtt - 2;
                double interval = 0.4;

                Point point = new Point();
                for (int i = 0; i < testCount; i++)
                {
                    Hp8156A.SetAtt(initialValue.ToString());
                    point.X = initialValue;
                    initialValue += interval;
                    //sb.Append(point.X.ToString());
                    Thread.Sleep(1000);
                    double result = Mp2100A.GetErrorRate();
                    //Error rate
                    if (result >= 0)
                    {
                        double s = Math.Log10(result);
                        //判定当结果不为0时
                        if (s > -9)
                        {
                            //取对数
                            point.Y = s;
                            listPoint.Add(point);
                        }
                    }


                }
                //噪声过滤
                while (IsCountChanged && listPoint.Count > 5)
                {
                    listPoint = PointFilter(listPoint);
                }

                //计算灵敏度
                SenPara = this.LinearRegression(listPoint);
                //灵敏度计算条件：Error rate@E-3
                SenPara.Sensitive = (ErrorRateInSensitivity - SenPara.RCA) / SenPara.RCB;
                return SenPara.Sensitive;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 灵敏度噪声过滤
        /// </summary>
        /// <param name="listPoint"></param>
        /// <returns></returns>
        public List<Point> PointFilter(List<Point> listPoint)
        {
            IsCountChanged = false;
            double[] slope = new double[listPoint.Count - 1];
            for (int i = 0; i < listPoint.Count - 1; i++)
            {
                slope[i] = (listPoint[i + 1].Y - listPoint[i].Y) / (listPoint[i + 1].X - listPoint[i].X);
            }
            double max = slope.Max();
            int indexMax = Array.IndexOf(slope, max);

            double min = slope.Min();
            int indexMin = Array.IndexOf(slope, min);
            if (Math.Abs(indexMax - indexMin) == 1)
            {
                listPoint.RemoveAt(Math.Max(indexMin, indexMax));
                IsCountChanged = true;
            }
            if (indexMax == 0 || indexMin == 0)
            {
                listPoint.RemoveAt(0);
                IsCountChanged = true;
            }
            if (indexMax == slope.Length - 1 || indexMin == slope.Length - 1)
            {
                listPoint.RemoveAt(listPoint.Count - 1);
                IsCountChanged = true;
            }
            return listPoint;
        }
        /// <summary>
        /// 线性拟合(根据point 返回灵敏度相应参数)
        /// </summary>
        /// <param name="parray"></param>
        private SenParas LinearRegression(List<Point> parray)
        {
            SenParas para = new SenParas();
            if (parray.Count < 2)
            {
                System.Console.WriteLine("点个数不能少于2");
                return null;
            }
            //求 X,Y平均值
            double averagex = 0; double averagey = 0;
            foreach (Point item in parray)
            {
                averagex += item.X;
                averagey += item.Y;
            }
            averagex /= parray.Count;
            averagey /= parray.Count;
            //经验回归系数的分子与分母
            double numerator = 0;
            double denominator = 0;
            foreach (Point p in parray)
            {
                numerator += (p.X - averagex) * (p.Y - averagey);
                denominator += (p.X - averagex) * (p.X - averagex);
            }
            //回归系数b（Regression Coefficient）
            para.RCB = numerator / denominator;
            //回归系数a
            para.RCA = averagey - para.RCB * averagex;
            //剩余平方和与回归平方和
            double residualSS = 0;  //（Residual Sum of Squares）
            double regressionSS = 0; //（Regression Sum of Squares）
            foreach (Point p in parray)
            {
                residualSS +=
                  (p.Y - para.RCA - para.RCB * p.X) *
                  (p.Y - para.RCA - para.RCB * p.X);
                regressionSS +=
                  (para.RCA + para.RCB * p.X - averagey) *
                  (para.RCA + para.RCB * p.X - averagey);
            }
            //计算R^2的值
            numerator = 0; denominator = 0;
            foreach (var item in parray)
            {
                denominator += Math.Pow((item.Y - averagey), 2.0);
                numerator += Math.Pow((item.Y - (item.X * para.RCB + para.RCA)), 2.0);
            }
            para.ResidualSS = residualSS;
            para.RegressionSS = regressionSS;
            para.RSquare = 1 - numerator / denominator;
            //拟合误差越接近1则表示越准确
            return para;
        }
        #endregion RxMethods
        #endregion Methods
    }
}
