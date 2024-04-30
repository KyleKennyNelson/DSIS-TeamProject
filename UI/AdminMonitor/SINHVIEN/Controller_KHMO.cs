using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.SINHVIEN
{
    public class Controller_KHMO
    {
        public static List<KHMO> GetKHMO(OracleConnection Conn)
        {
            var result = new List<KHMO>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    select * from ADMIN.PROJECT_KHMO
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {

                    var newKHMO = new KHMO()
                    {
                        MaHP = (string)row["MAHP"],
                        HK = (int)(decimal)row["HK"],
                        Nam = (int)(decimal)row["NAM"],
                        MaCT = (string)row["MACT"]
                    };
                    result.Add(newKHMO);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }
        public static void InsertKHMO(OracleConnection Conn, KHMO data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    insert into ADMIN.PROJECT_KHMO (MAHP,HK,NAM,MACT)
                                    values (:mahp,:hk,:nam,:mact)
                                    """;
            query.CommandType = CommandType.Text;
            query.BindByName = true;

            query.Parameters.Add(new OracleParameter("mahp", data.MaHP));
            query.Parameters.Add(new OracleParameter("hk", data.HK));
            query.Parameters.Add(new OracleParameter("nam", data.Nam));
            query.Parameters.Add(new OracleParameter("mact", data.MaCT));

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
