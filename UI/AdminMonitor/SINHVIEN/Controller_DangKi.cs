using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.SINHVIEN
{
    internal static class Controller_DangKi
    {
        public static List<DangKi> GetDangKi(OracleConnection Conn)
        {
            var result = new List<DangKi>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.PROJECT_DANGKI
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {
                    double DiemTH = 0;
                    double DiemQT = 0;
                    double DiemCK = 0;
                    double DiemTK = 0;

                    if (row["DIEMTH"] != DBNull.Value)
                    {
                        DiemTH = (double)(decimal)row["DIEMTH"];
                    }
                    if (row["DIEMQT"] != DBNull.Value)
                    {
                        DiemQT = (double)(decimal)row["DIEMQT"];
                    }
                    if (row["DIEMCK"] != DBNull.Value)
                    {
                        DiemCK = (double)(decimal)row["DIEMCK"];
                    }
                    if (row["DIEMTK"] != DBNull.Value)
                    {
                        DiemTK = (double)(decimal)row["DIEMTK"];
                    }

                    var newDangKi = new DangKi()
                    {
                        MaSV = (string)row["MASV"],
                        MaGV = (string)row["MAGV"],
                        MaHP = (string)row["MAHP"],
                        HK = (int)(decimal)row["HK"],
                        Nam = (int)(decimal)row["NAM"],
                        MaCT = (string)row["MACT"],

                        DiemTH = DiemTH,
                        DiemQT = DiemQT,
                        DiemCK = DiemCK,
                        DiemTK = DiemTK
                    };
                    result.Add(newDangKi);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }
        static public bool DeleteDangKi(OracleConnection Conn, string MASV, string MAHP, int HK, int NAM, string MACT)
        {
            bool result = false;
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    delete from ADMIN.PROJECT_DANGKI 
                                    where   MASV = :masv 
                                            and MAHP = :mahp 
                                            and HK = :hk
                                            and NAM = :nam 
                                            and MACT = :mact
                                    """;
            query.CommandType = CommandType.Text;
            //query.Parameters.Add(new OracleParameter("masv", MASV));
            query.Parameters.Add(new OracleParameter("masv", MASV));
            query.Parameters.Add(new OracleParameter("mahp", MAHP));
            query.Parameters.Add(new OracleParameter("hk", HK));
            query.Parameters.Add(new OracleParameter("nam", NAM));
            query.Parameters.Add(new OracleParameter("mact", MACT));
            try
            {
                int count = query.ExecuteNonQuery();
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }
        public static bool InsertDangKi(OracleConnection Conn, string MaSV, PhanCong data)
        {
            bool result = false;
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    insert into ADMIN.PROJECT_DANGKI(MASV,MAGV,MAHP,HK,NAM,MACT) 
                                    values (:masv,:magv,:mahp,:hk,:nam,:mact)
                                    """;
            query.CommandType = CommandType.Text;
            query.Parameters.Add(new OracleParameter("masv", MaSV));
            query.Parameters.Add(new OracleParameter("magv", data.MaGV));
            query.Parameters.Add(new OracleParameter("mahp", data.MaHP));
            query.Parameters.Add(new OracleParameter("hk", data.HK));
            query.Parameters.Add(new OracleParameter("nam", data.Nam));
            query.Parameters.Add(new OracleParameter("mact", data.MaCT));
            try
            {
                int count = query.ExecuteNonQuery();
                if(count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return result;
        }
    }
}
