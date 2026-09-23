using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models.ViewModels;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CaseFeedbackRepository : ICaseFeedbackRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public CaseFeedbackRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET CASE NOS — SP me koi lookup action nahi hai,
        // isliye seedha TRN_RCSAT_CASEREG se query kar rahe hain.
        // ───────────────────────────────────────────────
        public async Task<List<CaseFeedbackCaseNoItem>> GetCaseNosAsync(long caseTypeId, string? manualCaseNo, string courtCode)
        {
            var list = new List<CaseFeedbackCaseNoItem>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = @"
                SELECT TRN_RCSAT_CASEREGID, MCASE_NOO
                FROM TRN_RCSAT_CASEREG
                WHERE CASETYPE = :caseTypeId
                  AND NVL(CANCEL, 'F') = 'F'
                  AND (:manualCaseNo IS NULL OR UPPER(MCASE_NOO) LIKE '%' || UPPER(:manualCaseNo) || '%')
                ORDER BY MCASE_NOO";

            cmd.Parameters.Add(new OracleParameter("caseTypeId", OracleDbType.Int64) { Value = caseTypeId });
            cmd.Parameters.Add(new OracleParameter("manualCaseNo", OracleDbType.Varchar2)
            {
                Value = string.IsNullOrWhiteSpace(manualCaseNo) ? (object)DBNull.Value : manualCaseNo.Trim()
            });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var caseNo = reader["MCASE_NOO"]?.ToString();

                list.Add(new CaseFeedbackCaseNoItem
                {
                    Value = caseNo,
                    Text = caseNo,
                    CaseRegId = reader["TRN_RCSAT_CASEREGID"] == DBNull.Value ? null : Convert.ToInt64(reader["TRN_RCSAT_CASEREGID"])
                });
            }

            return list;
        }

        // ───────────────────────────────────────────────
        // SAVE (P_INPUT = 1) — file upload/save alag se controller me hota hai
        // ───────────────────────────────────────────────
        public async Task SaveAsync(CaseFeedbackViewModel model, string createdBy)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PROC_CASE_FEEDBACK";

            cmd.Parameters.Add("P_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_CASE_ID", OracleDbType.Int64).Value =
                model.CaseRegId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_NO", OracleDbType.Varchar2).Value =
                model.CaseNo ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_TYPE", OracleDbType.Varchar2).Value =
                model.CaseTypeId?.ToString() ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_STAY_DATE", OracleDbType.Date).Value =
                model.StayCheck ? model.StayDate : (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_DETAILS", OracleDbType.Varchar2).Value =
                model.CaseDetails ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_REMARK", OracleDbType.NClob).Value =
                model.Remark ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_REVISE_CASE_PURPOSE", OracleDbType.Int64).Value =
                model.ReviseCasePurposeId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_NEW_PURPOSE_CODE", OracleDbType.Varchar2).Value =
                model.ReviseCasePurposeId?.ToString() ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_NEXT_HEAR_DATE", OracleDbType.Date).Value =
                model.NextHearingDate ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = createdBy;

            cmd.Parameters.Add("P_STAY_CHECK", OracleDbType.Varchar2).Value =
                model.StayCheck ? "T" : "F";

            cmd.Parameters.Add("P_FILE_NAME", OracleDbType.Varchar2).Value =
                string.IsNullOrWhiteSpace(model.SavedFileName) ? (object)DBNull.Value : model.SavedFileName;

            cmd.Parameters.Add("P_FILE_PATH", OracleDbType.Varchar2).Value =
                string.IsNullOrWhiteSpace(model.SavedFilePath) ? (object)DBNull.Value : model.SavedFilePath;

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException($"Case feedback save failed: {ex.Message}", ex);
            }
        }
    }
}