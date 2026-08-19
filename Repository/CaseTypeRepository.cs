using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CaseTypeRepository : ICaseTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public CaseTypeRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL (V_INPUT = 5)
        // ───────────────────────────────────────────────
        public async Task<List<CaseTypeMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<CaseTypeMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapReader(reader));
            }

            return list;
        }

        // ───────────────────────────────────────────────
        // GET BY ID (V_INPUT = 3)
        // ───────────────────────────────────────────────
        public async Task<CaseTypeMaster?> GetByIdAsync(long id)
        {
            CaseTypeMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 3
            });

            cmd.Parameters.Add(new OracleParameter("P_CASE_TYPE_MASTID", OracleDbType.Int64)
            {
                Value = id
            });

            cmd.Parameters.Add(new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            });

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                model = MapReader(reader);
            }

            return await Task.FromResult(model);
        }

        // ───────────────────────────────────────────────
        // ADD (V_INPUT = 1)
        // ───────────────────────────────────────────────
        public async Task AddAsync(CaseTypeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_CASE_TYPE", OracleDbType.Varchar2).Value = model.CaseType;
            cmd.Parameters.Add("P_CASE_TYPE_ENG", OracleDbType.Varchar2).Value = model.CaseTypeEng;
            cmd.Parameters.Add("P_CASE_CODE", OracleDbType.Varchar2).Value = model.CaseCode;

            cmd.Parameters.Add("P_ORDER_LEVEL", OracleDbType.Int64)
               .Value = model.OrderLevel ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_TYPE_CATID", OracleDbType.Int64)
               .Value = model.CaseTypeCatId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_TAX_ORDER_LEVEL", OracleDbType.Int32)
               .Value = model.TaxOrderLevel ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_SHORT_NAME", OracleDbType.Varchar2)
               .Value = model.ShortName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_DISP_ORDER", OracleDbType.Int64)
               .Value = model.DispOrder ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CTYPE_ABBR", OracleDbType.Varchar2)
               .Value = model.CtypeAbbr ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_GROUP_CODE", OracleDbType.Varchar2)
               .Value = model.CaseGroupCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_TYPE_GROUP", OracleDbType.Varchar2)
               .Value = model.CaseTypeGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_GROUP", OracleDbType.Varchar2)
               .Value = model.CaseGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_RB_ID", OracleDbType.Int64)
               .Value = model.RbId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var status = reader["STATUS"]?.ToString();
                var message = reader["MESSAGE"]?.ToString();

                if (status == "ERROR")
                {
                    throw new Exception(message);
                }
            }

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // UPDATE (V_INPUT = 2)
        // ───────────────────────────────────────────────
        public async Task UpdateAsync(CaseTypeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_CASE_TYPE_MASTID", OracleDbType.Int64).Value = model.CaseTypeMastId;

            cmd.Parameters.Add("P_CASE_TYPE", OracleDbType.Varchar2).Value = model.CaseType;
            cmd.Parameters.Add("P_CASE_TYPE_ENG", OracleDbType.Varchar2).Value = model.CaseTypeEng;
            cmd.Parameters.Add("P_CASE_CODE", OracleDbType.Varchar2).Value = model.CaseCode;

            cmd.Parameters.Add("P_ORDER_LEVEL", OracleDbType.Int64)
               .Value = model.OrderLevel ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_TYPE_CATID", OracleDbType.Int64)
               .Value = model.CaseTypeCatId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_TAX_ORDER_LEVEL", OracleDbType.Int32)
               .Value = model.TaxOrderLevel ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_SHORT_NAME", OracleDbType.Varchar2)
               .Value = model.ShortName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_DISP_ORDER", OracleDbType.Int64)
               .Value = model.DispOrder ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CTYPE_ABBR", OracleDbType.Varchar2)
               .Value = model.CtypeAbbr ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_GROUP_CODE", OracleDbType.Varchar2)
               .Value = model.CaseGroupCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_TYPE_GROUP", OracleDbType.Varchar2)
               .Value = model.CaseTypeGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_GROUP", OracleDbType.Varchar2)
               .Value = model.CaseGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_RB_ID", OracleDbType.Int64)
               .Value = model.RbId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("P_CANCEL", OracleDbType.Varchar2)
               .Value = model.Cancel ?? "F";

            cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var status = reader["STATUS"]?.ToString();
                var message = reader["MESSAGE"]?.ToString();

                if (status == "ERROR")
                {
                    throw new Exception(message);
                }
            }

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // Helper
        // ───────────────────────────────────────────────
        private static CaseTypeMaster MapReader(OracleDataReader reader)
        {
            return new CaseTypeMaster
            {
                CaseTypeMastId = Convert.ToInt64(reader["CASE_TYPE_MASTID"]),
                CaseCode = reader["CASE_CODE"]?.ToString(),
                CaseType = reader["CASE_TYPE"]?.ToString(),
                CaseTypeEng = reader["CASE_TYPE_ENG"]?.ToString(),
                OrderLevel = reader["ORDER_LEVEL"] == DBNull.Value ? null : Convert.ToInt64(reader["ORDER_LEVEL"]),
                CtypeAbbr = reader["CTYPE_ABBR"]?.ToString(),
                CaseGroupCode = reader["CASE_GROUP_CODE"]?.ToString(),
                CaseTypeGroup = reader["CASE_TYPE_GROUP"]?.ToString(),
                CaseGroup = reader["CASE_GROUP"]?.ToString(),
                RbId = reader["RB_ID"] == DBNull.Value ? null : Convert.ToInt64(reader["RB_ID"]),
                CaseTypeCatId = reader["CASE_TYPE_CATID"] == DBNull.Value ? null : Convert.ToInt64(reader["CASE_TYPE_CATID"]),
                TaxOrderLevel = reader["TAX_ORDER_LEVEL"] == DBNull.Value ? null : Convert.ToInt32(reader["TAX_ORDER_LEVEL"]),
                ShortName = reader["SHORT_NAME"]?.ToString(),
                DispOrder = reader["DISP_ORDER"] == DBNull.Value ? null : Convert.ToInt64(reader["DISP_ORDER"]),
                Cancel = reader["CANCEL"]?.ToString(),
                InActive = reader["INACTIVE"]?.ToString(),
                CreatedBy = reader["CREATEDBY"]?.ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"])
            };
        }

        public async Task<List<CaseTypeMaster>> GetCaseTypeAsync(int pageNo, int rowCnt)
        {
            List<CaseTypeMaster> list = new();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 6;
            cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new CaseTypeMaster
                {
                    CaseTypeMastId = Convert.ToInt64(reader["case_type_mastid"]),
                    CaseType = reader["case_type"]?.ToString()
                    //CourtTypeName = reader["CourtType"]?.ToString(),
                    //CourtGroupCode = reader["CourtGroupId"]?.ToString(),
                    //InActive = reader["inactive"]?.ToString()
                });
            }

            return list;
        }
    }
}