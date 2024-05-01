using AdminMonitor.NVCOBAN;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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

namespace AdminMonitor.GIAOVIEN
{
    /// <summary>
    /// Interaction logic for GiaoVienScreen.xaml
    /// </summary>
    public partial class GiaoVienScreen : Window
    {
        public OracleConnection con;
        DataTable empDT;
        DataTable pcDT;


        string DisplayMode = "";


        DataTable svDT;
        DataTable dvDT;
        DataTable hpDT;
        DataTable khmDT;

        List<int> rowPerPageOptions = new List<int>() {
            4,16,32,64,128,256,512,1024
        };
        string _name;

        int _rowsPerPage = 15;
        int _currentPage = 1;
        int totalPages = -1;
        int totalItems = -1;
        public GiaoVienScreen(OracleConnection connectionstring)
        {
            InitializeComponent();
            con = connectionstring;
        }

        private void ThongBaoButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new ThongBaoWins(con);
            screen.Show();
        }

        private void GetEmpInfor()
        {
            try
            {

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                       SELECT *
                                       FROM admin.UV_TTCANHAN_NHANSU
                                    """;


                query.CommandType = CommandType.Text;
                OracleDataReader datareader = query.ExecuteReader();

                empDT = new DataTable();
                empDT.Load(datareader);

                dataGridViewEmpInfor.ItemsSource = empDT.AsDataView();
                _name = empDT.Rows[0]["HOTEN"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void GetPHANCONGInfor()
        {
            try
            {

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                       SELECT MAHP, HK, NAM, MACT
                                       FROM admin.UV_GIANGVIEN_PHANCONG
                                    """;


                query.CommandType = CommandType.Text;
                OracleDataReader datareader = query.ExecuteReader();

                pcDT = new DataTable();
                pcDT.Load(datareader);

                dataGridViewPHANCONGInfor.ItemsSource = pcDT.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void ChangePhoneNumbContextItem_Click(object sender, RoutedEventArgs e)
        {
            EditPhoneNumber screen = new EditPhoneNumber(con, _name);
            screen.ShowDialog();
        }

        private void GiaoVienScreen_Closed(object sender, EventArgs e)
        {
            if (con != null)
            {
                con.Close();
            }
        }

        private void GiaoVienScreen_Loaded(object sender, RoutedEventArgs e)
        {
            GetEmpInfor();
            GetPHANCONGInfor();
            MAGVLabel.Content= _name;
            rowPerPageOptionsComboBox.ItemsSource = rowPerPageOptions;
            rowPerPageOptionsComboBox.SelectedIndex = 1;
            ViewDangKyRadioButton.IsChecked = true;
        }

        private void GetDangKy(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        SELECT MASV, MAHP, HK, NAM, MACT, DIEMTH, DIEMQT, 
                                        DIEMCK, DIEMTK, count(*) over() as "TotalSinhVien"
                                        FROM admin.PROJECT_DANGKI
                                        order by MASV
                                        offset :Skip rows 
                                        fetch next :Take rows only
                                    """;


                query.CommandType = CommandType.Text;
                query.Parameters.Add(new OracleParameter("Skip", skip));
                query.Parameters.Add(new OracleParameter("Take", take));


                OracleDataReader datareader = query.ExecuteReader();
                svDT = new DataTable();
                svDT.Load(datareader);
                if (totalItems == -1 && svDT.Rows.Count > 0)
                {
                    totalItems = int.Parse(svDT.Rows[0]["TotalSinhVien"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                svDT.Columns.Remove("TotalSinhVien");
                dataGridView.ItemsSource = svDT.DefaultView;
                DisplayMode = "DangKy";

                PageCountTextBox.Text = $" {_currentPage}/{totalPages} ";
                TotalItemDisplayTextBox.Text = $" of {totalItems} item(s).";

                if (_currentPage == totalPages)
                {
                    NextButton.IsEnabled = false;
                }
                else
                {
                    NextButton.IsEnabled = true;
                }

                if (_currentPage == 1)
                {
                    PrevButton.IsEnabled = false;
                }
                else
                {
                    PrevButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void getDangKyButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetDangKy(_currentPage, _rowsPerPage);
        }

        private void CapNhatDuLieuSinhVien_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridView.SelectedItems[0];
            string MSSV = (string)row.Row.ItemArray[0];

            decimal DTH = (decimal)row.Row.ItemArray[5];
            decimal DQT = (decimal)row.Row.ItemArray[6];
            decimal DCK = (decimal)row.Row.ItemArray[7];
            decimal DTK = (decimal)row.Row.ItemArray[8];
            CapNhatDuLieuSinhVienScreen screen = new(con, MSSV, DTH, DQT, DCK, DTK);
            this.Hide();
            screen.ShowDialog();
            GetDangKy(_currentPage, _rowsPerPage);
            this.Show();
        }

        private void GetSinhVien(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        select MASV, HOTEN, PHAI, NGSINH, DIACHI, DT, MACT, 
                                        MANGANH, SOTCTL, DTBTL, count(*) over() as "TotalSinhVien"
                                        from ADMIN.PROJECT_SINHVIEN
                                        order by MASV
                                        offset :Skip rows 
                                        fetch next :Take rows only
                                    """;


                query.CommandType = CommandType.Text;
                query.Parameters.Add(new OracleParameter("Skip", skip));
                query.Parameters.Add(new OracleParameter("Take", take));


                OracleDataReader datareader = query.ExecuteReader();
                svDT = new DataTable();
                svDT.Load(datareader);
                if (totalItems == -1 && svDT.Rows.Count > 0)
                {
                    totalItems = int.Parse(svDT.Rows[0]["TotalSinhVien"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                svDT.Columns.Remove("TotalSinhVien");
                dataGridView.ItemsSource = svDT.DefaultView;
                DisplayMode = "SinhVien";

                PageCountTextBox.Text = $" {_currentPage}/{totalPages} ";
                TotalItemDisplayTextBox.Text = $" of {totalItems} item(s).";

                if (_currentPage == totalPages)
                {
                    NextButton.IsEnabled = false;
                }
                else
                {
                    NextButton.IsEnabled = true;
                }

                if (_currentPage == 1)
                {
                    PrevButton.IsEnabled = false;
                }
                else
                {
                    PrevButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void getSinhVienButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetSinhVien(_currentPage, _rowsPerPage);
            CapNhatDuLieuSinhVienContextItem.IsEnabled = false;
        }

        private void GetDonVi(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        select MADV, TENDV, TRGDV,
                                        count(*) over() as "TotalDonVi"
                                        from ADMIN.PROJECT_DONVI
                                        order by MADV
                                        offset :Skip rows 
                                        fetch next :Take rows only
                                    """;


                query.CommandType = CommandType.Text;
                query.Parameters.Add(new OracleParameter("Skip", skip));
                query.Parameters.Add(new OracleParameter("Take", take));


                OracleDataReader datareader = query.ExecuteReader();
                dvDT = new DataTable();
                dvDT.Load(datareader);
                if (totalItems == -1 && dvDT.Rows.Count > 0)
                {
                    totalItems = int.Parse(dvDT.Rows[0]["TotalDonVi"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                dvDT.Columns.Remove("TotalDonVi");
                dataGridView.ItemsSource = dvDT.DefaultView;
                DisplayMode = "DonVi";

                PageCountTextBox.Text = $" {_currentPage}/{totalPages} ";
                TotalItemDisplayTextBox.Text = $" of {totalItems} item(s).";

                if (_currentPage == totalPages)
                {
                    NextButton.IsEnabled = false;
                }
                else
                {
                    NextButton.IsEnabled = true;
                }

                if (_currentPage == 1)
                {
                    PrevButton.IsEnabled = false;
                }
                else
                {
                    PrevButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void getDonViButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetDonVi(_currentPage, _rowsPerPage);
            CapNhatDuLieuSinhVienContextItem.IsEnabled = false;
        }

        private void GetHocPhan(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        select MAHP, TENHP, SOTC, STLT, STTH, SOSVTD, 
                                        MADV, count(*) over() as "TotalHocPhan"
                                        from ADMIN.PROJECT_HOCPHAN
                                        order by MAHP
                                        offset :Skip rows 
                                        fetch next :Take rows only
                                    """;


                query.CommandType = CommandType.Text;
                query.Parameters.Add(new OracleParameter("Skip", skip));
                query.Parameters.Add(new OracleParameter("Take", take));


                OracleDataReader datareader = query.ExecuteReader();
                hpDT = new DataTable();
                hpDT.Load(datareader);
                if (totalItems == -1 && hpDT.Rows.Count > 0)
                {
                    totalItems = int.Parse(hpDT.Rows[0]["TotalHocPhan"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                hpDT.Columns.Remove("TotalHocPhan");
                dataGridView.ItemsSource = hpDT.DefaultView;
                DisplayMode = "HocPhan";

                PageCountTextBox.Text = $" {_currentPage}/{totalPages} ";
                TotalItemDisplayTextBox.Text = $" of {totalItems} item(s).";

                if (_currentPage == totalPages)
                {
                    NextButton.IsEnabled = false;
                }
                else
                {
                    NextButton.IsEnabled = true;
                }

                if (_currentPage == 1)
                {
                    PrevButton.IsEnabled = false;
                }
                else
                {
                    PrevButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void getHocPhanButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetHocPhan(_currentPage, _rowsPerPage);
            CapNhatDuLieuSinhVienContextItem.IsEnabled = false;
        }

        private void GetKHMo(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        select MAHP, HK, NAM, MACT,
                                        count(*) over() as "TotalKHMo"
                                        from ADMIN.PROJECT_KHMO
                                        order by MAHP, NAM, HK
                                        offset :Skip rows 
                                        fetch next :Take rows only
                                    """;


                query.CommandType = CommandType.Text;
                query.Parameters.Add(new OracleParameter("Skip", skip));
                query.Parameters.Add(new OracleParameter("Take", take));


                OracleDataReader datareader = query.ExecuteReader();
                khmDT = new DataTable();
                khmDT.Load(datareader);
                if (totalItems == -1 && khmDT.Rows.Count > 0)
                {
                    totalItems = int.Parse(khmDT.Rows[0]["TotalKHMo"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                khmDT.Columns.Remove("TotalKHMo");
                dataGridView.ItemsSource = khmDT.DefaultView;
                DisplayMode = "KHMo";

                PageCountTextBox.Text = $" {_currentPage}/{totalPages} ";
                TotalItemDisplayTextBox.Text = $" of {totalItems} item(s).";

                if (_currentPage == totalPages)
                {
                    NextButton.IsEnabled = false;
                }
                else
                {
                    NextButton.IsEnabled = true;
                }

                if (_currentPage == 1)
                {
                    PrevButton.IsEnabled = false;
                }
                else
                {
                    PrevButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void getKHMoButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetKHMo(_currentPage, _rowsPerPage);
            CapNhatDuLieuSinhVienContextItem.IsEnabled = false;
        }

        private void rowPerPageOptionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rowsPerPage = (int)rowPerPageOptionsComboBox.SelectedItem;
            _currentPage = 1;

            if (totalItems != -1)
            {
                if (DisplayMode == "DangKy")
                {
                    getDangKyButton_Click(sender, e);
                }
                if (DisplayMode == "SinhVien")
                {
                    getSinhVienButton_Click(sender, e);
                }
                else if (DisplayMode == "DonVi")
                {
                    getDonViButton_Click(sender, e);
                }
                else if (DisplayMode == "HocPhan")
                {
                    getHocPhanButton_Click(sender, e);
                }
                else if (DisplayMode == "KHMo")
                {
                    getKHMoButton_Click(sender, e);
                }
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                if (DisplayMode == "DangKy")
                {
                    GetDangKy(_currentPage, _rowsPerPage);
                }
                if (DisplayMode == "SinhVien")
                {
                    GetSinhVien(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "DonVi")
                {
                    GetDonVi(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "HocPhan")
                {
                    GetHocPhan(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "KHMo")
                {
                    GetKHMo(_currentPage, _rowsPerPage);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < totalPages)
            {
                _currentPage++;
                if (DisplayMode == "DangKy")
                {
                    GetDangKy(_currentPage, _rowsPerPage);
                }
                if (DisplayMode == "SinhVien")
                {
                    GetSinhVien(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "DonVi")
                {
                    GetDonVi(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "HocPhan")
                {
                    GetHocPhan(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "KHMo")
                {
                    GetKHMo(_currentPage, _rowsPerPage);
                }
            }
        }
    }
}
