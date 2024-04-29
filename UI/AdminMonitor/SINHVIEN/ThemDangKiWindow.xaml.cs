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
using System.Windows.Shapes;

namespace AdminMonitor.SINHVIEN
{
    /// <summary>
    /// Interaction logic for ThemDangKiWindow.xaml
    /// </summary>
    public partial class ThemDangKiWindow : Window
    {
        internal class DataGridItems: INotifyPropertyChanged
        {
            public PhanCong? Phancong { get; set; }
            public bool IsChecked { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        List<DataGridItems> dataGridContext = new List<DataGridItems>();
        OracleConnection Conn;
        string? MaSV = string.Empty;
        public ThemDangKiWindow(OracleConnection connection, string MaSV)
        {
            InitializeComponent();
            Conn = connection;
            this.MaSV = MaSV;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CancelButton.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            if(MaSV != null)
            {
                List<PhanCong>? data = null;
                await Task.Run(() => data = Controller_PhanCong.GetPhanCongs(Conn, MaSV));
                if(data != null)
                {
                    foreach (var phancong in data)
                    {
                        dataGridContext.Add(new DataGridItems { Phancong = phancong, IsChecked = false });
                    }
                    MainDataGrid.ItemsSource = dataGridContext;
                }
            }

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.IsIndeterminate = true;
            CancelButton.IsEnabled = true;
            ConfirmButton.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            bool status = true;

            CancelButton.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            await Task.Run(() => {
                try
                {
                    if (MaSV != null)
                    {
                        foreach (var item in dataGridContext)
                        {
                            if (item.IsChecked && item.Phancong != null)
                            {
                                Controller_DangKi.InsertDangKi(Conn, MaSV, item.Phancong);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    MessageBox.Show(ex.ToString(), "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.IsIndeterminate = true;
            CancelButton.IsEnabled = true;
            ConfirmButton.IsEnabled = true;

            if(status)
            {
                MessageBox.Show("Đăng kí học phần thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            DialogResult= true;
        }
    }
}
