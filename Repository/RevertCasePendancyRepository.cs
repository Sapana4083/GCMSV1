using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GCMS.Repository
{
    public class RevertCasePendancyRepository : IRevertCasePendancyRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public RevertCasePendancyRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET BY HEARING DATE (P_ACTION = 2)
        // ───────────────────────────────────────────────
        public async Task<RevertCasePendancy?> GetByHearingDateAsync(DateTime hearingDate)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCommand(conn, 2);

            cmd.Parameters.Add(new OracleParameter("P_REVD_PENDCYID", OracleDbType.Int64)
            { Direction = ParameterDirection.InputOutput, Value = DBNull.Value });

            cmd.Parameters.Add("P_HEARING_DATE", OracleDbType.Date).Value = hearingDate.Date;

            cmd.Parameters.Add(new OracleParameter("P_TRN_REVERT_CASEID", OracleDbType.Int64)
            { Direction = ParameterDirection.InputOutput, Value = DBNull.Value });

            AddRemainingNulls(cmd);

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            RevertCasePendancy? result = null;

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    if (result == null)
                    {
                        result = new RevertCasePendancy
                        {
                            RevdPendcyId = ToLong(reader["REVD_PENDCYID"]) ?? 0,
                            MemberName = ToLong(reader["PARENT_MEMBER_NAME"]),
                            HearingDate = reader["HEARING_DATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["HEARING_DATE"])
                        };
                    }

                    result.Cases.Add(new RevertCaseRow
                    {
                        TrnRevertCaseId = ToLong(reader["TRN_REVERT_CASEID"]) ?? 0,
                        SerialNo = reader["S_NO"] == DBNull.Value ? null : Convert.ToInt32(reader["S_NO"]),
                        CaseNo = reader["CASE_NO"]?.ToString(),
                        OfficerName = reader["MEMBER_NAME"]?.ToString(),
                        DecisionDate = reader["DECISION_DATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["DECISION_DATE"]),
                        AppellantNamee = reader["APPELLANT_NAMEE"]?.ToString(),
                        DeptNameHi = reader["DEPT_NAMEHI"]?.ToString(),
                        HDate = reader["HDATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["HDATE"])
                    });
                }
            }
            catch (OracleException ex) when (ex.Number == 20003)
            {
                return null; // no data found for hearing date
            }

            return result;
        }

        // ───────────────────────────────────────────────
        // ADD (P_ACTION = 1) — ek baar me ek parent + ek child row insert karta hai SP ke hisaab se.
        // Multiple rows ke liye har row ke liye alag se call karte hain (same parent id reuse hota hai
        // sirf pehli call se, baaki rows me parent dubara insert nahi karna — isliye yaha ek hi row
        // insert ho sakta hai per-call; controller/service multiple rows ke liye loop karega lekin
        // SP khud parent dubara insert kar dega agar dubara call kiya — isliye sirf PEHLI row ke
        // sath parent+child, baaki rows ke liye SP me alag "child-only insert" action nahi hai.
        // Filhal: SP sirf 1 case row support karta hai per insert call.
        // ───────────────────────────────────────────────
        public async Task<RevertCasePendancy> AddAsync(RevertCasePendancy model, string createdBy)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = CreateCommand(conn, 1);

            var parentIdParam = new OracleParameter("P_REVD_PENDCYID", OracleDbType.Int64)
            { Direction = ParameterDirection.InputOutput, Value = DBNull.Value };
            cmd.Parameters.Add(parentIdParam);

            cmd.Parameters.Add("P_CANCEL", OracleDbType.Char).Value = "F";
            cmd.Parameters.Add("P_SOURCEID", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("P_MAPNAME", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_MODIFIEDON", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = createdBy;
            cmd.Parameters.Add("P_CREATEDON", OracleDbType.Date).Value = DateTime.Now;
            cmd.Parameters.Add("P_WKID", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_APP_LEVEL", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_APP_DESC", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_APP_SLEVEL", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_CANCELREMARKS", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_WFROLES", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_MEMBER_NAME", OracleDbType.Int64).Value = model.MemberName ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_HEARING_DATE", OracleDbType.Date).Value = model.HearingDate ?? (object)DBNull.Value;

            var childIdParam = new OracleParameter("P_TRN_REVERT_CASEID", OracleDbType.Int64)
            { Direction = ParameterDirection.InputOutput, Value = DBNull.Value };
            cmd.Parameters.Add(childIdParam);

            var row = model.Cases.First();

            cmd.Parameters.Add("P_TRN_REVERT_CASEROW", OracleDbType.Int32).Value = row.SerialNo ?? 1;
            cmd.Parameters.Add("P_CASE_NO", OracleDbType.Varchar2).Value = row.CaseNo ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_OFFICER_NAME", OracleDbType.Varchar2).Value = row.OfficerName ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_DECISION_DATE", OracleDbType.Date).Value = row.DecisionDate ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_APPELLANT_NAMEE", OracleDbType.Varchar2).Value = row.AppellantNamee ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_DEPT_NAMEHI", OracleDbType.Varchar2).Value = row.DeptNameHi ?? (object)DBNull.Value;
            cmd.Parameters.Add("P_HDATE", OracleDbType.Date).Value = row.HDate ?? model.HearingDate ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            var result = new RevertCasePendancy
            {
                MemberName = model.MemberName,
                HearingDate = model.HearingDate
            };

            try
            {
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    result.RevdPendcyId = ToLong(reader["REVD_PENDCYID"]) ?? 0;

                    result.Cases.Add(new RevertCaseRow
                    {
                        TrnRevertCaseId = ToLong(reader["TRN_REVERT_CASEID"]) ?? 0,
                        SerialNo = reader["S_NO"] == DBNull.Value ? null : Convert.ToInt32(reader["S_NO"]),
                        CaseNo = reader["CASE_NO"]?.ToString(),
                        OfficerName = reader["MEMBER_NAME"]?.ToString(),
                        DecisionDate = reader["DECISION_DATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["DECISION_DATE"]),
                        AppellantNamee = reader["APPELLANT_NAMEE"]?.ToString(),
                        DeptNameHi = reader["DEPT_NAMEHI"]?.ToString(),
                        HDate = reader["HDATE"] == DBNull.Value ? null : Convert.ToDateTime(reader["HDATE"])
                    });
                }
            }
            catch (OracleException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

            return result;
        }

        private static OracleCommand CreateCommand(OracleConnection conn, int action)
        {
            var cmd = new OracleCommand("PROC_REVERT_CASE_PENDANCY", conn)
            {
                CommandType = CommandType.StoredProcedure,
                BindByName = true
            };
            cmd.Parameters.Add("P_ACTION", OracleDbType.Int32).Value = action;
            return cmd;
        }

        private static void AddRemainingNulls(OracleCommand cmd)
        {
            cmd.Parameters.Add("P_CANCEL", OracleDbType.Char).Value = "F";
            cmd.Parameters.Add("P_SOURCEID", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("P_MAPNAME", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_MODIFIEDON", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_CREATEDON", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("P_WKID", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_APP_LEVEL", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_APP_DESC", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_APP_SLEVEL", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_CANCELREMARKS", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_WFROLES", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_MEMBER_NAME", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("P_TRN_REVERT_CASEROW", OracleDbType.Int32).Value = DBNull.Value;
            cmd.Parameters.Add("P_CASE_NO", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_OFFICER_NAME", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_DECISION_DATE", OracleDbType.Date).Value = DBNull.Value;
            cmd.Parameters.Add("P_APPELLANT_NAMEE", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_DEPT_NAMEHI", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("P_HDATE", OracleDbType.Date).Value = DBNull.Value;
        }

        private static long? ToLong(object v)
        {
            if (v == null || v == DBNull.Value) return null;
            return v is OracleDecimal d ? d.ToInt64() : Convert.ToInt64(v);
        }
    }
}