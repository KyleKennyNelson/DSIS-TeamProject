using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdminMonitor.TRUONGDONVI
{
    /// <summary>
    /// Interaction logic for QuanLyDataPHANCONG.xaml
    /// </summary>
    public partial class QuanLyDataPHANCONG : Window
    {
        OracleConnection _Conn;

        private string _mode;

        private string _MaGV;
        private string _MaHP;
        private decimal _HK;
        private decimal _Nam;
        private string _MaCT;

        private string _newMaGV;
        private string _newMaHP;
        private decimal _newHK;
        private decimal _newNam;
        private string _newMaCT;
        public QuanLyDataPHANCONG(OracleConnection connection, string MaGV, 
                                    string MaHP, decimal HK, decimal Nam, string MaCT, string mode)
        {
            InitializeComponent();
            _Conn = connection;
            _mode = mode;

            _MaGV = MaGV;
            _MaHP = MaHP;
            _HK = HK;
            _Nam = Nam;
            _MaCT = MaCT;
    }

        private async void QuanLyDataPHANCONG_Loaded(object sender, RoutedEventArgs e)
        {
            CancelButton.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            if (_mode == "Update")
            {
                ConfirmButton.Content = _mode;
                NewMaGVBox.Text = _MaGV;

                NewMaHPBox.Text = _MaHP;
                NewMaHPBox.IsReadOnly = true;

                NewHKBox.Text = _HK.ToString();
                NewHKBox.IsReadOnly = true;

                NewNamBox.Text = _Nam.ToString();
                NewNamBox.IsReadOnly = true;

                NewMaCTBox.Text = _MaCT;
                NewMaCTBox.IsReadOnly = true;
            }
            else if (_mode == "Add")
                ConfirmButton.Content = _mode;

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

        private void NewMaGVBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                _newMaGV = NewMaGVBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewMaHPBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newMaHP = NewMaHPBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void NewHKBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newHK = Convert.ToInt32(NewHKBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void NewNamBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newNam = Convert.ToInt32(NewNamBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewMaCTBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newMaCT = NewMaCTBox.Text;
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
                    if (_mode == "Update")
                    {
                        if (_Conn.State == ConnectionState.Closed)
                        {
                            _Conn.Open();
                        }
                        if (_newMaGV != _MaGV)
                        {
                            OracleCommand query = _Conn.CreateCommand();
                            query.BindByName = true;
                            query.CommandText = """
                                                    UPDATE admin.UV_TRGDONVI_PHANCONG
                                                    SET MAGV = :newmagv
                                                    WHERE MAGV = :magv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            query.CommandType = CommandType.Text;
                            query.Parameters.Add(new OracleParameter(":newmagv", _newMaGV));
                            query.Parameters.Add(new OracleParameter(":magv", _MaGV));
                            query.Parameters.Add(new OracleParameter(":mahp", _MaHP));
                            query.Parameters.Add(new OracleParameter(":hk", Convert.ToInt32(_HK)));
                            query.Parameters.Add(new OracleParameter(":nam", Convert.ToInt32(_Nam)));
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
                    else if (_mode == "Add")
                    {
                        if (_Conn.State == ConnectionState.Closed)
                        {
                            _Conn.Open();
                        }
                        OracleCommand query = _Conn.CreateCommand();
                        query.BindByName = true;
                        query.CommandText = """
                                                INSERT INTO admin.UV_TRGDONVI_PHANCONG(MAGV, MAHP, HK, NAM, MACT)
                                                values(:magv, :mahp, :hk, :nam, :mact)
                                            """;
                        query.CommandType = CommandType.Text;
                        query.Parameters.Add(new OracleParameter(":magv", _newMaGV));
                        query.Parameters.Add(new OracleParameter(":mahp", _newMaHP));
                        query.Parameters.Add(new OracleParameter(":hk", _newHK));
                        query.Parameters.Add(new OracleParameter(":nam", _newNam));
                        query.Parameters.Add(new OracleParameter(":mact", _newMaCT));

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
                catch (Exception ex)
                {
                    status = false;
                    MessageBox.Show(ex.ToString(), "Không cập nhật được bảng phân công", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Cập nhật bảng phân công thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            DialogResult = true;
        }
    }
}
