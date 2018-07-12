#define UseDll
//#define UserCustom
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using XuxzLib.Communication;

namespace PssInstrument
{
    class Program
    {
        static SerialPort Port = new SerialPort();
        static byte[] SerBuf = new byte[256];
#if UseDll
        static void Main()
        {
            //串口初始化
            //SerialPortInitialization();
            //获取Dll 名称/版本
            byte[] name = new byte[100];
            byte[] version = new byte[100];
            DOAGetDllInfo(name,version);
            string n = Encoding.ASCII.GetString(name);
            string v = Encoding.ASCII.GetString(version);
            Console.WriteLine(n);
            Console.Write(v);
            //获取IDN
            DOAReadIDN(3, OPM_ENDSIGN_1, SerBuf);
            Console.ReadKey();
            Port.Close();
        }

        public const uint OPM_ENDSIGN_0 = 0;  //不带结结束位
        public const uint OPM_ENDSIGN_1 = 1; //结束位 \n
        public const uint OPM_ENDSIGN_2 = 2; //结束位 \r
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadIDN", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DOAReadIDN(uint cardID, uint endSign, byte[] idn);

        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DOAGetDllInfo(byte[] name, byte[] version);

        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAPCTxCStringFuncReg", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DOAPCTxCStringFuncReg(uint sign);

        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAPCRxCStringFuncReg", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DOAPCRxCStringFuncReg(uint sign);
#endif
#if UserCustom
        static void Main(string[] args)
        {
            char[] buffer = new char[256];
            //int count = 0;
            try
            {
                SerialPortInitialization();
                Port.Write("Card3 *IDN?\n");
                //for (int i = 0; i < 37; i++)
                //{
                //    count=Port.Read(buffer, i, 1);
                //}
                ReadData(Port, buffer);//尝试使用SerialPort.ReadLine();
                Console.WriteLine("Press any key to exit");                
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                Console.ReadKey();
            }
        }
#endif
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
