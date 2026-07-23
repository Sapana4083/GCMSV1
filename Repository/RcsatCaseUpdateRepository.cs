using GCMS.Models;
using GCMS.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Threading.Tasks;

namespace GCMS.Repository.Implementations
{
    public class RcsatCaseUpdateRepository : IRcsatCaseUpdateRepository
    {
        private readonly string _connectionString;

        public RcsatCaseUpdateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RcsatOracle");
        }

        public async Task<RcsatCaseUpdateViewModel> GetByLinkCaseAsync(string linkCase)
        {
            const string sql = @"
                SELECT TRN_RCSAT_CASEUPDATEID, COURT_NAME, COURT_CODE, CASETYPE, LINK_CASE, PARENT_CASENO,
                       PARENTCHILDCHK, CALCON, APP_NAME, RESP_NAME, INST_DATE, HDATE,
                       LINKCASENO, DISTRICT, PURPOSE_NAME, PURPOSEID, CASE_TYPE
                FROM TRN_RCSAT_CASEUPDATE
                WHERE LINK_CASE = :linkCase
                AND ROWNUM = 1";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("linkCase", linkCase));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new RcsatCaseUpdateViewModel
                {
                    CaseUpdateId = reader["TRN_RCSAT_CASEUPDATEID"] != DBNull.Value
                        ? Convert.ToInt64(reader["TRN_RCSAT_CASEUPDATEID"]) : (long?)null,
                    CourtName = reader["COURT_NAME"]?.ToString(),
                    CourtCode = reader["COURT_CODE"]?.ToString(),
                    CaseType = reader["CASETYPE"]?.ToString(),
                    LinkCase = reader["LINK_CASE"]?.ToString(),
                    ParentCaseNo = reader["PARENT_CASENO"]?.ToString(),
                    ParentChildChk = reader["PARENTCHILDCHK"]?.ToString(),
                    ConnectedCaseNo = reader["CALCON"]?.ToString(),
                    AppellantName = reader["APP_NAME"]?.ToString(),
                    RespondentName = reader["RESP_NAME"]?.ToString(),
                    InstitutionDate = reader["INST_DATE"] != DBNull.Value
                        ? Convert.ToDateTime(reader["INST_DATE"]) : (DateTime?)null,
                    HearingDate = reader["HDATE"] != DBNull.Value
                        ? Convert.ToDateTime(reader["HDATE"]) : (DateTime?)null,
                    LinkCaseNo = reader["LINKCASENO"]?.ToString(),
                    District = reader["DISTRICT"]?.ToString(),
                    PurposeName = reader["PURPOSE_NAME"]?.ToString(),
                    PurposeId = reader["PURPOSEID"]?.ToString(),
                    SubCaseType = reader["CASE_TYPE"]?.ToString()
                };
            }
            return null;
        }

        public async Task<long> SaveCaseWithLinkedRowsAsync(RcsatCaseUpdateViewModel model)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                long caseUpdateId;

                if (model.CaseUpdateId.HasValue)
                {
                    caseUpdateId = model.CaseUpdateId.Value;

                    const string updateSql = @"
                        UPDATE TRN_RCSAT_CASEUPDATE SET
                            COURT_NAME = :courtName, COURT_CODE = :courtCode, CASETYPE = :caseType,
                            LINK_CASE = :linkCase, PARENT_CASENO = :parentCaseNo,
                            PARENTCHILDCHK = :parentChildChk, CALCON = :calcon,
                            APP_NAME = :appName, RESP_NAME = :respName,
                            INST_DATE = :instDate, HDATE = :hdate, LINKCASENO = :linkCaseNo,
                            DISTRICT = :district, PURPOSE_NAME = :purposeName,
                            PURPOSEID = :purposeId, CASE_TYPE = :subCaseType
                        WHERE TRN_RCSAT_CASEUPDATEID = :caseUpdateId";

                    using var updateCmd = new OracleCommand(updateSql, connection);
                    updateCmd.Transaction = transaction;
                    AddParentParams(updateCmd, model);
                    updateCmd.Parameters.Add(new OracleParameter("caseUpdateId", caseUpdateId));
                    await updateCmd.ExecuteNonQueryAsync();
                }
                else
                {
                    const string insertSql = @"
                        INSERT INTO TRN_RCSAT_CASEUPDATE
                            (COURT_NAME, COURT_CODE, CASETYPE, LINK_CASE, PARENT_CASENO,
                             PARENTCHILDCHK, CALCON, APP_NAME, RESP_NAME, INST_DATE, HDATE,
                             LINKCASENO, DISTRICT, PURPOSE_NAME, PURPOSEID, CASE_TYPE)
                        VALUES
                            (:courtName, :courtCode, :caseType, :linkCase, :parentCaseNo,
                             :parentChildChk, :calcon, :appName, :respName, :instDate, :hdate,
                             :linkCaseNo, :district, :purposeName, :purposeId, :subCaseType)
                        RETURNING TRN_RCSAT_CASEUPDATEID INTO :newId";

                    using var insertCmd = new OracleCommand(insertSql, connection);
                    insertCmd.Transaction = transaction;
                    AddParentParams(insertCmd, model);

                    var idParam = new OracleParameter("newId", OracleDbType.Int64, System.Data.ParameterDirection.Output);
                    insertCmd.Parameters.Add(idParam);

                    await insertCmd.ExecuteNonQueryAsync();
                    caseUpdateId = Convert.ToInt64(((OracleDecimal)idParam.Value).Value);
                }

                foreach (var row in model.LinkedCases)
                {
                    row.CaseUpdateId = caseUpdateId;

                    if (row.CaseId.HasValue)
                    {
                        const string updateChildSql = @"
                            UPDATE TRN_RCSAT_LINKEDCASES SET
                                TRN_RCSAT_CASEUPDATEID = :caseUpdateId, COURTCODE = :courtCode,
                                CASE_TYPEE = :caseTypee, CASE_NO = :caseNo, CALCASE = :calCase,
                                MINCASE = :mainCase, INSDT = :insdt, HDT = :hdt,
                                APPNAME = :appName, RESPNAME = :respName, CTYPE = :ctype,
                                PURPOSE = :purpose, DNAME = :dname, APPADV = :appAdv,
                                RESPAD = :respAd, CONECT = :conect
                            WHERE TRN_RCSAT_CASEID = :caseId";

                        using var updateChildCmd = new OracleCommand(updateChildSql, connection);
                        updateChildCmd.Transaction = transaction;
                        AddChildParams(updateChildCmd, row);
                        updateChildCmd.Parameters.Add(new OracleParameter("caseId", row.CaseId.Value));
                        await updateChildCmd.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        const string insertChildSql = @"
                            INSERT INTO TRN_RCSAT_LINKEDCASES
                                (TRN_RCSAT_CASEUPDATEID, COURTCODE, CASE_TYPEE, CASE_NO, CALCASE,
                                 MINCASE, INSDT, HDT, APPNAME, RESPNAME, CTYPE, PURPOSE,
                                 DNAME, APPADV, RESPAD, CONECT)
                            VALUES
                                (:caseUpdateId, :courtCode, :caseTypee, :caseNo, :calCase,
                                 :mainCase, :insdt, :hdt, :appName, :respName, :ctype, :purpose,
                                 :dname, :appAdv, :respAd, :conect)";

                        using var insertChildCmd = new OracleCommand(insertChildSql, connection);
                        insertChildCmd.Transaction = transaction;
                        AddChildParams(insertChildCmd, row);
                        await insertChildCmd.ExecuteNonQueryAsync();
                    }
                }

                transaction.Commit();
                return caseUpdateId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private static void AddParentParams(OracleCommand cmd, RcsatCaseUpdateViewModel model)
        {
            cmd.Parameters.Add(new OracleParameter("courtName", model.CourtName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("courtCode", model.CourtCode ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("caseType", model.CaseType ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("linkCase", model.LinkCase ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("parentCaseNo", model.ParentCaseNo ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("parentChildChk", model.ParentChildChk ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("calcon", model.ConnectedCaseNo ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("appName", model.AppellantName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("respName", model.RespondentName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("instDate", (object)model.InstitutionDate ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("hdate", (object)model.HearingDate ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("linkCaseNo", model.LinkCaseNo ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("district", model.District ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("purposeName", model.PurposeName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("purposeId", model.PurposeId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("subCaseType", model.SubCaseType ?? (object)DBNull.Value));
        }

        private static void AddChildParams(OracleCommand cmd, LinkedCaseRowViewModel row)
        {
            cmd.Parameters.Add(new OracleParameter("caseUpdateId", row.CaseUpdateId));
            cmd.Parameters.Add(new OracleParameter("courtCode", row.CourtCode ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("caseTypee", row.CaseTypee ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("caseNo", row.CaseNo ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("calCase", row.CalCase ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("mainCase", row.MainCase ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("insdt", (object)row.InstitutionDate ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("hdt", (object)row.HearingDate ?? DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("appName", row.AppellantName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("respName", row.RespondentName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("ctype", row.CType ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("purpose", row.Purpose ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("dname", row.DistrictName ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("appAdv", row.AppellantAdvocate ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("respAd", row.RespondentAdvocate ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("conect", row.Connected ?? (object)DBNull.Value));
        }
    }
}