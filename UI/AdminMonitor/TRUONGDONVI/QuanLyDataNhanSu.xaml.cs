using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AdminMonitor.TRUONGDONVI
{
    /// <summary>
    /// Interaction logic for QuanLyDataNhanSu.xaml
    /// </summary>
    public partial class QuanLyDataNhanSu : Window
    {
        OracleConnection _Conn;
        private string _mode;

        private List<string> _MaNSList;
        private List<string> _MaCSList;

        private string _MaNV;
        private string _Hoten;
        private string _Phai;
        private DateTime _NgaySinh;
        private int _PhuCap;
        private string _DT;
        private string _VaiTro;
        private string _DonVi;
        private string _CoSo;

        private string _newMaNV;
        private string _newHoten;
        private string _newPhai;
        private DateTime _newNgaySinh;
        private int _newPhuCap;
        private string _newDT;
        private string _newVaiTro;
        private string _newDonVi;
        private string _newCoSo;
        public QuanLyDataNhanSu(OracleConnection connection, string MaNV, string Hoten, string Phai, DateTime NgaySinh,
                                int PhuCap, string DT, string VaiTro, string DonVi, string CoSo, string mode, List<string> MaNSList)
        {
            InitializeComponent();
            _MaNSList = MaNSList;
            _Conn = connection;
            _mode = mode;
            _MaNV = MaNV;
            _Hoten = Hoten;
            _Phai = Phai;
            _NgaySinh = NgaySinh;
            _PhuCap = PhuCap;
            _DT = DT;
            _VaiTro = VaiTro;
            _DonVi = DonVi;
            _CoSo = CoSo;
        }

        private async void QuanLyDataNhanSu_Loaded(object sender, RoutedEventArgs e)
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

                NewMaNSBox.Visibility = Visibility.Visible;
                NewMaNSBox.Text = _MaNV;
                NewMaNSBox.IsReadOnly = true;
                NewMaNSComboBox.Visibility = Visibility.Collapsed;

                NewHoTenBox.Text = _Hoten;
                //NewHoTenBox.IsReadOnly = true;

                NewPhaiBox.Text = _Phai;
                //NewPhaiBox.IsReadOnly = true;

                NewNgaySinhBox.Text = _NgaySinh.ToString("yyyy-MM-dd");
                //NewNgaySinhBox.IsReadOnly = true;

                NewPhuCapBox.Text = _PhuCap.ToString();
                //NewPhuCapBox.IsReadOnly = true;

                NewDTBox.Text = _DT;
                //NewDTBox.IsReadOnly = true;

                NewVaiTroBox.Text = _VaiTro;
                NewVaiTroBox.IsReadOnly = true;

                NewDonViBox.Text = _DonVi;
                NewDonViBox.IsReadOnly = true;

                NewCoSoBox.Text = _CoSo;
                //NewDonViBox.IsReadOnly = true;

            }
            else if (_mode == "Add")
            {
                ConfirmButton.Content = _mode;
                NewMaNSComboBox.ItemsSource = _MaNSList;
                NewMaNSBox.Visibility = Visibility.Collapsed;
                NewMaNSComboBox.Visibility = Visibility.Visible;

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

        private void NewDTBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string phonenumber = NewDTBox.Text;
                var r = new Regex("^\\d(\\d|(?<!-)-)*\\d$|^\\d$");
                if (r.IsMatch(phonenumber))
                    _newDT = phonenumber;
                else
                    MessageBox.Show("Input Wrong format for phone number", "Failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewPhuCapBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newPhuCap = Convert.ToInt32(NewPhuCapBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewNgaySinhBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newNgaySinh = DateTime.Parse(NewNgaySinhBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewHoTenBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newHoten = NewHoTenBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewPhaiBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newPhai = NewPhaiBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NewVaiTroBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _newVaiTro = NewVaiTroBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void NewDonViBox_TextChanged(object sender, EventArgs e)
        {
            try
            {

                _newDonVi = NewDonViBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void NewCoSoBox_TextChanged(object sender, EventArgs e)
        {
            try
            {

                _newCoSo = NewCoSoBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Input Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void NewMaNSComboBox_SelectionChanged(object sender, EventArgs e)
        {
            _newMaNV = (string)NewMaNSComboBox.SelectedItem;
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
                        OracleCommand query = _Conn.CreateCommand();
                        query.BindByName = true;
                        query.CommandText = """
                                                UPDATE admin.PROJECT_NHANSU
                                                SET HOTEN = :newhoten, PHAI = :newphai,
                                                    NGSINH = :newngaysinh, PHUCAP = :newphucap,
                                                    DT = :newdt, COSO = :newcoso
                                                WHERE MANV = :manv
                                            """;
                        
                        query.CommandType = CommandType.Text;
                        query.Parameters.Add(new OracleParameter(":newhoten", _newHoten));
                        query.Parameters.Add(new OracleParameter(":newphai", _newPhai));
                        query.Parameters.Add(new OracleParameter(":newngaysinh", _newNgaySinh));
                        query.Parameters.Add(new OracleParameter(":newphucap", _newPhuCap));
                        query.Parameters.Add(new OracleParameter(":newdt", _newDT));
                        query.Parameters.Add(new OracleParameter(":newcoso", _newCoSo));
                        query.Parameters.Add(new OracleParameter(":manv", _MaNV));

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
                    else if (_mode == "Add")
                    {
                        if (_Conn.State == ConnectionState.Closed)
                        {
                            _Conn.Open();
                        }
                        OracleCommand query = _Conn.CreateCommand();
                        query.BindByName = true;

                        query.CommandText = """
                                                INSERT INTO admin.PROJECT_NHANSU(MANV,HOTEN,PHAI,NGSINH,PHUCAP,
                                                                                DT,VAITRO,DONVI,COSO)
                                                values(:manv, :hoten, :phai, :ngsinh, :phucap, dt, vaitro, donvi, coso)
                                            """;

                        query.CommandType = CommandType.Text;
                        query.Parameters.Add(new OracleParameter(":manv", _newMaNV));
                        query.Parameters.Add(new OracleParameter(":hoten", NewHoTenBox.Text));
                        query.Parameters.Add(new OracleParameter(":phai", NewPhaiBox.Text));
                        query.Parameters.Add(new OracleParameter(":ngsinh", Convert.ToDateTime(NewNgaySinhBox.Text)));
                        query.Parameters.Add(new OracleParameter(":phucap", _newPhuCap));
                        query.Parameters.Add(new OracleParameter(":dt", _newDT));
                        query.Parameters.Add(new OracleParameter(":vaitro", NewVaiTroBox.Text));
                        query.Parameters.Add(new OracleParameter(":donvi", _newDonVi));
                        query.Parameters.Add(new OracleParameter(":coso", _newCoSo));

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
