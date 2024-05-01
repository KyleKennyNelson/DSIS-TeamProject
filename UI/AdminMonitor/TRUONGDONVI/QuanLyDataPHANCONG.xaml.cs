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
        private List<string> _MaGVList;
        private List<string> _MaHPList;

        private string _MaGV;
        private string _MaHP;
        private decimal _HK;
        private decimal _Nam;
        private string _MaCT;
        private string _role;

        private string _newMaGV;
        private string _newMaHP;
        private decimal _newHK;
        private decimal _newNam;
        private string _newMaCT;
        public QuanLyDataPHANCONG(OracleConnection connection, string MaGV, string MaHP,
                                    decimal HK, decimal Nam, string MaCT, string mode, string role, List<string>MaGVList, List<string>MaHPList)
        {
            InitializeComponent();
            _Conn = connection;
            _mode = mode;

            _MaGV = MaGV;
            _MaHP = MaHP;
            _HK = HK;
            _Nam = Nam;
            _MaCT = MaCT;
            _role = role;
            _MaGVList = MaGVList;
            _MaHPList = MaHPList;
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
                NewMaGVComboBox.ItemsSource = _MaGVList;
                NewMaHPComboBox.ItemsSource = _MaHPList;

                NewHKBox.Text = _HK.ToString();
                NewHKBox.IsReadOnly = true;

                NewNamBox.Text = _Nam.ToString();
                NewNamBox.IsReadOnly = true;

                NewMaCTBox.Text = _MaCT;
                NewMaCTBox.IsReadOnly = true;
            }
            else if (_mode == "Add")
            {
                ConfirmButton.Content = _mode;
                NewMaGVComboBox.ItemsSource = _MaGVList;
                NewMaGVComboBox.ItemsSource = _MaGVList;
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

        private void NewMaGVComboBox_SelectionChanged(object sender, EventArgs e)
        {
            _newMaGV = (string)NewMaGVComboBox.SelectedItem;
        }

        private void NewMaHPComboBox_SelectionChanged(object sender, EventArgs e)
        {
            _newMaHP = (string)NewMaHPComboBox.SelectedItem;
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
                            if (_role == "TruongKhoa")
                            {
                                query.CommandText = """
                                                    UPDATE admin.UV_TRGKHOA_PHANCONG
                                                    SET MAGV = :newmagv
                                                    WHERE MAGV = :magv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            }
                            else if (_role == "TruongDonVi")
                            {
                                query.CommandText = """
                                                    UPDATE admin.UV_TRGDONVI_PHANCONG
                                                    SET MAGV = :newmagv
                                                    WHERE MAGV = :magv
                                                        and MAHP = :mahp
                                                        and HK = :hk
                                                        and NAM = :nam
                                                        and MACT = :mact
                                                 """;
                            }
                            
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
                        OracleCommand query1 = _Conn.CreateCommand();
                        query1.BindByName = true;
                        OracleCommand query2 = _Conn.CreateCommand();
                        query2.BindByName = true;

                        if (_role == "TruongKhoa")
                        {
                            query1.CommandText = """
                                                    INSERT INTO admin.UV_TRGKHOA_PHANCONG(MAGV, MAHP, HK, NAM, MACT)
                                                    values(:magv, :mahp, :hk, :nam, :mact)
                                                """;

                            query2.CommandText = """
                                                    INSERT INTO admin.UV_TRGKHOA_PHANCONG(MAGV, MAHP, HK, NAM, MACT)
                                                    values(:magv, :mahp, :hk, :nam, :mact)
                                                 """;
                        }
                        else if (_role == "TruongDonVi")
                        {
                            query1.CommandText = """
                                                    INSERT INTO ADMIN.PROJECT_KHMO (MAHP,HK,NAM,MACT)
                                                    VALUES(:mahp, :hk, :nam, :mact)
                                                 """;

                            query2.CommandText = """
                                                    INSERT INTO admin.UV_TRGDONVI_PHANCONG(MAGV, MAHP, HK, NAM, MACT)
                                                    values(:magv, :mahp, :hk, :nam, :mact)
                                                 """;
                        }
                        
                        query1.CommandType = CommandType.Text;
                        query1.Parameters.Add(new OracleParameter(":mahp", _newMaHP));
                        query1.Parameters.Add(new OracleParameter(":hk", _newHK));
                        query1.Parameters.Add(new OracleParameter(":nam", _newNam));
                        query1.Parameters.Add(new OracleParameter(":mact", _newMaCT));

                        query2.CommandType = CommandType.Text;
                        query2.Parameters.Add(new OracleParameter(":magv", _newMaGV));
                        query2.Parameters.Add(new OracleParameter(":mahp", _newMaHP));
                        query2.Parameters.Add(new OracleParameter(":hk", _newHK));
                        query2.Parameters.Add(new OracleParameter(":nam", _newNam));
                        query2.Parameters.Add(new OracleParameter(":mact", _newMaCT));

                        try
                        {
                            query1.ExecuteNonQuery();
                            query2.ExecuteNonQuery();
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
