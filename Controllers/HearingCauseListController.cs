using GCMS.Data;
using GCMS.Models.ViewModels;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Controllers
{
    public class HearingCauseListController : Controller
    {
        private static readonly SelectListItem[] CauseListTypes =
        {
            new() { Value = "Regular", Text = "Regular" },
            new() { Value = "Supplementary", Text = "Supplementary" },
            new() { Value = "Urgent", Text = "Urgent" }
        };

        private static readonly SelectListItem[] BenchNumbers =
        {
            new() { Value = "1", Text = "1" },
            new() { Value = "2", Text = "2" },
            new() { Value = "R", Text = "R" }
        };

        private readonly ApplicationDbContext _context;
        private readonly IAdvocateService _advocateService;

        public HearingCauseListController(ApplicationDbContext context, IAdvocateService advocateService)
        {
            _context = context;
            _advocateService = advocateService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? hearingDate, string? benchTypeId, string? causeListType)
        {
            var model = await BuildViewModelAsync(hearingDate, benchTypeId, causeListType);

            if (hearingDate.HasValue && !string.IsNullOrWhiteSpace(benchTypeId))
            {
                model.Rows = await GetRowsAsync(hearingDate.Value, benchTypeId, model.CauseListType);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetHearingCauseList(DateTime hearingDate, string benchTypeId, string? causeListType)
        {
            var rows = await GetRowsAsync(hearingDate, benchTypeId, causeListType ?? "Regular");
            return Json(rows);
        }

        [HttpPost]
        public async Task<JsonResult> SaveRows([FromBody] SaveHearingCauseListRequest request)
        {
            if (request?.Rows == null || request.Rows.Count == 0)
            {
                return Json(new { success = false, message = "No rows selected for update." });
            }

            if (!request.HearingDate.HasValue || string.IsNullOrWhiteSpace(request.BenchNo))
            {
                return Json(new { success = false, message = "Hearing date and bench no are required." });
            }

            foreach (var row in request.Rows)
            {
                await SaveRowAsync(request, row);
            }

            return Json(new
            {
                success = true,
                message = "Hearing cause list updated. Appellant and advocate names were not changed."
            });
        }

        private async Task<HearingCauseListViewModel> BuildViewModelAsync(DateTime? hearingDate, string? benchTypeId, string? causeListType)
        {
            long? selectedBench = null;
            if (long.TryParse(benchTypeId, out var numericBench))
            {
                selectedBench = numericBench;
            }

            return new HearingCauseListViewModel
            {
                HearingDate = (hearingDate ?? DateTime.Today).Date,
                BenchTypeId = selectedBench,
                BenchNo = benchTypeId,
                CauseListType = string.IsNullOrWhiteSpace(causeListType) ? "Regular" : causeListType,
                BenchTypes = BenchNumbers,
                CauseListTypes = CauseListTypes,
                PrivateAdvocates = await GetPrivateAdvocatesAsync()
            };
        }

        private async Task<List<HearingCauseListRowViewModel>> GetRowsAsync(DateTime hearingDate, string benchTypeId, string causeListType)
        {
            var rows = new List<HearingCauseListRowViewModel>();
            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = CreateCommand(conn);

            cmd.Parameters.Add("p_action", OracleDbType.Varchar2).Value = "GET";
            cmd.Parameters.Add("p_tcsat_mult_herid", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_tcsat_mult_herid"].Value = DBNull.Value;
            cmd.Parameters.Add("p_cause_listid", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_cause_listid"].Value = DBNull.Value;
            cmd.Parameters.Add("p_hdate", OracleDbType.Date).Value = hearingDate.Date;
            cmd.Parameters.Add("p_bench_no", OracleDbType.Varchar2).Value = benchTypeId;
            cmd.Parameters.Add("p_cl_type", OracleDbType.Varchar2).Value = causeListType;
            cmd.Parameters.Add("p_court_code", OracleDbType.Varchar2).Value = GetCourtCode();
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = GetUserName();
            cmd.Parameters.Add("p_case_regid", OracleDbType.Decimal).Value = DBNull.Value;
            cmd.Parameters.Add("p_h_date", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("p_s_date", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("p_r_date", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("p_cdt", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("p_lsstay", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_lsreply", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                rows.Add(new HearingCauseListRowViewModel
                {
                    CauseListId = ToLong(reader["CAUSE_LISTID"]),
                    TcsatMultHerId = ToLong(reader["TCSAT_MULT_HERID"]),
                    CaseId = ToLong(reader["CASE_REGID"]),
                    SerialNo = ToInt(reader["CAUSE_LISTROW"]),
                    CaseNumber = reader["CASE_NO"]?.ToString() ?? string.Empty,
                    ParentCaseNumber = reader["LINKED_CASENO"]?.ToString(),
                    Purpose = reader["PURPOSE_TEXT"]?.ToString(),
                    HearingDate = ToDate(reader["H_DATE"]),
                    AppellantName = reader["APPELLANT_NAMEE"]?.ToString(),
                    AdvocateName = reader["ADVOCATE"]?.ToString(),
                    StayDate = ToDate(reader["S_DATE"]),
                    ReplyDate = ToDate(reader["R_DATE"]),
                    PurchaseDate = ToDate(reader["CDT"]),
                    IsStay = string.Equals(reader["LSSTAY"]?.ToString(), "Y", StringComparison.OrdinalIgnoreCase),
                    IsReply = string.Equals(reader["LSREPLY"]?.ToString(), "Y", StringComparison.OrdinalIgnoreCase),
                    LastUpdatedDate = ToDate(reader["LAST_UPDATED_DATE"]),
                    Name = reader["NAME"]?.ToString(),
                    Designation = reader["PRIVATE_DESIGNATION"]?.ToString(),
                    PrivateAdvocateId = ToNullableLong(reader["PRIVATEADVOCATE"]),
                    PrivateAdvocateName = null
                });
            }

            return rows;
        }

        private async Task<List<SelectListItem>> GetPrivateAdvocatesAsync()
        {
            var privateAdvocates = await _advocateService.GetPrivateAdvocatesAsync();

            return privateAdvocates
                .Select(advocate => new SelectListItem
                {
                    Value = advocate.MastRcsatAdvocateId.ToString(),
                    Text = advocate.AdvEngHi ?? advocate.AdvName ?? advocate.AdvNameHi
                })
                .ToList();
        }

        private async Task SaveRowAsync(SaveHearingCauseListRequest request, SaveHearingCauseListRowRequest row)
        {
            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = CreateCommand(conn);

            cmd.Parameters.Add("p_action", OracleDbType.Varchar2).Value = "SAVE";
            cmd.Parameters.Add("p_tcsat_mult_herid", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_tcsat_mult_herid"].Value = row.TcsatMultHerId == 0
                ? DBNull.Value
                : row.TcsatMultHerId;
            cmd.Parameters.Add("p_cause_listid", OracleDbType.Decimal).Direction = ParameterDirection.InputOutput;
            cmd.Parameters["p_cause_listid"].Value = row.CauseListId == 0
                ? DBNull.Value
                : row.CauseListId;
            cmd.Parameters.Add("p_hdate", OracleDbType.Date).Value = request.HearingDate!.Value.Date;
            cmd.Parameters.Add("p_bench_no", OracleDbType.Varchar2).Value = request.BenchNo;
            cmd.Parameters.Add("p_cl_type", OracleDbType.Varchar2).Value = request.CauseListType ?? "Regular";
            cmd.Parameters.Add("p_court_code", OracleDbType.Varchar2).Value = GetCourtCode();
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = GetUserName();
            cmd.Parameters.Add("p_case_regid", OracleDbType.Decimal).Value = row.CaseId == 0 ? DBNull.Value : row.CaseId;
            cmd.Parameters.Add("p_h_date", OracleDbType.Date).Value = row.HearingDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_s_date", OracleDbType.Date).Value = row.StayDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_r_date", OracleDbType.Date).Value = row.ReplyDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_cdt", OracleDbType.Date).Value = row.PurchaseDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_lsstay", OracleDbType.Varchar2).Value = row.IsStay ? "Y" : "N";
            cmd.Parameters.Add("p_lsreply", OracleDbType.Varchar2).Value = row.IsReply ? "Y" : "N";
            cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }

        private static OracleCommand CreateCommand(OracleConnection conn)
        {
            return new OracleCommand("PROC_TCSAT_MULT_HER", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };
        }

        private string GetCourtCode()
        {
            return HttpContext.Session.GetString("CourtCode") ?? string.Empty;
        }

        private string GetUserName()
        {
            return HttpContext.Session.GetString("Username") ?? User?.Identity?.Name ?? "SYSTEM";
        }

        private static long ToLong(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return 0;
            }

            return value is OracleDecimal oracleDecimal
                ? oracleDecimal.ToInt64()
                : Convert.ToInt64(value);
        }

        private static long? ToNullableLong(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return null;
            }

            return value is OracleDecimal oracleDecimal
                ? oracleDecimal.ToInt64()
                : Convert.ToInt64(value);
        }

        private static int ToInt(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return 0;
            }

            return value is OracleDecimal oracleDecimal
                ? oracleDecimal.ToInt32()
                : Convert.ToInt32(value);
        }

        private static DateTime? ToDate(object value)
        {
            return value == DBNull.Value || value == null
                ? null
                : Convert.ToDateTime(value);
        }
    }
}
