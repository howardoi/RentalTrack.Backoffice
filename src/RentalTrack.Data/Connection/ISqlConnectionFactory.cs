using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RentalTrack.Data.Configuration;

namespace RentalTrack.Data.Connection;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly DatabaseOptions _opt;

    public SqlConnectionFactory(IOptions<DatabaseOptions> options) => _opt = options.Value;

    public IDbConnection Create() => new SqlConnection(_opt.ConnectionString);
}
