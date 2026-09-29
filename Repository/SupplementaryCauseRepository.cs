using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class SupplementaryCauseRepository : ISupplementaryCauseRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public SupplementaryCauseRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<SupplementaryCause>> GetAllAsync()
        {
            var list = new List<SupplementaryCause>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();
            using var cmd = CreateCommand(conn, 4);
            AddNullParams(cmd);
            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(Map(reader));

            return list;
        }

        public async Task<SupplementaryCause?> GetByIdAsync(long supplyCauseListId)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(x => x.SupplyCauseListId == supplyCauseListId);
        }

        public async Task AddAsync(SupplementaryCause model, string courtCode, string createdBy)
        {
            await ExecuteAsync(1, model, courtCode, createdBy);
        }

        public async Task UpdateAsync(SupplementaryCause model, string courtCode, string createdBy)
        {
            await ExecuteAsync(2, model, courtCode, createdBy);
        }

        private async Task ExecuteAsync(int input, SupplementaryCause m, string courtCode, string createdBy)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();
            using var cmd = CreateCommand(conn, input);

            void P(string name, OracleDbType type, object? value) =>
                cmd.Parameters.Add(name, type).Value = value ?? DBNull.Value;

            P("P_TBL_SUPLTY_CAUSEID", OracleDbType.Int64, input == 2 ? m.TblSupltyCauseId : null);
            P("P_SUPPLY_CAUSE_LISTID", OracleDbType.Int64, input == 2 ? m.SupplyCauseListId : null);
            P("P_COURTNAME", OracleDbType.Int64, null);
            P("P_COURT_CODE", OracleDbType.Varchar2, courtCode);
            P("P_HEARING_DATE", OracleDbType.Date, m.HearingDate);
            P("P_CASETYPE", OracleDbType.Varchar2, m.CaseTypeName);
            P("P_CASETYPEID", OracleDbType.Int64, m.CaseTypeId);
            P("P_CASE_NO", OracleDbType.Varchar2, m.CaseNo);
            P("P_CTYPE", OracleDbType.Varchar2, m.CaseTypeName);
            P("P_PURP", OracleDbType.Varchar2, m.PurposeName);
            P("P_PURPOSE", OracleDbType.Int64, m.PurposeId);
            P("P_BENCHTYPE", OracleDbType.Varchar2, m.BenchType);
            P("P_HDATE", OracleDbType.Date, m.HearingDate);
            P("P_CASE_TYPEID", OracleDbType.Int64, m.CaseTypeId);
            P("P_CASE_ID", OracleDbType.Int64, m.CaseId);
            P("P_CCODE", OracleDbType.Varchar2, courtCode);
            P("P_CASE_TYPE", OracleDbType.Int64, m.CaseTypeId);
            P("P_CREATEDBY", OracleDbType.Varchar2, createdBy);
            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private static OracleCommand CreateCommand(OracleConnection conn, int input)
        {
            var cmd = new OracleCommand("PROC_SUPPLEMENTRY_CAUSE_LIST", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };
            cmd.Parameters.Add("P_INPUT", OracleDbType.Int32).Value = input;
            return cmd;
        }

        private static void AddNullParams(OracleCommand cmd)
        {
            // Sab optional params DEFAULT NULL hain, BindByName ke saath skip ho sakte hain.
        }

        private static SupplementaryCause Map(OracleDataReader r) => new()
        {
            TblSupltyCauseId = ToLong(r["TBL_SUPLTY_CAUSEID"]) ?? 0,
            SupplyCauseListId = ToLong(r["SUPPLY_CAUSE_LISTID"]) ?? 0,
            HearingDate = r["HEARING_DATE"] == DBNull.Value ? null : Convert.ToDateTime(r["HEARING_DATE"]),
            CaseTypeId = ToLong(r["CASETYPEID"]),
            CaseTypeName = r["CTYPE"]?.ToString(),
            CaseNo = r["CASE_NO"]?.ToString(),
            CaseId = ToLong(r["CASE_ID"]),
            PurposeId = ToLong(r["PURPOSE"]),
            PurposeName = r["PURP"]?.ToString(),
            BenchType = r["BENCHTYPE"]?.ToString()
        };

        private static long? ToLong(object v)
        {
            if (v == null || v == DBNull.Value) return null;
            return v is OracleDecimal d ? d.ToInt64() : Convert.ToInt64(v);
        }
    }
}