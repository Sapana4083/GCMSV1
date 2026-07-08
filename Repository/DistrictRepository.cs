using GCMS.Repository.Interfaces;
using GCMS.Data;
using GCMS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Repository
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly OracleConnectionFactory _connectionFactory;

        public DistrictRepository(
            ApplicationDbContext context,
            OracleConnectionFactory connectionFactory)
        {
            _context = context;
            _connectionFactory = connectionFactory;
        }

        public async Task<List<DistrictMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            var districts = new List<DistrictMaster>();

            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_dist_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 5;
            cmd.Parameters.Add("p_row_cnt", OracleDbType.Int32).Value = rowCnt;
            cmd.Parameters.Add("p_page_no", OracleDbType.Int32).Value = pageNo;

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                districts.Add(new DistrictMaster
                {
                    DistrictMastId = Convert.ToInt64(reader["DISTRICT_MASTID"]),
                    DistrictName = reader["DISTRICT_NAME"]?.ToString(),
                    DistrictCode = reader["DISTRICT_CODE"]?.ToString(),

                    StateName =
                        reader["STATE_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["STATE_NAME"]),

                    DivisionName =
                        reader["DIVISION_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DIVISION_NAME"]),

                    DistAbr = reader["DIST_ABR"]?.ToString(),
                    DistrictNameEng = reader["DISTRICT_NAME_ENG"]?.ToString(), // fixed: ENG not EMG
                    DistNameHinEng = reader["DIST_NAME_HINENG"]?.ToString(),
                    InActive = reader["INACTIVE"]?.ToString()
                });
            }

            return districts;
        }

        public async Task<DistrictMaster?> GetByIdAsync(long id)
        {
            DistrictMaster? district = null;

            using var conn = _connectionFactory.CreateConnection();

            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_dist_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32)
               .Value = 4;

            cmd.Parameters.Add("p_dist_mastid", OracleDbType.Int64)
               .Value = id;

            cmd.Parameters.Add("out_cursor",
                OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                district = new DistrictMaster
                {
                    DistrictMastId =
                        Convert.ToInt64(reader["DISTRICT_MASTID"]),

                    DistrictName =
                        reader["DISTRICT_NAME"]?.ToString(),

                    DistrictCode =
                        reader["DISTRICT_CODE"]?.ToString(),

                    StateName =
                        reader["STATE_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["STATE_NAME"]),

                    DivisionName =
                        reader["DIVISION_NAME"] == DBNull.Value
                        ? null
                        : Convert.ToInt64(reader["DIVISION_NAME"]),

                    DistAbr =
                        reader["DIST_ABR"]?.ToString(),

                    DistrictNameEng =
                        reader["DISTRICT_NAME_ENG"]?.ToString(),

                    DistNameHinEng =
                        reader["DIST_NAME_HINENG"]?.ToString(),


                    InActive =
                        reader["INACTIVE"]?.ToString()
                };
            }

            return await Task.FromResult(district);
        }

        public async Task AddAsync(DistrictMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_dist_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 1;
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = model.CreatedBy;
            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value = model.CreatedBy;
            cmd.Parameters.Add("p_district_name", OracleDbType.Varchar2).Value = model.DistrictName;
            cmd.Parameters.Add("p_district_code", OracleDbType.Varchar2).Value = model.DistrictCode;

            cmd.Parameters.Add("p_state_name", OracleDbType.Int64).Value =
                model.StateName.HasValue ? model.StateName.Value : (object)DBNull.Value;

            cmd.Parameters.Add("p_division_name", OracleDbType.Int64).Value =
                model.DivisionName.HasValue ? model.DivisionName.Value : (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_name_eng", OracleDbType.Varchar2).Value =
                model.DistrictNameEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_name_hineng", OracleDbType.Varchar2).Value =
                model.DistNameHinEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value = "F";

            cmd.Parameters.Add("out_cursor", OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex) when (ex.Number == 20001)
            {
                // Raised by proc's district_name duplicate check
                throw new InvalidOperationException("This District Name already exists.", ex);
            }
            catch (OracleException ex) when (ex.Number == 20002)
            {
                // Raised by proc's district_code duplicate check (after you add it)
                throw new InvalidOperationException("This District Code already exists.", ex);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                // ORA-00001 fallback — unique constraint hit without a proc-level check
                throw new InvalidOperationException("A record with this District Code or Name already exists.", ex);
            }

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(DistrictMaster model)
        {
            using var conn = _connectionFactory.CreateConnection();
            conn.Open();

            using var cmd = (OracleCommand)conn.CreateCommand();

            cmd.BindByName = true;
            cmd.CommandText = "proc_dist_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32).Value = 2;

            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value =
                model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_createdby", OracleDbType.Varchar2).Value =
                model.CreatedBy ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_district_name", OracleDbType.Varchar2).Value =
                model.DistrictName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_district_code", OracleDbType.Varchar2).Value =
                model.DistrictCode ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_state_name", OracleDbType.Int64).Value =
                model.StateName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_division_name", OracleDbType.Int64).Value =
                model.DivisionName ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_name_eng", OracleDbType.Varchar2).Value =
                model.DistrictNameEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_abr", OracleDbType.Varchar2).Value =
                model.DistAbr ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_apnakhata_id", OracleDbType.Varchar2).Value =
                DBNull.Value;

            cmd.Parameters.Add("p_dist_name_hineng", OracleDbType.Varchar2).Value =
                model.DistNameHinEng ?? (object)DBNull.Value;

            cmd.Parameters.Add("p_dist_mastid", OracleDbType.Int64).Value =
                model.DistrictMastId;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2).Value =
                model.InActive ?? "F";

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
            cmd.CommandText = "proc_dist_mast";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_input", OracleDbType.Int32)
               .Value = 3;

            cmd.Parameters.Add("p_dist_mastid", OracleDbType.Int64)
               .Value = id;

            cmd.Parameters.Add("p_inactive", OracleDbType.Varchar2)
               .Value = "T";

            cmd.Parameters.Add("out_cursor",
                OracleDbType.RefCursor)
                .Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            await Task.CompletedTask;
        }
    }
}