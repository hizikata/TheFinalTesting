using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Keithley keith = new Keithley("24");
                keith.SetMeasureVoltageOnlyPara("10");
                keith.Open();
                string data= keith.ReadData();
                Console.WriteLine(data);
                string[] paras = data.Split(',');
                double volage = Convert.ToDouble(paras[0]);
                Console.WriteLine(volage);
                Console.ReadKey();
                keith.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
