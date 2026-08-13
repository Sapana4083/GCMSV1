using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class BenchTypeRepository : IBenchTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public BenchTypeRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL (V_INPUT = 4 pattern nahi, so custom list — SP me
        // dedicated "get all" (non-filtered) branch nahi hai (5 sirf
        // BENCH_TYPE_CODE IN ('1','2','3','4') deta hai). Isliye V_INPUT=4
        // ko id ke bina call nahi kar sakte, isliye yaha hum P_BENCH_TYPE_MASTID
        // NULL bhej ke V_INPUT=4 use nahi karenge — is SP me list ke liye
        // seedha V_INPUT=5 hi available hai. Agar sabhi records (bina
        // BENCH_TYPE_CODE filter ke) chahiye to SP me naya branch add karna
        // hoga. Filhal V_INPUT=5 hi use kar rahe hain.
        // ───────────────────────────────────────────────
        public async Task<List<BenchTypeMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<BenchTypeMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_BENCH_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 4;

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BenchTypeMaster
                {
                    BenchTypeMastId = Convert.ToInt64(reader["BENCH_TYPE_MASTID"]),
                    BenchType = reader["BENCH_TYPE"]?.ToString(),
                    BenchTypeCode = reader["BENCH_TYPE_CODE"]?.ToString()
                });
            }

            return list;
        }

        // ───────────────────────────────────────────────
        // GET BY ID (V_INPUT = 4)
        // ───────────────────────────────────────────────
        public async Task<BenchTypeMaster?> GetByIdAsync(long id)
        {
            BenchTypeMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_BENCH_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 4
            });

            cmd.Parameters.Add(new OracleParameter("P_BENCH_TYPE_MASTID", OracleDbType.Int64)
            {
                Value = id
            });

            cmd.Parameters.Add(new OracleParameter("P_CURSOR", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            });

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                model = MapReader(reader);
            }

            return await Task.FromResult(model);
        }

        // ───────────────────────────────────────────────
        // ADD (V_INPUT = 1)
        // ───────────────────────────────────────────────
        public async Task AddAsync(BenchTypeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_BENCH_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_UNAME", OracleDbType.Varchar2)
               .Value = model.UName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_COURT_NAME", OracleDbType.Int64)
               .Value = model.CourtName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_COURT_CODE", OracleDbType.Varchar2)
               .Value = model.CourtCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_BENCH_TYPE", OracleDbType.Varchar2).Value = model.BenchType;

            cmd.Parameters.Add("P_BENCH_TYPE_CODE", OracleDbType.Varchar2)
               .Value = model.BenchTypeCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_MINI_LIMIT", OracleDbType.Decimal)
               .Value = model.MiniLimit ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_MAX_LIMIT", OracleDbType.Decimal)
               .Value = model.MaxLimit ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_IS_ACTIVE", OracleDbType.Varchar2)
               .Value = model.IsActive ?? "1";

            cmd.Parameters.Add("P_DUPCHECK", OracleDbType.Varchar2)
               .Value = model.DupCheck ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_BENCH_TYPE_ENG", OracleDbType.Varchar2)
               .Value = model.BenchTypeEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CANCEL", OracleDbType.Char)
               .Value = model.Cancel ?? "F";

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var status = reader["STATUS"]?.ToString();
                var message = reader["MESSAGE"]?.ToString();

                if (status == "ERROR")
                {
                    throw new Exception(message);
                }
            }

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // UPDATE (V_INPUT = 2)
        // ───────────────────────────────────────────────
        public async Task UpdateAsync(BenchTypeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_BENCH_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_BENCH_TYPE_MASTID", OracleDbType.Int64).Value = model.BenchTypeMastId;

            cmd.Parameters.Add("P_UNAME", OracleDbType.Varchar2)
               .Value = model.UName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_COURT_NAME", OracleDbType.Int64)
               .Value = model.CourtName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_COURT_CODE", OracleDbType.Varchar2)
               .Value = model.CourtCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_BENCH_TYPE", OracleDbType.Varchar2).Value = model.BenchType;

            cmd.Parameters.Add("P_BENCH_TYPE_CODE", OracleDbType.Varchar2)
               .Value = model.BenchTypeCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_MINI_LIMIT", OracleDbType.Decimal)
               .Value = model.MiniLimit ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_MAX_LIMIT", OracleDbType.Decimal)
               .Value = model.MaxLimit ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_IS_ACTIVE", OracleDbType.Varchar2)
               .Value = model.IsActive ?? "Y";

            cmd.Parameters.Add("P_DUPCHECK", OracleDbType.Varchar2)
               .Value = model.DupCheck ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_BENCH_TYPE_ENG", OracleDbType.Varchar2)
               .Value = model.BenchTypeEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CANCEL", OracleDbType.Char)
               .Value = model.Cancel ?? "N";

            cmd.Parameters.Add("P_CANCELREMARKS", OracleDbType.Varchar2)
               .Value = model.CancelRemarks ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2)
               .Value = model.UserName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var status = reader["STATUS"]?.ToString();
                var message = reader["MESSAGE"]?.ToString();

                if (status == "ERROR")
                {
                    throw new Exception(message);
                }
            }

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // Helper
        // ───────────────────────────────────────────────
        private static BenchTypeMaster MapReader(OracleDataReader reader)
        {
            return new BenchTypeMaster
            {
                BenchTypeMastId = Convert.ToInt64(reader["BENCH_TYPE_MASTID"]),
                UName = reader["UNAME"]?.ToString(),
                CourtName = reader["COURT_NAME"] == DBNull.Value ? null : Convert.ToInt64(reader["COURT_NAME"]),
                CourtCode = reader["COURT_CODE"]?.ToString(),
                BenchType = reader["BENCH_TYPE"]?.ToString(),
                BenchTypeCode = reader["BENCH_TYPE_CODE"]?.ToString(),
                MiniLimit = reader["MINI_LIMIT"] == DBNull.Value ? null : Convert.ToDecimal(reader["MINI_LIMIT"]),
                MaxLimit = reader["MAX_LIMIT"] == DBNull.Value ? null : Convert.ToDecimal(reader["MAX_LIMIT"]),
                IsActive = reader["IS_ACTIVE"]?.ToString(),
                DupCheck = reader["DUPCHECK"]?.ToString(),
                BenchTypeEng = reader["BENCH_TYPE_ENG"]?.ToString(),
                Cancel = reader["CANCEL"]?.ToString(),
                CancelRemarks = reader["CANCELREMARKS"]?.ToString(),
                CreatedBy = reader["CREATEDBY"]?.ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                UserName = reader["USERNAME"]?.ToString(),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"])
            };
        }
    }
}