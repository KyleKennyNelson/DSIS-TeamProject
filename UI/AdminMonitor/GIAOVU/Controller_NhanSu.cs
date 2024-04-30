using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.GIAOVU
{
    public class Controller_NhanSu
    {
        public static NhanSu? GetTTCaNhan(OracleConnection Conn, string MaNV)
        {
            NhanSu? result = null;

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.UV_TTCANHAN_NHANSU
                                    WHERE MANV = :manv
                                    FETCH FIRST 1 ROWS ONLY
                                    """;
            query.CommandType = CommandType.Text;
            query.Parameters.Add(new OracleParameter("manv", MaNV));
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                DataRow dataRowView = table.Rows[0];
                result = new NhanSu()
                {
                    MaNV = (string)dataRowView["MANV"],
                    HoTen = (string)dataRowView["HOTEN"],
                    GioiTinh = (string)dataRowView["PHAI"],
                    NgaySinh = (DateTime)dataRowView["NGSINH"],
                    SDT = (string)dataRowView["DT"],
                    CoSo = (string)dataRowView["COSO"],
                    VaiTro = (string)dataRowView["VAITRO"],
                    MaDV = (string)dataRowView["DONVI"],
                    PhuCap = (int)(decimal)dataRowView["PHUCAP"]

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }

        public static void UpdateSDT(OracleConnection Conn, string SDT)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    UPDATE ADMIN.UV_SDT_NHANSU SET DT = :sdt 
                                    """;
            query.CommandType = CommandType.Text;
            //query.Parameters.Add(new OracleParameter("masv", MASV));
            query.Parameters.Add(new OracleParameter("sdt", SDT));
            try
            {
                int count = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
