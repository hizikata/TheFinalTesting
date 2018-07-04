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

namespace TheFinalTesting.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// 指示设备等的初始化是否完成
        /// </summary>
        private bool IsReady = false;
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
        #endregion
        #region Properties
        /// <summary>
        /// 窗口列表
        /// </summary>
        public string[] StrCom { get { return _strCom; } }
        private readonly string[] _strCom;
        public string SelectedCom { get; set; }
        /// <summary>
        /// 待初始化设备展示
        /// </summary>
        public DeviceInfo[] Devices { get; } = new DeviceInfo[]
        {
            new DeviceInfo(true,"Aglient34401A",10,"高低压万用表(地址:10)"),
            new DeviceInfo(true,"AglientE3631A",9,"基板电源供应器(地址:9)"),
            new DeviceInfo(true,"HP8153A",5,"光功率计(地址:5)"),
            new DeviceInfo(true,"HP8156A",22,"光衰减器(地址:22)"),
            new DeviceInfo(true,"MP2100A",16,"眼图仪(地址:2)"),
            new DeviceInfo(true,"PST3202",15,"电源供应器(地址:15)"),
            new DeviceInfo(true,"AQ6317B",2,"频谱分析仪(地址未知)")

        };
        /// <summary>
        /// 设备初始化信息展示
        /// </summary>
        public String InitializationInfo
        {
            get { return _initializationInfo; }
            set
            {
                _initializationInfo = value;
                RaisePropertyChanged(() => InitializationInfo);
            }
        }
        private String _initializationInfo;
        #region TX Parameters

        private string _supplyCurrent;

        /// <summary>
        /// E3631 供应电流
        /// </summary>
        public string SupplyCurrent
        {
            get { return _supplyCurrent; }
            set { _supplyCurrent = value; RaisePropertyChanged(() => SupplyCurrent); }
        }

        private string _outputPower;
        /// <summary>
        /// 8153A 光功率
        /// </summary>
        public string OutputPower
        {
            get { return _outputPower; }
            set { _outputPower = value; RaisePropertyChanged(() => OutputPower); }
        }

        private string _extiRatio;
        /// <summary>
        /// MP2100A
        /// </summary>
        public string ExtiRatio
        {
            get { return _extiRatio; }
            set { _extiRatio = value; RaisePropertyChanged(() => ExtiRatio); }
        }
        private string _crossingRate;
        /// <summary>
        /// MP2100A
        /// </summary>
        public string CrossingRate
        {
            get { return _crossingRate; }
            set { _crossingRate = value; RaisePropertyChanged(() => CrossingRate); }
        }
        private string _centerWavelength;
        /// <summary>
        /// 中心波长
        /// </summary>
        public string CenterWavelength
        {
            get { return _centerWavelength; }
            set { _centerWavelength = value; RaisePropertyChanged(() => CenterWavelength); }
        }
        private string _differenceWavelength;
        /// <summary>
        /// 频谱仪 波长差
        /// </summary>
        public string DifferenceWavelength
        {
            get { return _differenceWavelength; }
            set { _differenceWavelength = value; RaisePropertyChanged(() => DifferenceWavelength); }
        }

        private string _SMSR;

        public string SMSR
        {
            get { return _SMSR; }
            set { _SMSR = value; RaisePropertyChanged(() => SMSR); }
        }
        private string _jitter;
        /// <summary>
        /// MP2100A 
        /// </summary>
        public string Jitter
        {
            get { return _jitter; }
            set { _jitter = value; RaisePropertyChanged(() => Jitter); }
        }
        private string _riseingTime;

        public string RisingTime
        {
            get { return _riseingTime; }
            set { _riseingTime = value; RaisePropertyChanged(() => RisingTime); }
        }
        private string _fallingTime;

        public string FallingTime
        {
            get { return _fallingTime; }
            set { _fallingTime = value; RaisePropertyChanged(() => FallingTime); }
        }

        private string _maskMargin;

        public string MaskMargin
        {
            get { return _maskMargin; }
            set { _maskMargin = value; RaisePropertyChanged(() => MaskMargin); }
        }

        #endregion
        #region Rx Parameters
        private string _sensitivity;

        public string Sensitivity
        {
            get { return _sensitivity; }
            set { _sensitivity = value; RaisePropertyChanged(() => Sensitivity); }
        }
        private string _sdAsserted;

        public string SdAsserted
        {
            get { return _sdAsserted; }
            set { _sdAsserted = value; RaisePropertyChanged(() => SdAsserted); }
        }
        private string _sdDesserted;

        public string SdDesserted
        {
            get { return _sdDesserted; }
            set { _sdDesserted = value; RaisePropertyChanged(() => SdDesserted); }
        }
        private string _sdHigh;

        public string SDHigh
        {
            get { return _sdHigh; }
            set { _sdHigh = value; RaisePropertyChanged(() => SDHigh); }
        }
        private string _sdLow;

        public string SDLow
        {
            get { return _sdLow; }
            set { _sdLow = value; RaisePropertyChanged(() => SDLow); }
        }

        #endregion
        #region DDMI/A0H Parameters
        private string _rxPoint1;

        public string RxPoint1
        {
            get { return _rxPoint1; }
            set { _rxPoint1 = value; RaisePropertyChanged(() => RxPoint1); }
        }
        private string _rxPoint2;

        public string RxPoint2
        {
            get { return _rxPoint2; }
            set { _rxPoint2 = value; RaisePropertyChanged(() => RxPoint2); }
        }
        private string _rxPoint3;

        public string RxPoint3
        {
            get { return _rxPoint3; }
            set { _rxPoint3 = value; RaisePropertyChanged(() => RxPoint3); }
        }
        private string _txPower;

        public string TxPower
        {
            get { return _txPower; }
            set { _txPower = value; RaisePropertyChanged(() => TxPower); }
        }
        private string _vcc;

        public string Vcc
        {
            get { return _vcc; }
            set { _vcc = value; RaisePropertyChanged(() => Vcc); }
        }
        private string _temp;

        public string Temp
        {
            get { return _temp; }
            set { _temp = value; RaisePropertyChanged(() => Temp); }
        }
        private string _bias;

        public string Bias
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
        #endregion
        #region CommandMethods
        private void ExecuteInitialize()
        {
            StringBuilder info = new StringBuilder();
            try
            {
                //串口初始化
                if (!string.IsNullOrEmpty(SelectedCom.Trim()))
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
                        InitializationInfo = info.ToString();
                    }
                    else
                    {
                        info.Append("初始化成功 \r\n");
                        info.Append("msg");
                        InitializationInfo = info.ToString();
                    }
                }
                else
                {
                    info.Append("请选择串口号！\r\n");
                    InitializationInfo = info.ToString();
                    return;
                }
                //设备初始化
                foreach (var item in Devices)
                {
                    if (item.IsSelected == true)
                    {
                        if (item.Address != 0)
                        {
                            string address = item.Address.ToString();
                            switch (Array.IndexOf(Devices, item))
                            {
                                case 0:
                                    info.Append("Aglient34401A初始化中。。。");
                                    if (Ag34401A != null)
                                        Ag34401A.Dispose();
                                    Ag34401A = new Aglient34401A(address);
                                    if (Ag34401A != null)
                                        info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

                                    break;
                                case 1:
                                    info.Append("AglientE3631A初始化中。。。");
                                    if (AgE3631A != null)
                                        AgE3631A.Dispose();
                                    AgE3631A = new AglientE3631A(address);
                                    if (AgE3631A != null)
                                        info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

                                    break;
                                case 2:
                                    info.Append("Hp8153A初始化中。。。");
                                    if (Hp8153A != null)
                                        Hp8153A.Dispose();
                                    Hp8153A = new HP8153A(address);
                                    info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

                                    break;
                                case 3:
                                    info.Append("Hp8156A初始化中。。。");
                                    if (Hp8156A != null)
                                        Hp8156A.Dispose();
                                    Hp8156A = new HP8156A(address);
                                    info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

                                    break;
                                case 4:
                                    info.Append("MP2100A初始化中。。。");
                                    if (Mp2100A != null)
                                        Mp2100A.Dispose();
                                    Mp2100A = new MP2100A(address);
                                    //Mp2100A = new MP2100A("TCPIP::192.168.100.101::5001::SOCKET", ConnectionType.Ethernet);

                                    info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

                                    break;
                                case 5:
                                    info.Append("P3202初始化中。。。");
                                    if (P3202 != null)
                                        P3202.Dispose();
                                    P3202 = new PST3202(address);
                                    info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

                                    break;
                                case 6:
                                    info.Append("AQ6317B初始化中。。。");
                                    if (Aq6317B != null)
                                        Aq6317B.Dispose();
                                    Aq6317B = new AQ6317B(address);
                                    info.Append("初始化OK \r\n");
                                    InitializationInfo = info.ToString();

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
            try
            {
                if (IsReady)
                {
                    //Initialize
                    AgE3631A.Open();
                    P3202.Open();
                    Thread.Sleep(200);
                    Mp2100A.AutoScale();
                    //Asserted,Desserted sdlow sdhigh
                    GetHighAndLow();
                    Thread.Sleep(200);
                    //sensitivity
                    GetSensitivity();
                    Hp8156A.SetAtt("28");
                    //ExtiRatois Crossing
                    GetTxParas();
                    //Sensitivity
                    //Temp,Vcc,Bias,TxPower,RxPoint 1/2/3
                    I2CTest();

                }
                else
                {
                    MessageBox.Show("初始化未完成", "系统提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
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
            P3202.Open();
            Thread.Sleep(200);
            Hp8156A.SetAtt("28");
            Thread.Sleep(1000);
            //Temp，Vcc,Bias,TxPower
            this.GetParas();
            Thread.Sleep(1000);
            //RxPoint 1
            Hp8156A.SetAtt("10");
            Thread.Sleep(1000);
            RxPoint1 = GetRxPower().ToString();
            //RxPoint 2
            Hp8156A.SetAtt("19");
            Thread.Sleep(1000);
            RxPoint2 = GetRxPower().ToString();
            //RxPoint 3
            Hp8156A.SetAtt("28");
            Thread.Sleep(1000);

            RxPoint3 = GetRxPower().ToString();
        }
        /// <summary>
        /// 通过I2C获取温度，Vcc,Bias,TxPower
        /// </summary>
        void GetParas()
        {
            if (Port == null || Port.IsOpen == false)
            {
                MessageBox.Show("请先初始化COM口", "系统提示");
            }
            else
            {
                //AOH 读取SN
                string sn;

                //A2H
                double temp, vcc, txPower, bais;
                short cache = 0;
                ushort ucache = 0;
                List<byte> data = TranBase.MyI2C_ReadA2HByte(SerBuf, Port, 96, 10);
                //温度计算 96，97
                cache = DigitTransform(data[0], data[1]);
                temp = (double)cache / 256;
                Temp = (temp).ToString();
                //Vcc 98，99
                ucache = UDigitTransform(data[2], data[3]);
                vcc = (double)ucache / 10000; //V
                Vcc = vcc.ToString();

                //Bais 100,101
                ucache = UDigitTransform(data[4], data[5]);
                bais = (double)ucache / 500;
                Bias = bais.ToString();
                //TxPower 102,103
                ucache = UDigitTransform(data[6], data[7]);
                txPower = (double)ucache / 10000; //mW
                TxPower = (Math.Log10(txPower) * 10).ToString();
                //RxPower 104,105
                //ucache = UDigitTransform(data[8], data[9]);
                //rxPower = (double)ucache / 10000; //mW
                //RxPower = (float)(Math.Log10(rxPower) * 10);
            }
        }
        //I2C获取RxPower
        float GetRxPower()
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
                return (float)(Math.Log10(rxPower) * 10);

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
            ExtiRatio = Mp2100A.GetER();
            CrossingRate = Mp2100A.GetCrossing();
            Jitter = Mp2100A.GetJitter();
            MaskMargin = Mp2100A.GetMaskMargin();
        }
        #endregion TxMethods
        #region RxMethods
        /// <summary>
        /// 获取Asserted/Desserted等
        /// </summary>
        bool GetHighAndLow()
        {
            double att = 28.5;
            double span = 0.2;
            double volage = 0;

            for (int i = 0; i < 40; i++)
            {
                Hp8156A.SetAtt(att.ToString());
                Thread.Sleep(200);
                volage = Convert.ToDouble(Ag34401A.GetVoltage().Trim());
                if (volage > 3.0)
                {
                    SdDesserted = att.ToString();
                    SDHigh = volage.ToString();
                    break;
                }
                att += 0.2;
            }
            for (int i = 0; i < 20; i++)
            {
                Hp8156A.SetAtt(att.ToString());
                Thread.Sleep(200);
                volage = Convert.ToDouble(Ag34401A.GetVoltage().Trim());
                if (volage < 0.5)
                {
                    SdAsserted = att.ToString();
                    SDLow = volage.ToString();
                    break;
                }
                att -= 0.2;
            }
            if (!string.IsNullOrEmpty(SdAsserted) && !string.IsNullOrEmpty(SdDesserted))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 计算灵敏度
        /// </summary>
        void GetSensitivity()
        {
            StringBuilder sb = new StringBuilder();
            List<Point> listPoint = new List<Point>();
            try
            {
                if (Hp8156A != null && Mp2100A != null)
                {
                    //测量数量 衰减初始值 步进
                    int testCount = 10;
                    double initialValue = 26;
                    double interval = 0.5;

                    Point point = new Point();
                    for (int i = 0; i < testCount; i++)
                    {
                        Hp8156A.Open();
                        Hp8156A.SetAtt(initialValue.ToString());
                        point.X = initialValue;
                        initialValue += interval;
                        //sb.Append(point.X.ToString());
                        Thread.Sleep(1000);
                        string errorRate = Mp2100A.GetErrorRate();
                        string str = errorRate.Substring(2);
                        //Error rate
                        double.TryParse(str.Trim(), out double result);
                        //判定当结果不为0时
                        if (result > 0)
                        {
                            //取对数
                            point.Y = Math.Log10(result);
                            listPoint.Add(point);
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
                    SenPara.Sensitive = (-3 - SenPara.RCA) / SenPara.RCB;
                    Sensitivity = SenPara.Sensitive.ToString();

                }
            }
            catch
            {

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
