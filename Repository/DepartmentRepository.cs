using GCMS.Data;
using GCMS.Models;
using GCMS.Models.Entities;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public DepartmentRepository(
            ApplicationDbContext context,
            OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        public async Task<List<DepartmentMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var departments = new List<DepartmentMaster>();

            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_department_master";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCnt;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("out_cursor",
                OracleDbType.RefCursor,
                ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                departments.Add(new DepartmentMaster
                {
                    DepartmentId = Convert.ToInt64(reader["DEPARTMENT_MASTID"]),
                    DepartmentName = reader["DEPTNAMEEN"]?.ToString(),
                    DepartmentNameHindi = reader["DEPTNAMEHI"]?.ToString(),
                    CourtCode = reader["COURTCODE"]?.ToString(),
                    Title = reader["TITLE"]?.ToString(),
                    Description = reader["DESCRIPTION"]?.ToString(),
                    IsActive = reader["ISACTIVE"]?.ToString(),
                    CreatedBy = reader["CREATEDBY"]?.ToString(),

                    CreatedOn = reader["CREATEDON"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["CREATEDON"]),

                    ModifiedBy = reader["MODIFIEDBY"]?.ToString(),

                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["MODIFIEDON"])
                });
            }

            return departments;
        }
        

        public async Task<DepartmentMaster?> GetByIdAsync(long id)
        {
            DepartmentMaster? department = null;

            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_department_master";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;

            cmd.Parameters.Add("p_department_mastid", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                department = new DepartmentMaster
                {
                    DepartmentId = Convert.ToInt64(reader["DEPARTMENT_MASTID"]),
                    DepartmentName = reader["DEPTNAMEEN"]?.ToString(),
                    DepartmentNameHindi = reader["DEPTNAMEHI"]?.ToString(),
                    CourtCode = reader["COURTCODE"]?.ToString(),
                    Title = reader["TITLE"]?.ToString(),
                    Description = reader["DESCRIPTION"]?.ToString(),
                    IsActive = reader["ISACTIVE"]?.ToString()
                };
            }

            return await Task.FromResult(department);
        }

        public async Task<int> SaveAsync(DepartmentMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_department_master";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = model.CreatedBy;

            cmd.Parameters.Add("p_deptnameen", OracleDbType.Varchar2).Value = model.DepartmentName;

            cmd.Parameters.Add("p_deptnamehi", OracleDbType.Varchar2).Value = model.DepartmentNameHindi;

            cmd.Parameters.Add("p_title", OracleDbType.Varchar2).Value = model.Title;

            cmd.Parameters.Add("p_description", OracleDbType.Varchar2).Value = model.Description;

            cmd.Parameters.Add("p_courtcode", OracleDbType.Varchar2).Value = model.CourtCode;

            cmd.Parameters.Add("p_isactive", OracleDbType.Varchar2).Value = model.IsActive;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            var result = cmd.ExecuteNonQuery();

            return await Task.FromResult(result);
        }

        public async Task<int> UpdateAsync(DepartmentMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_department_master";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_department_mastid", OracleDbType.Int64).Value = model.DepartmentId;

            cmd.Parameters.Add("p_deptnameen", OracleDbType.Varchar2).Value = model.DepartmentName;

            cmd.Parameters.Add("p_deptnamehi", OracleDbType.Varchar2).Value = model.DepartmentNameHindi;

            cmd.Parameters.Add("p_title", OracleDbType.Varchar2).Value = model.Title;

            cmd.Parameters.Add("p_description", OracleDbType.Varchar2).Value = model.Description;

            cmd.Parameters.Add("p_courtcode", OracleDbType.Varchar2).Value = model.CourtCode;

            cmd.Parameters.Add("p_isactive", OracleDbType.Varchar2).Value = model.IsActive;

            cmd.Parameters.Add("p_modifiedby", OracleDbType.Varchar2).Value = model.ModifiedBy;
            
            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            var result = cmd.ExecuteNonQuery();

            return await Task.FromResult(result);
        }

        public async Task<int> DeleteAsync(long id, string user)
        {
            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_department_master";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 3;

            cmd.Parameters.Add("p_department_mastid", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = user;
            cmd.Parameters.Add("p_modifiedby", OracleDbType.Varchar2).Value = user ?? (object)DBNull.Value;
            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            var result = cmd.ExecuteNonQuery();

            return await Task.FromResult(result);
        }

       
    }
}