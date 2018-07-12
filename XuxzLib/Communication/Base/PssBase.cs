using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace XuxzLib.Communication
{
    public class PssBase
    {
        public const int OPM_ENDSIGN_0 = 0;  //不带结结束位
        public const int OPM_ENDSIGN_1 = 1; //结束位 \n
        public const int OPM_ENDSIGN_2 = 2; //结束位 \r
    }
}
