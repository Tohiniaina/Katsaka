using Npgsql;

namespace Katsaka
{
    public class Recolte
    {
        private string idrecolte;
        private string idresponsable;
        private double nbrtolany;
        private double longueur;
        private double poidsrecolte;
        private DateTime daterecolte;
        
        public string getIdrecolte() {
            return this.idrecolte;
        }
        public string getIdresponsable() {
            return this.idresponsable;
        }
        public double getNbrtolany() {
            return this.nbrtolany;
        }
        public double getLongueur() {
            return this.longueur;
        }
        public double getPoidsrecolte() {
            return this.poidsrecolte;
        }
        public double getPoidsunit() {
            return (this.poidsrecolte/this.nbrtolany);
        }
        public DateTime getDaterecolte() {
            return this.daterecolte;
        }
        public string getIdparcelle(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string id = "";
            string query = "select idparcelle from responsable where idresponsable = '"+this.idresponsable+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetString(0);
                    }
                }
            }
            return id;
        }

        public void setIdrecolte(string id) {
            this.idrecolte = id;
        }
        public void setIdresponsable(string id) {
            this.idresponsable = id;
        }
        public void setNbrtolany(double nbr) {
            this.nbrtolany = nbr;
        }
        public void setLongueur(double longo) {
            this.longueur = longo;
        }
        public void setPoidsrecolte(double pds) {
            this.poidsrecolte = pds;
        }
        public void setDaterecolte(DateTime dt) {
            this.daterecolte = dt;
        }

        public static Recolte getRecoParcelle(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string idrespo = Parcelle.getParcelleById(c,id).getIdRespo(c);
            Recolte reco = new Recolte();
            string query = "select * from recolte where idresponsable = '"+idrespo+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reco.setIdrecolte(reader.GetString(0));
                        reco.setIdresponsable(reader.GetString(1));
                        reco.setNbrtolany(reader.GetDouble(2));
                        reco.setLongueur(reader.GetDouble(3));
                        reco.setPoidsrecolte(reader.GetDouble(4));
                        reco.setDaterecolte(reader.GetDateTime(5));
                    }
                }
            }
            return reco;
        } 

        public Recolte getRecoById(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Recolte reco = new Recolte();
            string query = "select * from recolte where idrecolte = '"+id+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reco.setIdrecolte(reader.GetString(0));
                        reco.setIdresponsable(reader.GetString(1));
                        reco.setNbrtolany(reader.GetDouble(2));
                        reco.setLongueur(reader.GetDouble(3));
                        reco.setPoidsrecolte(reader.GetDouble(4));
                        reco.setDaterecolte(reader.GetDateTime(5));
                    }
                }
            }
            return reco;
        } 

        public List<Anomalie> getAnomalie(NpgsqlConnection c) {
            List<Anomalie> ano = Anomalie.getAnomalieRecolte(c, this.idrecolte);
            return ano;
        }

        public void insert(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO recolte (idresponsable, nbrtolany, longueurtolany, poidsrecolte, daterecolte) VALUES ('"+this.idresponsable+"',"+this.nbrtolany+","+this.longueur+","+this.poidsrecolte+",'"+this.daterecolte+"')";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Recolte réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion des données : " + ex.Message);
            }
        }
    }
}