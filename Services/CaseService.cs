using GCMS.Models.Entities;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _repository;

        public CaseService(ICaseRepository repository)
        {
            _repository = repository;
        }

        // FINAL SUBMIT
        // Saves Step 1 + Step 2 + Step 3 + Step 4 using one Oracle stored procedure.
        public async Task<long> SaveFullCaseRegistrationAsync(
      CaseRegistrationWizardViewModel model,
      string createdBy)
        {
            long caseId = 0;

            var caseEntity = new CaseRegistration
            {
                InstitutionDate = model.InstitutionDate,
                CaseNo = model.CaseNumber,
                OrderNo = model.OrderNumber,
                DateOfOrder = model.DateofImpugnedOrder,
                OrderIssuedById = model.OrderIssuedById,
                CourtCode = model.CourtCode,
                CaseTypeId = model.CaseTypeId,
                CaseSubjectId = model.CaseSubjectId,
                CasePurposeId = model.CasePurposeId,
                HearingDate = model.HearingDate,
                BenchTypeId = model.BenchTypeId,
                LinkedCase = model.LinkedCaseNumber,
                OldCaseNo = model.OldCaseNumber,
                CreatedBy = createdBy
            };

            caseId = await _repository.SaveCaseAsync(caseEntity);

            var appellantEntity = new CaseAppellant
            {
                CaseId = caseId,
                AppellantName = model.AppellantName,
                Designation = model.DesignationId?.ToString(),
                District = model.DistrictId?.ToString(),
                MobileNo = model.MobileNumber,
                EmployeeId = model.EmployeeId,
                AdvocateId = model.AdvocateId,
                AdvocateEmail = model.AdvocateEmail,
                AdvocateMobile = model.AdvocateMobile?.ToString()
            };

            await _repository.SaveAppellantAsync(appellantEntity);

            var respondentEntity = new CaseRespondent
            {
                CaseId = caseId,
                DepartmentId = model.DepartmentId,
                AdvocateId = model.RespondentAdvocateId,
                AdvocateEmail = model.RespondentAdvocateEmail,
                AdvocateMobile = model.RespondentAdvocateMobile,
                CreatedBy = createdBy
            };

            await _repository.SaveRespondentAsync(respondentEntity);

            if (model.PrivateParties != null && model.PrivateParties.Any())
            {
                foreach (var party in model.PrivateParties)
                {
                    if (string.IsNullOrWhiteSpace(party.PartyName)
                        && string.IsNullOrWhiteSpace(party.Designation)
                        && party.AdvocateId == null)
                    {
                        continue;
                    }

                    var privatePartyEntity = new CasePrivateParty
                    {
                        CaseId = caseId,
                        PartyName = party.PartyName,
                        Designation = party.Designation,
                        AdvocateId = party.AdvocateId,
                        CreatedBy = createdBy
                    };

                    await _repository.SavePrivatePartyAsync(privatePartyEntity);
                }
            }

            return caseId;
        }

        // CASE READ / DELETE
        public async Task<CaseRegistration?> GetCaseAsync(long caseId)
        {
            return await _repository.GetCaseAsync(caseId);
        }

        public async Task DeleteCaseAsync(long caseId)
        {
            await _repository.DeleteCaseAsync(caseId);
        }

        // DROPDOWNS
        public async Task<IEnumerable<SelectListItem>> GetCaseTypesAsync()
        {
            return await _repository.GetCaseTypesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCaseSubjectsAsync()
        {
            return await _repository.GetCaseSubjectsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCasePurposesAsync()
        {
            return await _repository.GetCasePurposesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBenchTypesAsync()
        {
            return await _repository.GetBenchTypesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDepartmentsAsync()
        {
            return await _repository.GetDepartmentsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDesignationsAsync()
        {
            return await _repository.GetDesignationsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrictsAsync()
        {
            return await _repository.GetDistrictsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAdvocatesAsync()
        {
            return await _repository.GetAdvocatesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetOrderTypesAsync()
        {
            return await _repository.GetOrderTypesAsync();
        }
    }
}