using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CourtGroupRepository : ICourtGroupRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public CourtGroupRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CourtGroupMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            List<CourtGroupMaster> list = new();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_GROUP";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = pageNo;
            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Int32).Value = DBNull.Value;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new CourtGroupMaster
                {
                    CourtGroupId = Convert.ToInt64(reader["COURT_GROUPID"]),
                    CourtGroup = reader["COURT_GROUP"]?.ToString(),
                    CourtGroupCode = reader["COURT_GROUP_CODE"]?.ToString(),

                    InActive = reader["INACTIVE"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["INACTIVE"]),

                    Cancel = reader["CANCEL"]?.ToString(),

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

            return list;
        }

        public async Task<CourtGroupMaster?> GetByIdAsync(long id)
        {
            CourtGroupMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_GROUP";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 3;

            cmd.Parameters.Add("P_COURT_GROUPID", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Int32).Value = DBNull.Value;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                model = new CourtGroupMaster
                {
                    CourtGroupId = Convert.ToInt64(reader["COURT_GROUPID"]),
                    CourtGroup = reader["COURT_GROUP"]?.ToString(),
                    CourtGroupCode = reader["COURT_GROUP_CODE"]?.ToString(),

                    InActive = reader["INACTIVE"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["INACTIVE"]),

                    Cancel = reader["CANCEL"]?.ToString(),

                    CreatedBy = reader["CREATEDBY"]?.ToString(),

                    CreatedOn = reader["CREATEDON"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["CREATEDON"]),

                    ModifiedBy = reader["MODIFIEDBY"]?.ToString(),

                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["MODIFIEDON"])
                };
            }

            return model;
        }



        public async Task AddAsync(CourtGroupMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_GROUP";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_COURT_GROUP", OracleDbType.Varchar2).Value = model.CourtGroup;
            cmd.Parameters.Add("P_COURT_GROUP_CODE", OracleDbType.Varchar2).Value = model.CourtGroupCode;
            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Int32).Value = model.InActive;
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = model.CreatedBy;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
        }

        public async Task UpdateAsync(CourtGroupMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_GROUP";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_COURT_GROUPID", OracleDbType.Int64).Value = model.CourtGroupId;
            cmd.Parameters.Add("P_COURT_GROUP", OracleDbType.Varchar2).Value = model.CourtGroup;
            cmd.Parameters.Add("P_COURT_GROUP_CODE", OracleDbType.Varchar2).Value = model.CourtGroupCode;
            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Int32).Value = model.InActive;
            cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2).Value = model.ModifiedBy;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
        }
    }
}