using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PssHighLowTemperature.Model
{
    /// <summary>
    /// 设备模型
    /// </summary>
    public class Instru
    {
        public string Name { get; set; }
        public string Addr { get; set; }
        public string Remark { get; set; }
        public InstType Type{ get; set; }
        public Instru(string name ,string addr,string remark,InstType type)
        {
            this.Name = name;
            this.Addr = addr;
            this.Remark = remark;
            this.Type = type;
        }
    }
    public enum InstType
    {
        PssInstru,  //普赛斯设备
        GpibInstru  //GPIB控制设备
    }
}
