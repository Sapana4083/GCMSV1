using GCMS.Web.Models.DTOs;
using GCMS.WEB.Data;
using GCMS.WEB.Models;
using GCMS.WEB.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GCMS.WEB.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserAsync(string username)
        {
            return await _context.AxUsers
                .FirstOrDefaultAsync(x =>
                    x.UserName == username &&
                    x.Active == "T");
        }

        public async Task<UserDepartmentInfoDTO?> GetDepartmentAndCourtAsync(string username)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();

            command.CommandText = @"
        SELECT
            m.DEPTNAMEEN,
            c.COURT_NAME,
            c.COURT_CODE,
            u.SSOID,
            u.AXUSERSID
        FROM department_mast m,
             axcourts ax,
             court_name_mast c,
             axusers u
        WHERE ax.DEPTNAME = m.DEPARTMENT_MASTID
        AND ax.COURT_NAME = c.COURT_NAME_MASTID
        AND ax.AXUSERSID = u.AXUSERSID
        AND ax.ISDEFAULT = 'Yes'
        AND UPPER(u.USERNAME) = UPPER(:username)";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "username";
            parameter.Value = username;
            command.Parameters.Add(parameter);

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new UserDepartmentInfoDTO
            {
                UserSSO = reader["SSOID"]?.ToString(),
                AxUserID = reader["AXUSERSID"]?.ToString(),
                DepartmentName = reader["DEPTNAMEEN"]?.ToString(),
                CourtName = reader["COURT_NAME"]?.ToString(),
                CourtCode = reader["COURT_CODE"]?.ToString()
            };
        }
    }
}
