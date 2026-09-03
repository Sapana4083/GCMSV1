using GCMS.Data;
using GCMS.Models.Entities;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class CaseRepository : ICaseRepository
    {
        private readonly ApplicationDbContext _context;

        public CaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> SaveFullCaseRegistrationAsync(
            CaseRegistrationWizardViewModel model,
            string createdBy)
        {
            // FIX: Do NOT wrap the DbContext's own connection in a `using` block.
            // Disposing it here can break later calls on the same DbContext
            // within the same request/scope (ObjectDisposedException).
            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_CASEREG_FULL", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true // FIX: always bind by name, never rely on positional order
            };

            cmd.Parameters.Add("p_institutiondate", OracleDbType.Date).Value =
                model.InstitutionDate ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_case_no", OracleDbType.Varchar2).Value =
                model.CaseNumber ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_impugned_flag", OracleDbType.Varchar2).Value =
                model.IsImpungned ? "T" : "F";

            cmd.Parameters.Add("p_impugned_date", OracleDbType.Date).Value =
                model.DateofImpugnedOrder ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_desiofforder", OracleDbType.Int64).Value =
                model.OrderIssuedById ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_casetype", OracleDbType.Int64).Value =
                model.CaseTypeId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_casesubject", OracleDbType.Int64).Value =
                model.CaseSubjectId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_case_purpose_name", OracleDbType.Int64).Value =
                model.CasePurposeId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_hearingdate", OracleDbType.Date).Value =
                model.HearingDate ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_bench_type", OracleDbType.Int64).Value =
                model.BenchTypeId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_linked_case", OracleDbType.Varchar2).Value =
                model.LinkedCaseNumber ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_oldcasno", OracleDbType.Varchar2).Value =
                model.OldCaseNumber ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value =
                createdBy;

            cmd.Parameters.Add("p_appellant_name", OracleDbType.Varchar2).Value =
                model.AppellantName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_designation", OracleDbType.Varchar2).Value =
                model.DesignationId?.ToString() ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_adistrict_name", OracleDbType.Varchar2).Value =
                model.DistrictId?.ToString() ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_mobileno", OracleDbType.Int64).Value =
                model.MobileNumber ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_app_advocate", OracleDbType.Int64).Value =
                model.AdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_appadv_email", OracleDbType.Varchar2).Value =
                model.AdvocateEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_app_advmobile", OracleDbType.Varchar2).Value =
                model.AdvocateMobile?.ToString() ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_employeeid", OracleDbType.Varchar2).Value =
                model.EmployeeId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respondent_department", OracleDbType.Int64).Value =
                model.DepartmentId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_resp_advocate", OracleDbType.Int64).Value =
                model.RespondentAdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respadvemail", OracleDbType.Varchar2).Value =
                model.RespondentAdvocateEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respadvmobile", OracleDbType.Int64).Value =
                model.RespondentAdvocateMobile ?? (object)DBNull.Value;

            // FIX: Step 4 supports MULTIPLE private parties (model.PrivateParties).
            // The SP expects p_private_name / p_private_designation / p_privadvocatee
            // as comma-separated lists, split positionally via REGEXP_SUBSTR + LEVEL.
            // IMPORTANT: REGEXP_SUBSTR('[^,]+', ...) does NOT match an empty segment
            // between two commas — a blank Designation/AdvocateId would silently
            // shift the index and attach the WRONG value to the wrong party.
            // So every blank field is replaced with a single-space placeholder
            // to keep all three lists the same length/position.
            var privateNames = new List<string>();
            var privateDesignations = new List<string>();
            var privateAdvocateIds = new List<string>();

            foreach (var party in model.PrivateParties ?? new List<PrivatePartyRowViewModel>())
            {
                bool isFullyBlank =
                    string.IsNullOrWhiteSpace(party.PartyName)
                    && string.IsNullOrWhiteSpace(party.Designation)
                    && party.AdvocateId == null;

                if (isFullyBlank)
                    continue; // skip completely empty rows (e.g. unused extra row)

                privateNames.Add(
                    string.IsNullOrWhiteSpace(party.PartyName) ? " " : party.PartyName.Trim());

                privateDesignations.Add(
                    string.IsNullOrWhiteSpace(party.Designation) ? " " : party.Designation.Trim());

                privateAdvocateIds.Add(
                    party.AdvocateId.HasValue ? party.AdvocateId.Value.ToString() : " ");
            }

            string? privateNameList = privateNames.Count > 0
                ? string.Join(",", privateNames) : null;

            string? privateDesignationList = privateDesignations.Count > 0
                ? string.Join(",", privateDesignations) : null;

            string? privateAdvocateList = privateAdvocateIds.Count > 0
                ? string.Join(",", privateAdvocateIds) : null;

            cmd.Parameters.Add("p_private_name", OracleDbType.Varchar2).Value =
                (object?)privateNameList ?? DBNull.Value;

            cmd.Parameters.Add("p_private_designation", OracleDbType.Varchar2).Value =
                (object?)privateDesignationList ?? DBNull.Value;

            // FIX: SP defines p_privadvocatee as VARCHAR2 (comma-separated list,
            // parsed with REGEXP_SUBSTR + TO_NUMBER per private party), not NUMBER.
            cmd.Parameters.Add("p_privadvocatee", OracleDbType.Varchar2).Value =
                (object?)privateAdvocateList ?? DBNull.Value;

            var outCaseId = new OracleParameter("p_caseid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outCaseId);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex)
            {
                // FIX: surface the SP's RAISE_APPLICATION_ERROR message
                // (e.g. -20001..-20011 validation errors, -20999 generic)
                // as a clean exception instead of letting a raw OracleException
                // bubble up to the Service/Controller layer.
                throw new InvalidOperationException(
                    $"Case registration failed: {ex.Message}", ex);
            }

            if (outCaseId.Value == null || outCaseId.Value == DBNull.Value)
                return 0;

            return ((OracleDecimal)outCaseId.Value).ToInt64();
        }

        public async Task<CaseRegistration?> GetCaseAsync(long caseId)
        {
            return await _context.CaseRegistrations
                .FirstOrDefaultAsync(x => x.CaseId == caseId);
        }

        public Task DeleteCaseAsync(long caseId)
        {
            // TODO: Implement soft delete using Oracle stored procedure if required.
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<SelectListItem>> GetCaseTypesAsync()
        {
            return await _context.CaseTypes
                .Where(x => x.Cancel == "F")
                .OrderBy(x => x.CaseTypeEng)
                .Select(x => new SelectListItem
                {
                    Value = x.CaseTypeMastId.ToString(),
                    Text = x.CaseTypeEng
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCaseSubjectsAsync()
        {
            return await _context.CaseSubjects
                .Where(x => x.Cancel == "F")
                .OrderBy(x => x.SubjectEngHi)
                .Select(x => new SelectListItem
                {
                    Value = x.CaseSubjectId.ToString(),
                    Text = x.SubjectEngHi
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCasePurposesAsync()
        {
            return await _context.CasePurposes
                .Where(x => x.Cancel == "F")
                .OrderBy(x => x.CasePurposeName)
                .Select(x => new SelectListItem
                {
                    Value = x.CasePurposeMastId.ToString(),
                    Text = x.CasePurposeName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBenchTypesAsync()
        {
            return await _context.BenchTypes
                .Where(x => x.Cancel == "F")
                .OrderBy(x => x.BenchTypeEng)
                .Select(x => new SelectListItem
                {
                    Value = x.BenchTypeMastId.ToString(),
                    Text = x.BenchTypeEng
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDepartmentsAsync()
        {
            return await _context.DepartmentMasters
                .Where(x => x.IsActive == "T")
                .OrderBy(x => x.DepartmentName)
                .Select(x => new SelectListItem
                {
                    Value = x.DepartmentId.ToString(),
                    Text = x.DepartmentName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrictsAsync()
        {
            return await _context.DistrictMasters
                .Where(x => x.InActive == "T")
                .OrderBy(x => x.DistrictNameEng)
                .Select(x => new SelectListItem
                {
                    Value = x.DistrictMastId.ToString(),
                    Text = x.DistrictNameEng
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDesignationsAsync()
        {
            return await _context.DepartmentMasters
                .Where(x => x.IsActive == "T")
                .OrderBy(x => x.DepartmentName)
                .Select(x => new SelectListItem
                {
                    Value = x.DepartmentId.ToString(),
                    Text = x.DepartmentName
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAdvocatesAsync()
        {
            return await _context.DepartmentMasters
               .Where(x => x.IsActive == "T")
               .OrderBy(x => x.DepartmentName)
               .Select(x => new SelectListItem
               {
                   Value = x.DepartmentId.ToString(),
                   Text = x.DepartmentName
               })
               .ToListAsync();
            //return await _context.AdvocateMaster
            //    .Where(x => x.InActive == "T")
            //    .OrderBy(x => x.AdvocateName)
            //    .Select(x => new SelectListItem
            //    {
            //        Value = x.AdvocateMastId.ToString(),
            //        Text = x.AdvocateName
            //    })
            //    .ToListAsync();
        }

        // NOTE: no "Order Type Master" module mentioned in your recent work.
        // Left pointing at DepartmentMasters as a placeholder — replace with
        // the correct table/entity once that master exists, or remove this
        // method if p_desiofforder actually maps to something else entirely.
        public async Task<IEnumerable<SelectListItem>> GetOrderTypesAsync()
        {
            return await _context.DepartmentMasters
                .Where(x => x.IsActive == "T")
                .OrderBy(x => x.DepartmentName)
                .Select(x => new SelectListItem
                {
                    Value = x.DepartmentId.ToString(),
                    Text = x.DepartmentName
                })
                .ToListAsync();
        }

        public async Task<long> SaveCaseAsync(CaseRegistration caseRegistration)
        {
            await _context.CaseRegistrations.AddAsync(caseRegistration);
            await _context.SaveChangesAsync();

            return caseRegistration.CaseId;
        }

        public async Task<long> SaveAppellantAsync(CaseAppellant appellant)
        {
            await _context.CaseAppellants.AddAsync(appellant);
            await _context.SaveChangesAsync();

            return appellant.AppellantId;
        }

        public async Task<long> SaveRespondentAsync(CaseRespondent respondent)
        {
            await _context.CaseRespondents.AddAsync(respondent);
            await _context.SaveChangesAsync();

            return respondent.RespondentId;
        }

        public async Task<long> SavePrivatePartyAsync(CasePrivateParty privateParty)
        {
            await _context.CasePrivateParties.AddAsync(privateParty);
            await _context.SaveChangesAsync();

            return privateParty.PrivatePartyId;
        }

        // ───────────────────────────────────────────────
        // LIST (V_INPUT = 4) — paginated
        // ───────────────────────────────────────────────
        public async Task<List<CaseRegistrationListItem>> GetCaseListAsync(int pageNo, int rowCnt, string? searchText = null)
        {
            var list = new List<CaseRegistrationListItem>();

            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_CASEREG_FULL", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 4;

            cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("P_SEARCH_TEXT", OracleDbType.Varchar2).Value =
                string.IsNullOrWhiteSpace(searchText) ? (object)DBNull.Value : searchText.Trim();

            var caseIdParam = new OracleParameter("p_caseid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.InputOutput,
                Value = DBNull.Value
            };
            cmd.Parameters.Add(caseIdParam);

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new CaseRegistrationListItem
                {
                    RecordId = Convert.ToInt64(reader["RECORDID"]),
                    MCaseNoo = reader["MCASE_NOO"]?.ToString(),
                    InstitutionDate = reader["INSTITUTIONDATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["INSTITUTIONDATE"]),
                    CaseType = reader["CASETYPE"]?.ToString(),
                    CaseSubject = reader["CASESUBJECT"]?.ToString(),
                    CasePurposeName = reader["CASE_PURPOSE_NAME"]?.ToString(),
                    HearingDate = reader["HEARINGDATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["HEARINGDATE"]),
                    BenchType = reader["BENCH_TYPE"]?.ToString(),
                    CreatedBy = reader["CREATEDBY"]?.ToString(),
                    CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                    ModifiedBy = reader["MODIFIEDBY"]?.ToString(),
                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"]),
                    TotalCount = reader["TOTAL_COUNT"] == DBNull.Value ? 0 : Convert.ToInt64(reader["TOTAL_COUNT"])
                });
            }

            return list;
        }


        // ───────────────────────────────────────────────
        // GET BY ID (V_INPUT = 3) — Edit ke liye poora wizard model bharega
        // ───────────────────────────────────────────────
        public async Task<CaseRegistrationWizardViewModel?> GetFullCaseByIdAsync(long caseId)
        {
            CaseRegistrationWizardViewModel? model = null;

            var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_CASEREG_FULL", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 3;

            var caseIdParam = new OracleParameter("p_caseid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.InputOutput,
                Value = caseId
            };
            cmd.Parameters.Add(caseIdParam);

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                model = new CaseRegistrationWizardViewModel
                {
                    Id = Convert.ToInt64(reader["RECORDID"]),

                    // ── Step 1: Basic Details ──
                    InstitutionDate = reader["INSTITUTIONDATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["INSTITUTIONDATE"]),
                    CaseNumber = reader["MCASE_NOO"]?.ToString(),
                    IsImpungned = reader["ISIMPNULL"]?.ToString() == "T",
                    DateofImpugnedOrder = reader["DATE_OF_ORDER"] == DBNull.Value ? null : Convert.ToDateTime(reader["DATE_OF_ORDER"]),
                    OrderIssuedById = reader["DESIOFFORDER"] == DBNull.Value ? null : Convert.ToInt64(reader["DESIOFFORDER"]),
                    CaseTypeId = reader["CASETYPE"] == DBNull.Value ? null : Convert.ToInt64(reader["CASETYPE"]),
                    CaseSubjectId = reader["CASESUBJECT"] == DBNull.Value ? null : Convert.ToInt64(reader["CASESUBJECT"]),
                    CasePurposeId = reader["CASE_PURPOSE_NAME"] == DBNull.Value ? null : Convert.ToInt64(reader["CASE_PURPOSE_NAME"]),
                    HearingDate = reader["HEARINGDATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["HEARINGDATE"]),
                    BenchTypeId = reader["BENCH_TYPE"] == DBNull.Value ? null : Convert.ToInt64(reader["BENCH_TYPE"]),
                    LinkedCaseNumber = reader["LINKED_CASE"]?.ToString(),
                    OldCaseNumber = reader["PRVCASENO"]?.ToString(),

                    // ── Step 2: Appellant ──
                    AppellantName = reader["APPELLANT_NAME"]?.ToString(),
                    DesignationId = ParseNullableLong(reader["DESIGNATION"]),
                    DistrictId = ParseNullableLong(reader["ADISTRICT_NAME"]),
                    MobileNumber = reader["MOBILENO"] == DBNull.Value ? null : Convert.ToInt64(reader["MOBILENO"]),
                    AdvocateId = reader["APP_ADVOCATE"] == DBNull.Value ? null : Convert.ToInt64(reader["APP_ADVOCATE"]),
                    AdvocateEmail = reader["APPADV_EMAIL"]?.ToString(),
                    AdvocateMobile = ParseNullableLong(reader["APP_ADVMOBILE"]),
                    EmployeeId = reader["EMPLOYEEID"]?.ToString(),

                    // ── Step 3: Respondent ──
                    DepartmentId = reader["RESPONDENT_DEPARTMENT"] == DBNull.Value ? null : Convert.ToInt64(reader["RESPONDENT_DEPARTMENT"]),
                    RespondentAdvocateId = reader["RESP_ADVOCATE"] == DBNull.Value ? null : Convert.ToInt64(reader["RESP_ADVOCATE"]),
                    RespondentAdvocateEmail = reader["RESP_ADVEMAIL"]?.ToString(),
                    RespondentAdvocateMobile = reader["RESP_ADVMOBILE"] == DBNull.Value ? null : Convert.ToInt64(reader["RESP_ADVMOBILE"]),

                    // ── Step 4: Private Party ──
                    PrivatePartyName = reader["PRIVATE_NAME"]?.ToString(),
                    PrivateDesignation = reader["PRIVATE_DESIGNATION"]?.ToString()
                };

                // Multiple private parties ko wapas rows me split karna
                var names = SplitCsv(reader["PRIVATE_NAME"]?.ToString());
                var designations = SplitCsv(reader["PRIVATE_DESIGNATION"]?.ToString());
                var advocateIds = SplitCsv(reader["PRIVADVOCATEE"]?.ToString());

                var rows = new List<PrivatePartyRowViewModel>();
                int maxCount = Math.Max(names.Count, Math.Max(designations.Count, advocateIds.Count));

                for (int i = 0; i < maxCount; i++)
                {
                    rows.Add(new PrivatePartyRowViewModel
                    {
                        PartyName = i < names.Count ? names[i] : null,
                        Designation = i < designations.Count ? designations[i] : null,
                        AdvocateId = i < advocateIds.Count ? ParseNullableLong(advocateIds[i]) : null
                    });
                }

                if (rows.Count > 0)
                {
                    model.PrivateParties = rows;
                }
            }

            return model;
        }

        // ── Helpers ──
        private static long? ParseNullableLong(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            return ParseNullableLong(value.ToString());
        }

        private static long? ParseNullableLong(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return long.TryParse(value.Trim(), out var result) ? result : (long?)null;
        }

        private static List<string> SplitCsv(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new List<string>();
            return value.Split(',').Select(x => x.Trim()).ToList();
        }
    }
}