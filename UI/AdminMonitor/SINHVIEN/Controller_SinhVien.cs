using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.SINHVIEN
{
    internal class Controller_SinhVien
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

        static public List<SinhVien>? GetAllSinhVien(OracleConnection Conn)
        {
            List<SinhVien>? result = new();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.PROJECT_SINHVIEN
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {
                    result.Add(new SinhVien()
                    {
                        MASV = (string)row["MASV"],
                        HoTen = (string)row["HOTEN"],
                        GioiTinh = (string)row["PHAI"],
                        NgaySinh = (row["NGSINH"] != DBNull.Value) ? (DateTime)row["NGSINH"] : null,
                        DiaChi = (string)row["DIACHI"],
                        SDT = (string)row["DT"],
                        MaChuongTrinh = (string)row["MACT"],
                        MaNganh = (string)row["MANGANH"],
                        CoSo = (string)row["COSO"],
                        DTBTL = (row["DTBTL"] != DBNull.Value) ? (double)(decimal)row["DTBTL"] : 0,
                        SOTCTL = (row["SOTCTL"] != DBNull.Value) ? (int)(decimal)row["SOTCTL"] : 0
                    });
                }
                
            }
            catch (Exception ex)
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

        static public void InsertSinhVien(OracleConnection Conn, SinhVien data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    insert into ADMIN.PROJECT_SINHVIEN (MASV,HOTEN,PHAI,NGSINH,DIACHI,DT,MACT,MANGANH,COSO)
                                    values (:masv,:hoten,:phai,:ngsinh,:diachi,:sdt,:mact,:manganh,:coso)
                                    """;
            query.CommandType = CommandType.Text;

            query.Parameters.Add(new OracleParameter("masv", data.MASV));
            query.Parameters.Add(new OracleParameter("hoten", data.HoTen));
            query.Parameters.Add(new OracleParameter("phai", data.GioiTinh));
            query.Parameters.Add(new OracleParameter("ngsinh", OracleDbType.Date) { Value = data.NgaySinh});
            query.Parameters.Add(new OracleParameter("diachi", data.DiaChi));
            query.Parameters.Add(new OracleParameter("sdt", data.SDT));
            query.Parameters.Add(new OracleParameter("mact", data.MaChuongTrinh));
            query.Parameters.Add(new OracleParameter("coso", data.CoSo));
            query.Parameters.Add(new OracleParameter("manganh", data.MaNganh));

            try
            {
                int count = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        static public void UpdateSinhVien(OracleConnection Conn, SinhVien data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    update ADMIN.PROJECT_SINHVIEN
                                    set HOTEN = :hoten, PHAI = :phai, NGSINH = :ngsinh, DIACHI = :diachi, DT = :sdt, MACT = :mact, MANGANH = :manganh, COSO = :coso,SOTCTL = :sotctl,  DTBTL = :dtbtl
                                    where MASV = :masv
                                    """;
            query.CommandType = CommandType.Text; //
            query.BindByName = true;

            query.Parameters.Add(new OracleParameter("hoten", data.HoTen));
            query.Parameters.Add(new OracleParameter("phai", data.GioiTinh));
            query.Parameters.Add(new OracleParameter("ngsinh", OracleDbType.Date) { Value = data.NgaySinh });
            query.Parameters.Add(new OracleParameter("diachi", data.DiaChi));
            query.Parameters.Add(new OracleParameter("sdt", data.SDT));
            query.Parameters.Add(new OracleParameter("mact", data.MaChuongTrinh));
            query.Parameters.Add(new OracleParameter("manganh", data.MaNganh));
            query.Parameters.Add(new OracleParameter("coso", data.CoSo));
            query.Parameters.Add(new OracleParameter("masv", data.MASV));
            query.Parameters.Add(new OracleParameter("sotctl", data.SOTCTL));
            query.Parameters.Add(new OracleParameter("dtbtl", OracleDbType.BinaryDouble) { Value = data.DTBTL });

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
