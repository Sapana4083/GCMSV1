using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;
        public StateRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        public async Task<List<StateMaster>> GetAllAsync(
    int pageNo,
    int rowCnt)
        {
            var states = new List<StateMaster>();

            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_state_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32)
               .Value = 5;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32)
               .Value = rowCnt;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32)
               .Value = pageNo;

            cmd.Parameters.Add("out_cursor",
                OracleDbType.RefCursor,
                ParameterDirection.Output);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                states.Add(new StateMaster
                {
                    StateMastId = Convert.ToInt64(reader["STATE_MASTID"]),
                    StateName = reader["STATE_NAME"]?.ToString(),
                    StateNameHindi = reader["STATE_NAME_HINDI"]?.ToString(),
                    StateCode = reader["STATE_CODE"]?.ToString(),
                    Capital = reader["CAPITAL"]?.ToString(),
                    InActive = reader["INACTIVE"]?.ToString(),
                    CreatedBy = reader["CREATEDBY"]?.ToString(),
                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["MODIFIEDON"])
                });
            }

            return states;
        }

        public async Task<StateMaster?> GetByIdAsync(long id)
        {
            StateMaster? state = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_state_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("v_input", OracleDbType.Int32)
            {
                Value = 4
            });

            cmd.Parameters.Add(new OracleParameter("p_state_mastid", OracleDbType.Int64)
            {
                Value = id
            });

            cmd.Parameters.Add(new OracleParameter("out_cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            });

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                state = new StateMaster
                {
                    StateMastId = Convert.ToInt64(reader["STATE_MASTID"]),
                    StateName = reader["STATE_NAME"]?.ToString(),
                    StateNameHindi = reader["STATE_NAME_HINDI"]?.ToString(),
                    StateCode = reader["STATE_CODE"]?.ToString(),
                    Capital = reader["CAPITAL"]?.ToString(),
                    InActive = reader["INACTIVE"]?.ToString()
                };
            }

            return await Task.FromResult(state);
        }

        public async Task AddAsync(StateMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_state_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2)
               .Value = model.CreatedBy;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2)
               .Value = model.CreatedBy;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2)
               .Value = "F";

            cmd.Parameters.Add("p_capital", OracleDbType.Varchar2)
               .Value = model.Capital;

            cmd.Parameters.Add("p_state_name", OracleDbType.Varchar2)
               .Value = model.StateName;

            cmd.Parameters.Add("p_state_name_hindi", OracleDbType.Varchar2)
              .Value = model.StateNameHindi;

            cmd.Parameters.Add("p_state_code", OracleDbType.Varchar2)
               .Value = model.StateCode;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(StateMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_state_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_state_mastid", OracleDbType.Int64)
               .Value = model.StateMastId;

            cmd.Parameters.Add("p_state_name", OracleDbType.Varchar2)
               .Value = model.StateName;
            cmd.Parameters.Add("p_state_name_hindi", OracleDbType.Varchar2)
               .Value = model.StateNameHindi;

            cmd.Parameters.Add("p_state_code", OracleDbType.Varchar2)
               .Value = model.StateCode;

            cmd.Parameters.Add("p_capital", OracleDbType.Varchar2)
               .Value = model.Capital;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2)
              .Value = model.InActive;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_state_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32)
               .Value = 3;

            cmd.Parameters.Add("p_state_mastid", OracleDbType.Int64)
               .Value = id;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2)
               .Value = "T";

            // Required because procedure contains OUT SYS_REFCURSOR
            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }
    }
}
