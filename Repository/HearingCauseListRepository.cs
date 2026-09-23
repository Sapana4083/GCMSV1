using GCMS.Data;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class HearingCauseListRepository : IHearingCauseListRepository
    {
        private readonly ApplicationDbContext _context;

        public HearingCauseListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // GET ROWS
        // ============================================================

        public async Task<List<HearingCauseListRowViewModel>> GetRowsAsync(
            DateTime hearingDate,
            string benchTypeId,
            string causeListType,
            string courtCode,
            string createdBy)
        {
            var rows = new List<HearingCauseListRowViewModel>();

            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            using var cmd = CreateCommand(conn);

            // --------------------------------------------------------
            // PROCEDURE PARAMETERS
            // --------------------------------------------------------

            cmd.Parameters.Add("p_action", OracleDbType.Varchar2)
                .Value = "GET";

            cmd.Parameters.Add("p_tcsat_mult_herid", OracleDbType.Decimal)
                .Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_tcsat_mult_herid"].Value = DBNull.Value;

            cmd.Parameters.Add("p_cause_listid", OracleDbType.Decimal)
                .Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_cause_listid"].Value = DBNull.Value;

            cmd.Parameters.Add("p_hdate", OracleDbType.Date)
                .Value = hearingDate.Date;

            cmd.Parameters.Add("p_bench_no", OracleDbType.Varchar2)
                .Value = benchTypeId;

            cmd.Parameters.Add("p_cl_type", OracleDbType.Varchar2)
                .Value = causeListType;

            cmd.Parameters.Add("p_court_code", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(courtCode)
                    ? (object)DBNull.Value
                    : courtCode;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(createdBy)
                    ? (object)DBNull.Value
                    : createdBy;

            cmd.Parameters.Add("p_case_regid", OracleDbType.Decimal)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_h_date", OracleDbType.Date)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_s_date", OracleDbType.Date)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_r_date", OracleDbType.Date)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_cdt", OracleDbType.Date)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_lsstay", OracleDbType.Varchar2)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_lsreply", OracleDbType.Varchar2)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_appellant_name", OracleDbType.Varchar2)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_advocate", OracleDbType.Varchar2)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            // --------------------------------------------------------
            // EXECUTE
            // --------------------------------------------------------

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    rows.Add(new HearingCauseListRowViewModel
                    {
                        // ==================================================
                        // PRIMARY IDS
                        // ==================================================

                        CauseListId = ToLong(reader["CAUSE_LISTID"]),

                        TcsatMultHerId = ToLong(reader["TCSAT_MULT_HERID"]),

                        CaseId = ToLong(reader["CASE_REGID"]),

                        // ==================================================
                        // CASE INFORMATION
                        // ==================================================

                        SerialNo = ToInt(reader["S_NO"]),

                        CaseNumber = reader["CASE_NUMBER"]?.ToString()
                                     ?? string.Empty,

                        ParentCaseNumber = reader["PARENT_CASE"]?.ToString(),

                        Purpose = reader["PURPOSE"]?.ToString(),

                        // ==================================================
                        // HEARING
                        // ==================================================

                        HearingDate = ToDate(reader["CHILD_HEARING_DATE"]),

                        AppellantName = reader["APPELLANT_NAME"]?.ToString(),

                        AdvocateName = reader["ADVOCATE_NAME"]?.ToString(),

                        // ==================================================
                        // ADVOCATE
                        // ==================================================

                        PrivateAdvocateName =
                            reader["ADVOCATE_NAME"]?.ToString(),

                        // ==================================================
                        // STAY / REPLY / PURCHASE
                        // ==================================================

                        StayDate = ToDate(reader["STAY_DATE"]),

                        ReplyDate = ToDate(reader["REPLY_DATE"]),

                        PurchaseDate = ToDate(reader["PURCHASE_DATE"]),

                        IsStay = string.Equals(
                            reader["IT_STAY"]?.ToString(),
                            "Y",
                            StringComparison.OrdinalIgnoreCase),

                        IsReply = string.Equals(
                            reader["IT_REPLY"]?.ToString(),
                            "Y",
                            StringComparison.OrdinalIgnoreCase),

                        // ==================================================
                        // MULTIPLE HEARING
                        // ==================================================

                        MultipleHearingDate =
                            ToDate(reader["HEARING_DATE"]),

                        BenchNumber =
                            reader["BENCH_NO"]?.ToString(),

                        ClType =
                            reader["CAUSE_LIST_TYPE"]?.ToString(),

                        // ==================================================
                        // OTHER
                        // ==================================================

                        LastUpdatedDate = null,

                        CourtCode = null,

                        Priority = null,

                        ConnectedCaseNo = null,

                        Name = null,

                        Designation = null
                    });
                }
            }
            catch (OracleException ex) when (ex.Number == 20004)
            {
                // No data found from procedure
                return rows;
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException(
                    $"Error while getting hearing cause list: {ex.Message}",
                    ex);
            }

            return rows;
        }


        // ============================================================
        // SAVE / UPDATE ROW
        // ============================================================

        public async Task SaveRowAsync(
            SaveHearingCauseListRequest request,
            SaveHearingCauseListRowRequest row,
            string courtCode,
            string createdBy)
        {
            // --------------------------------------------------------
            // VALIDATION
            // --------------------------------------------------------

            if (row.CauseListId == 0)
            {
                throw new InvalidOperationException(
                    "Cause List ID is required to update this row.");
            }

            if (row.TcsatMultHerId == 0)
            {
                throw new InvalidOperationException(
                    "TCSAT Mult Her ID is required to update this row.");
            }

            if (!request.HearingDate.HasValue)
            {
                throw new InvalidOperationException(
                    "Hearing Date is required.");
            }

            // --------------------------------------------------------
            // CONNECTION
            // --------------------------------------------------------

            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            using var cmd = CreateCommand(conn);

            // --------------------------------------------------------
            // PROCEDURE PARAMETERS
            // --------------------------------------------------------

            cmd.Parameters.Add("p_action", OracleDbType.Varchar2)
                .Value = "UPDATE";

            cmd.Parameters.Add("p_tcsat_mult_herid", OracleDbType.Decimal)
                .Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_tcsat_mult_herid"].Value =
                row.TcsatMultHerId;

            cmd.Parameters.Add("p_cause_listid", OracleDbType.Decimal)
                .Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_cause_listid"].Value =
                row.CauseListId;

            cmd.Parameters.Add("p_hdate", OracleDbType.Date)
                .Value = request.HearingDate.Value.Date;

            cmd.Parameters.Add("p_bench_no", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(request.BenchNo)
                    ? (object)DBNull.Value
                    : request.BenchNo;

            cmd.Parameters.Add("p_cl_type", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(request.CauseListType)
                    ? "Regular"
                    : request.CauseListType;

            cmd.Parameters.Add("p_court_code", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(courtCode)
                    ? (object)DBNull.Value
                    : courtCode;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(createdBy)
                    ? (object)DBNull.Value
                    : createdBy;

            // --------------------------------------------------------
            // CASE
            // --------------------------------------------------------

            cmd.Parameters.Add("p_case_regid", OracleDbType.Decimal)
                .Value = row.CaseId == 0
                    ? (object)DBNull.Value
                    : row.CaseId;

            // --------------------------------------------------------
            // HEARING DATE
            // --------------------------------------------------------

            cmd.Parameters.Add("p_h_date", OracleDbType.Date)
                .Value = row.HearingDate.HasValue
                    ? row.HearingDate.Value
                    : (object)DBNull.Value;

            // --------------------------------------------------------
            // STAY
            // --------------------------------------------------------

            cmd.Parameters.Add("p_s_date", OracleDbType.Date)
                .Value = row.StayDate.HasValue
                    ? row.StayDate.Value
                    : (object)DBNull.Value;

            // --------------------------------------------------------
            // REPLY
            // --------------------------------------------------------

            cmd.Parameters.Add("p_r_date", OracleDbType.Date)
                .Value = row.ReplyDate.HasValue
                    ? row.ReplyDate.Value
                    : (object)DBNull.Value;

            // --------------------------------------------------------
            // PURCHASE
            // --------------------------------------------------------

            cmd.Parameters.Add("p_cdt", OracleDbType.Date)
                .Value = row.PurchaseDate.HasValue
                    ? row.PurchaseDate.Value
                    : (object)DBNull.Value;

            // --------------------------------------------------------
            // STAY FLAG
            // --------------------------------------------------------

            cmd.Parameters.Add("p_lsstay", OracleDbType.Varchar2)
                .Value = row.IsStay ? "Y" : "N";

            // --------------------------------------------------------
            // REPLY FLAG
            // --------------------------------------------------------

            cmd.Parameters.Add("p_lsreply", OracleDbType.Varchar2)
                .Value = row.IsReply ? "Y" : "N";

            // --------------------------------------------------------
            // APPELLANT
            //
            // Readonly field, therefore don't change it.
            // --------------------------------------------------------

            cmd.Parameters.Add("p_appellant_name", OracleDbType.Varchar2)
                .Value = DBNull.Value;

            // --------------------------------------------------------
            // PRIVATE ADVOCATE
            //
            // Your model uses PrivateAdvocateName.
            // --------------------------------------------------------

            cmd.Parameters.Add("p_advocate", OracleDbType.Varchar2)
                .Value = string.IsNullOrWhiteSpace(
                    row.PrivateAdvocateName)
                    ? (object)DBNull.Value
                    : row.PrivateAdvocateName;

            // --------------------------------------------------------
            // OUTPUT CURSOR
            // --------------------------------------------------------

            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            // --------------------------------------------------------
            // EXECUTE
            // --------------------------------------------------------

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException(
                    $"Hearing cause list update failed: {ex.Message}",
                    ex);
            }
        }


        // ============================================================
        // CREATE COMMAND
        // ============================================================

        private static OracleCommand CreateCommand(
            OracleConnection conn)
        {
            return new OracleCommand(
                "PROC_RCSAT_MULT_HER",
                conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };
        }


        // ============================================================
        // CONVERSION METHODS
        // ============================================================

        private static long ToLong(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            if (value is OracleDecimal oracleDecimal)
                return oracleDecimal.ToInt64();

            return Convert.ToInt64(value);
        }


        private static long? ToNullableLong(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            if (value is OracleDecimal oracleDecimal)
                return oracleDecimal.ToInt64();

            return Convert.ToInt64(value);
        }


        private static int ToInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            if (value is OracleDecimal oracleDecimal)
                return oracleDecimal.ToInt32();

            return Convert.ToInt32(value);
        }


        private static DateTime? ToDate(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToDateTime(value);
        }
    }
}