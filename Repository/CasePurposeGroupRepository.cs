using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repositories
{
    public class CasePurposeGroupRepository : ICasePurposeGroupRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public CasePurposeGroupRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ── GET ALL (V_INPUT = 5) ───────────────────────────────
        public async Task<List<CasePurposeGroupMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<CasePurposeGroupMaster>();

            using (var conn = (OracleConnection)_connectionFactory.CreateConnection())
            {
                await conn.OpenAsync();

                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "PROC_CASE_PURPOSE_GROUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;
                    cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = pageNo;
                    cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = rowCnt;

                    cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(Map(reader));
                        }
                    }
                }
            }

            return list;
        }

        // ── GET BY ID (V_INPUT = 3) ─────────────────────────────
        public async Task<CasePurposeGroupMaster?> GetByIdAsync(long id)
        {
            CasePurposeGroupMaster? model = null;

            using (var conn = (OracleConnection)_connectionFactory.CreateConnection())
            {
                await conn.OpenAsync();

                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "PROC_CASE_PURPOSE_GROUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 3;
                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP_MASTID", OracleDbType.Int32).Value = id;

                    cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            model = Map(reader);
                        }
                    }
                }
            }

            return model;
        }

        // ── ADD (V_INPUT = 1) ────────────────────────────────────
        public async Task AddAsync(CasePurposeGroupMaster model)
        {
            using (var conn = (OracleConnection)_connectionFactory.CreateConnection())
            {
                await conn.OpenAsync();

                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "PROC_CASE_PURPOSE_GROUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

                    cmd.Parameters.Add("P_CASE_CODE", OracleDbType.Varchar2).Value =
                        (object?)model.CaseCode ?? DBNull.Value;

                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP", OracleDbType.Varchar2).Value =
                        (object?)model.CasePurposeGroup ?? DBNull.Value;

                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP_ENG", OracleDbType.Varchar2).Value =
                        (object?)model.CasePurposeGroupEng ?? DBNull.Value;

                    cmd.Parameters.Add("P_ORDER_LEVEL", OracleDbType.Int32).Value =
                        (object?)model.OrderLevel ?? DBNull.Value;

                    cmd.Parameters.Add("P_RB_ID", OracleDbType.Varchar2).Value =
                        (object?)model.RbId ?? DBNull.Value;

                    cmd.Parameters.Add("P_RB_CLPRIORITY", OracleDbType.Int32).Value =
                        (object?)model.RbClPriority ?? DBNull.Value;

                    cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value =
                        (object?)model.CreatedBy ?? DBNull.Value;

                    cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        await ThrowIfErrorAsync(reader);
                    }
                }
            }
        }

        // ── UPDATE (V_INPUT = 2) ─────────────────────────────────
        public async Task UpdateAsync(CasePurposeGroupMaster model)
        {
            using (var conn = (OracleConnection)_connectionFactory.CreateConnection())
            {
                await conn.OpenAsync();

                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "PROC_CASE_PURPOSE_GROUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP_MASTID", OracleDbType.Int32).Value =
                        model.CasePurposeGroupMastId;

                    cmd.Parameters.Add("P_CASE_CODE", OracleDbType.Varchar2).Value =
                        (object?)model.CaseCode ?? DBNull.Value;

                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP", OracleDbType.Varchar2).Value =
                        (object?)model.CasePurposeGroup ?? DBNull.Value;

                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP_ENG", OracleDbType.Varchar2).Value =
                        (object?)model.CasePurposeGroupEng ?? DBNull.Value;

                    cmd.Parameters.Add("P_ORDER_LEVEL", OracleDbType.Int32).Value =
                        (object?)model.OrderLevel ?? DBNull.Value;

                    cmd.Parameters.Add("P_RB_ID", OracleDbType.Varchar2).Value =
                        (object?)model.RbId ?? DBNull.Value;

                    cmd.Parameters.Add("P_RB_CLPRIORITY", OracleDbType.Int32).Value =
                        (object?)model.RbClPriority ?? DBNull.Value;

                    cmd.Parameters.Add("P_CANCEL", OracleDbType.Varchar2).Value =
                        (object?)model.Cancel ?? "F";

                    cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2).Value =
                        (object?)model.ModifiedBy ?? DBNull.Value;

                    cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        await ThrowIfErrorAsync(reader);
                    }
                }
            }
        }

        // ── DELETE (soft-delete via UPDATE, Cancel = 'T') ────────
        // NOTE: PROC_CASE_PURPOSE_GROUP has no dedicated V_INPUT=4
        // branch, so this reuses V_INPUT=2 (update) and forces
        // P_CANCEL = 'T'. Add a real delete branch in the proc if
        // a hard/dedicated delete is ever needed.
        public async Task DeleteAsync(long id, string modifiedBy)
        {
            using (var conn = (OracleConnection)_connectionFactory.CreateConnection())
            {
                await conn.OpenAsync();

                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "PROC_CASE_PURPOSE_GROUP";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;
                    cmd.Parameters.Add("P_CASE_PURPOSE_GROUP_MASTID", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("P_CANCEL", OracleDbType.Varchar2).Value = "T";
                    cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2).Value =
                        (object?)modifiedBy ?? DBNull.Value;

                    cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        await ThrowIfErrorAsync(reader);
                    }
                }
            }
        }

        // ── Helper: map cursor row → model (used by GetAll / GetById) ──
        private static CasePurposeGroupMaster Map(OracleDataReader reader)
        {
            return new CasePurposeGroupMaster
            {
                CasePurposeGroupMastId = Convert.ToInt64(reader["CASE_PURPOSE_GROUP_MASTID"]),
                CaseCode = reader["CASE_CODE"] == DBNull.Value ? null : reader["CASE_CODE"].ToString(),
                CasePurposeGroup = reader["CASE_PURPOSE_GROUP"] == DBNull.Value ? null : reader["CASE_PURPOSE_GROUP"].ToString(),
                CasePurposeGroupEng = reader["CASE_PURPOSE_GROUP_ENG"] == DBNull.Value ? null : reader["CASE_PURPOSE_GROUP_ENG"].ToString(),
                OrderLevel = reader["ORDER_LEVEL"] == DBNull.Value ? null : Convert.ToInt32(reader["ORDER_LEVEL"]),
                RbId = reader["RB_ID"] == DBNull.Value ? null : reader["RB_ID"].ToString(),
                RbClPriority = reader["RB_CLPRIORITY"] == DBNull.Value ? null : Convert.ToInt32(reader["RB_CLPRIORITY"]),
                CreatedBy = reader["CREATEDBY"] == DBNull.Value ? null : reader["CREATEDBY"].ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                ModifiedBy = reader["MODIFIEDBY"] == DBNull.Value ? null : reader["MODIFIEDBY"].ToString(),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"]),
                Cancel = reader["CANCEL"] == DBNull.Value ? "F" : reader["CANCEL"].ToString()
            };
        }

        // ── Helper: read STATUS/MESSAGE row from Insert/Update, throw if ERROR ──
        private static async Task ThrowIfErrorAsync(OracleDataReader reader)
        {
            if (await reader.ReadAsync())
            {
                var status = reader["STATUS"]?.ToString();
                var message = reader["MESSAGE"]?.ToString() ?? "Unknown error.";

                if (status != "SUCCESS")
                {
                    throw new InvalidOperationException(message);
                }
            }
        }
    }
}