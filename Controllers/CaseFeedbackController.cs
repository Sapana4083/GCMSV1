using GCMS.Data;
using GCMS.Models.ViewModels;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Controllers
{
    public class CaseFeedbackController : Controller
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICasePurposeService _casePurposeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly OracleConnectionFactory _connectionFactory;

        public CaseFeedbackController(
            ICaseTypeService caseTypeService,
            ICasePurposeService casePurposeService,
            IWebHostEnvironment webHostEnvironment,
            OracleConnectionFactory connectionFactory)
        {
            _caseTypeService = caseTypeService;
            _casePurposeService = casePurposeService;
            _webHostEnvironment = webHostEnvironment;
            _connectionFactory = connectionFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await BindDropdowns();
            return View(new CaseFeedbackViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CaseFeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await BindDropdowns();
                return View(model);
            }

            var uploadedFiles = await SaveUploadedFiles(model.UploadFiles);
            await InsertCaseFeedback(model, uploadedFiles);
            TempData["SuccessMessage"] = "Case feedback saved successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseNos(long caseTypeId, string? manualCaseNo)
        {
            var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";
            var rows = new List<object>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PROC_CASE_FEEDBACK";

            cmd.Parameters.Add(new OracleParameter("p_action", OracleDbType.Varchar2) { Value = "CASE_NO" });
            cmd.Parameters.Add(new OracleParameter("p_court_code", OracleDbType.Varchar2) { Value = courtCode });
            cmd.Parameters.Add(new OracleParameter("p_casetypeid", OracleDbType.Int64) { Value = caseTypeId });
            cmd.Parameters.Add(new OracleParameter("p_manual_case_no", OracleDbType.Varchar2)
            {
                Value = string.IsNullOrWhiteSpace(manualCaseNo) ? DBNull.Value : manualCaseNo.Trim()
            });
            AddSaveParameters(cmd);
            cmd.Parameters.Add(new OracleParameter("p_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            });
            cmd.Parameters.Add(new OracleParameter("p_out_hearingid", OracleDbType.Int64) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new OracleParameter("p_out_casehistoryid", OracleDbType.Int64) { Direction = ParameterDirection.Output });

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var caseNo = reader["mcase_noo"]?.ToString();

                rows.Add(new
                {
                    value = caseNo,
                    text = caseNo,
                    caseRegId = reader["trn_rcsat_caseregid"]?.ToString()
                });
            }

            return Json(rows);
        }

        private async Task BindDropdowns()
        {
            ViewBag.CaseTypeList = await _caseTypeService.GetCaseTypeAsync(1, 1000);
            ViewBag.CasePurposeList = await _casePurposeService.GetCasePurposeAsync(1, 1000);
        }

        private async Task InsertCaseFeedback(CaseFeedbackViewModel model, List<UploadedFileInfo> uploadedFiles)
        {
            var userName = HttpContext.Session.GetString("Username") ?? User.Identity?.Name ?? "SYSTEM";
            var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";
            var courtName = HttpContext.Session.GetString("CourtName") ?? "";
            var firstFile = uploadedFiles.FirstOrDefault();
            var fileNames = string.Join(",", uploadedFiles.Select(x => x.OriginalFileName));
            var filePaths = string.Join(",", uploadedFiles.Select(x => x.RelativePath));

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();
            using var transaction = (OracleTransaction)conn.BeginTransaction();
            var caseNoValue = TryParseLong(model.CaseNo);

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PROC_CASE_FEEDBACK";

            cmd.Parameters.Add(new OracleParameter("p_action", OracleDbType.Varchar2) { Value = "SAVE" });
            cmd.Parameters.Add(new OracleParameter("p_casetypeid", OracleDbType.Int64) { Value = model.CaseTypeId ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_manual_case_no", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_user_name", OracleDbType.Varchar2) { Value = userName });
            cmd.Parameters.Add(new OracleParameter("p_created_by", OracleDbType.Varchar2) { Value = userName });
            cmd.Parameters.Add(new OracleParameter("p_court_name", OracleDbType.Varchar2) { Value = courtName });
            cmd.Parameters.Add(new OracleParameter("p_court_code", OracleDbType.Varchar2) { Value = courtCode });
            cmd.Parameters.Add(new OracleParameter("p_case_no", OracleDbType.Int64) { Value = caseNoValue ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_case_reg_id", OracleDbType.Int64) { Value = model.CaseRegId ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_doc_id", OracleDbType.Varchar2) { Value = Truncate(string.IsNullOrWhiteSpace(fileNames) ? null : fileNames, 200) ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_new_purpose_code", OracleDbType.Varchar2) { Value = model.ReviseCasePurposeId?.ToString() ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_next_hear_date", OracleDbType.Date) { Value = model.NextHearingDate ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_case_type", OracleDbType.Varchar2) { Value = model.CaseTypeId?.ToString() ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_stay_check", OracleDbType.Varchar2) { Value = model.StayCheck ? "Y" : "N" });
            cmd.Parameters.Add(new OracleParameter("p_last_stay", OracleDbType.Varchar2) { Value = model.StayCheck ? "Y" : "N" });
            cmd.Parameters.Add(new OracleParameter("p_stay_date", OracleDbType.Date) { Value = model.StayDate });
            cmd.Parameters.Add(new OracleParameter("p_revise_case_purpose", OracleDbType.Int64) { Value = model.ReviseCasePurposeId ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_remark", OracleDbType.NClob) { Value = model.Remark ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_history_remark", OracleDbType.Varchar2) { Value = Truncate(model.Remark, 10) ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_file_name", OracleDbType.Varchar2) { Value = Truncate(firstFile?.OriginalFileName, 50) ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_file_path", OracleDbType.Varchar2) { Value = Truncate(string.IsNullOrWhiteSpace(filePaths) ? null : filePaths, 500) ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_cdate", OracleDbType.Varchar2) { Value = DateTime.Now.ToString("dd/MM/yyyy") });
            cmd.Parameters.Add(new OracleParameter("p_cno", OracleDbType.Varchar2) { Value = model.CaseNo ?? (object)DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            });
            cmd.Parameters.Add(new OracleParameter("p_out_hearingid", OracleDbType.Int64) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new OracleParameter("p_out_casehistoryid", OracleDbType.Int64) { Direction = ParameterDirection.Output });

            await cmd.ExecuteNonQueryAsync();
            transaction.Commit();
        }

        private static void AddSaveParameters(OracleCommand cmd)
        {
            cmd.Parameters.Add(new OracleParameter("p_user_name", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_created_by", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_court_name", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_case_no", OracleDbType.Int64) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_case_reg_id", OracleDbType.Int64) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_doc_id", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_new_purpose_code", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_next_hear_date", OracleDbType.Date) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_case_type", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_stay_check", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_last_stay", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_stay_date", OracleDbType.Date) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_revise_case_purpose", OracleDbType.Int64) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_remark", OracleDbType.NClob) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_history_remark", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_file_name", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_file_path", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_cdate", OracleDbType.Varchar2) { Value = DBNull.Value });
            cmd.Parameters.Add(new OracleParameter("p_cno", OracleDbType.Varchar2) { Value = DBNull.Value });
        }

        private async Task<List<UploadedFileInfo>> SaveUploadedFiles(List<IFormFile> files)
        {
            var uploadedFiles = new List<UploadedFileInfo>();

            if (files == null || files.Count == 0)
            {
                return uploadedFiles;
            }

            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "case-feedback");
            Directory.CreateDirectory(uploadPath);

            foreach (var file in files.Where(f => f.Length > 0))
            {
                var fileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                uploadedFiles.Add(new UploadedFileInfo(
                    file.FileName,
                    $"/uploads/case-feedback/{fileName}"));
            }

            return uploadedFiles;
        }

        private static long? TryParseLong(string? value)
        {
            return long.TryParse(value, out var number) ? number : null;
        }

        private static string? Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value;
            }

            return value[..maxLength];
        }

        private sealed record UploadedFileInfo(string OriginalFileName, string RelativePath);
    }
}
