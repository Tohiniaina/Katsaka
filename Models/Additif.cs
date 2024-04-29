using Npgsql;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace Katsaka
{
    public class Additif
    {
        public int idajoutadditif {get;set;}
        public int idzezika;
        public string idparcelle;
        public double percentmin;
        public double percentmax;
        public double variation {get;set;}
    }
}