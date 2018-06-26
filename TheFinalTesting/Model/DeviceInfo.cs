using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFinalTesting.Model
{
    /// <summary>
    /// 设备信息
    /// </summary>
    internal class DeviceInfo
    {
        #region Properties
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// GPIB地址
        /// </summary>
        public int Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
        #region Constructors
        public DeviceInfo(bool isSelected,string deviceName,int address,string remark)
        {
            this.IsSelected = isSelected;
            this.DeviceName = deviceName;
            this.Address = address;
            this.Remark = remark;
        }
        #endregion

    }
}
