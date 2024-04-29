using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.SINHVIEN
{
    internal class Controler_SinhVien
    {
        static public SinhVien? GetSinhVien(OracleConnection Conn, string MASV)
        {
            SinhVien? result = null;

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.PROJECT_SINHVIEN
                                    WHERE MASV = :masv
                                    FETCH FIRST 1 ROWS ONLY
                                    """;
            query.CommandType = CommandType.Text;
            query.Parameters.Add(new OracleParameter("masv", MASV));
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                DataRow dataRowView = table.Rows[0];
                result = new SinhVien()
                {
                    MASV = (string)dataRowView["MASV"],
                    HoTen = (string)dataRowView["HOTEN"],
                    GioiTinh = (string)dataRowView["PHAI"],
                    NgaySinh = (DateTime)dataRowView["NGSINH"],
                    DiaChi = (string)dataRowView["DIACHI"],
                    SDT = (string)dataRowView["DT"],
                    MaChuongTrinh = (string)dataRowView["MACT"],
                    MaNganh = (string)dataRowView["MANGANH"],
                    CoSo = (string)dataRowView["COSO"],
                    DTBTL = (double)(decimal)dataRowView["DTBTL"],
                    SOTCTL = (int)(decimal)dataRowView["SOTCTL"]
                };
            }catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
            return result;
        }

        static public void UpdateSinhVien(OracleConnection Conn, string MASV, string SDT, string DiaChi)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    UPDATE ADMIN.PROJECT_SINHVIEN SET DT = :sdt, DIACHI = :diachi 
                                    """;
            query.CommandType = CommandType.Text;
            //query.Parameters.Add(new OracleParameter("masv", MASV));
            query.Parameters.Add(new OracleParameter("sdt", SDT));
            query.Parameters.Add(new OracleParameter("diachi", DiaChi));
            try
            {
                int count = query.ExecuteNonQuery();
            }catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
