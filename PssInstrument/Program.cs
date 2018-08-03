#define UseDll
#define DOA
#define OPM
#define BERT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using XuxzLib.Communication;
using System.Threading;

namespace PssInstrument
{
    class Program
    {
        static SerialPort Port = new SerialPort();

#if UseDll
        static void Main()
        {
            byte[] SerBuf = new byte[50];
            uint state = 0;
            double power = 0;
            //模块温度
            double temp= 0;
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
            //Bert
            PssBert.BertWRRegist(ptrTx, ptrRx);
            //串口初始化
            Rs232LinkInitial("COM9", 115200);
            Thread.Sleep(100);

#if OPM
            //OPM
            PssOPM.OPMReadIDN(PssBase.CARDID_10, PssBase.ENDSIGN_1, SerBuf);
            string opmidn = Encoding.ASCII.GetString(SerBuf);
            Console.WriteLine("光功率计IDN:{0}", opmidn);
            PssOPM.OPMReadPower(PssBase.CARDID_10, PssBase.ENDSIGN_1, PssOPM.OPM_CHANNEL_1, ref power);
            Console.WriteLine("校准前当前功率:{0}", power);

#endif
#if DOA
            //获取Dll 名称/版本
            byte[] name = new byte[100];
            byte[] version = new byte[100];
            PssDOA.DOAGetDllInfo(name, version);
            string n = Encoding.ASCII.GetString(name);
            string v = Encoding.ASCII.GetString(version);
            Console.WriteLine("当前dll Name:{0}", n);
            Console.WriteLine("当前dll Version:{0}", v);
            //获取IDN
            state = PssDOA.DOAReadIDN(PssBase.CARDID_3, PssBase.ENDSIGN_1, SerBuf);
            string idnmsg = Encoding.ASCII.GetString(SerBuf);
            Console.WriteLine("光衰减模块IDN:{0}", idnmsg);


            //cal
            //double cal = 3.16;
            //PssDOA.DOAConfCalibration(PssBase.CARDID_3, PssBase.ENDSIGN_1, cal);
            //PssDOA.DOAReadCalibration(PssBase.CARDID_3, PssBase.ENDSIGN_1, ref cal);
            //Console.WriteLine("更改后cal:{0}", cal);
            //波长
            uint wavelength = PssBase.WAVELENGTH_1550NM;
            PssDOA.DOAConfWavelength(PssDOA.CARDID_3, PssBase.ENDSIGN_1, wavelength);
            PssDOA.DOAReadWavelength(PssBase.CARDID_3, PssBase.ENDSIGN_1, ref wavelength);
            Console.WriteLine("波长：{0}", wavelength);
            //获取当前Atten
            double atten = 0;
            PssDOA.DOAReadAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, ref atten);
            Console.WriteLine("更改前衰减:{0}", atten);
            atten = -28; //衰减只能设置为负数
            state = PssDOA.DOAConfAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, atten);
            Thread.Sleep(300);
            if (state == 0)
                Console.WriteLine("att设置成功");
            else
            {
                Console.WriteLine("att设置失败");
            }
            state = PssDOA.DOAReadAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, ref atten);
            if (state == 0)
            {
                Console.WriteLine("更改后衰减:{0}", atten);
            }
            else
            {
                Console.WriteLine("查询Att失败");
            }
            PssOPM.OPMReadPower(PssBase.CARDID_10, PssBase.ENDSIGN_1, PssOPM.OPM_CHANNEL_1, ref power);
            Console.WriteLine("校准后当前功率:{0}", power);


            state = PssDOA.ReadDDM_Temperature(PssBase.CARDID_3, PssBase.ENDSIGN_1, 0XA3,ref  temp);
            if (state != 0)
            {
                Console.WriteLine("查询温度失败");
            }
            else
            {
                Console.WriteLine("temp:{0}", temp);
            }
            //查询SN  12位
            uint count = 0xC;
            state = PssDOA.ReadDDM(PssBase.CARDID_3, PssBase.ENDSIGN_1, 0xA1, 68, count, SerBuf);
            if (state != 0)
            {
                Console.WriteLine("查询SN失败");
            }
            else
            {
                string sn = Encoding.ASCII.GetString(SerBuf);
                sn = sn.Substring(0, 35);
                Console.WriteLine(sn);

                string[]datas = sn.Split(' ');


                byte[] byteSn = new byte[count];
                for (int i = 0; i < count; i++)
                {
                    byteSn[i] = Convert.ToByte(datas[i].Trim(),16); 
                }
                sn=(Encoding.ASCII.GetString(byteSn));
                Console.WriteLine(sn);

            }
#endif


#if BERT
            //Bert
            state = PssBert.BertIDNGet(PssBase.CARDID_2, PssBase.ENDSIGN_1, SerBuf);
            if (state != 0)
            {
                Console.WriteLine("Error:{0},获取IDN失败", state);
                return;
            }

            else
            {
                string bertIdn = Encoding.ASCII.GetString(SerBuf);
                Console.WriteLine("IDN of BERT:{0}", bertIdn);
            }
            Console.WriteLine("正在配置误码仪...");
            //Bert 配置


            //码型
            uint patter = PssBert.STYLE_PRB31;
            //幅值
            uint level = PssBert.LEVEL_800;
            //速率
            uint speed = PssBert.RATE_9G95;
            //时间0连续  非0 设备运行时间段 在时间到达后会自动关闭？
            uint time = 0;

            //PPG
            uint cardPPG = PssBase.CARDID_2;
            //PSS-BERT-O 1通道为光光通道 2 为光电通道。指的是两个不同的型号，这边选择光电通道
            uint channelPPG = PssBert.CHANAL_2;

            //4PPG
            uint card4PPG = PssBase.CARDID_1;
            uint channel4PPG = PssBert.CHANAL_1;


            //PPG-15G-4
            //4PPG配置
            //轮询配置4通道
            Console.WriteLine("开始配置4PPG");
            for (uint i = 0; i < 4; i++)
            {
                //patter set/get
                PssBert.BertPatterSet(card4PPG, PssBase.ENDSIGN_1, i, patter);
                Thread.Sleep(200);
                PssBert.BertPatterGet(card4PPG, PssBase.ENDSIGN_1, i, ref patter);
                Thread.Sleep(200);
                Console.WriteLine("patter:{0}", patter);
                //level set/get
                PssBert.BertLevelSet(card4PPG, PssBase.ENDSIGN_1, i, level);
                Thread.Sleep(200);
                PssBert.BertLevelGet(card4PPG, PssBase.ENDSIGN_1, i, ref level);
                Thread.Sleep(200);
                Console.WriteLine("level：{0}", level);
            }
            //速率
            PssBert.BertSpeedSet(card4PPG, PssBase.ENDSIGN_1, channel4PPG, speed);
            Thread.Sleep(4000);
            PssBert.BertSpeedGet(card4PPG, PssBase.ENDSIGN_1, channel4PPG, ref speed);
            Thread.Sleep(200);
            Console.WriteLine("speed:{0}", speed);
            //4通道轮询配置PG
            for (uint i = 0; i < 4; i++)
            {
                PssBert.BertPGStart(card4PPG, PssBase.ENDSIGN_1, i);
                Thread.Sleep(200);
            }


            Console.WriteLine("开始配置BERT15G");
            //配置BERT15G-O
            //patter set/get
            state=PssBert.BertPatterSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, patter);
            Thread.Sleep(200);
            patter = 0;
            state = PssBert.BertPatterGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref patter);
            Thread.Sleep(200);
            Console.WriteLine("Patter:{0}", patter);
            //level set/get
            state = PssBert.BertLevelSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, level);
            Thread.Sleep(200);
            level = 0;
            state = PssBert.BertLevelGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref level);
            Thread.Sleep(200);
            Console.WriteLine("level：{0}", level);
            //speed set/get
            state = PssBert.BertSpeedSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, speed);
            Thread.Sleep(4000);
            speed = 0;
            state = PssBert.BertSpeedGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref speed);
            Thread.Sleep(200);
            Console.WriteLine("speed:{0}", speed);
            //time set/get
            state = PssBert.BertTimeSet(cardPPG, PssBase.ENDSIGN_1, channelPPG, time);
            Thread.Sleep(200);
            time = 0;
            state =PssBert.BertTimeGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref time);
            Thread.Sleep(200);
            Console.WriteLine("time:{0}", time);

           

            //PPG、ED配置
            Console.WriteLine("开始配置PPG,ED Start");
            PssBert.BertPGStart(cardPPG, PssBase.ENDSIGN_1, channelPPG);
            Thread.Sleep(500);
            PssBert.BertEDStart(cardPPG, PssBase.ENDSIGN_1, channelPPG);
            Thread.Sleep(500);


            //光衰改变了后，立即要调用clear 函数  在设置延时  例如Thread(1000)  然后读取误码率。

            




            //double readAtten = 0;

            //读取误码
            uint syncState = 0, errorState = 0;
            double all = 0, errorCount = 0, ber = 0;



            for (int i = 0; i < 3; i++)
            {
                //state = PssDOA.DOAConfAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, atten);
                //Thread.Sleep(300);
                //atten -= 0.5;

                PssBert.BertClr(cardPPG, PssBase.ENDSIGN_1, channelPPG);
                Thread.Sleep(3000);

                state = PssBert.BertResult(cardPPG, PssBase.ENDSIGN_1, channelPPG, out syncState, out errorState, out errorCount, out all, out ber);
                Thread.Sleep(300);
                state = PssBert.BertResult(cardPPG, PssBase.ENDSIGN_1, channelPPG, out syncState, out errorState, out errorCount, out all, out ber);
                Thread.Sleep(3000);   
                if (state != 0)
                {
                    Console.WriteLine("获取误码失败:{0}", state);
                }
                else
                {
                    //state = PssDOA.DOAConfAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, -27.5);
                    //Thread.Sleep(300);
                    //state = PssDOA.DOAReadAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, ref readAtten);
                    //Console.WriteLine("Atten:{0}", readAtten);
                    Console.WriteLine("syncState:{0}", syncState);
                    Console.WriteLine("errorState:{0}", errorState);
                    Console.WriteLine("all:{0}", all);
                    Console.WriteLine("errorcount:{0}", errorCount);
                    Console.WriteLine("误码率:{0}", ber);
                }
            }

            //定时模式查询

            //uint searchTime = 0;

            //for (int i = 0; i < 10; i++)
            //{

            //    PssBert.BertCurrentTimeGet(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref searchTime);
            //    Thread.Sleep(200);
            //    //if (searchTime >= 10)
            //    //{
            //    state = PssDOA.DOAConfAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, atten);
            //    atten -= 0.5;
            //    Thread.Sleep(300);
            //    double attena = 0;
            //    PssDOA.DOAReadAtten(PssBase.CARDID_3, PssBase.ENDSIGN_1, ref attena);
            //    Thread.Sleep(300);
            //    Console.WriteLine("当前衰减:{0}", attena);

            //    Console.WriteLine("当前时间:{0}", searchTime);
            //    PssBert.BertClr(cardPPG, PssBase.ENDSIGN_1, channelPPG);
            //    Thread.Sleep(300);
            //    state = PssBert.BertResult(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref syncState, ref errorState, ref errorCount, ref all, ref ber);
            //    Thread.Sleep(5000);
            //    state = PssBert.BertResult(cardPPG, PssBase.ENDSIGN_1, channelPPG, ref syncState, ref errorState, ref errorCount, ref all, ref ber);
            //    if (state != 0)
            //    {
            //        Console.WriteLine("查询误码失败");
            //    }
            //    else
            //    {
            //        Console.WriteLine("syncState:{0}", syncState);
            //        Console.WriteLine("errorState:{0}", errorState);
            //        Console.WriteLine("all:{0}", all);
            //        Console.WriteLine("errorcount:{0}", errorCount);
            //        Console.WriteLine("误码率:{0}", ber);
            //    }
            //    //}
            //}




#endif
            //关闭串口
            PssRS232Driver.RS232Close();
            Console.ReadKey();
        }
        #region Methods
        public static uint Rs232LinkInitial(string comName, uint rate)
        {
            uint error = 0;
            //uint j = 0;
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

#endif
        //static void Main(string[] args)
        //{
        //    char[] buffer = new char[256];
        //    //int count = 0;
        //    try
        //    {
        //        SerialPortInitialization();
        //        Port.Write("Card3 *IDN?\n");
        //        //for (int i = 0; i < 37; i++)
        //        //{
        //        //    count=Port.Read(buffer, i, 1);
        //        //}
        //        ReadData(Port, buffer);//尝试使用SerialPort.ReadLine();
        //        Console.WriteLine("Press any key to exit");                
        //        Console.ReadKey();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //    }
        //    finally
        //    {
        //        Console.ReadKey();
        //    }
        //}
        static void SerialPortInitialization()
        {
            Port.PortName = "COM7";
            Port.Parity = 0;
            Port.BaudRate = 115200;
            Port.StopBits = StopBits.Two;
            Port.DataBits = 8;
            Port.ReadTimeout = 100;
            Port.WriteTimeout = 100;

            if (Port == null)
            {
                Console.WriteLine("串口初始化失败");
                Console.ReadKey();
            }
            else
            {
                Port.Open();
                Console.WriteLine("串口初始化成功");
            }
        }
        /// <summary>
        /// 递归写入数据
        /// </summary>
        /// <param name="port">串口对象</param>
        /// <param name="buffer">需要写入字符的数组</param>
        /// <param name="offset">offset</param>
        static void ReadData(SerialPort port, char[] buffer, int offset = 0)
        {
            StringBuilder sb = new StringBuilder(256);
            string msg = string.Empty;
            try
            {
                int count = port.Read(buffer, offset, 1);
                if (count == 1)
                {
                    ++offset;
                    ReadData(port, buffer, offset);
                }
            }
            catch (TimeoutException)
            {
                for (int i = 0; i < offset; i++)
                {
                    sb.Append(buffer[i]);
                }
                msg = sb.ToString();
                Console.Write(msg);
            }
        }
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void OPMGetDllInfo(ref string name, ref string version);

        ///SerialPort的Read()方法，读取无数据会返回超时Exception
    }
}
