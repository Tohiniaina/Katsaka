using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

public class SqlDB {
    NpgsqlConnection postgresConnection = null;
    MySqlConnection mysqlConnection = null;
    OleDbConnection accessConnection = null;

    public NpgsqlConnection ConnectPostgres() {
        string postgresConnectionString = "Server=localhost;Port=5432;Database=katsaka;User Id=postgres;Password=postgres;";
        postgresConnection = new NpgsqlConnection(postgresConnectionString);
        
        try
        {
            postgresConnection.Open();
            return postgresConnection;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur de connexion à la base de données PostgreSQL: " + ex.Message);
            return null;
        }
    }

    public MySqlConnection ConnectMySQL() {
        string mysqlConnectionString = "Server=localhost;Port=3306;Database=katsaka;Uid=root;Pwd=root;";
        mysqlConnection = new MySqlConnection(mysqlConnectionString);
        
        try
        {
            mysqlConnection.Open();
            return mysqlConnection;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur de connexion à la base de données MySQL: " + ex.Message);
            return null;
        }
    }

    public OleDbConnection ConnectAccess() {
        string AccessConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\s5\Prog\Katsaka\Katsaka.accdb;Persist Security Info=False;";
        accessConnection = new OleDbConnection(AccessConnectionString);
        
        try
        {
            accessConnection.Open();
            return accessConnection;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Erreur de connexion à la base de données Access : " + ex.Message);
            return null;
        }
    }

    public void Close() {
        if (postgresConnection != null)
        {
            postgresConnection.Close();
        }

        if (mysqlConnection != null)
        {
            mysqlConnection.Close();
        }

        if (accessConnection != null)
        {
            accessConnection.Close();
        }
    }
}
