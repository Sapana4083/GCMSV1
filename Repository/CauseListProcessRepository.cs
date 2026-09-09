using GCMS.Data;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CauseListProcessRepository : ICauseListProcessRepository
    {
        private readonly ApplicationDbContext _context;

        public CauseListProcessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ───────────────────────────────────────────────
        // GENERATE — sp_tribunal_causelist_process call karta hai
        // (koi OUT cursor nahi, sirf INSERT/UPDATE process)
        // ───────────────────────────────────────────────
        public async Task GenerateCauseListAsync(CauseListGenerateViewModel model)
        {
            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand("sp_tribunal_causelist_process", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };

            cmd.Parameters.Add("p1", OracleDbType.Varchar2).Value =
                model.DocId ?? (object)DBNull.Value;

            cmd.Parameters.Add("hdate", OracleDbType.Date).Value =
                model.HearingDate ?? (object)DBNull.Value;

            cmd.Parameters.Add("l_type", OracleDbType.Varchar2).Value =
                model.CauseListType ?? (object)DBNull.Value;

            cmd.Parameters.Add("dcdt", OracleDbType.Date).Value =
                model.DocDate ?? (object)DBNull.Value;

            cmd.Parameters.Add("ccode", OracleDbType.Varchar2).Value =
                model.CourtCode ?? (object)DBNull.Value;

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException(
                    $"Cause list generation failed: {ex.Message}", ex);
            }
        }

        // ───────────────────────────────────────────────
        // GET GENERATED LIST — generate ke baad result dikhane ke liye
        // ───────────────────────────────────────────────
        public async Task<List<CauseListGeneratedRow>> GetGeneratedListAsync(DateTime hearingDate, string courtCode)
        {
            var list = new List<CauseListGeneratedRow>();

            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand(@"
                SELECT
                    CASE_NO,
                    CASE_PURPOSE,
                    PRIORITY,
                    BENCH_TYPE,
                    BENCH_NUMBER,
                    APPELLANT_NAME,
                    RESPONDENT_NAME,
                    APPELLANT_ADVOCATE,
                    RESPONDENT_ADVOCATE,
                    CASE_TYPE,
                    DISTRICT,
                    INSTITUTION_DATE
                FROM CL_FILE_TEMP_NEW_TRIBUNAL
                WHERE HEARING_DATE = :hdate
                  AND COURT_CODE = :ccode
                ORDER BY PRIORITY, CASE_NO", conn);

            cmd.Parameters.Add(new OracleParameter("hdate", OracleDbType.Date) { Value = hearingDate });
            cmd.Parameters.Add(new OracleParameter("ccode", OracleDbType.Varchar2) { Value = courtCode });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new CauseListGeneratedRow
                {
                    CaseNo = reader["CASE_NO"]?.ToString(),
                    CasePurpose = reader["CASE_PURPOSE"]?.ToString(),
                    Priority = reader["PRIORITY"] == DBNull.Value ? null : Convert.ToInt32(reader["PRIORITY"]),
                    BenchType = reader["BENCH_TYPE"]?.ToString(),
                    BenchNumber = reader["BENCH_NUMBER"]?.ToString(),
                    AppellantName = reader["APPELLANT_NAME"]?.ToString(),
                    RespondentName = reader["RESPONDENT_NAME"]?.ToString(),
                    AppellantAdvocate = reader["APPELLANT_ADVOCATE"]?.ToString(),
                    RespondentAdvocate = reader["RESPONDENT_ADVOCATE"]?.ToString(),
                    CaseType = reader["CASE_TYPE"]?.ToString(),
                    District = reader["DISTRICT"]?.ToString(),
                    InstitutionDate = reader["INSTITUTION_DATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["INSTITUTION_DATE"])
                });
            }

            return list;
        }
    }
}