using GCMS.Models; // adjust to match where LinkedCaseRowViewModel actually lives
using GCMS.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCMS.Repository.Implementations
{
    public class LinkedCaseRepository : ILinkedCaseRepository
    {
        private readonly string _connectionString;

        public LinkedCaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RcsatOracle");
        }

        public async Task<List<LinkedCaseRowViewModel>> GetLinkedCasesAsync(string mainCase)
        {
            var list = new List<LinkedCaseRowViewModel>();

            const string sql = @"
                SELECT TRN_RCSAT_CASEID, COURTCODE, CASE_TYPEE, CASE_NO, CALCASE, MINCASE,
                       INSDT, HDT, APPNAME, RESPNAME, CTYPE, PURPOSE, DNAME, APPADV, RESPAD, CONECT
                FROM TRN_RCSAT_LINKEDCASES
                WHERE MINCASE = :mainCase
                ORDER BY TRN_RCSAT_CASEID";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("mainCase", mainCase));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public async Task<LinkedCaseRowViewModel> GetByCaseNoAsync(string caseNo)
        {
            const string sql = @"
                SELECT TRN_RCSAT_CASEID, COURTCODE, CASE_TYPEE, CASE_NO, CALCASE, MINCASE,
                       INSDT, HDT, APPNAME, RESPNAME, CTYPE, PURPOSE, DNAME, APPADV, RESPAD, CONECT
                FROM TRN_RCSAT_LINKEDCASES
                WHERE CASE_NO = :caseNo
                AND ROWNUM = 1";

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("caseNo", caseNo));

            using var reader = await command.ExecuteReaderAsync();
            return await reader.ReadAsync() ? Map(reader) : null;
        }

        public async Task<bool> CaseExistsAsync(string caseNo)
        {
            const string sql = "SELECT COUNT(1) FROM TRN_RCSAT_LINKEDCASES WHERE CASE_NO = :caseNo";
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("caseNo", caseNo));
            return Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
        }

        public async Task<bool> SaveAsync(LinkedCaseRowViewModel row)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            string sql = row.CaseId.HasValue
                ? @"UPDATE TRN_RCSAT_LINKEDCASES SET
                        COURTCODE = :courtCode, CASE_TYPEE = :caseTypee, CASE_NO = :caseNo,
                        CALCASE = :calCase, MINCASE = :mainCase, INSDT = :insdt, HDT = :hdt,
                        APPNAME = :appName, RESPNAME = :respName, CTYPE = :ctype, PURPOSE = :purpose,
                        DNAME = :dname, APPADV = :appAdv, RESPAD = :respAd, CONECT = :conect
                    WHERE TRN_RCSAT_CASEID = :caseId"
                : @"INSERT INTO TRN_RCSAT_LINKEDCASES
                        (COURTCODE, CASE_TYPEE, CASE_NO, CALCASE, MINCASE, INSDT, HDT,
                         APPNAME, RESPNAME, CTYPE, PURPOSE, DNAME, APPADV, RESPAD, CONECT)
                    VALUES
                        (:courtCode, :caseTypee, :caseNo, :calCase, :mainCase, :insdt, :hdt,
                         :appName, :respName, :ctype, :purpose, :dname, :appAdv, :respAd, :conect)";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("courtCode", row.CourtCode ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("caseTypee", row.CaseTypee ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("caseNo", row.CaseNo ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("calCase", row.CalCase ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("mainCase", row.MainCase ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("insdt", (object)row.InstitutionDate ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("hdt", (object)row.HearingDate ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("appName", row.AppellantName ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("respName", row.RespondentName ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("ctype", row.CType ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("purpose", row.Purpose ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("dname", row.DistrictName ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("appAdv", row.AppellantAdvocate ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("respAd", row.RespondentAdvocate ?? (object)DBNull.Value));
            command.Parameters.Add(new OracleParameter("conect", row.Connected ?? (object)DBNull.Value));

            if (row.CaseId.HasValue)
                command.Parameters.Add(new OracleParameter("caseId", row.CaseId.Value));

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long caseId)
        {
            const string sql = "DELETE FROM TRN_RCSAT_LINKEDCASES WHERE TRN_RCSAT_CASEID = :caseId";
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("caseId", caseId));
            return await command.ExecuteNonQueryAsync() > 0;
        }

        private static LinkedCaseRowViewModel Map(OracleDataReader reader) => new()
        {
            CaseId = reader["TRN_RCSAT_CASEID"] != DBNull.Value ? Convert.ToInt64(reader["TRN_RCSAT_CASEID"]) : null,
            CourtCode = reader["COURTCODE"]?.ToString(),
            CaseTypee = reader["CASE_TYPEE"]?.ToString(),
            CaseNo = reader["CASE_NO"]?.ToString(),
            CalCase = reader["CALCASE"]?.ToString(),
            MainCase = reader["MINCASE"]?.ToString(),
            InstitutionDate = reader["INSDT"] != DBNull.Value ? Convert.ToDateTime(reader["INSDT"]) : null,
            HearingDate = reader["HDT"] != DBNull.Value ? Convert.ToDateTime(reader["HDT"]) : null,
            AppellantName = reader["APPNAME"]?.ToString(),
            RespondentName = reader["RESPNAME"]?.ToString(),
            CType = reader["CTYPE"]?.ToString(),
            Purpose = reader["PURPOSE"]?.ToString(),
            DistrictName = reader["DNAME"]?.ToString(),
            AppellantAdvocate = reader["APPADV"]?.ToString(),
            RespondentAdvocate = reader["RESPAD"]?.ToString(),
            Connected = reader["CONECT"]?.ToString()
        };
    }
}