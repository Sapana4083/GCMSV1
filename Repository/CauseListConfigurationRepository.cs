using GCMS.Data;
using GCMS.Models;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CauseListConfigurationRepository : ICauseListConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public CauseListConfigurationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<CasePurposeMaster>> GetCasePurposesAsync()
        {
            return await _context.CasePurposeMasters
                .Where(x => x.InActive == "Y")
                .OrderBy(x => x.CasePurposeName)
                .ToListAsync();
        }

        // ───────────────────────────────────────────────
        // LIST — direct query (koi List SP diya nahi gaya)--Naveen Sharma
        // ───────────────────────────────────────────────
        public async Task<List<CauseListParamsListItem>> GetCauseListAsync()
        {
            var list = new List<CauseListParamsListItem>();

            var conn = (OracleConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = new OracleCommand(@"
        SELECT
            TRN_RCSAT_CLP_PARAMSID,
            DOCNO,
            REMARKS,
            CAUSE_LIST_TYPE,
            CC_LIMIT,
            ISACTIVE,
            DOCDT
        FROM TRN_RCSAT_CLP_PARAMS
        ORDER BY TRN_RCSAT_CLP_PARAMSID DESC", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new CauseListParamsListItem
                {
                    TrnRcsatClpParamsId = Convert.ToInt64(reader["TRN_RCSAT_CLP_PARAMSID"]),
                    DocNo = reader["DOCNO"]?.ToString(),
                    Remarks = reader["REMARKS"]?.ToString(),
                    CauseListType = reader["CAUSE_LIST_TYPE"]?.ToString(),
                    CaseCountLimit = reader["CC_LIMIT"] == DBNull.Value ? null : Convert.ToInt32(reader["CC_LIMIT"]),
                    IsActive = reader["ISACTIVE"]?.ToString(),
                    DocDt = reader["DOCDT"] == DBNull.Value ? null : Convert.ToDateTime(reader["DOCDT"])
                });
            }

            return list;
        }

        // ───────────────────────────────────────────────
        // GET BY ID — header + detail rows dono, Edit form ke liye
        // ───────────────────────────────────────────────
        public async Task<CauseListConfigurationViewModel?> GetCauseListByIdAsync(long id)
        {
            var conn = (OracleConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            CauseListConfigurationViewModel? model = null;

            using (var cmd = new OracleCommand(@"
        SELECT TRN_RCSAT_CLP_PARAMSID, DOCNO, REMARKS, CAUSE_LIST_TYPE, CC_LIMIT, ISACTIVE
        FROM TRN_RCSAT_CLP_PARAMS
        WHERE TRN_RCSAT_CLP_PARAMSID = :id", conn))
            {
                cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int64) { Value = id });

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    model = new CauseListConfigurationViewModel
                    {
                        CLPNo = Convert.ToInt64(reader["TRN_RCSAT_CLP_PARAMSID"]),
                        Remarks = reader["REMARKS"]?.ToString(),
                        CauseListType = reader["CAUSE_LIST_TYPE"]?.ToString(),
                        CaseCountLimit = reader["CC_LIMIT"] == DBNull.Value ? null : Convert.ToInt32(reader["CC_LIMIT"]),
                        Active = reader["ISACTIVE"]?.ToString() == "T"
                    };
                }
            }

            if (model == null) return null;

            using (var cmd = new OracleCommand(@"
        SELECT CASEPURPOSE, PURPOSEPRIORITY, DBONE, DBTWO
        FROM TRN_RCSAT_CLP_PARAMSDTL
        WHERE TRN_RCSAT_CLP_PARAMSID = :id
        ORDER BY PURPOSEPRIORITY", conn))
            {
                cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int64) { Value = id });

                using var reader = await cmd.ExecuteReaderAsync();

                var rows = new List<CauseListCasePurposeViewModel>();
                int i = 1;

                while (await reader.ReadAsync())
                {
                    rows.Add(new CauseListCasePurposeViewModel
                    {
                        Id = i++,
                        CasePurposeId = reader["CASEPURPOSE"] == DBNull.Value ? null : Convert.ToInt64(reader["CASEPURPOSE"]),
                        PurposePriority = reader["PURPOSEPRIORITY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PURPOSEPRIORITY"]),
                        DBBenchOne = reader["DBONE"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DBONE"]),
                        DBBenchTwo = reader["DBTWO"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DBTWO"])
                    });
                }

                model.CasePurposes = rows;
            }

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<long> SaveCauseListAsync(CauseListConfigurationViewModel model, string createdBy)
        {
            var conn = (OracleConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            bool isUpdate = model.CLPNo.HasValue && model.CLPNo.Value > 0;
            long headerId;

            if (isUpdate)
            {
                headerId = model.CLPNo!.Value;

                using (var cmd = new OracleCommand(@"
            UPDATE TRN_RCSAT_CLP_PARAMS
            SET REMARKS = :remarks,
                CAUSE_LIST_TYPE = :causeListType,
                CC_LIMIT = :ccLimit,
                ISACTIVE = :isActive
            WHERE TRN_RCSAT_CLP_PARAMSID = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("remarks", OracleDbType.Varchar2) { Value = (object?)model.Remarks ?? DBNull.Value });
                    cmd.Parameters.Add(new OracleParameter("causeListType", OracleDbType.Varchar2) { Value = (object?)model.CauseListType ?? DBNull.Value });
                    cmd.Parameters.Add(new OracleParameter("ccLimit", OracleDbType.Int32) { Value = (object?)model.CaseCountLimit ?? DBNull.Value });
                    cmd.Parameters.Add(new OracleParameter("isActive", OracleDbType.Varchar2) { Value = model.Active ? "T" : "F" });
                    cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int64) { Value = headerId });

                    await cmd.ExecuteNonQueryAsync();
                }

                // Purane detail rows delete karke naye insert karenge (replace pattern)
                using (var cmd = new OracleCommand(
                    "DELETE FROM TRN_RCSAT_CLP_PARAMSDTL WHERE TRN_RCSAT_CLP_PARAMSID = :id", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int64) { Value = headerId });
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            else
            {
                using (var cmd = new OracleCommand(
                    "SELECT NVL(MAX(TRN_RCSAT_CLP_PARAMSID), 0) + 1 FROM TRN_RCSAT_CLP_PARAMS", conn))
                {
                    headerId = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                }

                using (var cmd = new OracleCommand(@"
            INSERT INTO TRN_RCSAT_CLP_PARAMS
            (TRN_RCSAT_CLP_PARAMSID, DOCNO, REMARKS, CAUSE_LIST_TYPE, CC_LIMIT, ISACTIVE, DOCDT, CREATEDBY, CREATEDON)
            VALUES
            (:id, :docNo, :remarks, :causeListType, :ccLimit, :isActive, SYSDATE, :createdBy, SYSDATE)", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int64) { Value = headerId });
                    cmd.Parameters.Add(new OracleParameter("docNo", OracleDbType.Varchar2) { Value = headerId.ToString() });
                    cmd.Parameters.Add(new OracleParameter("remarks", OracleDbType.Varchar2) { Value = (object?)model.Remarks ?? DBNull.Value });
                    cmd.Parameters.Add(new OracleParameter("causeListType", OracleDbType.Varchar2) { Value = (object?)model.CauseListType ?? DBNull.Value });
                    cmd.Parameters.Add(new OracleParameter("ccLimit", OracleDbType.Int32) { Value = (object?)model.CaseCountLimit ?? DBNull.Value });
                    cmd.Parameters.Add(new OracleParameter("isActive", OracleDbType.Varchar2) { Value = model.Active ? "T" : "F" });
                    cmd.Parameters.Add(new OracleParameter("createdBy", OracleDbType.Varchar2) { Value = createdBy });

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            // Detail rows insert (dono Insert aur Update case me)
            if (model.CasePurposes != null && model.CasePurposes.Count > 0)
            {
                long nextDtlId;
                using (var cmd = new OracleCommand(
                    "SELECT NVL(MAX(TRN_RCSAT_CLP_PARAMSDTLID), 0) FROM TRN_RCSAT_CLP_PARAMSDTL", conn))
                {
                    nextDtlId = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                }

                foreach (var row in model.CasePurposes)
                {
                    if (row.CasePurposeId == null) continue;

                    nextDtlId++;

                    using var cmd = new OracleCommand(@"
                INSERT INTO TRN_RCSAT_CLP_PARAMSDTL
                (TRN_RCSAT_CLP_PARAMSDTLID, TRN_RCSAT_CLP_PARAMSID, CASEPURPOSE, PURPOSEPRIORITY, DBONE, DBTWO)
                VALUES
                (:dtlId, :headerId, :casePurpose, :priority, :dbOne, :dbTwo)", conn);

                    cmd.Parameters.Add(new OracleParameter("dtlId", OracleDbType.Int64) { Value = nextDtlId });
                    cmd.Parameters.Add(new OracleParameter("headerId", OracleDbType.Int64) { Value = headerId });
                    cmd.Parameters.Add(new OracleParameter("casePurpose", OracleDbType.Int64) { Value = row.CasePurposeId.Value });
                    cmd.Parameters.Add(new OracleParameter("priority", OracleDbType.Int32) { Value = row.PurposePriority });
                    cmd.Parameters.Add(new OracleParameter("dbOne", OracleDbType.Int32) { Value = row.DBBenchOne });
                    cmd.Parameters.Add(new OracleParameter("dbTwo", OracleDbType.Int32) { Value = row.DBBenchTwo });

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return headerId;
        }
    }

}
