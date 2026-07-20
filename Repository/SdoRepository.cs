using GCMS.Data;
using GCMS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class SdoRepository : ISdoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public SdoRepository(
            ApplicationDbContext context,
            OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        #region Add

        public async Task AddAsync(SdoMaster model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_sdo_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_district_name_eng", OracleDbType.Varchar2).Value = DBNull.Value;

            cmd.Parameters.Add("p_sdo_name_eng", OracleDbType.Varchar2).Value =
                (object?)model.SdoNameEng ?? DBNull.Value;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
                (object?)model.Username ?? DBNull.Value;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value =
                (object?)model.CreatedBy ?? DBNull.Value;

            cmd.Parameters.Add("p_district_mastid", OracleDbType.Int64).Value =
                (object?)model.DistrictMastId ?? DBNull.Value;

            cmd.Parameters.Add("p_sdo_name", OracleDbType.Varchar2).Value =
                (object?)model.SdoName ?? DBNull.Value;

            cmd.Parameters.Add("p_sdo_code", OracleDbType.Varchar2).Value =
                (object?)model.SdoCode ?? DBNull.Value;

            cmd.Parameters.Add("p_district_name1", OracleDbType.Varchar2).Value =
                (object?)model.DistrictName1 ?? DBNull.Value;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value = "F";

            cmd.Parameters.Add("p_sdo_mastid", OracleDbType.Int64).Value = DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region Update

        public async Task UpdateAsync(SdoMaster model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_sdo_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_district_name_eng", OracleDbType.Varchar2).Value = DBNull.Value;

            cmd.Parameters.Add("p_sdo_name_eng", OracleDbType.Varchar2).Value =
                (object?)model.SdoNameEng ?? DBNull.Value;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
                (object?)model.Username ?? DBNull.Value;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = DBNull.Value;

            cmd.Parameters.Add("p_district_mastid", OracleDbType.Int64).Value =
                (object?)model.DistrictMastId ?? DBNull.Value;

            cmd.Parameters.Add("p_sdo_name", OracleDbType.Varchar2).Value =
                (object?)model.SdoName ?? DBNull.Value;

            cmd.Parameters.Add("p_sdo_code", OracleDbType.Varchar2).Value =
                (object?)model.SdoCode ?? DBNull.Value;

            cmd.Parameters.Add("p_district_name1", OracleDbType.Varchar2).Value =
                (object?)model.DistrictName1 ?? DBNull.Value;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value =
                (object?)model.Inactive ?? "F";

            cmd.Parameters.Add("p_sdo_mastid", OracleDbType.Int64).Value =
                model.SdoMastId;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region Get By Id

        public async Task<SdoMaster?> GetByIdAsync(long sdoMastId)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_sdo_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;

            cmd.Parameters.Add("p_district_name_eng", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_name_eng", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_district_mastid", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_name", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_code", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_district_name1", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value = DBNull.Value;

            cmd.Parameters.Add("p_sdo_mastid", OracleDbType.Int64).Value = sdoMastId;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new SdoMaster
                {
                    SdoMastId = Convert.ToInt64(reader["SDO_MASTID"]),
                    DistrictMastId = reader["DISTRICT_NAME"] == DBNull.Value ? null : Convert.ToInt64(reader["DISTRICT_NAME"]),
                    SdoName = reader["SDO_NAME"]?.ToString(),
                    SdoCode = reader["SDO_CODE"]?.ToString(),
                    Inactive = reader["INACTIVE"]?.ToString(),
                    SdoNameEng = reader["SDO_NAME_ENG"]?.ToString(),
                    DistrictName1 = reader["DISTRICT_NAME1"]?.ToString()
                };
            }

            return null;
        }

        #endregion

        #region Get All

        public async Task<List<SdoMaster>> GetAllAsync(int pageNo, int rowCount)
        {
            List<SdoMaster> list = new();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_sdo_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;

            cmd.Parameters.Add("p_district_name_eng", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_name_eng", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_district_mastid", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_name", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_code", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_district_name1", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_mastid", OracleDbType.Int64).Value = DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCount;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new SdoMaster
                {
                    SdoMastId = Convert.ToInt64(reader["SDO_MASTID"]),
                    DistrictMastId = reader["DISTRICT_NAME"] == DBNull.Value ? null : Convert.ToInt64(reader["DISTRICT_NAME"]),
                    SdoName = reader["SDO_NAME"]?.ToString(),
                    SdoCode = reader["SDO_CODE"]?.ToString(),
                    Inactive = reader["INACTIVE"]?.ToString(),
                    SdoNameEng = reader["SDO_NAME_ENG"]?.ToString(),
                    DistrictName1 = reader["DISTRICT_NAME_ENG"]?.ToString(),
                    CreatedBy = reader["CREATEDBY"]?.ToString(),
                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["MODIFIEDON"]),
                });
            }

            return list;
        }

        #endregion
    }
}