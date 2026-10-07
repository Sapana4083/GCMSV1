using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class RcsatClpParamsRepository : IRcsatClpParamsRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public RcsatClpParamsRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<RcsatClpParams>> SearchAsync(DateTime fromDate, DateTime toDate)
        {
            var list = new List<RcsatClpParams>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_CLP_PARAMS", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };

            cmd.Parameters.Add("P_FROM_DATE", OracleDbType.Date).Value = fromDate.Date;
            cmd.Parameters.Add("P_TO_DATE", OracleDbType.Date).Value = toDate.Date;
            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new RcsatClpParams
                    {
                        TrnRcsatClpParamsId = ToLong(reader["TRN_RCSAT_CLP_PARAMSID"]) ?? 0,
                        DocNo = reader["DOCNO"]?.ToString(),
                        DocDt = reader["DOCDT"] == DBNull.Value ? null : Convert.ToDateTime(reader["DOCDT"]),
                        IsActive = reader["ISACTIVE"]?.ToString(),
                        Remarks = reader["REMARKS"]?.ToString(),
                        CauseListType = reader["CAUSE_LIST_TYPE"]?.ToString(),
                        CcLimit = reader["CC_LIMIT"] == DBNull.Value ? null : Convert.ToInt32(reader["CC_LIMIT"]),
                        CourtCode = reader["COURT_CODE"]?.ToString(),
                        CreatedBy = reader["CREATEDBY"]?.ToString(),
                        CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"])
                    });
                }
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

            return list;
        }

        private static long? ToLong(object v)
        {
            if (v == null || v == DBNull.Value) return null;
            return v is OracleDecimal d ? d.ToInt64() : Convert.ToInt64(v);
        }
    }
}