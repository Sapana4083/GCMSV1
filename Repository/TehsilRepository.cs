using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class TehsilRepository : ITehsilRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TehsilRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Add

        public async Task AddAsync(TehsilMaster model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_tehsil_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
                model.Username ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value =
                model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_state_id", OracleDbType.Int64).Value =
                model.StateId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_id_old", OracleDbType.Int64).Value =
                model.DistIdOld ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_sdo_name_id", OracleDbType.Int64).Value =
                model.SdoNameId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_tehsil_name", OracleDbType.Varchar2).Value =
                model.TehsilName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_tehsil_code", OracleDbType.Varchar2).Value =
                model.TehsilCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value =
                model.Inactive ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_tehsil_eng", OracleDbType.Varchar2).Value =
                model.TehsilEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_gis_tehsilid", OracleDbType.Int64).Value =
                model.GisTehsilId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_apnakhata_id", OracleDbType.Varchar2).Value =
                model.ApnaKhataId ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_name_id", OracleDbType.Varchar2).Value =
                model.DistNameId ?? (object)DBNull.Value;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }

        #endregion

        #region Update

        public async Task UpdateAsync(TehsilMaster model)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_tehsil_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            // Update
            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
                (object?)model.Username ?? DBNull.Value;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value =
                DBNull.Value;

            cmd.Parameters.Add("p_tehsil_mastid", OracleDbType.Int64).Value =
                model.TehsilMastId;

            cmd.Parameters.Add("p_state_id", OracleDbType.Int64).Value =
                (object?)model.StateId ?? DBNull.Value;

            cmd.Parameters.Add("p_dist_id_old", OracleDbType.Int64).Value =
                (object?)model.DistIdOld ?? DBNull.Value;

            cmd.Parameters.Add("p_sdo_name_id", OracleDbType.Int64).Value =
                (object?)model.SdoNameId ?? DBNull.Value;

            cmd.Parameters.Add("p_tehsil_name", OracleDbType.Varchar2).Value =
                (object?)model.TehsilName ?? DBNull.Value;

            cmd.Parameters.Add("p_tehsil_code", OracleDbType.Varchar2).Value =
                (object?)model.TehsilCode ?? DBNull.Value;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value =
                (object?)model.Inactive ?? "F";

            cmd.Parameters.Add("p_tehsil_eng", OracleDbType.Varchar2).Value =
                (object?)model.TehsilEng ?? DBNull.Value;

            cmd.Parameters.Add("p_gis_tehsilid", OracleDbType.Int64).Value =
                (object?)model.GisTehsilId ?? DBNull.Value;

            cmd.Parameters.Add("p_apnakhata_id", OracleDbType.Varchar2).Value =
                (object?)model.ApnaKhataId ?? DBNull.Value;

            cmd.Parameters.Add("p_dist_name_id", OracleDbType.Int64).Value =
                (object?)model.DistNameId ?? DBNull.Value;

            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;

            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();
        }
        #endregion

        #region Get By Id

        public async Task<TehsilMaster?> GetByIdAsync(long tehsilMastId)
        {
            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_tehsil_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 4;

            cmd.Parameters.Add("p_tehsil_mastid", OracleDbType.Int64).Value = tehsilMastId;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_state_id", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_dist_id_old", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_name_id", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_name", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_code", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_eng", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_gis_tehsilid", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_apnakhata_id", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_dist_name_id", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = 10;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = 1;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
               .Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new TehsilMaster
                {
                    TehsilMastId = Convert.ToInt64(reader["TEHSIL_MASTID"]),

                    StateId = reader["STATE_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["STATE_NAME"]),

                    DistIdOld = reader["DISTRICT_NAME_OLD"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DISTRICT_NAME_OLD"]),

                    SdoNameId = reader["SDO_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["SDO_NAME"]),

                    DistNameId = reader["DISTRICT_NAME"] == DBNull.Value
                    ? 0
                   : Convert.ToInt64(reader["DISTRICT_NAME"]),

                    TehsilName = reader["TEHSIL_NAME"] == DBNull.Value
                        ? null
                        : reader["TEHSIL_NAME"].ToString(),

                    TehsilCode = reader["TEHSIL_CODE"] == DBNull.Value
                        ? null
                        : reader["TEHSIL_CODE"].ToString(),

                    Inactive = reader["INACTIVE"] == DBNull.Value
                        ? null
                        : reader["INACTIVE"].ToString(),

                    TehsilEng = reader["TEHSIL_ENG"] == DBNull.Value
                        ? null
                        : reader["TEHSIL_ENG"].ToString(),

                    GisTehsilId = reader["GIS_TEHSILID"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["GIS_TEHSILID"]),

                    ApnaKhataId = reader["APNAKHATA_ID"] == DBNull.Value
                        ? null
                        : reader["APNAKHATA_ID"].ToString()
                };
            }

            return null;
        }

        #endregion

        #region Get All

        public async Task<List<TehsilMaster>> GetAllAsync(int pageNo, int rowCount)
        {
            List<TehsilMaster> list = new();

            using var conn = (OracleConnection)_connectionFactory.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_tehsil_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;

            // Unused parameters
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_mastid", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_state_id", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_dist_id_old", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_sdo_name_id", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_name", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_code", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_tehsil_eng", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_gis_tehsilid", OracleDbType.Int64).Value = DBNull.Value;
            cmd.Parameters.Add("p_apnakhata_id", OracleDbType.Varchar2).Value = DBNull.Value;
            cmd.Parameters.Add("p_dist_name_id", OracleDbType.Int64).Value = DBNull.Value;

            // Pagination
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCount;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            // Output Cursor
            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new TehsilMaster
                {
                    TehsilMastId = Convert.ToInt64(reader["TEHSIL_MASTID"]),

                    StateId = reader["STATE_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["STATE_NAME"]),

                    DistIdOld = reader["DISTRICT_NAME_OLD"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DISTRICT_NAME_OLD"]),

                    DistNameId = reader["DISTRICT_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DISTRICT_NAME"]),

                    SdoNameId = reader["SDO_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["SDO_NAME"]),

                    TehsilName = reader["TEHSIL_NAME"] == DBNull.Value
                        ? null
                        : reader["TEHSIL_NAME"].ToString(),

                    TehsilCode = reader["TEHSIL_CODE"] == DBNull.Value
                        ? null
                        : reader["TEHSIL_CODE"].ToString(),

                    Inactive = reader["INACTIVE"] == DBNull.Value
                        ? null
                        : reader["INACTIVE"].ToString(),

                    TehsilEng = reader["TEHSIL_ENG"] == DBNull.Value
                        ? null
                        : reader["TEHSIL_ENG"].ToString(),

                    GisTehsilId = reader["GIS_TEHSILID"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["GIS_TEHSILID"]),

                    ApnaKhataId = reader["APNAKHATA_ID"] == DBNull.Value
                        ? null
                        : reader["APNAKHATA_ID"].ToString(),

                    CreatedBy = reader["CREATEDBY"] == DBNull.Value
                        ? null
                        : reader["CREATEDBY"].ToString(),

                    CreatedOn = reader["CREATEDON"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["CREATEDON"]),

                    Username = reader["USERNAME"] == DBNull.Value
                        ? null
                        : reader["USERNAME"].ToString(),

                    ModifiedOn = reader["MODIFIEDON"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["MODIFIEDON"])
                });
            }

            return list;
        }

        #endregion
    }
}