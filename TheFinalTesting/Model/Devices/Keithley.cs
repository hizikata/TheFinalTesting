using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XuxzLib.Communication;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// 电源供应器Keithley
    /// </summary>
    public class Keithley
    {
        //int DefRM = 0;
        //int Vi = 0;
        //int Status;
        //string DeviceConn;
        /// <summary>
        /// Keithley 构造函数
        /// </summary>
        /// <param name="add"></param>
        public Keithley(string add)
        {
            int resourceManager = 0, viError;
            int seesion = 0;
            viError = visa32.viOpenDefaultRM(out resourceManager);
            viError = visa32.viOpen(resourceManager, "GPIB0::24::INSTR",
                visa32.VI_NO_LOCK, visa32.VI_TMO_IMMEDIATE, out seesion);
            CheckStatus(seesion, viError);
            viError = visa32.viPrintf(seesion, "*IDN?\n");
            viError = visa32.viRead(seesion, out string result, 100);
            System.Windows.MessageBox.Show(result);
        }
        public string GetData()
        {
            
            //WriteCommand(":SOUR:FUNC CURR");
            //WriteCommand(":SOUR:CURR:MODE FIX");
            //WriteCommand(":SOUR:CURR:RANG 0.2");
            //WriteCommand(":SOUR:CURR:LEV 0");
            //WriteCommand(":SENS:FUNC \"VOLT\"");
            //WriteCommand(":SENS:VOLT:RANG 10");
            //WriteCommand(":OUTP ON");
            //return WriteAndRead("READ");
            return string.Empty;
        }
        protected void CheckStatus(int vi, int status)
        {
            if ((status < visa32.VI_SUCCESS))
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder(256);
                visa32.viStatusDesc(vi, status, err);
                throw new Exception(err.ToString());
            }
        }
        /******
         * *RST
         * :SOUR:FUNC VOLT
         * :SOUR:VOLT:MODE FIX
         * :SOUR:VOLT:RANG 10
         * :SOUR
         * ****/
        /**
         *   viError = visa32.viPrintf(seesion, ":SOUR:FUNC CURR\n");
           viError = visa32.viPrintf(seesion, ":SOUR:CURR:MODE FIX\n");
           viError = visa32.viPrintf(seesion, ":SOUR:CURR:RANG 0.2\n");
           viError = visa32.viPrintf(seesion, ":SOUR:CURR:LEV 0\n");
           viError = visa32.viPrintf(seesion, ":SENS:FUNC \"VOLT\"\n");
           //viError = visa32.viPrintf(seesion, ":SENS:VOL:PROT 21\n");
           viError = visa32.viPrintf(seesion, ":SENS:VOLT:RANG 10\n");
           viError = visa32.viPrintf(seesion, ":OUTP ON\n");
           viError = visa32.viPrintf(seesion, "READ?\n");
           viError = visa32.viRead(seesion, out result, 100);
         * ***/

    }
}
