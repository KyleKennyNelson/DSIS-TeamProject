using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AdminMonitor.GIAOVIEN
{
    /// <summary>
    /// Interaction logic for CapNhatDuLieuSinhVienScreen.xaml
    /// </summary>
    public partial class CapNhatDuLieuSinhVienScreen : Window
    {
        OracleConnection _Conn;
        private string _MSSV;

        private decimal _DTH;
        private decimal _DQT;
        private decimal _DCK;
        private decimal _DTK;
        public CapNhatDuLieuSinhVienScreen(OracleConnection connection, string MSSV, decimal DTH,
                                                            decimal DQT, decimal DCK, decimal DTK)
        {
            InitializeComponent();
            _Conn = connection;
            _MSSV = MSSV;

            _DTH = DTH;
            _DQT = DQT;
            _DCK = DCK;
            _DTK = DTK;
    }

        private async void CapNhatDuLieuSinhVienScreen_Loaded(object sender, RoutedEventArgs e)
        {
            LabellTitle.Content = $"Cập nhật điểm số cho sinh viên MSSV: {_MSSV}";
            CancelButton.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            if (_MSSV != null)
            {
                DTHBox.Text = _DTH.ToString();
                NewDQTBox.Text = _DQT.ToString();
                NewDCKBox.Text = _DCK.ToString();
                NewDTKBox.Text = _DTK.ToString();
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
                    if (_MSSV != null)
                    {
                        //if(NewDTHBox.Text != _DTH.ToString())
                        //{
                            if (_Conn.State == ConnectionState.Closed)
                            {
                                _Conn.Open();
                            }
                            OracleCommand query = _Conn.CreateCommand();
                            query.BindByName = true;
                            query.CommandText = """
                                                   UPDATE admin.PROJECT_DANGKI
                                                   SET DIEMTH = '7'
                                                   WHERE MASV = 'SV015'
                                                 """;
                            query.CommandType = CommandType.Text;

                            string diemTH = "7";
                            query.Parameters.Add(new OracleParameter("diemTH", diemTH));
                            //query.Parameters.Add(new OracleParameter("mssv", _MSSV));

                            try
                            {
                                query.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    MessageBox.Show(ex.ToString(), "Không cập nhật được điểm số", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (status)
            {
                MessageBox.Show("Cập nhật điểm số cho sinh viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            DialogResult = true;
        }
    }
}
