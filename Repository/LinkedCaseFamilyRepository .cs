using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace GCMS.Repository
{
    public class LinkedCaseFamilyRepository : ILinkedCaseFamilyRepository
    {
        private readonly string _connectionString;

        public LinkedCaseFamilyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RcsatOracle");
        }

        public async Task<List<LinkedCaseFamilyViewModel>> GetCaseFamilyAsync(
            string linkCase, string parentCaseNo, string courtCode)
        {
            var list = new List<LinkedCaseFamilyViewModel>();

            const string sql = @"
            SELECT A.TRN_RCSAT_CASEREGID,
                   A.MCASE_NOO CASE_NO,
                   A.LINKED_CASE AS CONNECTED_CASE_NO,
                   A.HEARINGDATE,
                   B.MAST_RCSAT_CSPURPOSEID CASE_PURPOSE_MASTID,
                   B.PURPOSEENGHI CASE_PURPOSE_NAME,
                   A.APPELLANT_NAMEE PRIMARY_APPELLANT,
                   RD.DEPT_NAMEHI PRIMARY_RESPONDENT,
                   C.DISTRICT_NAME
            FROM VW_RCSAT_CASE_FAMILYS A1,
                 TRN_RCSAT_CASEREG A,
                 MAST_RCSAT_CSPURPOSE B,
                 CM_RCSAT_DEPT RD,
                 DISTRICT_MAST C
            WHERE A1.CASE_NO = A.CASE_NO
              AND A.RESPONDENT_DEPARTMENTT = RD.CM_RCSAT_DEPTID
              AND A.CASE_PURPOSE_NAME = B.MAST_RCSAT_CSPURPOSEID
              AND A.DISTRICT_NAME = C.DISTRICT_MASTID
              AND A.CASE_NO != :linkCase
              AND A.CASE_NO != :parentCaseNo
              AND A1.PARENTCASE = :parentCaseNo
              AND A.COURT_CODE = :courtCode
            ORDER BY A1.LEVELNO, A1.CASE_NO";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("linkCase", linkCase ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("parentCaseNo", parentCaseNo ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("courtCode", courtCode ?? (object)DBNull.Value));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new LinkedCaseFamilyViewModel
                {
                    CaseRegId = Convert.ToInt64(reader["TRN_RCSAT_CASEREGID"]),
                    CaseNo = reader["CASE_NO"]?.ToString(),
                    ConnectedCaseNo = reader["CONNECTED_CASE_NO"]?.ToString(),
                    HearingDate = reader["HEARINGDATE"] != DBNull.Value
                        ? Convert.ToDateTime(reader["HEARINGDATE"]) : null,
                    CasePurposeMastId = reader["CASE_PURPOSE_MASTID"]?.ToString(),
                    CasePurposeName = reader["CASE_PURPOSE_NAME"]?.ToString(),
                    PrimaryAppellant = reader["PRIMARY_APPELLANT"]?.ToString(),
                    PrimaryRespondent = reader["PRIMARY_RESPONDENT"]?.ToString(),
                    DistrictName = reader["DISTRICT_NAME"]?.ToString()
                });
            }
            return list;
        }
    }
}
