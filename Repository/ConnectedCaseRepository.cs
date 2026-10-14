using GCMS.Data;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class ConnectedCaseRepository : IConnectedCaseRepository
    {
        private const string Proc = "PROC_TRN_RCSAT_CONNECTED_CASEUPDATE";
        private readonly OracleConnectionFactory _connectionFactory;

        public ConnectedCaseRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ACTION 4 — child case no se row
        public async Task<ConnectedCaseRowViewModel?> GetRowByCaseNoAsync(string caseNo)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCmd(conn, 4, null, null, out _, out _);
            cmd.Parameters.Add("P_CASE_NO", OracleDbType.Varchar2).Value = caseNo;
            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            try
            {
                using var r = await cmd.ExecuteReaderAsync();
                if (!await r.ReadAsync()) return null;

                return new ConnectedCaseRowViewModel
                {
                    LinkedCaseId = ToLong(r["TRN_RCSAT_LINKEDCASESID"]),
                    CaseUpdateId = ToLong(r["TRN_RCSAT_CASEUPDATEID"]),
                    RowNo = ToInt(r["TRN_RCSAT_LINKEDCASESROW"]),
                    CaseNo = r["CASE_NO"]?.ToString(),
                    MainCase = r["MINCASE"]?.ToString(),
                    HearingDate = ToDate(r["HDT"]),
                    AppellantName = r["APPNAME"]?.ToString(),
                    CaseTypeId = ToLong(r["CASE_TYPEE"]),
                    CaseTypeName = r["CTYPE"]?.ToString(),
                    RespondentName = r["RESPNAME"]?.ToString(),
                    Purpose = r["PURPOSE"]?.ToString(),
                    District = r["DNAME"]?.ToString(),
                    CourtCode = r["COURTCODE"]?.ToString()
                };
            }
            catch (OracleException ex) when (ex.Number == 20012)
            {
                return null; // not found
            }
        }

        // ACTION 3 — parent + children
        public async Task<ConnectedCaseUpdateViewModel?> GetByIdAsync(long caseUpdateId)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCmd(conn, 3, caseUpdateId, null, out _, out _);
            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            ConnectedCaseUpdateViewModel? m = null;

            try
            {
                using var r = await cmd.ExecuteReaderAsync();
                while (await r.ReadAsync())
                {
                    m ??= new ConnectedCaseUpdateViewModel
                    {
                        CaseUpdateId = ToLong(r["TRN_RCSAT_CASEUPDATEID"]),
                        CaseTypeName = r["CASE_TYPE"]?.ToString(),
                        CaseTypeId = ToLong(r["CASETYPE"]),
                        LinkCase = r["LINK_CASE"]?.ToString(),
                        AppellantName = r["APP_NAME"]?.ToString(),
                        RespondentName = r["RESP_NAME"]?.ToString(),
                        InstitutionDate = ToDate(r["INST_DATE"]),
                        HearingDate = ToDate(r["HDATE"]),
                        CourtName = r["COURT_NAME"]?.ToString(),
                        CourtCode = r["COURT_CODE"]?.ToString(),
                        District = r["DISTRICT"]?.ToString(),
                        PurposeName = r["PURPOSE_NAME"]?.ToString()
                    };

                    if (r["TRN_RCSAT_LINKEDCASESID"] != DBNull.Value)
                    {
                        m.Rows.Add(new ConnectedCaseRowViewModel
                        {
                            LinkedCaseId = ToLong(r["TRN_RCSAT_LINKEDCASESID"]),
                            CaseUpdateId = m.CaseUpdateId,
                            RowNo = ToInt(r["TRN_RCSAT_LINKEDCASESROW"]),
                            CaseNo = r["CASE_NO"]?.ToString(),
                            MainCase = r["MINCASE"]?.ToString(),
                            HearingDate = ToDate(r["HDT"]),
                            AppellantName = r["APPNAME"]?.ToString(),
                            CaseTypeId = ToLong(r["CASE_TYPEE"]),
                            CaseTypeName = r["CTYPE"]?.ToString(),
                            RespondentName = r["RESPNAME"]?.ToString(),
                            Purpose = r["PURPOSE"]?.ToString(),
                            District = r["DNAME"]?.ToString(),
                            CourtCode = r["COURTCODE"]?.ToString()
                        });
                    }
                }
            }
            catch (OracleException ex) when (ex.Number == 20010)
            {
                return null;
            }

            return m;
        }

        // ACTION 1 (pehli new row) / 5 (baaki new rows) / 2 (existing row) — ek transaction me
        public async Task<long> SaveAsync(ConnectedCaseUpdateViewModel model, string userName)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();

            long parentId = model.CaseUpdateId ?? 0;
            int rowNo = 0;

            try
            {
                foreach (var row in model.Rows)
                {
                    rowNo++;
                    bool isExisting = row.LinkedCaseId.HasValue && row.LinkedCaseId.Value > 0;
                    int action = isExisting ? 2 : (parentId == 0 ? 1 : 5);

                    using var cmd = CreateCmd(conn, action,
                        parentId == 0 ? null : parentId,
                        isExisting ? row.LinkedCaseId : null,
                        out var parentParam, out var childParam);
                    cmd.Transaction = tx;

                    void S(string n, OracleDbType t, object? v) =>
                        cmd.Parameters.Add(n, t).Value = v ?? DBNull.Value;

                    if (action != 5)
                    {
                        S("P_CANCEL", OracleDbType.Char, "F");
                        S("P_USERNAME", OracleDbType.Varchar2, userName);
                        S("P_CREATEDBY", OracleDbType.Varchar2, userName);
                        S("P_COURT_NAME", OracleDbType.Varchar2, model.CourtName);
                        S("P_COURT_CODE", OracleDbType.Varchar2, model.CourtCode);
                        S("P_CASETYPE", OracleDbType.Int64, model.CaseTypeId);
                        S("P_LINK_CASE", OracleDbType.Varchar2, model.LinkCase);
                        S("P_APP_NAME", OracleDbType.Varchar2, model.AppellantName);
                        S("P_RESP_NAME", OracleDbType.Varchar2, model.RespondentName);
                        S("P_INST_DATE", OracleDbType.Date, model.InstitutionDate);
                        S("P_HDATE", OracleDbType.Date, model.HearingDate);
                        S("P_LINKCASENO", OracleDbType.Varchar2, model.LinkCase);
                        S("P_DISTRICT", OracleDbType.Varchar2, model.District);
                        S("P_PURPOSE_NAME", OracleDbType.Varchar2, model.PurposeName);
                        S("P_CASE_TYPE", OracleDbType.Varchar2, model.CaseTypeName);
                    }

                    S("P_TRN_RCSAT_LINKEDCASESROW", OracleDbType.Int32, rowNo);
                    S("P_CASE_NO", OracleDbType.Varchar2, row.CaseNo);
                    S("P_MINCASE", OracleDbType.Varchar2, model.LinkCase);
                    S("P_LINK_HDT", OracleDbType.Date, row.HearingDate);
                    S("P_LINK_APPNAME", OracleDbType.Varchar2, row.AppellantName);
                    S("P_CASE_TYPEE", OracleDbType.Int64, row.CaseTypeId);
                    S("P_LINK_RESPNAME", OracleDbType.Varchar2, row.RespondentName);
                    S("P_CTYPE", OracleDbType.Varchar2, row.CaseTypeName);
                    S("P_PURPOSE", OracleDbType.Varchar2, row.Purpose);
                    S("P_DNAME", OracleDbType.Varchar2, row.District);
                    S("P_LINK_COURTCODE", OracleDbType.Varchar2, model.CourtCode);
                    cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    await cmd.ExecuteNonQueryAsync();

                    if (parentId == 0 && parentParam.Value is OracleDecimal d && !d.IsNull)
                        parentId = d.ToInt64();
                }

                tx.Commit();
                return parentId;
            }
            catch (OracleException ex)
            {
                tx.Rollback();
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        // Children seedha linked-cases table se (MINCASE = main case)
        public async Task<List<ConnectedCaseRowViewModel>> GetRowsByMainCaseAsync(string mainCase)
        {
            var list = new List<ConnectedCaseRowViewModel>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = @"
        SELECT TRN_RCSAT_LINKEDCASESID, TRN_RCSAT_CASEUPDATEID, TRN_RCSAT_LINKEDCASESROW,
               CASE_NO, MINCASE, HDT, APPNAME, CASE_TYPEE, RESPNAME, CTYPE, PURPOSE, DNAME, COURTCODE
        FROM GCMS.TRN_RCSAT_LINKEDCASES
        WHERE TRIM(MINCASE) = :mainCase
        ORDER BY TRN_RCSAT_LINKEDCASESID";
            cmd.Parameters.Add(new OracleParameter("mainCase", OracleDbType.Varchar2) { Value = mainCase.Trim() });

            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
            {
                list.Add(new ConnectedCaseRowViewModel
                {
                    LinkedCaseId = ToLong(r["TRN_RCSAT_LINKEDCASESID"]),
                    CaseUpdateId = ToLong(r["TRN_RCSAT_CASEUPDATEID"]),
                    RowNo = ToInt(r["TRN_RCSAT_LINKEDCASESROW"]),
                    CaseNo = r["CASE_NO"]?.ToString(),
                    MainCase = r["MINCASE"]?.ToString(),
                    HearingDate = ToDate(r["HDT"]),
                    AppellantName = r["APPNAME"]?.ToString(),
                    CaseTypeId = ToLong(r["CASE_TYPEE"]),
                    CaseTypeName = r["CTYPE"]?.ToString(),
                    RespondentName = r["RESPNAME"]?.ToString(),
                    Purpose = r["PURPOSE"]?.ToString(),
                    District = r["DNAME"]?.ToString(),
                    CourtCode = r["COURTCODE"]?.ToString()
                });
            }
            return list;
        }

        // Fallback lookup from Case Registration (used when SP data is blank)
        public async Task<ConnectedCaseRowViewModel?> GetCaseRegDetailsAsync(string caseNo, long? caseTypeId)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            var sql = @"
        SELECT C.MCASE_NOO, C.INSTITUTIONDATE, C.HEARINGDATE, C.CASETYPE,
               A.APPELLANT_NAME, D.DEPENGHI, P.PURPOSEENGHI
        FROM TRN_RCSAT_CASEREG C
        LEFT JOIN TRN_RCSAT_APPELLANT A ON A.TRN_RCSAT_CASEREGID = C.TRN_RCSAT_CASEREGID
        LEFT JOIN TRN_RCSAT_RESPONDENT R ON R.TRN_RCSAT_CASEREGID = C.TRN_RCSAT_CASEREGID
        LEFT JOIN CM_RCSAT_DEPT D ON D.CM_RCSAT_DEPTID = R.RESPONDENT_DEPARTMENT
        LEFT JOIN MAST_RCSAT_CSPURPOSE P ON P.MAST_RCSAT_CSPURPOSEID = C.CASE_PURPOSE_NAME
        WHERE TRIM(C.MCASE_NOO) = :caseNo
          AND NVL(C.CANCEL, 'F') = 'F'"
                + (caseTypeId.HasValue ? " AND C.CASETYPE = :caseTypeId" : "")
                + " AND ROWNUM = 1";

            using var cmd = (OracleCommand)conn.CreateCommand();
            cmd.BindByName = true;
            cmd.CommandText = sql;
            cmd.Parameters.Add(new OracleParameter("caseNo", OracleDbType.Varchar2) { Value = caseNo.Trim() });
            if (caseTypeId.HasValue)
                cmd.Parameters.Add(new OracleParameter("caseTypeId", OracleDbType.Int64) { Value = caseTypeId.Value });

            using var r = await cmd.ExecuteReaderAsync();
            if (!await r.ReadAsync()) return null;

            return new ConnectedCaseRowViewModel
            {
                CaseNo = r["MCASE_NOO"]?.ToString(),
                CaseTypeId = ToLong(r["CASETYPE"]),
                HearingDate = ToDate(r["HEARINGDATE"]),
                AppellantName = r["APPELLANT_NAME"]?.ToString(),
                RespondentName = r["DEPENGHI"]?.ToString(),
                Purpose = r["PURPOSEENGHI"]?.ToString(),
                // Institution date ke liye temporary use (header me kaam aata hai)
                CaseTypeName = r["INSTITUTIONDATE"] == DBNull.Value ? null : Convert.ToDateTime(r["INSTITUTIONDATE"]).ToString("yyyy-MM-dd")
            };
        }

        private static OracleCommand CreateCmd(OracleConnection conn, int action, long? parentId, long? childId,
            out OracleParameter parentParam, out OracleParameter childParam)
        {
            var cmd = new OracleCommand(Proc, conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };
            cmd.Parameters.Add("P_ACTION", OracleDbType.Int32).Value = action;

            parentParam = new OracleParameter("P_TRN_RCSAT_CASEUPDATEID", OracleDbType.Int64)
            { Direction = ParameterDirection.InputOutput, Value = parentId.HasValue ? parentId.Value : DBNull.Value };
            cmd.Parameters.Add(parentParam);

            childParam = new OracleParameter("P_TRN_RCSAT_LINKEDCASESID", OracleDbType.Int64)
            { Direction = ParameterDirection.InputOutput, Value = childId.HasValue ? childId.Value : DBNull.Value };
            cmd.Parameters.Add(childParam);

            return cmd;
        }

        private static long? ToLong(object v) =>
            v == null || v == DBNull.Value ? null : v is OracleDecimal d ? d.ToInt64() : Convert.ToInt64(v);

        private static int? ToInt(object v) =>
            v == null || v == DBNull.Value ? null : v is OracleDecimal d ? d.ToInt32() : Convert.ToInt32(v);

        private static DateTime? ToDate(object v) =>
            v == null || v == DBNull.Value ? null : Convert.ToDateTime(v);
    }
}