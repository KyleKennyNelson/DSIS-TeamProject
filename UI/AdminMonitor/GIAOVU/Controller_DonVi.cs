using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminMonitor.GIAOVU
{
    public class Controller_DonVi
    {
        public static List<DonVi> getAllDonVi(OracleConnection Conn)
        {
            var result = new List<DonVi>();

            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    SELECT *
                                    FROM ADMIN.PROJECT_DONVI
                                    """;
            query.CommandType = CommandType.Text;
            try
            {
                OracleDataReader datareader = query.ExecuteReader();

                var table = new DataTable();
                table.Load(datareader);

                foreach (DataRow row in table.Rows)
                {

                    var newDonVi = new DonVi()
                    {
                        MaDV = (string)row["MADV"],
                        TenDonVi = (string)row["TENDV"],
                        TruongDonVi = (string)row["TRGDV"],
                        CoSo = (string)row["COSO"]
                    };
                    result.Add(newDonVi);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return result;
        }
        public static void InsertDonVi(OracleConnection Conn, DonVi data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    insert into ADMIN.PROJECT_DONVI (MADV,TENDV,TRGDV,COSO)
                                    values (:madv,:tendv,:trgdv,:coso)
                                    """;
            query.CommandType = CommandType.Text;
            query.BindByName = true;

            query.Parameters.Add(new OracleParameter("madv", data.MaDV));
            query.Parameters.Add(new OracleParameter("tendv", data.TenDonVi));
            query.Parameters.Add(new OracleParameter("trgdv", data.TruongDonVi));
            query.Parameters.Add(new OracleParameter("coso", data.CoSo));

            try
            {
                int count = query.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static void UpdateDonVi(OracleConnection Conn, DonVi data)
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            OracleCommand query = Conn.CreateCommand();
            query.CommandText = """
                                    update ADMIN.PROJECT_DONVI
                                    set TENDV = :tendv, TRGDV = :trgdv, COSO = :coso
                                    where MADV = :madv
                                    """;
            query.CommandType = CommandType.Text;
            query.BindByName = true;

            query.Parameters.Add(new OracleParameter("madv", data.MaDV));
            query.Parameters.Add(new OracleParameter("tendv", data.TenDonVi));
            query.Parameters.Add(new OracleParameter("trgdv", data.TruongDonVi));
            query.Parameters.Add(new OracleParameter("coso", data.CoSo));

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
