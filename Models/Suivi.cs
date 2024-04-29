using Npgsql;

namespace Katsaka
{
    public class Suivi
    {
        private string idsuivi;
        private string idresponsable;
        private DateTime datesuivi;
        private string idparcelle;
        private double nbrtahony;
        private double nbrtolany;
        private double longueur;
        private double croissance;
        private int niveauverrete;
        private int semaine;
        
        public string getIdsuivi() {
            return this.idsuivi;
        }
        public string getIdresponsable() {
            return this.idresponsable;
        }
        public DateTime getDatesuivi() {
            return this.datesuivi;
        }
        public string getIdparcelle() {
            return this.idparcelle;
        }
        public double getNbrtahony() {
            return this.nbrtahony;
        }
        public double getNbrtolany() {
            return this.nbrtolany;
        }
        public double getLongueur() {
            return this.longueur;
        }
        public double getCroissance() {
            return this.croissance;
        }
        public int getNiveauverrete() {
            return this.niveauverrete;
        }
        public int getSemaine() {
            return this.semaine;
        }

        public void setIdsuivi(string id) {
            this.idsuivi = id;
        }
        public void setIdresponsable(string id) {
            if(id == null) {
                throw new ArgumentException("Id responsable null");
            }
            this.idresponsable = id;
        }       

        public void setDatesuivi(DateTime date) {
            this.datesuivi = date;
        }

        public void setIdparcelle(string id) {
            if(id == null) {
                throw new ArgumentException("Id parcelle null");
            }
            this.idparcelle = id;
            this.idresponsable = getIdRespo(null);
        }

        public void setNbrtahony(double nbr) {
            if(nbr < 0) {
                throw new ArgumentException("Le Nbr tahony doit etre positive");
            }
            this.nbrtahony = nbr;
        }

        public void setCroissance(double croissance) {
            this.croissance = croissance;
        }
        public void setSemaine(int semaine) {
            this.semaine = semaine;
        }

        public void setNbrtolany(double nbr) {
            if(nbr < 0) {
                throw new ArgumentException("Le Nbr tolany doit etre positive");
            }
            this.nbrtolany = nbr;
        }

        public void setLongueur(double longu) {
            if(longu < 0) {
                throw new ArgumentException("Le longueur du tolany doit etre positive");
            }
            this.longueur = longu;
        }

        public void setNiveauverrete(int niv) {
            if(niv < 0) {
                throw new ArgumentException("Le Niveau de verrete doit etre positive");
            }
            if(niv > 100) {
                throw new ArgumentException("Le Niveau de verrete doit etre inferieur à 100");
            }
            this.niveauverrete = niv;
        }
        
        public List<Suivi> getAllSuivi(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Suivi> suivis = new List<Suivi>();
            string query = "select * from suivi";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Suivi suivi = new Suivi();
                        suivi.idsuivi = reader.GetString(0);
                        suivi.idresponsable = reader.GetString(1);
                        suivi.datesuivi = reader.GetDateTime(2);
                        suivi.nbrtahony = reader.GetDouble(3);
                        suivi.nbrtolany = reader.GetDouble(4);
                        suivi.longueur = reader.GetDouble(5);
                        suivi.niveauverrete = reader.GetInt32(6);
                        suivi.croissance = reader.GetDouble(7);
                        suivi.semaine = reader.GetInt32(8);
                        suivis.Add(suivi);
                    }
                }
            }
            return suivis;
        }

        public Suivi getSuiviById(NpgsqlConnection c, string id) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            Suivi suivi = new Suivi();
            string query = "select * from suivi where idsuivi = '"+id+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        suivi.idsuivi = reader.GetString(0);
                        suivi.idresponsable = reader.GetString(1);
                        suivi.datesuivi = reader.GetDateTime(2);
                        suivi.nbrtahony = reader.GetDouble(3);
                        suivi.nbrtolany = reader.GetDouble(4);
                        suivi.longueur = reader.GetDouble(5);
                        suivi.niveauverrete = reader.GetInt32(6);
                        suivi.croissance = reader.GetDouble(7);
                        suivi.semaine = reader.GetInt32(8);
                    }
                }
            }
            return suivi;
        }

        public List<Anomalie> getAnomalie(NpgsqlConnection c) {
            List<Anomalie> ano = Anomalie.getAnomalieSuivi(c, this.idsuivi);
            return ano;
        }

        public string getIdRespo(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            string idrespo = "";
            string query = "select idresponsable from responsable where idparcelle = '"+this.idparcelle+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idrespo = reader.GetString(0);
                    }
                }
            }
            c.Close();
            return idrespo;
        }

        public void insert(NpgsqlConnection c) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO suivi (idresponsable,datesuivi,nbrtahony,nbrtolany,longueurtolany,nivverrete,croissance,semaine) VALUES ('"+this.idresponsable+"','"+this.datesuivi+"',"+this.nbrtahony+","+this.nbrtolany+","+this.longueur+","+this.niveauverrete+","+this.croissance+","+this.semaine+")";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Suivi réussie !");
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