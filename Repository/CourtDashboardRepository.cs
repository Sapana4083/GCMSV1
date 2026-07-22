using GCMS.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using GCMS.Models;

namespace GCMS.Repository
{
    public class CourtDashboardRepository : ICourtDashboardRepository
    {
        private readonly string _connectionString;

        public CourtDashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RcsatOracle");
        }

        public async Task<List<CourtDashboardViewModel>> GetCourtDashboardDataAsync()
        {
            var dashboard = new List<CourtDashboardViewModel>();

            const string sql = @"
            SELECT 
                dc.COURTNAME AS COURT_NAME,
                dc.RegisteredCases AS Registered_Cases,
                dc.RegisteredCases - dc.PendingCases AS Decided,
                dc.PendingCases AS Pending_Cases,
                NVL(du.Users, 0) AS Users
            FROM (
                SELECT 'RCSAT - JAIPUR' AS COURTNAME, COUNT(*) AS RegisteredCases,
                       SUM(CASE WHEN CASE_DECISION_DATE IS NULL THEN 1 ELSE 0 END) AS PENDINGCASES
                FROM TRN_RCSAT_CASEREG WHERE COURT_CODE = '18143'
                UNION ALL
                SELECT 'RCSAT - JODHPUR' AS COURTNAME, COUNT(*) AS RegisteredCases,
                       SUM(CASE WHEN CASE_DECISION_DATE IS NULL THEN 1 ELSE 0 END) AS PENDINGCASES
                FROM TRN_RCSAT_CASEREG WHERE COURT_CODE = '18144'
            ) dc
            LEFT JOIN (
                SELECT 'RCSAT - JAIPUR' AS COURTNAME, COUNT(DISTINCT a.username) AS Users
                FROM axusers a, axcourts b, Court_NAME_MAST c, DEPARTMENT_MAST ct
                WHERE a.AXUSERSID = b.AXUSERSID 
                  AND b.COURT_NAME = c.COURT_NAME_MASTID
                  AND c.DEPTID = ct.DEPARTMENT_MASTID
                  AND ct.DEPARTMENT_MASTID = 1714990007202 
                  AND a.ACTFLAG = 'T' 
                  AND c.COURT_NAME_MASTID = '1716660005958'
                UNION ALL
                SELECT 'RCSAT - JODHPUR' AS COURTNAME, COUNT(DISTINCT a.username) AS Users
                FROM axusers a, axcourts b, Court_NAME_MAST c, DEPARTMENT_MAST ct
                WHERE a.AXUSERSID = b.AXUSERSID 
                  AND b.COURT_NAME = c.COURT_NAME_MASTID
                  AND c.DEPTID = ct.DEPARTMENT_MASTID
                  AND ct.DEPARTMENT_MASTID = 1714990007202 
                  AND a.ACTFLAG = 'T' 
                  AND c.COURT_NAME_MASTID = '1751770006856'
            ) du ON dc.COURTNAME = du.COURTNAME";

            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new OracleCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dashboard.Add(new CourtDashboardViewModel
                        {
                            CourtName = reader["COURT_NAME"]?.ToString(),
                            RegisteredCases = reader["Registered_Cases"] != DBNull.Value
                                ? Convert.ToInt32(reader["Registered_Cases"]) : 0,
                            Decided = reader["Decided"] != DBNull.Value
                                ? Convert.ToInt32(reader["Decided"]) : 0,
                            PendingCases = reader["Pending_Cases"] != DBNull.Value
                                ? Convert.ToInt32(reader["Pending_Cases"]) : 0,
                            Users = reader["Users"] != DBNull.Value
                                ? Convert.ToInt32(reader["Users"]) : 0
                        });
                    }
                }
            }

            return dashboard;
        }
    }
}
