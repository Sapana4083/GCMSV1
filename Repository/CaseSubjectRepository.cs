using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CaseSubjectRepository : ICaseSubjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public CaseSubjectRepository(ApplicationDbContext context, OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        // ───────────────────────────────────────────────
        // GET ALL — SP me list ka branch nahi hai, isliye
        // EF Core se seedha table read kar rahe hain.
        // Agar SP me V_INPUT=5 add ho jaye to isko bhi
        // SP-based bana denge.
        // ───────────────────────────────────────────────
        public async Task<List<CaseSubjectMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var list = new List<CaseSubjectMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_MAST_RCSAT_CSSUBJECT";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

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
        // GET BY ID (V_INPUT = 4)
        // ───────────────────────────────────────────────
        public async Task<CaseSubjectMaster?> GetByIdAsync(long id)
        {
            CaseSubjectMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_MAST_RCSAT_CSSUBJECT";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new OracleParameter("V_INPUT", OracleDbType.Int32)
            {
                Value = 4
            });

            cmd.Parameters.Add(new OracleParameter("P_MAST_RCSAT_CSSUBJECTID", OracleDbType.Int64)
            {
                Value = id
            });

            cmd.Parameters.Add(new OracleParameter("P_CURSOR", OracleDbType.RefCursor)
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
        public async Task AddAsync(CaseSubjectMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_MAST_RCSAT_CSSUBJECT";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_SUBJECT", OracleDbType.Varchar2).Value = model.Subject;
            cmd.Parameters.Add("P_SUBJECTHI", OracleDbType.Varchar2).Value = model.SubjectHi;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "F";

            cmd.Parameters.Add("P_SUBJECTENGHI", OracleDbType.Varchar2)
               .Value = model.SubjectEngHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2)
               .Value = model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CANCEL", OracleDbType.Char)
               .Value = model.Cancel ?? "F";

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
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
        public async Task UpdateAsync(CaseSubjectMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_MAST_RCSAT_CSSUBJECT";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_MAST_RCSAT_CSSUBJECTID", OracleDbType.Int64).Value = model.CaseSubjectId;

            cmd.Parameters.Add("P_SUBJECT", OracleDbType.Varchar2).Value = model.Subject;
            cmd.Parameters.Add("P_SUBJECTHI", OracleDbType.Varchar2).Value = model.SubjectHi;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2)
               .Value = model.InActive ?? "N";

            cmd.Parameters.Add("P_SUBJECTENGHI", OracleDbType.Varchar2)
               .Value = model.SubjectEngHi ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CANCEL", OracleDbType.Char)
               .Value = model.Cancel ?? "N";

            cmd.Parameters.Add("P_CANCELREMARKS", OracleDbType.Varchar2)
               .Value = model.CancelRemarks ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2)
               .Value = model.UserName ?? (object)DBNull.Value;

            cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor)
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
        private static CaseSubjectMaster MapReader(OracleDataReader reader)
        {
            return new CaseSubjectMaster
            {
                CaseSubjectId = Convert.ToInt64(reader["MAST_RCSAT_CSSUBJECTID"]),
                Cancel = reader["CANCEL"]?.ToString(),
                Subject = reader["SUBJECT"]?.ToString(),
                SubjectHi = reader["SUBJECTHI"]?.ToString(),
                InActive = reader["INACTIVE"]?.ToString(),
                SubjectEngHi = reader["SUBJECTENGHI"]?.ToString(),
                CancelRemarks = reader["CANCELREMARKS"]?.ToString(),
                SourceId = reader["SOURCEID"] == DBNull.Value ? null : Convert.ToInt64(reader["SOURCEID"]),
                MapName = reader["MAPNAME"]?.ToString(),
                WkId = reader["WKID"]?.ToString(),
                AppLevel = reader["APP_LEVEL"] == DBNull.Value ? null : Convert.ToInt32(reader["APP_LEVEL"]),
                AppDesc = reader["APP_DESC"] == DBNull.Value ? null : Convert.ToInt32(reader["APP_DESC"]),
                AppSLevel = reader["APP_SLEVEL"] == DBNull.Value ? null : Convert.ToInt32(reader["APP_SLEVEL"]),
                WfRoles = reader["WFROLES"]?.ToString(),
                CreatedBy = reader["CREATEDBY"]?.ToString(),
                CreatedOn = reader["CREATEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["CREATEDON"]),
                UserName = reader["USERNAME"]?.ToString(),
                ModifiedOn = reader["MODIFIEDON"] == DBNull.Value ? null : Convert.ToDateTime(reader["MODIFIEDON"])
            };
        }
    }
}