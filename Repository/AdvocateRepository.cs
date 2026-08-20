using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace GCMS.Repository
{
    public class AdvocateRepository : IAdvocateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public AdvocateRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL (V_INPUT = 5)
        // ───────────────────────────────────────────────
        public async Task<List<AdvocateMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<AdvocateMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapReader(reader));
            }

            if (pageNo > 0 && rowCnt > 0)
            {
                return await Task.FromResult(
                    list.Skip((pageNo - 1) * rowCnt).Take(rowCnt).ToList());
            }

            return await Task.FromResult(list);
        }

        // ───────────────────────────────────────────────
        // GET BY ID (V_INPUT = 3) + Department/Court mappings
        // (SP inhe return nahi karta, isliye seedha table se lete hain)
        // ───────────────────────────────────────────────
        public async Task<AdvocateMaster?> GetByIdAsync(long id)
        {
            AdvocateMaster? model = null;

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            // ── Base advocate data (V_INPUT = 3) ──
            using (var cmd = (OracleCommand)conn.CreateCommand())
            {
                cmd.BindByName = true;
                cmd.CommandText = "PROC_RCSAT_ADVOCATE";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32) { Value = 3 });
                cmd.Parameters.Add(new OracleParameter("P_MAST_RCSAT_ADVOCATEID", OracleDbType.Int64) { Value = id });
                cmd.Parameters.Add(new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    model = MapReader(reader);
                }
            }

            if (model == null)
            {
                return null;
            }

            // ── Department mapping (V_INPUT = 8) ──
            model.DepartmentIds = await GetMappingIdsAsync(conn, id, vInput: 8, columnName: "DEPTNAME");

            // ── Court mapping (V_INPUT = 7) ──
            model.CourtIds = await GetMappingIdsAsync(conn, id, vInput: 7, columnName: "COURT_NAME");

            return model;
        }

        private static async Task<string?> GetMappingIdsAsync(OracleConnection conn, long advocateId, int vInput, string columnName)
        {
            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32) { Value = vInput });
            cmd.Parameters.Add(new OracleParameter("P_MAST_RCSAT_ADVOCATEID", OracleDbType.Int64) { Value = advocateId });
            cmd.Parameters.Add(new OracleParameter("OUT_CURSOR", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            var ids = new StringBuilder();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (ids.Length > 0) ids.Append(',');
                ids.Append(reader[columnName]);
            }

            return await Task.FromResult(ids.Length > 0 ? ids.ToString() : null);
        }

        // ───────────────────────────────────────────────
        // ADD (V_INPUT = 1)
        // ───────────────────────────────────────────────
        public async Task AddAsync(AdvocateMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_ADVNAME", OracleDbType.Varchar2).Value = model.AdvName;
            cmd.Parameters.Add("P_ADVNAMEHI", OracleDbType.Varchar2).Value = model.AdvNameHi;

            cmd.Parameters.Add("P_ADVENGHI", OracleDbType.Varchar2)
               .Value = model.AdvEngHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ADVEMAIL", OracleDbType.Varchar2)
               .Value = model.AdvEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ADVMOBILE", OracleDbType.Int64)
               .Value = model.AdvMobile ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_BARCOUNCILNO", OracleDbType.Varchar2)
               .Value = model.BarCouncilNo ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_DEPARTMENT_IDS", OracleDbType.Varchar2)
               .Value = string.IsNullOrWhiteSpace(model.DepartmentIds) ? (object)DBNull.Value : model.DepartmentIds;

            cmd.Parameters.Add("P_COURT_IDS", OracleDbType.Varchar2)
               .Value = string.IsNullOrWhiteSpace(model.CourtIds) ? (object)DBNull.Value : model.CourtIds;

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
        public async Task UpdateAsync(AdvocateMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_MAST_RCSAT_ADVOCATEID", OracleDbType.Int64).Value = model.MastRcsatAdvocateId;

            cmd.Parameters.Add("P_ADVNAME", OracleDbType.Varchar2).Value = model.AdvName;
            cmd.Parameters.Add("P_ADVNAMEHI", OracleDbType.Varchar2).Value = model.AdvNameHi;

            cmd.Parameters.Add("P_ADVENGHI", OracleDbType.Varchar2)
               .Value = model.AdvEngHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ADVEMAIL", OracleDbType.Varchar2)
               .Value = model.AdvEmail ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_ADVMOBILE", OracleDbType.Int64)
               .Value = model.AdvMobile ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_BARCOUNCILNO", OracleDbType.Varchar2)
               .Value = model.BarCouncilNo ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_DEPARTMENT_IDS", OracleDbType.Varchar2)
               .Value = string.IsNullOrWhiteSpace(model.DepartmentIds) ? (object)DBNull.Value : model.DepartmentIds;

            cmd.Parameters.Add("P_COURT_IDS", OracleDbType.Varchar2)
               .Value = string.IsNullOrWhiteSpace(model.CourtIds) ? (object)DBNull.Value : model.CourtIds;

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
        // GET ADVOCATES BY COURT CODE (V_INPUT = 6)
        // ───────────────────────────────────────────────
        public async Task<List<AdvocateMaster>> GetAdvocatesByCourtCodeAsync(string courtCode)
        {
            var list = new List<AdvocateMaster>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 6;

            cmd.Parameters.Add("P_COURT_IDS", OracleDbType.Varchar2).Value = courtCode;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new AdvocateMaster
                {
                    MastRcsatAdvocateId = Convert.ToInt64(reader["MAST_RCSAT_ADVOCATEID"]),
                    AdvEngHi = reader["ADVENGHI"]?.ToString(),
                    AdvEmail = reader["ADVEMAIL"]?.ToString(),
                    AdvMobile = reader["ADVMOBILE"] == DBNull.Value ? null : Convert.ToInt64(reader["ADVMOBILE"])
                });
            }

            return await Task.FromResult(list);
        }

        // ───────────────────────────────────────────────
        // GET RESPONDENT ADVOCATE BY COURT CODE + DEPARTMENT (V_INPUT = 9)
        // ───────────────────────────────────────────────
        public async Task<List<AdvocateMaster>> GetRespondentAdvocatesAsync(string courtCode, string departmentName)
        {
            var list = new List<AdvocateMaster>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 9;

            cmd.Parameters.Add("P_COURT_IDS", OracleDbType.Varchar2).Value = courtCode;
            cmd.Parameters.Add("P_DEPARTMENT_IDS", OracleDbType.Varchar2).Value = departmentName;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new AdvocateMaster
                {
                    MastRcsatAdvocateId = Convert.ToInt64(reader["MAST_RCSAT_ADVOCATEID"]),
                    AdvEngHi = reader["ADVENGHI"]?.ToString(),
                    AdvEmail = reader["ADVEMAIL"]?.ToString(),
                    AdvMobile = reader["ADVMOBILE"] == DBNull.Value ? null : Convert.ToInt64(reader["ADVMOBILE"]),
                    DepEngHi = reader["DEPENGHI"]?.ToString()
                });
            }

            return await Task.FromResult(list);
        }

        // ───────────────────────────────────────────────
        // GET PRIVATE ADVOCATE (V_INPUT = 10)
        // ───────────────────────────────────────────────
        public async Task<List<AdvocateMaster>> GetPrivateAdvocatesAsync()
        {
            var list = new List<AdvocateMaster>();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_RCSAT_ADVOCATE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new AdvocateMaster
                {
                    MastRcsatAdvocateId = Convert.ToInt64(reader["MAST_RCSAT_ADVOCATEID"]),
                    AdvEngHi = reader["ADVENGHI"]?.ToString(),
                    AdvEmail = reader["ADVEMAIL"]?.ToString(),
                    AdvMobile = reader["ADVMOBILE"] == DBNull.Value ? null : Convert.ToInt64(reader["ADVMOBILE"])
                });
            }

            return await Task.FromResult(list);
        }

        // ───────────────────────────────────────────────
        // Helper
        // ───────────────────────────────────────────────
        private static AdvocateMaster MapReader(OracleDataReader reader)
        {
            return new AdvocateMaster
            {
                MastRcsatAdvocateId = Convert.ToInt64(reader["MAST_RCSAT_ADVOCATEID"]),
                AdvName = reader["ADVNAME"]?.ToString(),
                AdvNameHi = reader["ADVNAMEHI"]?.ToString(),
                AdvEngHi = reader["ADVENGHI"]?.ToString(),
                AdvEmail = reader["ADVEMAIL"]?.ToString(),
                AdvMobile = reader["ADVMOBILE"] == DBNull.Value ? null : Convert.ToInt64(reader["ADVMOBILE"]),
                BarCouncilNo = reader["BARCOUNCILNO"]?.ToString(),
                InActive = reader["INACTIVE"]?.ToString(),
                CreatedBy = reader["CREATEDBY"]?.ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"])
            };
        }
    }
}