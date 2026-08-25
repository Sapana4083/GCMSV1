using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class RcsatDepartmentRepository : IRcsatDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public RcsatDepartmentRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL (V_INPUT = 5) — paged list
        // ───────────────────────────────────────────────
        public async Task<List<RcsatDepartmentMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<RcsatDepartmentMaster>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_DEPT_MASTER";
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
        // GET BY ID (V_INPUT = 4)
        // ───────────────────────────────────────────────
        public async Task<RcsatDepartmentMaster?> GetByIdAsync(long id)
        {
            RcsatDepartmentMaster? model = null;

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_DEPT_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 4
            });

            cmd.Parameters.Add(new OracleParameter("P_DEPARTMENT_MASTID", OracleDbType.Int64)
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
        public async Task AddAsync(RcsatDepartmentMaster model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_DEPT_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_DEPTNAMEEN", OracleDbType.Varchar2).Value = model.DeptName;

            cmd.Parameters.Add("P_DEPTNAMEHI", OracleDbType.Varchar2)
               .Value = model.DeptNameHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ISACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // UPDATE (V_INPUT = 2)
        // ───────────────────────────────────────────────
        public async Task UpdateAsync(RcsatDepartmentMaster model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_DEPT_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_DEPARTMENT_MASTID", OracleDbType.Int64).Value = model.CmRcsatDeptId;

            cmd.Parameters.Add("P_DEPTNAMEEN", OracleDbType.Varchar2).Value = model.DeptName;

            cmd.Parameters.Add("P_DEPTNAMEHI", OracleDbType.Varchar2)
               .Value = model.DeptNameHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ISACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        // ───────────────────────────────────────────────
        // LIST DEPARTMENT NAME (V_INPUT = 6) — dropdown ke liye
        // ───────────────────────────────────────────────
        public async Task<List<RcsatDepartmentMaster>> GetDepartmentNameListAsync()
        {
            var list = new List<RcsatDepartmentMaster>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_DEPT_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 6;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapReader(reader));
            }

            return await Task.FromResult(list);
        }

        // ───────────────────────────────────────────────
        // GET DEPARTMENT DETAIL (V_INPUT = 7)
        // ───────────────────────────────────────────────
        public async Task<RcsatDepartmentMaster?> GetDepartmentDetailAsync(long id)
        {
            RcsatDepartmentMaster? model = null;

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_DEPT_MASTER";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 7
            });

            cmd.Parameters.Add(new OracleParameter("P_DEPARTMENT_MASTID", OracleDbType.Int64)
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
        // Helper
        // ───────────────────────────────────────────────
        private static RcsatDepartmentMaster MapReader(OracleDataReader reader)
        {
            return new RcsatDepartmentMaster
            {
                CmRcsatDeptId = Convert.ToInt64(reader["CM_RCSAT_DEPTID"]),
                DeptName = reader["DEPT_NAME"]?.ToString(),
                DeptNameHi = reader["DEPT_NAMEHI"]?.ToString(),
                DepEngHi = reader["DEPENGHI"]?.ToString(),
                InActive = reader["INACTIVE"]?.ToString()
            };
        }
    }
}