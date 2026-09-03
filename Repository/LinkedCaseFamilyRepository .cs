using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

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
            try {
                var list = new List<LinkedCaseFamilyViewModel>(); 

                //const string sql = @"
                //SELECT A.TRN_RCSAT_CASEREGID,
                //       A.MCASE_NOO CASE_NO,
                //       A.LINKED_CASE AS CONNECTED_CASE_NO,
                //       A.HEARINGDATE HDATE,
                //       B.MAST_RCSAT_CSPURPOSEID CASE_PURPOSE_MASTID,
                //       B.PURPOSEENGHI PURPOSE_NAME,
                //       A.APPELLANT_NAMEE APP_NAME,
                //       RD.DEPT_NAMEHI RESP_NAME,
                //       C.DISTRICT_NAME DISTRICT
                //FROM VW_RCSAT_CASE_FAMILYS A1,
                //     TRN_RCSAT_CASEREG A,
                //     MAST_RCSAT_CSPURPOSE B,
                //     CM_RCSAT_DEPT RD,
                //     DISTRICT_MAST C
                //WHERE A1.CASE_NO = A.MCASE_NOO
                //  AND A.RESPONDENT_DEPARTMENTT = RD.CM_RCSAT_DEPTID
                //  AND A.CASE_PURPOSE_NAME = B.MAST_RCSAT_CSPURPOSEID
                //  AND A.DISTRICT_NAME = C.DISTRICT_MASTID
                //  AND A.MCASE_NOO != :linkCase
                //  AND A.MCASE_NOO != :parentCaseNo
                //  AND A1.PARENTCASE = :parentCaseNo
                //  AND A.COURT_CODE = :courtCode
                //  ORDER BY A1.LEVELNO, A1.CASE_NO";
                const string sql = @"
                            SELECT 
                            A.TRN_RCSAT_CASEREGID,
                             A.APPELLANT_NAMEE APP_NAME,
                                     RD.DEPT_NAMEHI RESP_NAME,
                                     A.INSTITUTIONDATE INST_DATE,
                                   A.HEARINGDATE HDATE,
                             CT.CASE_TYPE CASETYPE,
                              B.PURPOSEENGHI PURPOSE_NAME,
                              C.DISTRICT_NAME DISTRICT,
                                     a1.case_no CASE_NO,
                                    A1.LINKED_CASE_NO AS LINKED_CASE_NO,
                                     B.mast_rcsat_cspurposeid PURPOSEID,
                                     A.court_code,
                                     A.LINKED_CASE as ParentCaseNo
                                     
                                FROM VW_rcsat_CASE_FAMILYS a1,
                                     trn_rcsat_casereg a,
                                     mast_rcsat_cspurpose  b,
                                     cm_rcsat_dept rd,
                                     district_mast c,
                                     case_type_mast ct
                               WHERE     A1.CASE_NO = A.MCASE_NOO
                                     and A.RESPONDENT_DEPARTMENTT = RD.CM_RCSAT_DEPTID 
                                     AND A.CASE_PURPOSE_NAME = B.mast_rcsat_cspurposeID
                                     AND A.DISTRICT_NAME = C.DISTRICT_MASTID
                                    AND A.casetype = CT.CASE_TYPE_MASTID
                                    AND A.CASE_NO != :link_case 
                                    AND A.CASE_NO != :parent_caseno
                                     AND A1.LINKED_CASE_NO = :parent_caseno  
                            and a.court_code = :court_code       
                            ORDER BY a1.LEVELNO, A1.CASE_NO";

                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new OracleCommand(sql, connection);
                command.BindByName = true;
                
                var pLinkCase = new OracleParameter("link_case", OracleDbType.Varchar2)
                {
                    Value = (object)linkCase ?? DBNull.Value
                };
                var pParentCaseNo = new OracleParameter("parent_caseno", OracleDbType.Varchar2)
                {
                    Value = (object)parentCaseNo ?? DBNull.Value
                };
                var pCourtCode = new OracleParameter("court_code", OracleDbType.Varchar2)
                {
                    Value = (object)courtCode ?? DBNull.Value
                };

                command.Parameters.Add(pLinkCase);
                command.Parameters.Add(pParentCaseNo);
                command.Parameters.Add(pCourtCode);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new LinkedCaseFamilyViewModel
                    {
                        //CaseUpdateId = reader["TRN_RCSAT_CASEUPDATEID"] != DBNull.Value
                        //    ? Convert.ToInt64(reader["TRN_RCSAT_CASEUPDATEID"]) : (long?)null,


                        //CourtName = reader["COURT_NAME"]?.ToString(),
                        TRN_RCSAT_CASEREGID = reader["TRN_RCSAT_CASEREGID"]?.ToString(),
                        CourtCode = reader["COURT_CODE"]?.ToString(),
                        CaseType = reader["CASETYPE"]?.ToString(),
                         ChildCase = reader["CASE_NO"]?.ToString(),
                        ParentCaseNo = reader["PARENTCASENO"]?.ToString(),
                        //ParentChildChk = reader["PARENTCHILDCHK"]?.ToString(),
                        //ConnectedCaseNo = reader["CALCON"]?.ToString(),
                        AppellantName = reader["APP_NAME"]?.ToString(),
                        RespondentName = reader["RESP_NAME"]?.ToString(),
                        InstitutionDate = reader["INST_DATE"] != DBNull.Value
                            ? Convert.ToDateTime(reader["INST_DATE"]) : (DateTime?)null,
                        HearingDate = reader["HDATE"] != DBNull.Value
                            ? Convert.ToDateTime(reader["HDATE"]) : (DateTime?)null,
                        LinkCaseNo = reader["LINKED_CASE_NO"]?.ToString(),
                        District = reader["DISTRICT"]?.ToString(),
                        PurposeName = reader["PURPOSE_NAME"]?.ToString(),
                        //PurposeId = reader["PURPOSEID"]?.ToString(),
                        PurposeId = reader["PURPOSEID"] != DBNull.Value? reader["PURPOSEID"].ToString(): "0",
                        //SubCaseType = reader["CASE_TYPE"]?.ToString()
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
