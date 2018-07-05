using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XuxzLib.Communication
{
    public class Agilent86100A:DeviceBase
    {
        public Agilent86100A(string add):base(add)
        {
            DeviceName = "Agilent86100A";
        }
        //crossing :MEAS:CGR:CROS? CHAN3  (选择channle)
        //average power :MEAS:APOW? DEC|WATT,CHAN3  单位：分贝|uW
        //Ext.ratio(消光比) :MEAS:CGR:ERAT? DEC|Ratio,CHAN3 单位:分贝|比例
        //AUTOSCALE    :AUT
        //CAL     :CAL:MOD:VERT LMOD|RMOD  
        //CAL     :CAL:CONT 继续    :CAL:CANC 取消
        //Extinction ratio cal   :CAL:ERAT:STAR CHAN3
        //MODEL temperature  :CALibrate:MODule:TIME? {LMODule | RMODule | CHANnel <N>}
    }
}
