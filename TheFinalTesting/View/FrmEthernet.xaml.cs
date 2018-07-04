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
using XuxzLib.Communication;
using TheFinalTesting.Model;

namespace TheFinalTesting.View
{
    /// <summary>
    /// FrmEthernet.xaml 的交互逻辑
    /// </summary>
    public partial class FrmEthernet : Window
    {
        string[] ConnStr = { "TCPIP::192.168.100.101::5001::SOCKET", "TCPIP::192.168.100.101::inst0::INSTR", "TCPIP::192.168.100.101::hislip::INSTR",
                            "TCPIP::192.168.100.102::5001::SOCKET", "TCPIP::192.168.100.102::inst0::INSTR", "TCPIP::192.168.100.102::hislip::INSTR"};
        MP2100A Mp2100A;
        public FrmEthernet()
        {
            InitializeComponent();
            cmbConnStr.ItemsSource = ConnStr;
        }

        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mp2100A = new MP2100A(cmbConnStr.SelectedItem.ToString().Trim(), ConnectionType.Ethernet);
                MessageBox.Show(Mp2100A.GetIdn()+"hello world", "系统提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");
            }
        }

        private void btnsend_Click(object sender, RoutedEventArgs e)
        {
            Mp2100A.GetIdn();
        }

        private void btnread_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnsendandsend_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
