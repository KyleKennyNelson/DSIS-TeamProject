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
    }
}
