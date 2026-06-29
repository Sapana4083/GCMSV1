using GCMS.Web.Repository.Interfaces;
using GCMS.WEB.Data;
using GCMS.WEB.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Web.Repository
{
    public class DivisionRepository : IDivisionRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;
        public DivisionRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }



        public async Task<List<DivisionMaster>> GetAllAsync(
     int pageNo,
     int rowCnt)
        {
            var list = new List<DivisionMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_division_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32)
               .Value = 5;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32)
               .Value = rowCnt;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32)
               .Value = pageNo;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new DivisionMaster
                {
                    DivisionMastId = Convert.ToInt64(reader["DIVISION_MASTID"]),
                    DivisionName = reader["DIVISION_NAME"]?.ToString(),
                    DivisionNameHindi = reader["DIVISION_NAME_HINDI"]?.ToString(),
                    StateName = reader["STATE_NAME"]?.ToString(),
                    InActive = reader["INACTIVE"]?.ToString(),
                    CreatedBy = reader["CREATEDBY"]?.ToString(),
                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["MODIFIEDON"])
                });
            }

            return list;
        }

        public async Task<DivisionMaster?> GetByIdAsync(long id)
        {
            DivisionMaster? division = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_division_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;

            cmd.Parameters.Add("p_division_mastid", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                division = new DivisionMaster
                {
                    DivisionMastId = Convert.ToInt64(reader["DIVISION_MASTID"]),
                    DivisionName = reader["DIVISION_NAME"]?.ToString(),
                    DivisionNameHindi = reader["DIVISION_NAME_HINDI"]?.ToString(),
                    StateName = reader["STATE_NAME"]?.ToString(),
                    InActive = reader["INACTIVE"]?.ToString()
                };
            }

            return await Task.FromResult(division);
        }

        public async Task AddAsync(DivisionMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_division_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
                model.UserName ?? "SYSTEM";

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value =
                model.CreatedBy ?? "SYSTEM";

            cmd.Parameters.Add("p_division_name", OracleDbType.Varchar2).Value =
                model.DivisionName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_division_name_hindi", OracleDbType.Varchar2).Value =
                model.DivisionNameHindi ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_state_name", OracleDbType.Varchar2).Value =
                model.StateName ?? (object)DBNull.Value;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(DivisionMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_division_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_division_mastid", OracleDbType.Int64).Value =
                model.DivisionMastId;

            cmd.Parameters.Add("p_division_name", OracleDbType.Varchar2).Value =
                model.DivisionName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_division_name_hindi", OracleDbType.Varchar2).Value =
                model.DivisionNameHindi ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_state_name", OracleDbType.Varchar2).Value =
                model.StateName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value =
                model.InActive ?? "F";
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
               model.UserName ?? "SYSTEM";

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_division_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 3;

            cmd.Parameters.Add("p_division_mastid", OracleDbType.Int64)
               .Value = id;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2)
               .Value = "T";

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }

    }
}
