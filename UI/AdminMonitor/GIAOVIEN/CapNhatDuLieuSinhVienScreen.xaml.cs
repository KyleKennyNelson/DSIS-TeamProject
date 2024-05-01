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
    ///
    public partial class CapNhatDuLieuSinhVienScreen : Window
    {
        OracleConnection _Conn;
        private string _MSSV;
        private string _MaHP;
        private decimal _HK;
        private decimal _Nam;
        private string _MaCT;


        private decimal _DTHshow;
        private decimal _DQTshow;
        private decimal _DCKshow;
        private decimal _DTKshow;

        private float _newDTH;
        private float _newDQT;
        private float _newDCK;
        private float _newDTK;
        public CapNhatDuLieuSinhVienScreen(OracleConnection connection, string MSSV, string MaHP, decimal HK,
                                           decimal Nam, string MaCT, decimal DTH, decimal DQT, decimal DCK, decimal DTK)
        {
            InitializeComponent();
            _Conn = connection;
            _MSSV = MSSV;
            _MaHP = MaHP;
            _HK = HK;
            _Nam = Nam;
            _MaCT = MaCT;

            _DTHshow = DTH;
            _DQTshow = DQT;
            _DCKshow = DCK;
            _DTKshow = DTK;
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
                NewDTHBox.Text = _DTHshow.ToString();
                NewDQTBox.Text = _DQTshow.ToString();
                NewDCKBox.Text = _DCKshow.ToString();
                NewDTKBox.Text = _DTKshow.ToString();
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

        private void NewDTHBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(NewDTHBox.Text != "")
                {
                    _newDTH = float.Parse(NewDTHBox.Text);
                    if (_newDTH < 0 || _newDTH > 10)
                    {
                        _newDTH = (float)_DTHshow;
                        MessageBox.Show("the score have to be in range (0, 10)", "Error");
                    }
                }
                else
                {
                    _newDTH = (float)_DTHshow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewDQTBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (NewDQTBox.Text != "")
                {
                    _newDQT = float.Parse(NewDQTBox.Text);
                    if (_newDQT < 0 || _newDQT > 10)
                    {
                        _newDQT = (float)_DQTshow;
                        MessageBox.Show("the score have to be in range (0, 10)", "Error");
                    }
                }
                else
                {
                    _newDQT = (float)_DQTshow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void NewDCKBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (NewDCKBox.Text != "")
                {
                    _newDCK = float.Parse(NewDCKBox.Text);
                    if (_newDCK < 0 || _newDCK > 10)
                    {
                        _newDCK = (float)_DCKshow;
                        MessageBox.Show("the score have to be in range (0, 10)", "Error");
                    }
                }
                else
                {
                    _newDCK = (float)_DCKshow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
        private void NewDTKBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (NewDCKBox.Text != "")
                {
                    _newDTK = float.Parse(NewDTKBox.Text);
                    if (_newDTK < 0 || _newDTK > 10)
                    {
                        _newDTK = (float)_DTKshow;
                        MessageBox.Show("the score have to be in range (0, 10)", "Error");
                    }
                }
                else
                {
                    _newDTK = (float)_DTKshow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
                        if (_Conn.State == ConnectionState.Closed)
                        {
                            _Conn.Open();
                        }
                        if (_newDTH != ((float)_DTHshow))
                        {
                            OracleCommand query = _Conn.CreateCommand();
                            query.BindByName = true;
                            query.CommandText = """
                                                    UPDATE admin.PROJECT_DANGKI
                                                    SET DIEMTH = :diemTH
                                                    WHERE MASV = :mssv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            query.CommandType = CommandType.Text;
                            query.Parameters.Add(new OracleParameter(":diemTH", _newDTH));
                            query.Parameters.Add(new OracleParameter(":mssv", _MSSV));
                            query.Parameters.Add(new OracleParameter(":mahp", _MaHP));
                            query.Parameters.Add(new OracleParameter(":hk", _HK));
                            query.Parameters.Add(new OracleParameter(":nam", _Nam));
                            query.Parameters.Add(new OracleParameter(":mact", _MaCT));

                            try
                            {
                                query.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        if (_newDQT != ((float)_DQTshow))
                        {
                            OracleCommand query = _Conn.CreateCommand();
                            query.BindByName = true;
                            query.CommandText = """
                                                    UPDATE admin.PROJECT_DANGKI
                                                    SET DIEMQT = :diemQT
                                                    WHERE MASV = :mssv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            query.CommandType = CommandType.Text;
                            query.Parameters.Add(new OracleParameter(":diemQT", _newDQT));
                            query.Parameters.Add(new OracleParameter(":mssv", _MSSV));
                            query.Parameters.Add(new OracleParameter(":mahp", _MaHP));
                            query.Parameters.Add(new OracleParameter(":hk", _HK));
                            query.Parameters.Add(new OracleParameter(":nam", _Nam));
                            query.Parameters.Add(new OracleParameter(":mact", _MaCT));

                            try
                            {
                                query.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        if (_newDCK != ((float)_DCKshow))
                        {
                            OracleCommand query = _Conn.CreateCommand();
                            query.BindByName = true;
                            query.CommandText = """
                                                    UPDATE admin.PROJECT_DANGKI
                                                    SET DIEMCK = :diemCK
                                                    WHERE MASV = :mssv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            query.CommandType = CommandType.Text;
                            query.Parameters.Add(new OracleParameter(":diemCK", _newDCK));
                            query.Parameters.Add(new OracleParameter(":mssv", _MSSV));
                            query.Parameters.Add(new OracleParameter(":mahp", _MaHP));
                            query.Parameters.Add(new OracleParameter(":hk", _HK));
                            query.Parameters.Add(new OracleParameter(":nam", _Nam));
                            query.Parameters.Add(new OracleParameter(":mact", _MaCT));

                            try
                            {
                                query.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        if (_newDTK != ((float)_DTKshow))
                        {
                            OracleCommand query = _Conn.CreateCommand();
                            query.BindByName = true;
                            query.CommandText = """
                                                    UPDATE admin.PROJECT_DANGKI
                                                    SET DIEMTK = :diemTK
                                                    WHERE MASV = :mssv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            query.CommandType = CommandType.Text;
                            query.Parameters.Add(new OracleParameter(":diemTK", _newDTK));
                            query.Parameters.Add(new OracleParameter(":mssv", _MSSV));
                            query.Parameters.Add(new OracleParameter(":mahp", _MaHP));
                            query.Parameters.Add(new OracleParameter(":hk", _HK));
                            query.Parameters.Add(new OracleParameter(":nam", _Nam));
                            query.Parameters.Add(new OracleParameter(":mact", _MaCT));

                            try
                            {
                                query.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
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
