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
using static FsCheck.TestResult;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AdminMonitor.TRUONGDONVI
{
    /// <summary>
    /// Interaction logic for QuanLyPhanCongScreen.xaml
    /// </summary>
    public partial class QuanLyPhanCongScreen : Window
    {
        public QuanLyPhanCongScreen(OracleConnection con)
        {
            InitializeComponent();
            _con = con;
        }
        public OracleConnection _con { get; set; }
        DataTable pcDT = new DataTable();

        List<int> rowPerPageOptions = new List<int>() {
            4,16,32,64,128,256,512,1024
        };

        int _rowsPerPage = 15;
        int _currentPage = 1;
        int totalPages = -1;
        int totalItems = -1;

        private void QLPC_Loaded(object sender, RoutedEventArgs e)
        {
            GetPHANCONGInfor(_currentPage, _rowsPerPage);
            rowPerPageOptionsComboBox.ItemsSource = rowPerPageOptions;
            rowPerPageOptionsComboBox.SelectedIndex = 1;
        }

        private void GetPHANCONGInfor(int page, int rowsPerPage)
        {
            try
            {
                int skip = (page - 1) * rowsPerPage;
                int take = rowsPerPage;

                OracleCommand query = _con.CreateCommand();
                query.CommandText = """
                                       SELECT MAGV, MAHP, HK, NAM, MACT, count(*) over() as "TotalItems"
                                       FROM admin.UV_TRGDONVI_PHANCONG
                                       Order by MAHP, NAM, HK
                                       offset :Skip rows
                                       fetch next :Take rows only
                                    """;


                query.CommandType = CommandType.Text;
                query.Parameters.Add(new OracleParameter("Skip", skip));
                query.Parameters.Add(new OracleParameter("Take", take));
                OracleDataReader datareader = query.ExecuteReader();

                pcDT = new DataTable();
                pcDT.Load(datareader);

                if (totalItems == -1 && pcDT.Rows.Count > 0)
                {
                    if (pcDT.Rows[0] != null)
                    {
                        totalItems = int.Parse(pcDT.Rows[0]["TotalItems"].ToString());
                    }

                    totalPages = (totalItems / rowsPerPage);
                    if (totalItems % rowsPerPage == 0) totalPages = (totalItems / rowsPerPage);
                    else totalPages = (int)(totalItems / rowsPerPage) + 1;
                }

                pcDT.Columns.Remove("TotalItems");
                dataGridViewPHANCONGInfor.ItemsSource = pcDT.AsDataView();

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

        private void AddPhanCong_Click(object sender, RoutedEventArgs e)
        {
            string mode = "Add";
            var screen = new QuanLyDataPHANCONG(_con, null, null, 0, 0, null, mode);
            screen.ShowDialog();
            GetPHANCONGInfor(_currentPage, _rowsPerPage);
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
            var screen = new QuanLyDataPHANCONG(_con, MAGV, MAHP, HK, NAM, MACT, mode);
            screen.ShowDialog();
            GetPHANCONGInfor(_currentPage, _rowsPerPage);
        }


        private void DeletePhanCong_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridViewPHANCONGInfor.SelectedItems[0];
            string MAGV = (string)row.Row.ItemArray[0];
            string MAHP = (string)row.Row.ItemArray[1];
            decimal HK = (decimal)row.Row.ItemArray[2];
            decimal NAM = (decimal)row.Row.ItemArray[3];
            string MACT = (string)row.Row.ItemArray[4];


            OracleCommand query = _con.CreateCommand();
            query.CommandText = """
                                    DELETE FROM admin.UV_TRGDONVI_PHANCONG
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
                GetPHANCONGInfor(_currentPage, _rowsPerPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                return;
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                GetPHANCONGInfor(_currentPage, _rowsPerPage);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < totalPages)
            {
                _currentPage++;
                GetPHANCONGInfor(_currentPage, _rowsPerPage);
            }
        }

        private void rowPerPageOptionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rowsPerPage = (int)rowPerPageOptionsComboBox.SelectedItem;
            _currentPage = 1;
            totalItems = -1;
            GetPHANCONGInfor(_currentPage, _rowsPerPage);
        }
    }
}
