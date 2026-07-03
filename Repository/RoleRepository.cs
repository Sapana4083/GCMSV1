using GCMS.Data;
using GCMS.Repository.Interfaces;
using GCMS.Models;
using Microsoft.AspNetCore.Connections;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;
        public RoleRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ===========================
        // Get All RoleMaster
        // ===========================
        public async Task<List<RoleMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            List<RoleMaster> list = new List<RoleMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_roles";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new RoleMaster
                {
                    RoleId = Convert.ToInt64(reader["ROLE_ID"]),

                    RoleName =
                        reader["ROLE_NAME"] == DBNull.Value
                        ? null
                        : reader["ROLE_NAME"].ToString(),

                    RoleType =
                        reader["ROLE_TYPE"] == DBNull.Value
                        ? null
                        : reader["ROLE_TYPE"].ToString(),

                    IsActvFlag =
                        reader["IS_ACTV_FLAG"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(reader["IS_ACTV_FLAG"]),

                    IsDelFlag =
                        reader["IS_DEL_FLAG"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(reader["IS_DEL_FLAG"]),

                    OrderNo =
                        reader["ORDER_NO"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["ORDER_NO"])
                });
            }

            return list;
        }

        // ===========================
        // Get Role By Id
        // ===========================
        public async Task<RoleMaster?> GetByIdAsync(long id)
        {
            RoleMaster? role = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_roles";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;
            cmd.Parameters.Add("p_role_id", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                role = new RoleMaster
                {
                    RoleId = Convert.ToInt64(reader["ROLE_ID"]),

                    RoleName =
                        reader["ROLE_NAME"] == DBNull.Value
                        ? null
                        : reader["ROLE_NAME"].ToString(),

                    RoleType =
                        reader["ROLE_TYPE"] == DBNull.Value
                        ? null
                        : reader["ROLE_TYPE"].ToString(),

                    IsActvFlag =
                        reader["IS_ACTV_FLAG"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(reader["IS_ACTV_FLAG"]),

                    IsDelFlag =
                        reader["IS_DEL_FLAG"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(reader["IS_DEL_FLAG"]),

                    OrderNo =
                        reader["ORDER_NO"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["ORDER_NO"]),

                    CreateBy =
                        reader["CREATE_BY"] == DBNull.Value
                        ? null
                        : reader["CREATE_BY"].ToString(),

                    ModifyBy =
                        reader["MODIFY_BY"] == DBNull.Value
                        ? null
                        : reader["MODIFY_BY"].ToString()
                };
            }

            return await Task.FromResult(role);
        }

        // ===========================
        // Add Role
        // ===========================
        public async Task AddAsync(RoleMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_roles";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_role_name", OracleDbType.Varchar2).Value =
                model.RoleName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_role_type", OracleDbType.Varchar2).Value =
                model.RoleType ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_create_by", OracleDbType.Varchar2).Value =
                model.CreateBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_order_no", OracleDbType.Int32).Value =
                model.OrderNo.HasValue ? model.OrderNo.Value : (object)DBNull.Value;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex) when (ex.Number == 20001)
            {
                throw new InvalidOperationException("This Role Name already exists.", ex);
            }

            await Task.CompletedTask;
        }

        // ===========================
        // Update Role
        // ===========================
        public async Task UpdateAsync(RoleMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_roles";
            cmd.CommandType = CommandType.StoredProcedure;

            // Derive flags — if active, IsDelFlag = 0; if inactive, IsDelFlag = 1
            int isActvFlag = model.IsActvFlag ?? 0;

            int isDelFlag = isActvFlag == 1 ? 0 : 1;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_role_id", OracleDbType.Int64).Value = model.RoleId;

            cmd.Parameters.Add("p_role_name", OracleDbType.Varchar2).Value =
                model.RoleName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_role_type", OracleDbType.Varchar2).Value =
                model.RoleType ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_is_actv_flag", OracleDbType.Int32).Value = isActvFlag;

            cmd.Parameters.Add("p_is_del_flag", OracleDbType.Int32).Value = isDelFlag;

            cmd.Parameters.Add("p_modify_by", OracleDbType.Varchar2).Value =
                model.ModifyBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_order_no", OracleDbType.Int32).Value =
                model.OrderNo.HasValue ? model.OrderNo.Value : (object)DBNull.Value;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }
    }
}