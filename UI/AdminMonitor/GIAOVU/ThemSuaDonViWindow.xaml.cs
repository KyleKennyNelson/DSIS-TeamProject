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
using System.Windows.Shapes;

namespace AdminMonitor.GIAOVU
{
    /// <summary>
    /// Interaction logic for ThemSuaDonViWindow.xaml
    /// </summary>
    public partial class ThemSuaDonViWindow : Window
    {
        class Modes
        {
            public static int Insert => 0;
            public static int Update => 1;
        }

        OracleConnection Conn;
        DonVi? _TTDonVi;
        int mode = Modes.Insert;
        public ThemSuaDonViWindow(OracleConnection connection, DonVi data)
        {
            InitializeComponent();
            Conn = connection;
            _TTDonVi = data;
            mode = Modes.Update;
            DataContext = _TTDonVi;
            mainTitle.Content = "Chỉnh sửa thông tin đơn vị";
            this.Title = "Chỉnh sửa thông tin đơn vị";
            MaDonViTextBox.IsEnabled = false;
        }
        public ThemSuaDonViWindow(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
            mode = Modes.Insert;
            _TTDonVi = new DonVi()
            {
                MaDV = "DV000"
            };
            DataContext = _TTDonVi;
            mainTitle.Content = "Thêm đơn vị";
            this.Title = "Thêm đơn vị";
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;

            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;

            try
            {
                if (_TTDonVi != null)
                {
                    if (mode == Modes.Update)
                    {
                        await Task.Run(() => Controller_DonVi.UpdateDonVi(Conn, _TTDonVi));
                    }
                    else if (mode == Modes.Insert)
                    {
                        await Task.Run(() => Controller_DonVi.InsertDonVi(Conn, _TTDonVi));
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                MessageBox.Show(ex.ToString(), "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            if (flag)
            {
                MessageBox.Show("Sửa thông tin đơn vị thành công!", "Thành công", MessageBoxButton.OK);
                loadingBar.IsIndeterminate = true;
                SaveButton.IsEnabled = true;
                DialogResult = true;
            }
        }

        private void CancelButotn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
