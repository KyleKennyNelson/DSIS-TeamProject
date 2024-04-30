using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.SINHVIEN
{
    internal class Controller_PhanCong
    {
        public static List<PhanCong> GetPhanCongs(OracleConnection Conn, string MaSV)
        {
            var result = new List<PhanCong>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT PC.*
                                    FROM ADMIN.PROJECT_PHANCONG PC
                                        inner join ADMIN.PROJECT_SINHVIEN SV on PC.MACT = SV.MACT
                                    where SV.MASV = :masv1
                                    except 
                                    select PC.*
                                    from ADMIN.PROJECT_PHANCONG PC
                                        inner join ADMIN.PROJECT_DANGKI DK on   PC.MAGV = DK.MAGV
                                                                                and PC.MAHP = DK.MAHP
                                                                                and PC.HK = DK.HK
                                                                                and PC.MACT = DK.MACT
                                                                                and PC.NAM = DK.NAM
                                    where DK.MASV = :masv2
                                    """;
            query.CommandType = CommandType.Text;
            query.Parameters.Add(new OracleParameter("masv1", MaSV));
            query.Parameters.Add(new OracleParameter("masv2", MaSV));
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {
                    result.Add(new PhanCong
                    {
                        MaGV = (string)row["MAGV"],
                        MaHP = (string)row["MAHP"],
                        HK = (int)(decimal)row["HK"],
                        Nam = (int)(decimal)row["NAM"],
                        MaCT = (string)row["MACT"]
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }

        public static List<PhanCong> GetPhanCongs(OracleConnection Conn)
        {
            var result = new List<PhanCong>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.PROJECT_PHANCONG
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {

                    var newPhanCong = new PhanCong()
                    {
                        MaGV = (string)row["MAGV"],
                        MaHP = (string)row["MAHP"],
                        HK = (int)(decimal)row["HK"],
                        Nam = (int)(decimal)row["NAM"],
                        MaCT = (string)row["MACT"],
                    };
                    result.Add(newPhanCong);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }

        static public bool DeletePhanCong(OracleConnection Conn, PhanCong data)
        {
            int count = 0;

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    delete from ADMIN.PROJECT_PHANCONG 
                                    where   MAGV = :magv
                                            and MAHP = :mahp
                                            and HK = :hk
                                            and NAM = :nam
                                            and MACT = :mact
                                    """;
            query.CommandType = CommandType.Text;
            query.BindByName = true;
            //query.Parameters.Add(new OracleParameter("masv", MASV));
            query.Parameters.Add(new OracleParameter("magv", data.MaGV));
            query.Parameters.Add(new OracleParameter("mahp", data.MaHP));
            query.Parameters.Add(new OracleParameter("hk", data.HK));
            query.Parameters.Add(new OracleParameter("nam", data.Nam));
            query.Parameters.Add(new OracleParameter("mact", data.MaCT));

            try
            {
                count = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            if(count == 0)
            {
                return false;
            }
            else { return true; }
        }
    }
}
