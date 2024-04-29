using Npgsql;

namespace Katsaka
{
    public class Mpamboly
    {
        private string idmpamboly;
        private string nommpamboly;

        public string getIdmpamboly() {
            return this.idmpamboly;
        }
        public string getNommpamboly() {
            return this.nommpamboly;
        }

        public void setNommpamboly(string nom) {
            if(nom == null) {
                throw new ArgumentException("Nom Mpamboly null");
            }
            this.nommpamboly = nom;
        }

        public List<Mpamboly> getAllMpamboly(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Mpamboly> allMpamboly = new List<Mpamboly>();
            string query = "select * from mpamboly";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Mpamboly mpamboly = new Mpamboly();
                        mpamboly.idmpamboly = reader.GetString(0);
                        mpamboly.nommpamboly = reader.GetString(1);
                        allMpamboly.Add(mpamboly);
                    }
                }
            }
            return allMpamboly;
        }

        public List<Parcelle> getListParcelle(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Parcelle> parcelles = new List<Parcelle>();
            string query = "select * from champ where idmpamboly = '"+this.idmpamboly+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Parcelle parcelle = new Parcelle();
                        parcelle.setIdparcelle(reader.GetString(0));
                        parcelle.setNomparcelle(reader.GetString(1));
                        parcelle.setIdterrain(reader.GetString(2));
                        parcelle.setMesure(reader.GetDouble(3));
                        parcelles.Add(parcelle);
                    }
                }
            }
            return parcelles;
        }

        public Mpamboly getMpambolyById(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string query = "select * from mpamboly where idmpamboly = '"+id+"'";
            Mpamboly mpamboly = new Mpamboly();
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mpamboly.idmpamboly = reader.GetString(0);
                        mpamboly.nommpamboly = reader.GetString(1);
                    }
                }
            }
            return mpamboly;
        } 

        public void insert(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO mpamboly (nommpamboly) VALUES ('"+this.nommpamboly+"')";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Mpamboly réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion du parcelle : " + ex.Message);
            }
        }
    }
}