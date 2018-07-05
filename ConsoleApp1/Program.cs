using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheFinalTesting.Model;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunSimple();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            //int status;
            //status = visa32.viOpenDefaultRM(out int defrm);
            //try
            //{
            //    status = visa32.viOpen(defrm, "GPIB0::24:INSTR", 0, 0, out int vi);
            //    CheckStatus(defrm, status);
            //    Console.WriteLine("press any key to exit.");
            //    Console.ReadKey();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine("press any key to exit.");
            //    Console.ReadKey();
            //}
        }
        private static void RunSimple()
        {
            int resourceManager = 0, viError;
            int seesion = 0;
            viError = visa32.viOpenDefaultRM(out resourceManager);
            viError = visa32.viOpen(resourceManager, "GPIB0::24::INSTR",
                visa32.VI_NO_LOCK, visa32.VI_TMO_IMMEDIATE, out seesion);
            CheckStatus(seesion, viError);
            viError = visa32.viPrintf(seesion, "*IDN?\n");
            viError = visa32.viRead(seesion, out string result, 100);
            Console.WriteLine(result);
            Console.WriteLine();

            viError = visa32.viPrintf(seesion, ":SOUR:FUNC CURR\n");
            viError = visa32.viPrintf(seesion, ":SOUR:CURR:MODE FIX\n");
            viError = visa32.viPrintf(seesion, ":SOUR:CURR:RANG 0.2\n");
            viError = visa32.viPrintf(seesion, ":SOUR:CURR:LEV 0\n");
            viError = visa32.viPrintf(seesion, ":SENS:FUNC \"VOLT\"\n");
            //viError = visa32.viPrintf(seesion, ":SENS:VOL:PROT 21\n");
            viError = visa32.viPrintf(seesion, ":SENS:VOLT:RANG 10\n");
            viError = visa32.viPrintf(seesion, ":OUTP ON\n");
            viError = visa32.viPrintf(seesion, "READ?\n");
            viError = visa32.viRead(seesion, out result, 100);
            Console.WriteLine(result);
            visa32.viClear(seesion);
            visa32.viClear(resourceManager);

        }
        private static void CheckStatus(int vi, int status)
        {
            if ((status < visa32.VI_SUCCESS))
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder(256);
                visa32.viStatusDesc(vi, status, err);
                throw new Exception(err.ToString());
            }
        }
    }
}
