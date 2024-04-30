using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminMonitor.GIAOVU
{
    /// <summary>
    /// Interaction logic for TTCaNhanNVUserControl.xaml
    /// </summary>
    public partial class TTCaNhanNVUserControl : UserControl
    {
        OracleConnection Conn;
        string _MaNV;
        NhanSu? TTCaNhan;
        public TTCaNhanNVUserControl(OracleConnection Conn, string MaNV)
        {
            InitializeComponent();
            _MaNV = MaNV;
            this.Conn = Conn;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;

            NhanSu? data = null;
            await Task.Run(() => data = Controller_NhanSu.GetTTCaNhan(Conn,_MaNV));
            TTCaNhan = data;

            DataContext = TTCaNhan;
            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingBar.IsIndeterminate = true;
            SaveButton.IsEnabled = true;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;
            if (TTCaNhan != null && TTCaNhan.SDT != null)
            {
                await Task.Run(() => Controller_NhanSu.UpdateSDT(Conn, TTCaNhan.SDT));
            }
            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thành công", MessageBoxButton.OK);
            UserControl_Loaded(sender, e);
            loadingBar.IsIndeterminate = true;
            SaveButton.IsEnabled = true;
        }
    }
}
