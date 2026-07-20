using GCMS.Data;
using GCMS.Models.Entities;
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

        public async Task<long> SaveCaseAsync(CaseRegistration model)
        {
            long caseId = 0;
            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd = new OracleCommand("proc_trn_rcsat_casereg", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_INSTITUTIONDATE", OracleDbType.Date).Value = model.InstitutionDate;

            cmd.Parameters.Add("P_CASE_NO", OracleDbType.Varchar2).Value = model.CaseNo;

            cmd.Parameters.Add("P_MCASE_NOO", OracleDbType.Varchar2).Value = model.ManualCaseNo;

            cmd.Parameters.Add("P_ORDER_NO", OracleDbType.Varchar2).Value = model.OrderNo;

            cmd.Parameters.Add("P_DATE_OF_ORDER", OracleDbType.Date).Value = model.DateOfOrder;

            cmd.Parameters.Add("P_DESIOFFORDER", OracleDbType.Int64).Value = model.OrderIssuedById;

            cmd.Parameters.Add("P_COURT_CODE", OracleDbType.Varchar2).Value = model.CourtCode;

            cmd.Parameters.Add("P_CASETYPE", OracleDbType.Int64).Value = model.CaseTypeId;

            cmd.Parameters.Add("P_CASESUBJECT", OracleDbType.Int64).Value = model.CaseSubjectId;

            cmd.Parameters.Add("P_CASE_PURPOSE_NAME", OracleDbType.Int64).Value = model.CasePurposeId;

            cmd.Parameters.Add("P_HEARINGDATE", OracleDbType.Date).Value = model.HearingDate;

            cmd.Parameters.Add("P_BENCH_TYPE", OracleDbType.Int64).Value = model.BenchTypeId;

            cmd.Parameters.Add("P_LINKED_CASE", OracleDbType.Varchar2).Value = model.LinkedCase;

            cmd.Parameters.Add("P_OLDCASNO", OracleDbType.Varchar2).Value = model.OldCaseNo;

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = model.CreatedBy;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64).Value = DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            var outCaseId = new OracleParameter("p_caseid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outCaseId);

            var refCursor = new OracleParameter("out_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(refCursor);

            await cmd.ExecuteNonQueryAsync();

            if (outCaseId.Value != DBNull.Value)
            {
                caseId = Convert.ToInt64(outCaseId.Value.ToString());
            }

            return caseId;
        }
        public async Task<bool> UpdateCaseAsync(CaseRegistration model)
        {           
            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd =
                new OracleCommand("PROC_TRN_RCSAT_CASEREG", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_INSTITUTIONDATE", OracleDbType.Date).Value = model.InstitutionDate;
            cmd.Parameters.Add("P_CASE_NO", OracleDbType.Varchar2).Value = model.CaseNo;
            cmd.Parameters.Add("P_MCASE_NOO", OracleDbType.Varchar2).Value = model.ManualCaseNo;
            cmd.Parameters.Add("P_ORDER_NO", OracleDbType.Varchar2).Value = model.OrderNo;
            cmd.Parameters.Add("P_DATE_OF_ORDER", OracleDbType.Date).Value = model.DateOfOrder;
            cmd.Parameters.Add("P_DESIOFFORDER", OracleDbType.Int64).Value = model.OrderIssuedById;
            cmd.Parameters.Add("P_COURT_CODE", OracleDbType.Varchar2).Value = model.CourtCode;
            cmd.Parameters.Add("P_CASETYPE", OracleDbType.Int64).Value = model.CaseTypeId;
            cmd.Parameters.Add("P_CASESUBJECT", OracleDbType.Int64).Value = model.CaseSubjectId;
            cmd.Parameters.Add("P_CASE_PURPOSE_NAME", OracleDbType.Int64).Value = model.CasePurposeId;
            cmd.Parameters.Add("P_HEARINGDATE", OracleDbType.Date).Value = model.HearingDate;
            cmd.Parameters.Add("P_BENCH_TYPE", OracleDbType.Int64).Value = model.BenchTypeId;
            cmd.Parameters.Add("P_LINKED_CASE", OracleDbType.Varchar2).Value = model.LinkedCase;
            cmd.Parameters.Add("P_OLDCASNO", OracleDbType.Varchar2).Value = model.OldCaseNo;
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = model.CreatedBy;

            cmd.Parameters.Add("P_TRN_RCSAT_CASEREGID", OracleDbType.Int64).Value = model.CaseId;

            cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = 10;
            cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_CASEID", OracleDbType.Int64).Direction = ParameterDirection.Output;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<CaseRegistration?> GetCaseByIdAsync(long caseId)
        {
            throw new NotImplementedException();
        }

        public async Task<long> SaveAppellantAsync(CaseAppellant model)
        {
            long appellantId = 0;

            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_APPELLANT", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64)
                .Value = model.CaseId;

            cmd.Parameters.Add("p_appellant_name", OracleDbType.Varchar2)
                .Value = model.AppellantName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_designation", OracleDbType.Varchar2)
                .Value = model.Designation ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_adistrict_name", OracleDbType.Varchar2)
                .Value = model.District ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_mobileno", OracleDbType.Int64)
                .Value = model.MobileNo == null
                    ? DBNull.Value
                    : model.MobileNo;

            cmd.Parameters.Add("p_app_advocate", OracleDbType.Int64)
                .Value = model.AdvocateId == null
                    ? DBNull.Value
                    : model.AdvocateId;

            cmd.Parameters.Add("p_appadv_email", OracleDbType.Varchar2)
                .Value = model.AdvocateEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_app_advmobile", OracleDbType.Varchar2)
                .Value = model.AdvocateMobile ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_employeeid", OracleDbType.Varchar2)
                .Value = model.EmployeeId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_trn_rcsat_appellantid", OracleDbType.Int64)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32)
                .Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32)
                .Value = 1;

            var outId = new OracleParameter("p_appellantid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outId);

            var refCursor = new OracleParameter("out_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(refCursor);

            await cmd.ExecuteNonQueryAsync();

            if (outId.Value != DBNull.Value)
            {
                appellantId = ((OracleDecimal)outId.Value).ToInt64();
                //appellantId = Convert.ToInt64(outId.Value.ToString());
            }

            return appellantId;
        }
        public async Task<bool> UpdateAppellantAsync(CaseAppellant model)
        {
            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd =
                new OracleCommand("PROC_TRN_RCSAT_APPELLANT", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            // Update
            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64)
                .Value = model.CaseId;

            cmd.Parameters.Add("p_appellant_name", OracleDbType.Varchar2)
                .Value = (object?)model.AppellantName ?? DBNull.Value;

            cmd.Parameters.Add("p_designation", OracleDbType.Varchar2)
                .Value = (object?)model.Designation ?? DBNull.Value;

            cmd.Parameters.Add("p_adistrict_name", OracleDbType.Varchar2)
                .Value = (object?)model.District ?? DBNull.Value;

            cmd.Parameters.Add("p_mobileno", OracleDbType.Int64)
                .Value = model.MobileNo ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_app_advocate", OracleDbType.Int64)
                .Value = model.AdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_appadv_email", OracleDbType.Varchar2)
                .Value = (object?)model.AdvocateEmail ?? DBNull.Value;

            cmd.Parameters.Add("p_app_advmobile", OracleDbType.Varchar2)
                .Value = (object?)model.AdvocateMobile ?? DBNull.Value;

            cmd.Parameters.Add("p_employeeid", OracleDbType.Varchar2)
                .Value = (object?)model.EmployeeId ?? DBNull.Value;

            // Update ke liye existing Appellant Id bhejna hai
            cmd.Parameters.Add("p_trn_rcsat_appellantid", OracleDbType.Int64)
                .Value = model.AppellantId;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_appellantid", OracleDbType.Int64)
                .Direction = ParameterDirection.Output;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<long> SaveRespondentAsync(CaseRespondent model)
        {
            long respondentId = 0;

            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_RESPONDENT", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            // Insert
            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64)
                .Value = model.CaseId;

            cmd.Parameters.Add("p_respondent_department", OracleDbType.Int64)
                .Value = model.DepartmentId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_resp_advocate", OracleDbType.Int64)
                .Value = model.AdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respadvemail", OracleDbType.Varchar2)
                .Value = model.AdvocateEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respadvmobile", OracleDbType.Int64)
                .Value = model.AdvocateMobile ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respondent_designation", OracleDbType.Varchar2)
                .Value = model.Designation ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_oicname", OracleDbType.Varchar2)
                .Value = model.OICName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_oicmobileno", OracleDbType.Varchar2)
                .Value = model.OICMobileNo ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_trn_rcsat_respondentid", OracleDbType.Int64)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            var outId = new OracleParameter("p_respondentid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outId);

            var refCursor = new OracleParameter("out_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(refCursor);

            await cmd.ExecuteNonQueryAsync();

            if (outId.Value != DBNull.Value)
            {
                respondentId = ((OracleDecimal)outId.Value).ToInt64();
                //respondentId = Convert.ToInt64(outId.Value.ToString());
            }

            return respondentId;
        }
        public async Task<bool> UpdateRespondentAsync(CaseRespondent model)
        {
            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_RESPONDENT", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            // Update
            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64)
                .Value = model.CaseId;

            cmd.Parameters.Add("p_respondent_department", OracleDbType.Int64)
                .Value = model.DepartmentId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_resp_advocate", OracleDbType.Int64)
                .Value = model.AdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respadvemail", OracleDbType.Varchar2)
                .Value = model.AdvocateEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respadvmobile", OracleDbType.Int64)
                .Value = model.AdvocateMobile ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_respondent_designation", OracleDbType.Varchar2)
                .Value = model.Designation ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_oicname", OracleDbType.Varchar2)
                .Value = model.OICName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_oicmobileno", OracleDbType.Varchar2)
                .Value = model.OICMobileNo ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_trn_rcsat_respondentid", OracleDbType.Int64)
                .Value = model.RespondentId;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_respondentid", OracleDbType.Int64)
                .Direction = ParameterDirection.Output;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<long> SavePrivatePartyAsync(CasePrivateParty model)
        {
            long privatePartyId = 0;

            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd =
                new OracleCommand("PROC_TRN_RCSAT_PRIVATE_PARTY", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64)
                .Value = model.CaseId;

            cmd.Parameters.Add("p_name", OracleDbType.Varchar2)
                .Value = model.PartyName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_private_designation", OracleDbType.Varchar2)
                .Value = model.Designation ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_privadvocatee", OracleDbType.Int64)
                .Value = model.AdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_trn_rcsat_private_partyid", OracleDbType.Int64)
                .Value = DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            var outId = new OracleParameter("p_privatepartyid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outId);

            var refCursor = new OracleParameter("out_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(refCursor);

            await cmd.ExecuteNonQueryAsync();

            if (outId.Value != DBNull.Value)
            {
                privatePartyId = ((OracleDecimal)outId.Value).ToInt64();
               // privatePartyId = Convert.ToInt64(outId.Value);
            }

            return privatePartyId;
        }
        public async Task<bool> UpdatePrivatePartyAsync(CasePrivateParty model)
        {
            using var conn =
                (OracleConnection)_context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd =
                new OracleCommand("PROC_TRN_RCSAT_PRIVATE_PARTY", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_trn_rcsat_caseregid", OracleDbType.Int64)
                .Value = model.CaseId;

            cmd.Parameters.Add("p_name", OracleDbType.Varchar2)
                .Value = model.PartyName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_private_designation", OracleDbType.Varchar2)
                .Value = model.Designation ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_privadvocatee", OracleDbType.Int64)
                .Value = model.AdvocateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_trn_rcsat_private_partyid", OracleDbType.Int64)
                .Value = model.PrivatePartyId;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_privatepartyid", OracleDbType.Int64)
                .Direction = ParameterDirection.Output;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<CaseRegistration?> GetCaseAsync(long caseId)
        {
            return await _context.CaseRegistrations
                .FirstOrDefaultAsync(x => x.CaseId == caseId);
        }
        public async Task DeleteCaseAsync(long casid)
        {
            // Next
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
                .OrderBy(x => x.DeptNameEn)
                .Select(x => new SelectListItem
                {
                    Value = x.DepartmentMastId.ToString(),
                    Text = x.DeptNameEn
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
                .OrderBy(x => x.DeptNameEn)
                .Select(x => new SelectListItem
                {
                    Value = x.DepartmentMastId.ToString(),
                    Text = x.DeptNameEn
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<SelectListItem>> GetAdvocatesAsync()
        {
            return await _context.AdvocateMaster
                .Where(x => x.InActive == "T")
                .OrderBy(x => x.AdvocateName)
                .Select(x => new SelectListItem
                {
                    Value = x.AdvocateMastId.ToString(),
                    Text = x.AdvocateName
                })
                .ToListAsync();
        }

       
    }
}