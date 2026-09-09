using GCMS.Data;
using GCMS.Models;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
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
    }

}
