using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class VcLinkRepository : IVcLinkRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public VcLinkRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL — SP me list action nahi hai, seedha table query
        // ───────────────────────────────────────────────
        public async Task<List<VcLink>> GetAllAsync(string courtCode)
        {
            var list = new List<VcLink>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = @"
                SELECT TRN_RCSAT_VCLINKID, COURTCODE, HEARINGDATE, TYPE, BENCH, VCLINK, CREATEDBY, CREATEDON
                FROM TRN_RCSAT_VCLINK
                WHERE NVL(CANCEL,'F') = 'F'
                  AND COURTCODE = :courtCode
                ORDER BY HEARINGDATE DESC, TRN_RCSAT_VCLINKID DESC";

            cmd.Parameters.Add(new OracleParameter("courtCode", OracleDbType.Varchar2) { Value = courtCode });

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(Map(reader));

            return list;
        }

        // ───────────────────────────────────────────────
        // GET BY ID (P_INPUT = 3)
        // ───────────────────────────────────────────────
        public async Task<VcLink?> GetByIdAsync(long id)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCommand(conn, 3);
            var idParam = new OracleParameter("P_TRN_RCSAT_VCLINKID", OracleDbType.Int64)
            {
                Direction = ParameterDirection.InputOutput,
                Value = id
            };
            cmd.Parameters.Add(idParam);

            AddOptionalParams(cmd, null);
            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        // ───────────────────────────────────────────────
        // ADD (P_INPUT = 1)
        // ───────────────────────────────────────────────
        public async Task AddAsync(VcLink model, string courtCode, string createdBy)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCommand(conn, 1);

            var idParam = new OracleParameter("P_TRN_RCSAT_VCLINKID", OracleDbType.Int64)
            {
                Direction = ParameterDirection.InputOutput,
                Value = DBNull.Value
            };
            cmd.Parameters.Add(idParam);

            cmd.Parameters.Add("P_COURTCODE", OracleDbType.Varchar2).Value = courtCode;
            cmd.Parameters.Add("P_HEARINGDATE", OracleDbType.Date).Value = model.HearingDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = model.Type ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_BENCH", OracleDbType.Varchar2).Value = model.Bench ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_VCLINK", OracleDbType.Varchar2).Value = model.VcLinkUrl ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = createdBy;
            cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = DBNull.Value;
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

        // ───────────────────────────────────────────────
        // UPDATE (P_INPUT = 2)
        // ───────────────────────────────────────────────
        public async Task UpdateAsync(VcLink model, string courtCode, string createdBy)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCommand(conn, 2);

            var idParam = new OracleParameter("P_TRN_RCSAT_VCLINKID", OracleDbType.Int64)
            {
                Direction = ParameterDirection.InputOutput,
                Value = model.TrnRcsatVclinkId
            };
            cmd.Parameters.Add(idParam);

            cmd.Parameters.Add("P_COURTCODE", OracleDbType.Varchar2).Value = courtCode;
            cmd.Parameters.Add("P_HEARINGDATE", OracleDbType.Date).Value = model.HearingDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = model.Type ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_BENCH", OracleDbType.Varchar2).Value = model.Bench ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_VCLINK", OracleDbType.Varchar2).Value = model.VcLinkUrl ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = createdBy;
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
            var cmd = new OracleCommand("PROC_TRN_RCSAT_VCLINK", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };
            cmd.Parameters.Add("P_INPUT", OracleDbType.Int32).Value = input;
            return cmd;
        }

        private static void AddOptionalParams(OracleCommand cmd, string? dummy)
        {
            // Get (input=3) ke liye baaki sab params DEFAULT NULL hain, skip kar sakte hain
            // kyunki BindByName=true hai aur named optional params Oracle me skip ho sakte hain.
        }

        private static VcLink Map(OracleDataReader r) => new()
        {
            TrnRcsatVclinkId = ToLong(r["TRN_RCSAT_VCLINKID"]) ?? 0,
            CourtCode = r["COURTCODE"]?.ToString(),
            HearingDate = r["HEARINGDATE"] == DBNull.Value ? null : Convert.ToDateTime(r["HEARINGDATE"]),
            Type = r["TYPE"]?.ToString(),
            Bench = r["BENCH"]?.ToString(),
            VcLinkUrl = r["VCLINK"]?.ToString(),
            CreatedBy = r["CREATEDBY"]?.ToString(),
            CreatedOn = r["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(r["CREATEDON"])
        };

        private static long? ToLong(object v)
        {
            if (v == null || v == DBNull.Value) return null;
            return v is OracleDecimal d ? d.ToInt64() : Convert.ToInt64(v);
        }
    }
}