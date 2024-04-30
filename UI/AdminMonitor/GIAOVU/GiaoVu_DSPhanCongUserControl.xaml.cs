using AdminMonitor.SINHVIEN;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for GiaoVu_DSPhanCongUserControl.xaml
    /// </summary>
    public partial class GiaoVu_DSPhanCongUserControl : UserControl
    {
        OracleConnection Conn;
        List<PhanCong>? list = null;
        public GiaoVu_DSPhanCongUserControl(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            List<PhanCong>? data = null;
            await Task.Run(() => data = Controller_PhanCong.GetPhanCongs(Conn));
            list = data;
            MainDataGrid.ItemsSource = list;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));

            loadingProgressBar.IsIndeterminate = true;
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PhanCong? selected = (PhanCong?)MainDataGrid.SelectedItem;
            if (selected != null)
            {
                loadingProgressBar.IsIndeterminate = false;
                loadingProgressBar.Value = 10;
                await Task.Run(() => Thread.Sleep(10));
                loadingProgressBar.Value = 40;

                bool result = false;
                try
                {
                    await Task.Run(() => result = Controller_PhanCong.DeletePhanCong(Conn, selected));
                }
                catch (Exception ex)
                {
                    
                }
                

                await Task.Run(() => Thread.Sleep(25));
                loadingProgressBar.Value = 80;
                await Task.Run(() => Thread.Sleep(50));
                loadingProgressBar.Value = 100;
                await Task.Run(() => Thread.Sleep(25));
                loadingProgressBar.IsIndeterminate = true;

                if (result)
                {
                    MessageBox.Show("Xoá thông tin phân công thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Không thể xoá phân công này!", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UserControl_Loaded(sender, e);
            }
        }
    }
}
