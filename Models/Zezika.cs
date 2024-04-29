using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace Katsaka
{
    public class Zezika
    {
        private int idzezika;
        private string nomzezika;
        private double prixunitaire;
        private double prixmoyenne;

        public int getIdzezika() {
            return this.idzezika;
        }
        public string getNomzezika() {
            return this.nomzezika;
        }
        public double getPrixunitaire() {
            return this.prixunitaire;
        }
        public double getPrixmoyenne() {
            OleDbConnection c = new SqlDB().ConnectAccess();
            return Depense.getPrixMoyenneZezika(c,this.idzezika);
            c.Close();
        }
        
        public void setIdzezika(int idzezika) {
            if(idzezika == null) {
                throw new ArgumentException("Id zezika null");
            }
            this.idzezika = idzezika;
        }
        public void setNomzezika(string nom) {
            if(nom == "") {
                throw new ArgumentException("Nom zezika null");
            }
            this.nomzezika = nom;
        }
        public void setPrixunitaire(double prix) {
            if(prix < 0) {
                throw new ArgumentException("Prix zezika invalide");
            }
            this.prixunitaire = prix;
        }

        public List<Zezika> getAllZezika(MySqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectMySQL();
            }
            List<Zezika> allZezika = new List<Zezika>();
            string query = "select * from zezika";
            using (MySqlCommand command = new MySqlCommand(query, c))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Zezika zezika = new Zezika();
                        zezika.idzezika = reader.GetInt32(0);
                        zezika.nomzezika = reader.GetString(1);
                        zezika.prixunitaire = reader.GetDouble(2);
                        allZezika.Add(zezika);
                    }
                }
            }
            return allZezika;
        }

        public static Zezika getZezikaById(MySqlConnection c, int id) {
            if(c == null) {
                c = new SqlDB().ConnectMySQL();
            }
            string query = "select * from zezika where idzezika = "+id;
            Zezika zezika = new Zezika();
            using (MySqlCommand command = new MySqlCommand(query, c))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zezika.idzezika = reader.GetInt32(0);
                        zezika.nomzezika = reader.GetString(1);
                        zezika.prixunitaire = reader.GetDouble(2);
                    }
                }
            }
            return zezika;
        } 

        public void insert(MySqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectMySQL();
            }
            try
            {
                string query = "INSERT INTO zezika (nomzezika, prixunitaire) VALUES ('"+this.nomzezika+"',"+this.prixunitaire+")";
                using (MySqlCommand command = new MySqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Zezika réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion du Zezika : " + ex.Message);
            }
        }
    }
}