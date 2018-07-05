using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheFinalTesting.Model;
using XuxzLib.Communication;

namespace TheFinalTesting.View
{
    /// <summary>
    /// FrmDebug.xaml 的交互逻辑
    /// </summary>
    public partial class FrmDebug : Window
    {
        #region Fields
        IDeviceCommonMethods Device;
        int[] Address = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30 };
        #endregion
        public FrmDebug()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbAddress.ItemsSource = Address;
        }

        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAddress.SelectedIndex != -1)
            {
                Device = new DeviceBase(cmbAddress.SelectedItem.ToString());
                tbResult.Text = Device.GetIdn();
            }
            else
            {
                MessageBox.Show("请选择设备地址", "系统提示");
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if(Device==null)
            {
                MessageBox.Show("请先初始化设备", "系统提示");
                return;
            }
            if (!string.IsNullOrEmpty(txtCommand.Text.Trim()))
            {
                Device.WriteCommand(txtCommand.Text.Trim());
            }
            else
            {
                MessageBox.Show("请输入命令文本", "系统提示");
            }

        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            if (Device == null)
            {
                MessageBox.Show("请先初始化设备", "系统提示");
                return;
            }
            try
            {
                tbResult.Text= Device.ReadCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }

        }

        private void btnSendAndRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btnSend_Click(this, null);
                tbResult.Text= Device.ReadCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
        }
    }
}
