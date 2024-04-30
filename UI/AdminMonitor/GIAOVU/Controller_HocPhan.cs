using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.GIAOVU
{
    public class Controller_HocPhan
    {
        public static List<HocPhan> getAllHocPhan(OracleConnection Conn)
        {
            var result = new List<HocPhan>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.PROJECT_HOCPHAN
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {

                    var newHocPhan = new HocPhan()
                    {
                        MaHP = (string)row["MAHP"],
                        TenHP = (string)row["TENHP"],
                        SoTinChi = (int)(decimal)row["SOTC"],
                        SoTietLiThuyet = (int)(decimal)row["STLT"],
                        SoTietThucHanh = (int)(decimal)row["STTH"],
                        SoSinhVienThamDu = (int)(decimal)row["SOSVTD"],
                        MaDV = (string)row["MADV"],
                    };
                    result.Add(newHocPhan);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }

        public static void InsertHocPhan(OracleConnection Conn, HocPhan data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    insert into ADMIN.PROJECT_HOCPHAN (MAHP,TENHP,SOTC,STLT,STTH,SOSVTD,MADV)
                                    values (:mahp,:tenhp,:sotc,:stlt,:stth,:sosvtd,:madv)
                                    """;
            query.CommandType = CommandType.Text;
            query.BindByName = true;

            query.Parameters.Add(new OracleParameter("mahp", data.MaHP));
            query.Parameters.Add(new OracleParameter("tenhp", data.TenHP));
            query.Parameters.Add(new OracleParameter("sotc", data.SoTinChi));
            query.Parameters.Add(new OracleParameter("stlt", data.SoTietLiThuyet));
            query.Parameters.Add(new OracleParameter("stth", data.SoTietThucHanh));
            query.Parameters.Add(new OracleParameter("sosvtd", data.SoSinhVienThamDu));
            query.Parameters.Add(new OracleParameter("madv", data.MaDV));

            try
            {
                int count = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static void UpdateHocPhan(OracleConnection Conn, HocPhan data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    update ADMIN.PROJECT_HOCPHAN
                                    set TENHP = :tenhp,SOTC = :sotc,STLT = :stlt,STTH = :stth,SOSVTD = :sosvtd,MADV = :madv
                                    where MAHP = :mahp
                                    """;
            query.CommandType = CommandType.Text;
            query.BindByName = true;

            query.Parameters.Add(new OracleParameter("mahp", data.MaHP));
            query.Parameters.Add(new OracleParameter("tenhp", data.TenHP));
            query.Parameters.Add(new OracleParameter("sotc", data.SoTinChi));
            query.Parameters.Add(new OracleParameter("stlt", data.SoTietLiThuyet));
            query.Parameters.Add(new OracleParameter("stth", data.SoTietThucHanh));
            query.Parameters.Add(new OracleParameter("sosvtd", data.SoSinhVienThamDu));
            query.Parameters.Add(new OracleParameter("madv", data.MaDV));

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
