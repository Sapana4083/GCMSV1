using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace GCMS.Data
{
    public class OracleConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public OracleConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new OracleConnection(
                _configuration.GetConnectionString("RcsatOracle"));
        }
    }
}
