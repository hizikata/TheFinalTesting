using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace XuxzLib.Communication
{
    public class PssBase
    {
        #region Constants
        //结束位
        public const uint ENDSIGN_0 = 0;  //不带结结束位
        public const uint ENDSIGN_1 = 1; //结束位 \n
        public const uint ENDSIGN_2 = 2; //结束位 \r

        //CardId
        public const uint CARDID_0 = 0; //配置为0时 不发送CardID,即OPM作为独立设备使用，不应用于插卡式机箱
        public const uint CARDID_1 = 1; //配置为1~10时，OPM作为插盒应用于插卡式主机箱中，协议前加CardID号
        public const uint CARDID_2 = 2;
        public const uint CARDID_3 = 3;
        public const uint CARDID_4 = 4;
        public const uint CARDID_5 = 5;
        public const uint CARDID_6 = 6;
        public const uint CARDID_7 = 7;
        public const uint CARDID_8 = 8;
        public const uint CARDID_9 = 9;
        public const uint CARDID_10 = 10;

        //定义波长
        public const uint WAVELENGTH_850NM = 850;
        public const uint WAVELENGTH_1270NM = 1270;
        public const uint AVELENGTH_1310NM = 1310;
        public const uint WAVELENGTH_1330NM = 1330;
        public const uint WAVELENGTH_1490NM = 1490;
        public const uint WAVELENGTH_1550NM = 1550;
        #endregion
        #region Fields
        public static string ModeName;
        #endregion
    }
}
