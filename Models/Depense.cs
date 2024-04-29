using System.Data.OleDb;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Katsaka
{
    public class Depense
    {
        private int iddepense;
        private int idzezika;
        private string idparcelle;
        private double quantite;
        private double prixunitaire;
        private DateTime date;

        public int getIddepense() {
            return this.iddepense;
        }
        public int getIdzezika() {
            return this.idzezika;
        }
        public Zezika getZezika() {
            MySqlConnection c = new SqlDB().ConnectMySQL();
            Zezika zezika = Zezika.getZezikaById(c,this.idzezika);
            c.Close();
            return zezika;
        }
        public string getIdparcelle() {
            return this.idparcelle;
        }
        public Parcelle getParcelle() {
            NpgsqlConnection c = new SqlDB().ConnectPostgres();
            Parcelle p = Parcelle.getParcelleById(c,this.idparcelle);
            c.Close();
            return p;
        }
        public double getQuantite() {
            return this.quantite;
        }
        public double getPrixunitaire() {
            return this.prixunitaire;
        }
        public DateTime getDate() {
            return this.date;
        }
        public double getPrixtotal() {
            return this.prixunitaire*this.quantite;
        }

        public double getTotal() {
            OleDbConnection c = new SqlDB().ConnectAccess();
            List<Depense> dep = new Depense().getAllDepense(c);
            c.Close();
            double ans = 0;
            foreach(var Depense in dep) {
                ans += Depense.getPrixtotal();
            }
            return ans;
        }
        
        public void setIddepense(int iddepense) {
            if(idzezika == null) {
                throw new ArgumentException("Id depense null");
            }
            this.iddepense = iddepense;
        }
        public void setIdzezika(int idzezika) {
            if(idzezika == null) {
                throw new ArgumentException("Id zezika null");
            }
            this.idzezika = idzezika;
        }
        public void setIdparcelle(string id) {
            if(id == null) {
                throw new ArgumentException("Id parcelle null");
            }
            this.idparcelle = id;
        }
        public void setQuantite(double qte) {
            if(qte < 0 || qte == null) {
                throw new ArgumentException("Quantite zezika invalide");
            }
            this.quantite = qte;
        }
        public void setPrixunitaire(double prix) {
            if(prix < 0 || prix == null) {
                throw new ArgumentException("Prix unitaire Zezika invalide");
            }
            this.prixunitaire = prix;
        }
        public void setDate(DateTime dat) {
            this.date = dat;
        }

        public List<Depense> getAllDepense(OleDbConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectAccess();
            }
            List<Depense> allDepense = new List<Depense>();
            string query = "select * from depense";
            using (OleDbCommand command = new OleDbCommand(query, c))
            {
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Depense depense = new Depense();
                        depense.iddepense = reader.GetInt32(0);
                        depense.idzezika = reader.GetInt32(1);
                        depense.idparcelle = reader.GetString(2);
                        depense.quantite = (double)reader.GetDecimal(3);
                        depense.date = reader.GetDateTime(4);
                        depense.prixunitaire = (double)reader.GetDecimal(5);
                        allDepense.Add(depense);
                    }
                }
            }
            return allDepense;
        }

        public static double getPrixMoyenneZezika(OleDbConnection c,int idzezika) {
            List<Depense> sommeZezika = new Depense().getAllDepense(c);
            double prix = 0;
            int repetition = 0;
            foreach(var zezika in sommeZezika) {
                if(zezika.idzezika == idzezika) {
                    prix += zezika.getPrixunitaire();
                    repetition+=1;
                }
            }
            if(prix != 0) {
                return (prix/repetition);
            }
            return 0;
        }

        public List<Depense> getDepenseByIdZezika(OleDbConnection c,int id, string idparcelle) {
            if(c == null) {
                c = new SqlDB().ConnectAccess();
            }
            List<Depense> allDepense = new List<Depense>();
            string query = @"select * from depense where idzezika = @idzezika and idparcelle = @idparcelle";
            using (OleDbCommand command = new OleDbCommand(query, c))
            {
                command.Parameters.AddWithValue("@idzezika", id);
                command.Parameters.AddWithValue("@idparcelle", idparcelle);
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Depense depense = new Depense();
                        depense.iddepense = reader.GetInt32(0);
                        depense.idzezika = reader.GetInt32(1);
                        depense.idparcelle = reader.GetString(2);
                        depense.quantite = (double)reader.GetDecimal(3);
                        depense.date = reader.GetDateTime(4);
                        depense.prixunitaire = (double)reader.GetDecimal(5);
                        allDepense.Add(depense);
                    }
                }
            }
            return allDepense;
        }

        public void insert(OleDbConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectAccess();
            }
            try
            {
                string query = @"INSERT INTO depense (idzezika, idparcelle, quantite, prixunitaire, daty) VALUES (@idzezika,@idparcelle,@qte,@prixunitaire,@daty)";
                using (OleDbCommand command = new OleDbCommand(query, c))
                {
                    command.Parameters.AddWithValue("@idzezika", this.idzezika);
                    command.Parameters.AddWithValue("@idparcelle", this.idparcelle);
                    command.Parameters.AddWithValue("@qte", this.quantite);
                    command.Parameters.AddWithValue("@prixunitaire", this.prixunitaire);
                    command.Parameters.AddWithValue("@daty", this.date);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Depense réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion du Depense : " + ex.Message);
            }
        }
    }
}