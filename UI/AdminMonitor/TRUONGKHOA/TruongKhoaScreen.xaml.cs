using AdminMonitor.GIAOVIEN;
using AdminMonitor.NVCOBAN;
using AdminMonitor.TRUONGDONVI;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

namespace AdminMonitor.TRUONGKHOA
{
    /// <summary>
    /// Interaction logic for TruongKhoaScreen.xaml
    /// </summary>
    public partial class TruongKhoaScreen : Window
    {
        public TruongKhoaScreen(OracleConnection connectionstring)
        {
            InitializeComponent();
            con = connectionstring;
        }
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
                                       SELECT MAGV, MAHP, HK, NAM, MACT
                                       FROM admin.UV_TRGKHOA_PHANCONG
                                       Order by MAHP, NAM, HK
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

        private void AddPhanCong_Click(object sender, RoutedEventArgs e)
        {
            string mode = "Add";
            string role = "TruongKhoa";
            var screen = new QuanLyDataPHANCONG(con, null, null, 0, 0, null, mode, role);
            screen.ShowDialog();
            GetPHANCONGInfor();
        }

        private void UpdatePhanCong_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridViewPHANCONGInfor.SelectedItems[0];
            string MAGV = (string)row.Row.ItemArray[0];
            string MAHP = (string)row.Row.ItemArray[1];
            decimal HK = (decimal)row.Row.ItemArray[2];
            decimal NAM = (decimal)row.Row.ItemArray[3];
            string MACT = (string)row.Row.ItemArray[4];
            string mode = "Update";
            string role = "TruongKhoa";
            var screen = new QuanLyDataPHANCONG(con, MAGV, MAHP, HK, NAM, MACT, mode, role);
            screen.ShowDialog();
            GetPHANCONGInfor();
        }


        private void DeletePhanCong_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridViewPHANCONGInfor.SelectedItems[0];
            string MAGV = (string)row.Row.ItemArray[0];
            string MAHP = (string)row.Row.ItemArray[1];
            decimal HK = (decimal)row.Row.ItemArray[2];
            decimal NAM = (decimal)row.Row.ItemArray[3];
            string MACT = (string)row.Row.ItemArray[4];


            OracleCommand query = con.CreateCommand();
            query.CommandText = """
                                    DELETE FROM admin.UV_TRGKHOA_PHANCONG
                                    WHERE MAGV = :magv
                                        and MAHP = :mahp
                                        and HK = :hk
                                        and NAM = :nam
                                        and MACT = :mact
                                """;
            query.CommandType = CommandType.Text;
            query.Parameters.Add(new OracleParameter(":magv", MAGV));
            query.Parameters.Add(new OracleParameter(":mahp", MAHP));
            query.Parameters.Add(new OracleParameter(":hk", Convert.ToInt32(HK)));
            query.Parameters.Add(new OracleParameter(":nam", Convert.ToInt32(NAM)));
            query.Parameters.Add(new OracleParameter(":mact", MACT));

            try
            {
                query.ExecuteNonQuery();
                GetPHANCONGInfor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                return;
            }
        }

        private void ChangePhoneNumbContextItem_Click(object sender, RoutedEventArgs e)
        {
            EditPhoneNumber screen = new EditPhoneNumber(con, _name);
            screen.ShowDialog();
        }

        private void TruongKhoaScreen_Closed(object sender, EventArgs e)
        {
            if (con != null)
            {
                con.Close();
            }
        }

        private void TruongKhoaScreen_Loaded(object sender, RoutedEventArgs e)
        {
            GetEmpInfor();
            GetPHANCONGInfor();
            MATDVLabel.Content = _name;
            rowPerPageOptionsComboBox.ItemsSource = rowPerPageOptions;
            rowPerPageOptionsComboBox.SelectedIndex = 1;
            ViewNhanSuRadioButton.IsChecked = true;
        }

        private void GetNhanSu(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        SELECT MANV, HOTEN, PHAI, NGSINH, PHUCAP, DT, VAITRO,
                                        DONVI, COSO, count(*) over() as "TotalNhanSu"
                                        FROM admin.Project_NHANSU
                                        order by MANV
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
                    totalItems = int.Parse(svDT.Rows[0]["TotalNhanSu"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                svDT.Columns.Remove("TotalNhanSu");
                dataGridView.ItemsSource = svDT.DefaultView;
                DisplayMode = "NhanSu";

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

        private void getNhanSuButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetNhanSu(_currentPage, _rowsPerPage);
            CapNhatDuLieuSinhVienContextItem.IsEnabled = false;
            ThemDuLieuNhanSuContextItem.IsEnabled = true;
            XoaDuLieuNhanSuContextItem.IsEnabled = true;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = true;
        }

        private void ThemDuLieuNhanSu_Click(object sender, RoutedEventArgs e)
        {
            string mode = "Add";
            var screen = new QuanLyDataNhanSu(con, null, null, null, DateTime.Now, 0, null, null, null, null, mode);
            screen.ShowDialog();
            GetNhanSu(_currentPage, _rowsPerPage);
        }

        private void CapNhatDuLieuNhanSu_Click(object sender, RoutedEventArgs e)
        {
            string mode = "Update";
            DataRowView row = (DataRowView)dataGridView.SelectedItems[0];
            string MANV = (string)row.Row.ItemArray[0];
            string HOTEN = (string)row.Row.ItemArray[1];
            string PHAI = (string)row.Row.ItemArray[2];
            DateTime NGSINH = (DateTime)row.Row.ItemArray[3];
            int PHUCAP = Convert.ToInt32((decimal)row.Row.ItemArray[4]);
            string DT = (string)row.Row.ItemArray[5];
            string VAITRO = (string)row.Row.ItemArray[6];
            string DONVI = (string)row.Row.ItemArray[7];
            string COSO = (string)row.Row.ItemArray[8];

            var screen = new QuanLyDataNhanSu(con, MANV, HOTEN, PHAI, NGSINH, PHUCAP, DT,
                                              VAITRO, DONVI, COSO, mode);
            screen.ShowDialog();
            GetNhanSu(_currentPage, _rowsPerPage);
        }


        private void XoaDuLieuNhanSu_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridView.SelectedItems[0];
            string MANV = (string)row.Row.ItemArray[0];
            //string HOTEN = (string)row.Row.ItemArray[1];
            //string PHAI = (string)row.Row.ItemArray[2];
            //DateTime NGSINH = (DateTime)row.Row.ItemArray[3];
            //int PHUCAP = Convert.ToInt32((decimal)row.Row.ItemArray[4]);
            //string DT = (string)row.Row.ItemArray[5];
            //string VAITRO = (string)row.Row.ItemArray[6];
            //string DONVI = (string)row.Row.ItemArray[7];
            //string COSO = (string)row.Row.ItemArray[8];


            OracleCommand query = con.CreateCommand();
            query.CommandText = """
                                    DELETE FROM admin.PROJECT_NHANSU
                                    WHERE MANV = :manv
                                """;
            query.CommandType = CommandType.Text;
            query.Parameters.Add(new OracleParameter(":manv", MANV));

            try
            {
                query.ExecuteNonQuery();
                GetNhanSu(_currentPage, _rowsPerPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                return;
            }
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
                                        DIEMCK, DIEMTK, count(*) over() as "TotalDangKy"
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
                    totalItems = int.Parse(svDT.Rows[0]["TotalDangKy"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                svDT.Columns.Remove("TotalDangKy");
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
            CapNhatDuLieuSinhVienContextItem.IsEnabled = true;
            ThemDuLieuNhanSuContextItem.IsEnabled = false;
            XoaDuLieuNhanSuContextItem.IsEnabled = false;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = false;
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
            ThemDuLieuNhanSuContextItem.IsEnabled = false;
            XoaDuLieuNhanSuContextItem.IsEnabled = false;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = false;
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
            ThemDuLieuNhanSuContextItem.IsEnabled = false;
            XoaDuLieuNhanSuContextItem.IsEnabled = false;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = false;
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
            ThemDuLieuNhanSuContextItem.IsEnabled = false;
            XoaDuLieuNhanSuContextItem.IsEnabled = false;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = false;
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
            ThemDuLieuNhanSuContextItem.IsEnabled = false;
            XoaDuLieuNhanSuContextItem.IsEnabled = false;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = false;
        }

        private void GetPhanCong(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = con.CreateCommand();
                query.CommandText = """
                                        select MAGV, MAHP, HK, NAM, MACT,
                                        count(*) over() as "TotalPhanCong"
                                        from ADMIN.Project_PHANCONG
                                        order by MAGV, MAHP, NAM, HK
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
                    totalItems = int.Parse(khmDT.Rows[0]["TotalPhanCong"].ToString());
                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }


                khmDT.Columns.Remove("TotalPhanCong");
                dataGridView.ItemsSource = khmDT.DefaultView;
                DisplayMode = "NhanSu";

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

        private void getPhanCongButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            totalItems = -1;
            GetPhanCong(_currentPage, _rowsPerPage);
            CapNhatDuLieuSinhVienContextItem.IsEnabled = false;
            ThemDuLieuNhanSuContextItem.IsEnabled = false;
            XoaDuLieuNhanSuContextItem.IsEnabled = false;
            CapNhatDuLieuNhanSuContextItem.IsEnabled = false;
        }

        private void rowPerPageOptionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rowsPerPage = (int)rowPerPageOptionsComboBox.SelectedItem;
            _currentPage = 1;

            if (totalItems != -1)
            {
                if (DisplayMode == "NhanSu")
                {
                    getNhanSuButton_Click(sender, e);
                }
                else if (DisplayMode == "SinhVien")
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
                else if (DisplayMode == "PhanCong")
                {
                    getPhanCongButton_Click(sender, e);
                }
                else if (DisplayMode == "DangKy")
                {
                    getDangKyButton_Click(sender, e);
                }
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                if (DisplayMode == "NhanSu")
                {
                    GetNhanSu(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "SinhVien")
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
                else if (DisplayMode == "PhanCong")
                {
                    GetPhanCong(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "DangKy")
                {
                    GetDangKy(_currentPage, _rowsPerPage);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < totalPages)
            {
                _currentPage++;
                if (DisplayMode == "NhanSu")
                {
                    GetNhanSu(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "SinhVien")
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
                else if (DisplayMode == "PhanCong")
                {
                    GetPhanCong(_currentPage, _rowsPerPage);
                }
                else if (DisplayMode == "DangKy")
                {
                    GetDangKy(_currentPage, _rowsPerPage);
                }
            }
        }
    }
}
