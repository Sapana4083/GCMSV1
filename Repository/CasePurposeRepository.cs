using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CasePurposeRepository : ICasePurposeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public CasePurposeRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL (V_INPUT = 5)
        // ───────────────────────────────────────────────
        public async Task<List<CasePurposeMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<CasePurposeMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_PURPOSE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapReader(reader));
            }

            // SP list branch pagination support nahi karta — in-memory paging
            if (pageNo > 0 && rowCnt > 0)
            {
                return await Task.FromResult(
                    list.Skip((pageNo - 1) * rowCnt).Take(rowCnt).ToList());
            }

            return await Task.FromResult(list);
        }

        // ───────────────────────────────────────────────
        // GET BY ID (V_INPUT = 3)
        // ───────────────────────────────────────────────
        public async Task<CasePurposeMaster?> GetByIdAsync(long id)
        {
            CasePurposeMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_PURPOSE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 3
            });

            cmd.Parameters.Add(new OracleParameter("P_CASE_PURPOSE_MASTID", OracleDbType.Int64)
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
        public async Task AddAsync(CasePurposeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_PURPOSE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_CASE_PURPOSE_GROUP", OracleDbType.Int64)
               .Value = model.CasePurposeGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_PURPOSE_CODE", OracleDbType.Varchar2)
               .Value = model.CasePurposeCode;

            cmd.Parameters.Add("P_CASE_PURPOSE_NAME", OracleDbType.Varchar2)
               .Value = model.CasePurposeName;

            cmd.Parameters.Add("P_CASE_PURPOSE_DESCRIPTION", OracleDbType.Varchar2)
               .Value = model.CasePurposeDescription ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_PURPOSE_ENG", OracleDbType.Varchar2)
               .Value = model.CasePurposeEng;

            cmd.Parameters.Add("P_ORDER_LEVEL", OracleDbType.Int32)
               .Value = model.OrderLevel ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_COURTCODE", OracleDbType.Varchar2)
               .Value = model.CourtCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_DISP_ORDER", OracleDbType.Int32)
               .Value = model.DispOrder ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_PURPOSE_SUB_GROUP", OracleDbType.Varchar2)
               .Value = model.PurposeSubGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ISCOMPLETE", OracleDbType.Varchar2)
               .Value = model.IsComplete ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_RB_ID", OracleDbType.Int32)
               .Value = model.RbId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            // ExecuteReader use karo, NonQuery nahi — taaki SP ka ERROR status pakad sake
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
        public async Task UpdateAsync(CasePurposeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_PURPOSE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_CASE_PURPOSE_MASTID", OracleDbType.Int64)
               .Value = model.CasePurposeMastId;

            cmd.Parameters.Add("P_CASE_PURPOSE_GROUP", OracleDbType.Int64)
               .Value = model.CasePurposeGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_PURPOSE_CODE", OracleDbType.Varchar2)
               .Value = model.CasePurposeCode;

            cmd.Parameters.Add("P_CASE_PURPOSE_NAME", OracleDbType.Varchar2)
               .Value = model.CasePurposeName;

            cmd.Parameters.Add("P_CASE_PURPOSE_DESCRIPTION", OracleDbType.Varchar2)
               .Value = model.CasePurposeDescription ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CASE_PURPOSE_ENG", OracleDbType.Varchar2)
               .Value = model.CasePurposeEng;

            cmd.Parameters.Add("P_ORDER_LEVEL", OracleDbType.Int32)
               .Value = model.OrderLevel ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_COURTCODE", OracleDbType.Varchar2)
               .Value = model.CourtCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_DISP_ORDER", OracleDbType.Int32)
               .Value = model.DispOrder ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_PURPOSE_SUB_GROUP", OracleDbType.Varchar2)
               .Value = model.PurposeSubGroup ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ISCOMPLETE", OracleDbType.Varchar2)
               .Value = model.IsComplete ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_RB_ID", OracleDbType.Int32)
               .Value = model.RbId ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2)
               .Value = model.UserName ?? (object)DBNull.Value;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // Helper — reader se model map karna
        // ───────────────────────────────────────────────
        private static CasePurposeMaster MapReader(OracleDataReader reader)
        {
            return new CasePurposeMaster
            {
                CasePurposeMastId = Convert.ToInt64(reader["CASE_PURPOSE_MASTID"]),
                CasePurposeGroup = reader["CASE_PURPOSE_GROUP"] == DBNull.Value ? null : Convert.ToInt64(reader["CASE_PURPOSE_GROUP"]),
                CasePurposeCode = reader["CASE_PURPOSE_CODE"]?.ToString(),
                CasePurposeName = reader["CASE_PURPOSE_NAME"]?.ToString(),
                CasePurposeDescription = reader["CASE_PURPOSE_DESCRIPTION"]?.ToString(),
                CasePurposeEng = reader["CASE_PURPOSE_ENG"]?.ToString(),
                OrderLevel = reader["ORDER_LEVEL"] == DBNull.Value ? null : Convert.ToInt32(reader["ORDER_LEVEL"]),
                CourtCode = reader["COURTCODE"]?.ToString(),
                DispOrder = reader["DISP_ORDER"] == DBNull.Value ? null : Convert.ToInt32(reader["DISP_ORDER"]),
                PurposeSubGroup = reader["PURPOSE_SUB_GROUP"]?.ToString(),
                IsComplete = reader["ISCOMPLETE"]?.ToString(),
                RbId = reader["RB_ID"] == DBNull.Value ? null : Convert.ToInt32(reader["RB_ID"]),
                InActive = reader["INACTIVE"]?.ToString() ?? "F",
                CreatedBy = reader["CREATEDBY"]?.ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                UserName = reader["USERNAME"]?.ToString(),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"])
            };
        }

        public async Task<List<CasePurposeMaster>> GetDropDownAsync(int pageNo, int rowCnt)
        {
            var list = new List<CasePurposeMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_CASE_PURPOSE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 6;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new CasePurposeMaster
                {
                    CasePurposeMastId = Convert.ToInt64(reader["CasePurposeMastId"]),
                    CasePurposeEng = reader["CASE_PURPOSE_ENG"]?.ToString()
                });
                //list.Add(MapReader(reader));
            }

            // SP list branch pagination support nahi karta — in-memory paging
            if (pageNo > 0 && rowCnt > 0)
            {
                return await Task.FromResult(
                    list.Skip((pageNo - 1) * rowCnt).Take(rowCnt).ToList());
            }

            return await Task.FromResult(list);
        }
    }
}