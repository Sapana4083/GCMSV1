using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Microsoft.AspNetCore.Connections;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public MenuRepository(
            ApplicationDbContext context,
            OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        public async Task<List<MenuMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            List<MenuMaster> list = new();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_menu";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new MenuMaster
                {
                    MenuId = Convert.ToInt64(reader["MENU_ID"]),
                    MenuName = reader["MENU_NAME"]?.ToString(),
                    ParentId = reader["PARENT_ID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt64(reader["PARENT_ID"]),
                    MenuUrl = reader["MENU_URL"]?.ToString(),
                    IsActvFlag = Convert.ToInt32(reader["IS_ACTV_FLAG"]),
                    IsDelFlag = Convert.ToInt32(reader["IS_DEL_FLAG"]),
                    CreateBy = reader["CREATE_BY"]?.ToString(),
                    ModifyBy = reader["MODIFY_BY"]?.ToString()
                });
            }

            return list;
        }

        public async Task<MenuMaster?> GetByIdAsync(long id)
        {
            MenuMaster? menu = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_menu";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;
            cmd.Parameters.Add("p_menu_id", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                menu = new MenuMaster
                {
                    MenuId = Convert.ToInt64(reader["MENU_ID"]),
                    MenuName = reader["MENU_NAME"]?.ToString(),
                    ParentId = reader["PARENT_ID"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["PARENT_ID"]),
                    MenuUrl = reader["MENU_URL"]?.ToString(),
                    IsActvFlag = Convert.ToInt32(reader["IS_ACTV_FLAG"]),
                    IsDelFlag = Convert.ToInt32(reader["IS_DEL_FLAG"]),
                    CreateBy = reader["CREATE_BY"]?.ToString(),
                    ModifyBy = reader["MODIFY_BY"]?.ToString()
                };
            }

            return menu;
        }

        public async Task AddAsync(MenuMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_menu";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_menu_name", OracleDbType.Varchar2)
                .Value = model.MenuName;

            cmd.Parameters.Add("p_parent_id", OracleDbType.Int64)
                .Value = model.ParentId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_menu_url", OracleDbType.Clob)
                .Value = model.MenuUrl ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_create_by", OracleDbType.Varchar2)
                .Value = model.CreateBy;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(MenuMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_gcms_menu";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_menu_id", OracleDbType.Int64)
                .Value = model.MenuId;

            cmd.Parameters.Add("p_menu_name", OracleDbType.Varchar2)
                .Value = model.MenuName;

            cmd.Parameters.Add("p_parent_id", OracleDbType.Int64)
                .Value = model.ParentId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_menu_url", OracleDbType.Clob)
                .Value = model.MenuUrl ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_is_actv_flag", OracleDbType.Int32)
                .Value = model.IsActvFlag;

            cmd.Parameters.Add("p_is_del_flag", OracleDbType.Int32)
                .Value = model.IsDelFlag;

            cmd.Parameters.Add("p_modify_by", OracleDbType.Varchar2)
                .Value = model.ModifyBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }
    }
}