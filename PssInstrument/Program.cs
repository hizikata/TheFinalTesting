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
            try
            {
                Port.PortName = "COM1";
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
                string name = string.Empty, version = string.Empty;
                OPMGetDllInfo(ref name, ref version);
                Console.WriteLine(name);
                Console.WriteLine(version);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
        [DllImport("PSS_OPM-C_DLL.dll", EntryPoint = "OPMGetDllInfo", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void OPMGetDllInfo(ref string name, ref string version);

    }
}
