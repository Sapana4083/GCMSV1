using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class CourtTypeRepository : ICourtTypeRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public CourtTypeRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CourtTypeMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            List<CourtTypeMaster> list = new();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("P_ROW_CNT", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("P_PAGE_NO", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new CourtTypeMaster
                {
                    CourtTypeMastId = Convert.ToInt64(reader["COURT_TYPE_MASTID"]),
                    CourtType = reader["COURT_TYPE"]?.ToString(),
                    CourtTypeName = reader["COURT_TYPE_NAME"]?.ToString(),
                    
                    CourtGroupCode = reader["COURT_GROUP_CODE"]?.ToString(),

                    CourtCategory = reader["COURT_CATEGORY"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["COURT_CATEGORY"]),

                    DepartId = reader["DEPARTID"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DEPARTID"]),

                    HierarchyLevel = reader["HIERARCHY_LEVEL"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["HIERARCHY_LEVEL"]),

                    DispOrder = reader["DISP_ORDER"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["DISP_ORDER"]),

                    
                    InActive = reader["INACTIVE"]?.ToString(),

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

        public async Task<CourtTypeMaster?> GetByIdAsync(long id)
        {
            CourtTypeMaster? model = null;

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 3;
            cmd.Parameters.Add("P_COURT_TYPE_MASTID", OracleDbType.Int64).Value = id;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                model = new CourtTypeMaster
                {
                    CourtTypeMastId = Convert.ToInt64(reader["COURT_TYPE_MASTID"]),
                    CourtType = reader["COURT_TYPE"]?.ToString(),
                    CourtTypeName = reader["COURT_TYPE_NAME"]?.ToString(),
                    
                    CourtGroupCode = reader["COURT_GROUP_CODE"]?.ToString(),

                    CourtCategory = reader["COURT_CATEGORY"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["COURT_CATEGORY"]),

                    DepartId = reader["DEPARTID"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DEPARTID"]),

                    HierarchyLevel = reader["HIERARCHY_LEVEL"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["HIERARCHY_LEVEL"]),

                    DispOrder = reader["DISP_ORDER"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["DISP_ORDER"]),

                    InActive = reader["INACTIVE"]?.ToString()
                };
            }

            return model;
        }

        public async Task AddAsync(CourtTypeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("P_COURT_TYPE", OracleDbType.Varchar2).Value = model.CourtType;
            cmd.Parameters.Add("P_COURT_TYPE_NAME", OracleDbType.Varchar2).Value = model.CourtTypeName;
          
            cmd.Parameters.Add("P_COURT_GROUP_CODE", OracleDbType.Varchar2).Value = model.CourtGroupCode;
            cmd.Parameters.Add("P_COURT_CATEGORY", OracleDbType.Int64).Value = (object?)model.CourtCategory ?? DBNull.Value;
            cmd.Parameters.Add("P_DEPARTID", OracleDbType.Int64).Value = (object?)model.DepartId ?? DBNull.Value;
            cmd.Parameters.Add("P_HIERARCHY_LEVEL", OracleDbType.Int32).Value = (object?)model.HierarchyLevel ?? DBNull.Value;
            cmd.Parameters.Add("P_DISP_ORDER", OracleDbType.Int32).Value = (object?)model.DispOrder ?? DBNull.Value;
            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2).Value = model.InActive ?? "F";
            cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = model.CreatedBy;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
        }

        public async Task UpdateAsync(CourtTypeMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("P_COURT_TYPE_MASTID", OracleDbType.Int64).Value = model.CourtTypeMastId;
            cmd.Parameters.Add("P_COURT_TYPE", OracleDbType.Varchar2).Value = model.CourtType;
            cmd.Parameters.Add("P_COURT_TYPE_NAME", OracleDbType.Varchar2).Value = model.CourtTypeName;
           
            cmd.Parameters.Add("P_COURT_GROUP_CODE", OracleDbType.Varchar2).Value = model.CourtGroupCode;
            cmd.Parameters.Add("P_COURT_CATEGORY", OracleDbType.Int64).Value = (object?)model.CourtCategory ?? DBNull.Value;
            cmd.Parameters.Add("P_DEPARTID", OracleDbType.Int64).Value = (object?)model.DepartId ?? DBNull.Value;
            cmd.Parameters.Add("P_HIERARCHY_LEVEL", OracleDbType.Int32).Value = (object?)model.HierarchyLevel ?? DBNull.Value;
            cmd.Parameters.Add("P_DISP_ORDER", OracleDbType.Int32).Value = (object?)model.DispOrder ?? DBNull.Value;
            cmd.Parameters.Add("P_INACTIVE", OracleDbType.Varchar2).Value = model.InActive ?? "F";
            cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2).Value = model.ModifiedBy;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
        }

        public async Task<List<LovModel>> GetCourtCategoryAsync()
        {
            List<LovModel> list = new();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "PROC_COURT_TYPE";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("V_INPUT", OracleDbType.Int32).Value = 6;

            cmd.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new LovModel
                {
                    Id = Convert.ToInt64(reader["LOVVALUESDTLID"]),
                    Name = reader["ALLOWED_VALUE"].ToString()
                });
            }

            return list;
        }
    }
}