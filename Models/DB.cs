using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
public interface IDB
{
        MySqlConnection GetMySqlConnection();
}

public class DB : IDB
{
    public string ConnectionString;

    public DB(IConfiguration configuration)
    {
        this.ConnectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public MySqlConnection GetMySqlConnection()
    {
        MySqlConnection tMySqlConnection = new MySqlConnection
        {
            ConnectionString = this.ConnectionString
        };

        tMySqlConnection.Open();

        return tMySqlConnection;
    }

}