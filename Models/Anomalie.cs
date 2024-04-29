using Npgsql;

namespace Katsaka
{
    public class Anomalie
    {
        private string idanomalie;
        private string idsuivi;
        private string idrecolte;
        private string idtypeanomalie;
        private string motif;
        
        public string getIdanomalie() {
            return this.idanomalie;
        }
        public string getIdsuivi() {
            return this.idsuivi;
        }
        public string getIdtypeanomalie() {
            return this.idtypeanomalie;
        }
        public string getIdrecolte() {
            return this.idrecolte;
        }
        public string getMotif() {
            return this.motif;
        }
        public void setIdanomalie(string idano) {
            if(idano == null) {
                throw new ArgumentException("Id anomalie null");
            }
            this.idanomalie = idano;
        }
        public void setIdsuivi(string idsuivi) {
            if(idsuivi == null) {
                throw new ArgumentException("Id suivi null");
            }
            this.idsuivi = idsuivi;
        }
        public void setIdrecolte(string idrecolte) {
            if(idrecolte == null) {
                throw new ArgumentException("Id recolte null");
            }
            this.idrecolte = idrecolte;
        }
        public void setIdtypeanomalie(string idtype) {
            if(idtype == null) {
                throw new ArgumentException("Id type anomalie null");
            }
            this.idtypeanomalie = idtype;
        }

        public static List<Anomalie> getAnomalieSuivi(NpgsqlConnection c,string idsuivi) {
            if(c == null) {
                SqlDB connexion = new SqlDB();
                c = connexion.ConnectPostgres();
            }
            List<Anomalie> anomalies = new List<Anomalie>();
            string query = "select * from detailsano where idsuivi = '"+idsuivi+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        Anomalie anomalie = new Anomalie();
                        anomalie.idanomalie = reader.GetString(0);
                        anomalie.idsuivi = reader.GetString(1);
                        anomalie.idtypeanomalie = reader.GetString(2);
                        anomalie.motif = reader.GetString(3);
                        anomalies.Add(anomalie);
                    }
                }
            }
            return anomalies;
        }

        public List<Anomalie> getAllAnomalie(NpgsqlConnection c) {
            if(c == null) {
                SqlDB connexion = new SqlDB();
                c = connexion.ConnectPostgres();
            }
            List<Anomalie> anomalies = new List<Anomalie>();
            string query = "select * from detailsano";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        Anomalie anomalie = new Anomalie();
                        anomalie.idanomalie = reader.GetString(0);
                        anomalie.idsuivi = reader.GetString(1);
                        anomalie.idtypeanomalie = reader.GetString(2);
                        anomalie.motif = reader.GetString(3);
                        anomalies.Add(anomalie);
                    }
                }
            }
            return anomalies;
        }

        public string getParcelle() {
            NpgsqlConnection c = new SqlDB().ConnectPostgres();
            string idparcelle = "";
            string query = "select idparcelle from detailsano as dt join suivi as s on dt.idsuivi = s.idsuivi join responsable as r on s.idresponsable = r.idresponsable where idanomalie = '"+this.idanomalie+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        idparcelle = reader.GetString(0);
                    }
                }
            }
            c.Close();
            return idparcelle;
        }

        public static List<Anomalie> getAnomalieRecolte(NpgsqlConnection c, string idrecolte) {
            if(c == null) {
                SqlDB connexion = new SqlDB();
                c = connexion.ConnectPostgres();
            }
            List<Anomalie> anomalies = new List<Anomalie>();
            string query = "select * from detailsanoreco where idrecolte = '"+idrecolte+"'";
            using (NpgsqlCommand command = new NpgsqlCommand(query, c))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        Anomalie anomalie = new Anomalie();
                        anomalie.idanomalie = reader.GetString(0);
                        anomalie.idrecolte = reader.GetString(1);
                        anomalie.idtypeanomalie = reader.GetString(2);
                        anomalie.motif = reader.GetString(3);
                        anomalies.Add(anomalie);
                    }
                }
            }
            return anomalies;
        }

        public static void checkAnomalie(NpgsqlConnection c, Suivi suivi1, Suivi suivi2) {
            if(suivi1.getNbrtahony() > suivi2.getNbrtahony()) {
                Anomalie ano = new Anomalie();
                ano.setIdsuivi(suivi2.getIdsuivi());
                ano.setIdtypeanomalie("TYA1");
                ano.insert(c);
            }
            if(suivi1.getNbrtolany() > suivi2.getNbrtolany()) {
                Anomalie ano = new Anomalie();
                ano.setIdsuivi(suivi2.getIdsuivi());
                ano.setIdtypeanomalie("TYA2");
                ano.insert(c);
            }
            if(getMoyenneCroissance(c,suivi2.getIdparcelle()) > suivi2.getCroissance()) {
                Anomalie ano = new Anomalie();
                ano.setIdsuivi(suivi2.getIdsuivi());
                ano.setIdtypeanomalie("TYA3");
                ano.insert(c);
            }
            if(suivi1.getNiveauverrete() > suivi2.getNiveauverrete()) {
                Anomalie ano = new Anomalie();
                ano.setIdsuivi(suivi2.getIdsuivi());
                ano.setIdtypeanomalie("TYA4");
                ano.insert(c);
            }
        }

        public void checkAnomalieRecolte(Suivi suivi1, Recolte reco, NpgsqlConnection c) {
            if((suivi1.getNbrtolany()*suivi1.getNbrtahony()) > reco.getNbrtolany()) {
                Anomalie ano = new Anomalie();
                ano.setIdrecolte(reco.getIdrecolte());
                ano.setIdtypeanomalie("TYA2");
                ano.insertanoreco(c);
            }
            if(suivi1.getLongueur() > reco.getLongueur()) {
                Anomalie ano = new Anomalie();
                ano.setIdrecolte(reco.getIdrecolte());
                ano.setIdtypeanomalie("TYA3");
                ano.insertanoreco(c);
            }
        }

        public static double getMoyenneCroissance(NpgsqlConnection c ,string idparcelle) {
            if(c == null) {
                c = new SqlDB().ConnectPostgres();
            }
            List<Parcelle> parcelle = Parcelle.getAllParcelle(c);
            double moyenne = 0;
            double somme = 0;
            for (int i = 0; i < parcelle.Count; i++)
            {
                if(parcelle[i].getIdparcelle() != idparcelle) {
                    Suivi suivi = parcelle[i].getLastSuivi(c);
                    somme += suivi.getCroissance();
                }
            }
            moyenne = somme/(parcelle.Count);
            return moyenne;
        }

        public void insert(NpgsqlConnection c) {
            if(c == null) {
                SqlDB connexion = new SqlDB();
                c = connexion.ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO anomalie (idsuivi,idtypeanomalie) VALUES ('"+this.idsuivi+"','"+this.idtypeanomalie+"')";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Anomalie réussie !");
            }
            catch (Exception ex)
            {
                // Gérer les exceptions de connexion ou de requête ici
                c.Close();
                throw new ArgumentException("Erreur lors de l'insertion des données : " + ex.Message);
            }
        }

        public void insertanoreco(NpgsqlConnection c) {
            if(c == null) {
                SqlDB connexion = new SqlDB();
                c = connexion.ConnectPostgres();
            }
            try
            {
                string query = "INSERT INTO anomalierecolte (idrecolte,idtypeanomalie) VALUES ('"+this.idrecolte+"','"+this.idtypeanomalie+"')";
                using (NpgsqlCommand command = new NpgsqlCommand(query, c))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Insertion nouveau Anomalie réussie !");
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