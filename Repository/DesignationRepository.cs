using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public DesignationRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL (V_INPUT = 5)
        // ───────────────────────────────────────────────
        public async Task<List<DesignationMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<DesignationMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_DESIGNATION_MASTER";
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
        public async Task<DesignationMaster?> GetByIdAsync(long id)
        {
            DesignationMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_DESIGNATION_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 3
            });

            cmd.Parameters.Add(new OracleParameter("P_CM_RCSAT_DESIGN_TMPID", OracleDbType.Int64)
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
        public async Task AddAsync(DesignationMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_DESIGNATION_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_DESG_NAME", OracleDbType.Varchar2).Value = model.DesgName;
            cmd.Parameters.Add("P_DESG_NAMEHI", OracleDbType.Varchar2).Value = model.DesgNameHi;

            cmd.Parameters.Add("P_DESGENGHI", OracleDbType.Varchar2)
               .Value = model.DesgEngHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "0";

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
        public async Task UpdateAsync(DesignationMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_DESIGNATION_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_CM_RCSAT_DESIGN_TMPID", OracleDbType.Int64).Value = model.CmRcsatDesignTmpId;

            cmd.Parameters.Add("P_DESG_NAME", OracleDbType.Varchar2).Value = model.DesgName;
            cmd.Parameters.Add("P_DESG_NAMEHI", OracleDbType.Varchar2).Value = model.DesgNameHi;

            cmd.Parameters.Add("P_DESGENGHI", OracleDbType.Varchar2)
               .Value = model.DesgEngHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "0";

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
        private static DesignationMaster MapReader(OracleDataReader reader)
        {
            return new DesignationMaster
            {
                CmRcsatDesignTmpId = Convert.ToInt64(reader["CM_RCSAT_DESIGN_TMPID"]),
                Cancel = reader["CANCEL"]?.ToString(),
                DesgName = reader["DESG_NAME"]?.ToString(),
                DesgNameHi = reader["DESG_NAMEHI"]?.ToString(),
                DesgEngHi = reader["DESGENGHI"]?.ToString(),
                DesgCode = reader["DESG_CODE"]?.ToString(),
                InActive = reader["INCATIVE"]?.ToString(),
                CreatedBy = reader["CREATEDBY"]?.ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"])
            };
        }
    }
}