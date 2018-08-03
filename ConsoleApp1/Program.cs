//#define FullTest
#define SimpleTest
//#define SerialPortTest
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp1
{
    class Program
    {
#if SimpleTest
        static void Main(string[] args)
        {
            try
            {
                string a = "0.0E+000";
                bool flag = double.TryParse(a, out double result);
                double test = 0.0E+00;
                double b = Math.Log10(result);
                if (test == 0)
                {
                    Console.WriteLine("转换后值为0");
                }
                else if (test == 1)
                {
                    Console.WriteLine("转换后值为1");
                }
                Console.WriteLine(flag);
                Console.WriteLine(b);
                Console.ReadKey();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                Console.ReadKey();
            }

        }
#endif
#if SerialPortTest
        static SerialPort _serialPort;
        static bool _continue;
        public static void Main()
        {
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            //_serialPort.PortName = SetPortName(_serialPort.PortName);
            //_serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            //_serialPort.Parity = SetPortParity(_serialPort.Parity);
            //_serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            //_serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            //_serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.Write("Name: ");
            name = Console.ReadLine();

            Console.WriteLine("Type QUIT to exit");

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format("<{0}>: {1}", name, message));
                }
            }

            readThread.Join();
            _serialPort.Close();
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }
#endif

#if FullTest
        static SenParas SenPara;
        static bool IsCountChanged = false;
        //static string Sensitivity;
        /// <summary>
        /// 眼图仪
        /// </summary>
        static MP2100A Mp;
        /// <summary>
        /// Keithley
        /// </summary>
        static Keithley Keith;
        static HP8153A Hp8153;
        /// <summary>
        /// 衰减器
        /// </summary>
        static HP8156A Hp8156;
        /// <summary>
        /// 电源供应器
        /// </summary>
        static AglientE3631A AgE3631;
        /// <summary>
        /// 高低压万用表
        /// </summary>
        static Aglient34401A Ag34401;
        static void Main(string[] args)
        {
            string supplyCurrent;
            string outputPower;
            double voltage = 0;
            double ini = 28.0;
            double span = 0.2;
            double sd_Desserted = 0, sd_Asserted = 0, hysis = 0;
            string crossing;
            string extiRatio;
            string jitter;
            string maskMargin;
            string sensitivity;
            bool saturation;
            bool txDisable;
            try
            {
                AgE3631 = new AglientE3631A("9");
                Keith = new Keithley("24");
                Hp8156 = new HP8156A("22");
                Mp = new MP2100A("16");
                Hp8153 = new HP8153A("5");

                Console.WriteLine(Keith.GetIdn());
                Console.WriteLine(Hp8156.GetIdn());
                Console.WriteLine(Mp.GetIdn());
                Console.WriteLine(Hp8153.GetIdn());
                Console.WriteLine(AgE3631.GetIdn());

                AgE3631.Open();
                Hp8156.Open();
                supplyCurrent = AgE3631.GetCurrent();
                outputPower = Hp8153.ReadPower("2");
                Console.WriteLine("Supply Current:{0}", supplyCurrent);
                Console.WriteLine("Output Power:{0}", outputPower);
                Console.WriteLine();

                Mp.AutoScale();
                Keith.SetMeasureVoltageOnlyPara("10");
                Keith.Open();
                //SD_Desserted, SD_High
                for (int i = 0; i <= 40; i++)
                {
                    Hp8156.SetAtt(ini.ToString());
                    Thread.Sleep(300);
                    voltage = GetKeithData();
                    Thread.Sleep(200);
                    if (voltage >= 2.0)
                    {
                        sd_Desserted = ini;
                        break;
                    }
                    else
                    {
                        ini += span;
                    }
                }
                Console.WriteLine("SDHigh:{0}", voltage);
                Console.WriteLine("SdDesserted:{0}", sd_Desserted);
                Console.WriteLine();

                //SD_Asserted,SD_Low
                for (int i = 0; i < 40; i++)
                {
                    Hp8156.SetAtt(ini.ToString());
                    Thread.Sleep(300);
                    voltage = GetKeithData();
                    Thread.Sleep(300);
                    if (voltage <= 0.5)
                    {
                        sd_Asserted = ini;
                        break;
                    }
                    else
                    {
                        ini -= 0.1;
                    }
                }
                Console.WriteLine("SdLow:{0}", voltage);
                Console.WriteLine("SdAsserted:{0}", sd_Asserted);
                hysis = sd_Desserted - sd_Asserted;
                Console.WriteLine("Hys:{0}", hysis);
                Console.WriteLine();



                Hp8156.SetAtt("28");
                Thread.Sleep(2000);
        #region Mp2100
                crossing = Mp.GetCrossing();
                extiRatio = Mp.GetER();


                Console.WriteLine("Crossing:{0}", crossing);
                Console.WriteLine("Extinction Ratio:{0}", extiRatio);
                jitter = Mp.GetJitter();
                maskMargin = Mp.GetMaskMargin();
                Console.WriteLine("Jitter:{0}", jitter);
                Console.WriteLine("Mask Margin:{0}", maskMargin);
                Console.WriteLine();
                //Sensitivity
                GetSensitivity();
                sensitivity = SenPara.Sensitive.ToString();
                Console.WriteLine("Sensitivity:{0}", sensitivity);
        #endregion

                //Saturation
                Hp8156.SetAtt("9");
                Thread.Sleep(2000);
                Console.WriteLine();
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Saturation:{0}", Mp.GetErrorRate());
                    Thread.Sleep(1000);
                }
                Hp8156.Close();
                Hp8156.SetAtt("28");
                AgE3631.SetOutput("25", "2.0", "0.5");
                Thread.Sleep(1000);
                Console.WriteLine("TxDisable:{0}", Hp8153.ReadPower("2"));

                Console.WriteLine("测试完成");
                Console.ReadKey();
                Keith.Close();
                AgE3631.Close();
                Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
        static double GetKeithData()
        {
            string data = Keith.ReadData();
            string[] paras = data.Split(',');
            if (double.TryParse(paras[0], out double voltage))
            {
                return voltage;
            }
            else
            {
                return 0.00;
            }
        }
        static void GetSensitivity()
        {
            StringBuilder sb = new StringBuilder();
            List<Point> listPoint = new List<Point>();
            try
            {

                //测量数量 衰减初始值 步进
                int testCount = 10;
                double initialValue = 26;
                double interval = 0.5;

                Point point = new Point();
                for (int i = 0; i < testCount; i++)
                {
                    //Hp8156.Open();
                    Hp8156.SetAtt(initialValue.ToString());
                    point.X = initialValue;
                    initialValue += interval;
                    //sb.Append(point.X.ToString());
                    Thread.Sleep(1000);
                    string errorRate = Mp.GetErrorRate();
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
                    Console.WriteLine("X:{0},Y:{1}", point.X, point.Y);
                }
                Console.WriteLine();
                //噪声过滤
                while (IsCountChanged && listPoint.Count > 5)
                {
                    listPoint = PointFilter(listPoint);
                }
                Console.WriteLine();
                foreach (var item in listPoint)
                {
                    Console.WriteLine("X:{0},Y:{1}", item.X, item.Y);
                }
                //计算灵敏度
                SenPara = LinearRegression(listPoint);
                //灵敏度计算条件：Error rate@E-3
                SenPara.Sensitive = (-3 - SenPara.RCA) / SenPara.RCB;
            }
            catch
            {

            }
        }
        static SenParas LinearRegression(List<Point> parray)
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
        static List<Point> PointFilter(List<Point> listPoint)
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
        static void Dispose()
        {
            if (Hp8156 != null)
                Hp8156.Dispose();
            if (Keith != null)
                Keith.Dispose();
            if (Mp != null)
                Mp.Dispose();
            if (Hp8153 != null)
                Hp8153.Dispose();
            if (AgE3631 != null)
                AgE3631.Dispose();
        }
#endif
    }
}
