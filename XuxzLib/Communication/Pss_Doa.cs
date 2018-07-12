using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XuxzLib.Communication
{
    /// <summary>
    /// 普赛斯 光衰减
    /// </summary>
    public class Pss_Doa
    {
        public const int OPM_ENDSIGN_0 = 0;  //不带结结束位
        public const int OPM_ENDSIGN_1 = 1; //结束位 \n
        public const int OPM_ENDSIGN_2 = 2; //结束位 \r
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReadIDN", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint DOAReadIDN(uint hardwareIndex,uint cardID,uint endSign,ref byte idn);
        [DllImport("PSS_DOA-C_DLL.dll", EntryPoint = "DOAReset", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint DOAReset(uint hardwareIndex, uint cardID, uint endSign);

    }
}
