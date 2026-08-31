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
            using var conn = (OracleConnection)_context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand("PROC_TRN_RCSAT_CASEREG_FULL", conn)
            {
                CommandType = CommandType.StoredProcedure
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

            cmd.Parameters.Add("p_private_name", OracleDbType.Varchar2).Value =
                model.PrivatePartyName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_private_designation", OracleDbType.Varchar2).Value =
                model.PrivateDesignation ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_privadvocatee", OracleDbType.Int64).Value =
                model.PrivateAdvocateId ?? (object)DBNull.Value;

            var outCaseId = new OracleParameter("p_caseid", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outCaseId);

            await cmd.ExecuteNonQueryAsync();

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
    }
}