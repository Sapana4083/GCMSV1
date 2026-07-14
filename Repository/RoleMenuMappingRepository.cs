using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection.PortableExecutable;

namespace GCMS.Repository
{
    public class RoleMenuMappingRepository : IRoleMenuMappingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public RoleMenuMappingRepository(
            ApplicationDbContext context,
            OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        public async Task<List<RoleMenuMapping>> GetAllAsync(int pageNo, int rowCnt)
        {
            List<RoleMenuMapping> list = new();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_role_menu_mapping";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new RoleMenuMapping
                {
                    RoleMenuId = Convert.ToInt64(reader["ROLE_MENU_ID"]),
                    RoleId = Convert.ToInt64(reader["ROLE_ID"]),
                    MenuId = Convert.ToInt64(reader["MENU_ID"]),
                    ParentMenuId = reader["PARENT_MENU_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["PARENT_MENU_ID"]),
                    IsActvFlag = Convert.ToInt32(reader["IS_ACTV_FLAG"]),
                    IsDelFlag = Convert.ToInt32(reader["IS_DEL_FLAG"])
                });
            }

            return list;
        }

        public async Task AddAsync(RoleMenuMapping model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_role_menu_mapping";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_role_id", OracleDbType.Int64).Value = model.RoleId;

            cmd.Parameters.Add("p_menu_id", OracleDbType.Int64).Value = model.MenuId;

            cmd.Parameters.Add("p_is_actv_flag", OracleDbType.Int32).Value = model.IsActvFlag;

            cmd.Parameters.Add("p_is_del_flag", OracleDbType.Int32).Value = model.IsDelFlag;

            cmd.Parameters.Add("p_create_by", OracleDbType.Varchar2).Value =
                string.IsNullOrWhiteSpace(model.CreateBy)
                    ? (object)DBNull.Value
                    : model.CreateBy;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex) when (ex.Number == 20001)
            {
                throw new InvalidOperationException(
                    "This menu is already mapped to the selected role.", ex);
            }
            catch (OracleException ex) when (ex.Number == 1403)
            {
                throw new InvalidOperationException(
                    "Selected menu does not exist.", ex);
            }
        }

        public async Task UpdateAsync(RoleMenuMapping model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_role_menu_mapping";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_role_id", OracleDbType.Int64).Value = model.RoleId;

            cmd.Parameters.Add("p_menu_id", OracleDbType.Int64).Value = model.MenuId;

            cmd.Parameters.Add("p_is_actv_flag", OracleDbType.Int32).Value = model.IsActvFlag;

            cmd.Parameters.Add("p_is_del_flag", OracleDbType.Int32).Value = model.IsDelFlag;

            cmd.Parameters.Add("p_modify_by", OracleDbType.Varchar2).Value =
                string.IsNullOrWhiteSpace(model.ModifyBy)
                    ? (object)DBNull.Value
                    : model.ModifyBy;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RoleMenuMapping?> GetByIdAsync(long id)
        {
            RoleMenuMapping? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_role_menu_mapping";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;
            cmd.Parameters.Add("p_role_menu_id", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                model = new RoleMenuMapping
                {
                    RoleMenuId = Convert.ToInt64(reader["ROLE_MENU_ID"]),
                    RoleId = Convert.ToInt64(reader["ROLE_ID"]),
                    MenuId = Convert.ToInt64(reader["MENU_ID"]),
                    ParentMenuId = reader["PARENT_MENU_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["PARENT_MENU_ID"]),
                    IsActvFlag = Convert.ToInt32(reader["IS_ACTV_FLAG"]),
                    IsDelFlag = Convert.ToInt32(reader["IS_DEL_FLAG"])
                };
            }

            return model;
        }

        public async Task<List<RoleMenuMapping>> GetByRoleIdAsync(long roleId)
        {
            List<RoleMenuMapping> list = new();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_role_menu_mapping";
            cmd.CommandType = CommandType.StoredProcedure;

            // Input Parameters
            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;
            cmd.Parameters.Add("p_role_id", OracleDbType.Int64).Value = roleId;
            cmd.Parameters.Add("p_is_actv_flag", OracleDbType.Int32).Value = 1;
            cmd.Parameters.Add("p_is_del_flag", OracleDbType.Int32).Value = 0;

            // Output Cursor
            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                          .Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new RoleMenuMapping
                {
                    RoleMenuId = reader["ROLE_MENU_ID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["ROLE_MENU_ID"]),
                    RoleId = reader["ROLE_ID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["ROLE_ID"]),
                    MenuId = reader["MENU_ID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["MENU_ID"]),
                    ParentMenuId = reader["PARENT_MENU_ID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["PARENT_MENU_ID"]),
                    IsActvFlag = reader["IS_ACTV_FLAG"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IS_ACTV_FLAG"]),
                    IsDelFlag = reader["IS_DEL_FLAG"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IS_DEL_FLAG"])
                });
            }

            return list;
        }

        public async Task<RoleMenuMapping?> GetByRoleAndMenuAsync(long roleId, long menuId)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_role_menu_mapping";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;

            cmd.Parameters.Add("p_role_id", OracleDbType.Int64).Value = roleId;
            cmd.Parameters.Add("p_menu_id", OracleDbType.Int64).Value = menuId;

            // Required for pagination in procedure
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 1;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                          .Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new RoleMenuMapping
                {
                    RoleMenuId = reader["ROLE_MENU_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["ROLE_MENU_ID"]),

                    RoleId = reader["ROLE_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["ROLE_ID"]),

                    MenuId = reader["MENU_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["MENU_ID"]),

                    ParentMenuId = reader["PARENT_MENU_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["PARENT_MENU_ID"]),

                    IsActvFlag = reader["IS_ACTV_FLAG"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(reader["IS_ACTV_FLAG"]),

                    IsDelFlag = reader["IS_DEL_FLAG"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(reader["IS_DEL_FLAG"]),

                    CreateBy = reader["CREATE_BY"] == DBNull.Value
                        ? null
                        : reader["CREATE_BY"].ToString(),

                    ModifyBy = reader["MODIFY_BY"] == DBNull.Value
                        ? null
                        : reader["MODIFY_BY"].ToString()
                };
            }

            return null;
        }
    }
}