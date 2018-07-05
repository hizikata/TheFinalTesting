using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace PssInstrument
{
    class Program
    {
        static SerialPort Port = new SerialPort();
        static void Main(string[] args)
        {
            char[] buffer = new char[256];
            //int count = 0;
            try
            {
                Port.PortName = "COM7";
                Port.Parity = 0;
                Port.BaudRate = 115200;
                Port.StopBits = StopBits.Two;
                Port.DataBits = 8;
                Port.ReadTimeout = 100;
                Port.WriteTimeout = 100;
                Port.Open();
                if (Port == null)
                {
                    Console.WriteLine("串口初始化失败");
                    Console.ReadKey();
                }
                Port.Write("*IDN?\n");
                //for (int i = 0; i < 37; i++)
                //{
                //    count=Port.Read(buffer, i, 1);
                //}
                string msg = ReadData(Port, buffer);//尝试使用SerialPort.ReadLine();
                Console.WriteLine(msg);

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
        /// <summary>
        /// 递归写入数据
        /// </summary>
        /// <param name="port">串口对象</param>
        /// <param name="buffer">需要写入字符的数组</param>
        /// <param name="offset">offset</param>
        static string ReadData(SerialPort port,char[]buffer,int offset=0)
        {
            StringBuilder sb = new StringBuilder(256);
            string msg=string.Empty;
            try
            {
                int count = port.Read(buffer, offset, 1);
                if (count == 1)
                {
                    ++offset;
                    ReadData(port, buffer, offset);
                }
                return msg;
            }
            catch (TimeoutException)
            {
                for (int i = 0; i < offset; i++)
                {
                    sb.Append(buffer[i]);
                }
                msg = sb.ToString();
                return msg;
            }
        }
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void OPMGetDllInfo(ref string name, ref string version);

        ///SerialPort的Read()方法，读取无数据会返回超时Exception
    }
}
