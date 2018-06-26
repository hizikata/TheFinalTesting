using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using TheFinalTesting.Model;

namespace TheFinalTesting.ViewModel
{
    internal class MainViewModel:ViewModelBase
    {
        #region Fields
        /// <summary>
        /// 高低压万用表
        /// </summary>
        internal Aglient34401A Ag34401A;
        /// <summary>
        /// 基板电源供应器
        /// </summary>
        internal AglientE3631A AgE3631A;
        /// <summary>
        /// 光功率计
        /// </summary>
        internal HP8153A Hp8153A;
        /// <summary>
        /// 光衰减器
        /// </summary>
        internal HP8156A Hp8156A;
        /// <summary>
        /// 眼图仪
        /// </summary>
        internal MP2100A Mp2100A;
        /// <summary>
        /// 电源供应器
        /// </summary>
        internal PST3202 P3202;
        /// <summary>
        /// 频谱分析仪
        /// </summary>
        internal AQ6317B Aq6317B;
        #endregion
        #region Properties
        public DeviceInfo[] Devices { get; } = new DeviceInfo[]
        {
            new DeviceInfo(true,"Aglient34401A",0,"高低压万用表(地址未知)"),
            new DeviceInfo(true,"AglientE3631A",9,"基板电源供应器(地址:9)"),
            new DeviceInfo(true,"HP8153A",5,"光功率计(地址:5)"),
            new DeviceInfo(true,"HP8156A",22,"光衰减器(地址:22)"),
            new DeviceInfo(true,"MP2100A",2,"眼图仪(地址:2)"),
            new DeviceInfo(true,"PST3202",15,"电源供应器(地址:15)"),
            new DeviceInfo(true,"AQ6317B",0,"频谱分析仪(地址未知)")

        };
        #endregion
        #region Consturctors
        #endregion
        #region Commands
        #endregion
        #region CommandMethods
        #endregion
    }
}
